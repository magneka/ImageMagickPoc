using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleApp2
{
    public interface IScStreamLoader
    {
        Stream GetImage(string path, string fileName, string ext);
    }

    public class ScFileStreamLoader : IScStreamLoader
    {
        public Stream GetImage (string path, string fileName, string ext)
        {
            var fullFileName = $"{path}/{fileName}{ext}";
            
            FileStream fileStream = new FileStream(fullFileName, FileMode.Open, FileAccess.Read);
            
            return fileStream;
        }
    }

    public class ScUriStreamLoader : IScStreamLoader
    {
        public Stream GetImage(string path, string uri, string ext)
        {

            // Load stream from uri
            using (var client = new WebClient())
            {
                var content = client.DownloadData(uri);
                var stream = new MemoryStream(content);

                return stream;   
            }
            
        }
    }

    public class ScS3StreamLoader : IScStreamLoader
    {
        public Stream GetImage(string path, string Uri, string ext)
        {
            throw new NotImplementedException();
        }
    }
}
