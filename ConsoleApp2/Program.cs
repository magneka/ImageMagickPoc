using System.Collections.Generic;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ScParams = new ScParams
            {
                FileNames = new List<string> { 
                    "ImageShop_1043452714",
                    "ImageShop_1068846278",  
                    "ImageShop_1293598474",
                    "ImageShop_1068846257", 
                    "ImageShop_1220809846",   
                    "ImageShop_1459133075" 
                },
                InFolder = "c:/magnea/testbilder/JPG/originals",
                OutFolder = "c:/magnea/konverterte",
                Ext = "jpg",
                Quality = 85,
                Sizes = new List<int> { 250, 500 },
                AutoLevel = true,
                ColorSpace = "sRGB",
                GammaCorrect = 0.9,
                Interlace = "Jpeg"
            };


            var logger = new ConsoleLogger();
            var scaler = new ScScaler(ScParams, logger);
            
            scaler.ScaleImages();
           
        }
    }
}
