using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp2
{
    public interface IScLogger
    {
        void Log(string message);
    }
    public class ConsoleLogger: IScLogger
    {
        void IScLogger.Log(string message)
        {
            Console.WriteLine(message);
        }       
    }
}
