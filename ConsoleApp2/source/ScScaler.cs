using ImageMagick;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace ConsoleApp2
{
    public class ScScaler
    {
        private ScParams _ScParams;
        private IScLogger _Logger;
        private IScStreamLoader _StreamLoader;
        private IImageSaver _ImageSaver;

        public ScScaler(string jsonParams, IScLogger Logger)
        {
            ScParams scParams = JsonSerializer.Deserialize<ScParams>(jsonParams);

            _ScParams = scParams;
            _Logger = Logger;
            _ImageSaver = new ImageSaverFile();

            if (scParams.Location.ToLower().Equals("file"))
            {
                _StreamLoader = new ScFileStreamLoader();   

            } else if (scParams.Location.ToLower().Equals("uri"))
            {
                _StreamLoader = new ScUriStreamLoader();
            }
            else if (scParams.Location.ToLower().Equals("s3"))
            {
                _StreamLoader = new ScS3StreamLoader();
            }
            else 
            {
                throw new Exception("ScScaler no loader type defined");
            }
        }

        public void ScaleImages()
        {
           
            foreach (var file in _ScParams.Files)
            {
                var filename = file.Name;
                var delimiter = "/";
                if (!String.IsNullOrEmpty(file.Uri))
                {
                    delimiter = "";
                    filename = file.Uri;
                }

                var inFileName = $"{_ScParams.InFolder}{delimiter}{filename}.{_ScParams.Ext}";

                var loadwatch = Stopwatch.StartNew();
                
                Stream imageStream = _StreamLoader.GetImage(_ScParams.InFolder, filename, _ScParams.Ext);
                
                loadwatch.Stop();
                var loadElapsedMs = loadwatch.ElapsedMilliseconds;
                Log($"Loading file into stream: {file.Name}{_ScParams.Ext} took: {loadElapsedMs} msek");

                try
                {
                    foreach (var size in _ScParams.Sizes)
                    {
                        var watch = Stopwatch.StartNew();

                        var outFileName = $"{_ScParams.OutFolder}/{file.Name}-{size}{_ScParams.Ext}";
                        
                        // Important: Set pointer to start of stream                    
                        imageStream.Position = 0;

                        using (var image = new MagickImage(imageStream))
                        {
                            var newSize = new MagickGeometry(size);
                            newSize.IgnoreAspectRatio = false;
                            image.Resize(newSize);

                            image.Quality = _ScParams.Quality;

                            if (_ScParams.AutoLevel)
                            {
                                image.AutoLevel();
                            }

                            if (_ScParams.ColorSpace.Equals("sRGB"))
                            {
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

                            if (_ScParams.Strip)
                            {
                                image.Strip();
                            }

                            if (_ScParams.Interlace.Equals("Jpeg"))
                            {
                                image.Interlace = Interlace.Jpeg;
                            }

                            //image.Write(outFileName);
                            _ImageSaver.SaveImage(image, outFileName);

                            watch.Stop();
                            var elapsedMs = watch.ElapsedMilliseconds;

                            Log($"Converted file: {file}-{size}{_ScParams.Ext} Duration: {elapsedMs} msek");

                        };

                    }
                }
                finally
                {
                    imageStream.Close();
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

