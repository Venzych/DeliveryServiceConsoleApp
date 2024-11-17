using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceConsoleApp
{
    public class Reader
    {
        public string Path { get; }

        public Reader (string filePath)
        {
            Path = filePath;
        }

        public string[] ReadLines()
        {
            try
            {
                // Проверяем, существует ли файл
                if (!File.Exists(Path))
                {
                    throw new FileNotFoundException("Файл не найден.", Path);
                }

                // Чтение всех строк из файла
                return File.ReadAllLines(Path);
            }
            catch (Exception ex)
            {
                // Обработка исключений
                Console.WriteLine($"Произошла ошибка при чтении файла: {ex.Message}");
                return Array.Empty<string>();
            }
        }
    }
}
