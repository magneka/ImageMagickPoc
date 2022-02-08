using ImageMagick;
using System;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp2
{
    public class ScScaler
    {
        private ScParams _ScParams;
        private IScLogger _Logger;

        public ScScaler(ScParams scParams, IScLogger Logger)
        {
            _ScParams = scParams;
            _Logger = Logger;
        }

        public void ScaleImages()
        {
           
            foreach (var file in _ScParams.FileNames)
            {
                var inFileName = $"{_ScParams.InFolder}/{file}.{_ScParams.Ext}";

                using (FileStream fileStream = new FileStream(inFileName, FileMode.Open, FileAccess.Read))
                {
                    foreach (var size in _ScParams.Sizes)
                    {
                        var watch = Stopwatch.StartNew();

                        var outFileName = $"{_ScParams.OutFolder}/{file}-{size}.{_ScParams.Ext}";

                        // Important: Set pointer to start of stream                    
                        fileStream.Position = 0;

                        using (var image = new MagickImage(fileStream))
                        {
                            var newSize = new MagickGeometry(size);
                            newSize.IgnoreAspectRatio = false;
                            image.Resize(newSize);
                            
                            image.Quality = _ScParams.Quality;
                            
                            if (_ScParams.AutoLevel) { 
                                image.AutoLevel(); 
                            }
                            
                            if (_ScParams.ColorSpace.Equals("sRGB")) {
                                image.ColorSpace = ColorSpace.sRGB;
                            }

                            if (_ScParams.Modulate != null)
                            {
                                image.Modulate(
                                    new Percentage((double)_ScParams.Modulate.Item1), 
                                    new Percentage((double)_ScParams.Modulate.Item2)
                                 );
                            }
                            
                            if (_ScParams.GammaCorrect != null)
                            {
                                image.GammaCorrect((double)_ScParams.GammaCorrect);
                            }

                            if (_ScParams.Strip) {
                                image.Strip();
                            }

                            if (_ScParams.Interlace.Equals("Jpeg"))
                            {
                                image.Interlace = Interlace.Jpeg; 
                            }

                            image.Write(outFileName);

                            watch.Stop();
                            var elapsedMs = watch.ElapsedMilliseconds;

                            Log ($"Converted file: {file}.{_ScParams.Ext} Duration: {elapsedMs}");
                            
                        };
                    }
                }
            }
            Log("Done");
        }

        public void Log (String message)
        {
            if (_Logger != null)
            {
               _Logger.Log (message);
            }
        }
    }
}

