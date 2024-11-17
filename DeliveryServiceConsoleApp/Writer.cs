using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceConsoleApp
{
    public class Writer
    {
        public string Path { get; }

        public Writer(string filePath)
        {
            Path = filePath;
        }

        public void Print(List<Package> packageArray, bool append = false)
        {
            using (StreamWriter sw = new StreamWriter(Path, append))
            {

            }
            foreach (var package in packageArray)
            {
                using (StreamWriter sw = new StreamWriter(Path, true))
                {
                    sw.WriteLine($"PackageID: {package.PackageId}. Weight: {package.PackageWeight}. " +
                    $"To {package.PackageDistrict} by {package.DeliveryTime}.");
                }
            }
        }
    }
}
