using ImageMagick;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp2
{
    public interface IImageSaver
    {
        public bool SaveImage(MagickImage image, string fileName);
    } 

    internal class ImageSaverFile: IImageSaver
    {
        public bool SaveImage (MagickImage image, string fileName)
        {
            try
            {
                image.Write(fileName);
                return true;
            }
            catch (Exception)
            {
                // TODO Catch here or higer, maybe just log what went wrong, and rethrow...
                return false;
                
            }
            
        }
    }

    internal class ImageSaverS3 : IImageSaver
    {
        public bool SaveImage(MagickImage image, string fileName)
        {
            throw new NotImplementedException();
            //return true;
        }

    }

    internal class ImageSaverPost : IImageSaver
    {
        public bool SaveImage(MagickImage image, string fileName)
        {
            throw new NotImplementedException();   
            //return true;
        }

    }
}
