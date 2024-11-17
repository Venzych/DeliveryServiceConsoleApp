using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceConsoleApp
{
    public class Logger
    {
        public string Path { get; private set; }
        public Logger (string path) { Path = path; }
        public void ChangePath (string path) { Path = path; }
        public void Done(string message)
        {
            using (StreamWriter sw = new StreamWriter(Path, true))
            {
                sw.WriteLine($"<DONE> {message}");
            }
        }
        public void Error(string message) 
        {
            using (StreamWriter sw = new StreamWriter(Path, true))
            {
                sw.WriteLine($"<ERROR> {message}");
            }
        }
        public void Close() 
        {
            using (StreamWriter sw = new StreamWriter(Path, true))
            {
                sw.WriteLine($"<DONE> Программа завершена.");
                sw.WriteLine("-----------------------------");
            }
        }
    }
}
