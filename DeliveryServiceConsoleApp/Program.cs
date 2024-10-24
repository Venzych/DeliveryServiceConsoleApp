using DeliveryServiceConsoleApp;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.IO;

public class Program
{
    static void Main(string[] args)
    {
        string _deliveryLog;
        string _deliveryOrder;
        District _cityDistrict;
        DateTime _firstDeliveryDateTime;
        string filePath = "D:\\VS works\\workApp\\DeliveryServiceConsoleApp\\DeliveryServiceConsoleApp\\Files\\data.txt";

        #region Валидация вводимых аргументов
        if (args.Length > 0)
        {
            #region _deliveryLog
            _deliveryLog = args[0];

            if (File.Exists(_deliveryLog))
            {
                using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
                {
                    sw.WriteLine($"<DONE> Файл {_deliveryLog} найден");
                }
                //Console.WriteLine($"Файл {_deliveryLog} найден"); //Для дебага
            }
            else
            {
                Console.WriteLine("Файл не найден: " + _deliveryLog);
                return;
            }
            #endregion

            #region _deliveryOrder
            _deliveryOrder = args[1];

            if (File.Exists(_deliveryOrder))
            {
                using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
                {
                    sw.WriteLine($"<DONE> Файл {_deliveryOrder} найден");
                }
                //Console.WriteLine($"Файл {_deliveryOrder} найден"); //Для дебага
            }
            else
            {
                Console.WriteLine("Файл не найден: " + _deliveryOrder);
                using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
                {
                    sw.WriteLine($"<ERROR> Файл {_deliveryOrder} не найден");
                }
                return;
            }
            #endregion

            #region _cityDistrictArg
            string _cityDistrictArg = args[2];

            if (Enum.TryParse<District>(_cityDistrictArg, true, out District district))
            {
                _cityDistrict = district;
                using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
                {
                    sw.WriteLine($"<DONE> Выбранный район: {district}");
                }
                Console.WriteLine($"Выбранный район: {district}");
            }
            else
            {
                Console.WriteLine("Введённая строка не соответствует значениям перечисления District.");
                using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
                {
                    sw.WriteLine($"<ERROR> Введённая строка не соответствует значениям перечисления District.");
                }
                return;
            }
            #endregion

            #region _firstDeliveryDateTimeArg
            string _firstDeliveryDateTimeArg = args[3];
            string dataFormat = "yyyy-MM-dd HH:mm:ss";

            if (DateTime.TryParseExact(_firstDeliveryDateTimeArg, dataFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                _firstDeliveryDateTime = date;
                using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
                {
                    sw.WriteLine($"<DONE> Выбранная дата: {date}\n");
                }
                Console.WriteLine($"Выбранная дата: {date}");
            }
            else
            {
                Console.WriteLine("Введённая строка не является корректной датой в заданном формате.");
                using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
                {
                    sw.WriteLine($"<ERROR> Введённая строка не является корректной датой в заданном формате.");
                }
                return;
            }
            #endregion
        }
        else
        {
            Console.WriteLine("Не указаны аргументы командной строки.");
            return;
        }
        #endregion

        List<Package>? packageArray = new List<Package>();


        /*
        //CreatePackage(Guid.NewGuid(), 1.3, District.Arbat, DateTime.Now.AddMinutes(30)); // Создание конретной посылки вручную
        for (int i = 0; i < 15; i++)
            CreateRandomPackage; //Создание конкретного числа экземпляров
        */


        ReadFile(filePath);
        if (packageArray.Count() == 0)
        {
            Console.WriteLine("Нет данных.");
            using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
            {
                sw.WriteLine("Нет данных.");
            }
        }
        



            packageArray = FiltredByDistrict(packageArray, _cityDistrict); //_cityDistrict (реализация по условию)
        if (packageArray.Count != 0)
        {
            packageArray = SortedByDateTime(packageArray); //_firstDeliveryDateTime (реализация по условию)
        }
        if (packageArray.Count != 0)
        {
            OutSortedPackageArray(packageArray);
        }
        else
        {
            Console.WriteLine($"No one package in the {_cityDistrict} from {_firstDeliveryDateTime} to {_firstDeliveryDateTime.AddMinutes(30)}.");
        }

        using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
        {
            sw.WriteLine($"<DONE> Программа завершена.");
            sw.WriteLine("-----------------------------");
        }






        void CreatePackage(Guid packageId, double packageWeight, District packageDistrict, DateTime deliveryTime)
        {
            Package package = new Package(packageId, packageWeight, packageDistrict, deliveryTime);

            var context = new ValidationContext(package);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(package, context, results, true))
            {
                //Console.WriteLine("Не удалось создать объект Package"); // Для дебага
                foreach (var error in results)
                {
                    using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
                    {
                        sw.WriteLine($"<ERROR> PackageId: {package.PackageId}. {error.ErrorMessage}.");
                    }
                    //Console.WriteLine(error.ErrorMessage); //Для дебага
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
                {
                    sw.WriteLine($"<DONE> Объект Package успешно создан. PackageId: {package.PackageId}");
                }
                //Console.WriteLine($"Объект Package успешно создан. PackageId: {package.PackageId}\n"); //Для дебага
                packageArray.Add(package);
            }
        }

        void CreateRandomPackage()
        {
            Random random = new Random();


            Guid packageId = Guid.NewGuid();


            double packageWeight;
            double min = -10.00; // Для создания проверки на валидирование данных. Верный интервал начинается с 0.01
            double max = 100.00;
            packageWeight = min + (max - min) * random.NextDouble();
            packageWeight = Math.Round(packageWeight, 2);


            District packageDistrict;
            Array enumValues = Enum.GetValues(typeof(District));
            int randomIndex = random.Next(enumValues.Length);
            packageDistrict = (District)enumValues.GetValue(randomIndex);


            DateTime deliveryTime;
            int year = random.Next(2024, 2025); // Случайный год (для данной выборки год только 2024)
            int month = random.Next(10, 11); // Случайный месяц (для данной выборки месяц только 10)
            int day = random.Next(1, DateTime.DaysInMonth(year, month) + 1); // Случайный день в данном месяце
            int hour = random.Next(0, 24); // Случайный час от 0 до 23
            int minute = random.Next(0, 60); // Случайная минута от 0 до 59
            int second = random.Next(0, 60); // Случайная секунда от 0 до 59

            deliveryTime = new DateTime(year, month, day, hour, minute, second);

            /*
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine($"{packageId} {packageWeight} {packageDistrict} {deliveryTime}");
            }
             // Создание новых тестов
            */

            CreatePackage(packageId, packageWeight, packageDistrict, deliveryTime);
        }

        List<Package> FiltredByDistrict(List<Package> packageArray, District _cityDistrict)
        {
            return packageArray.Where(p => p.PackageDistrict == _cityDistrict).ToList();
        }

        List<Package> SortedByDateTime(List<Package> packageArray)
        {
            var _lastDeliveryDateTime = _firstDeliveryDateTime.AddMinutes(30);
            return packageArray.OrderBy(p => p.DeliveryTime).Where(p => p.DeliveryTime <= _lastDeliveryDateTime && p.DeliveryTime >= _firstDeliveryDateTime).ToList();
        }

        void ReadFile(string filePath)
        {
            try
            {              
                foreach (var line in File.ReadLines(filePath))
                {
                    var parts = line.Split(' ');
                    if (parts.Length < 4)
                    {
                        using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
                        {
                            sw.WriteLine("<ERROR> Недостаточно данных в строке: " + line);
                        }
                        continue;
                    }

                    // Преобразуем каждый элемент в соответствующий тип
                    Guid guid = Guid.Parse(parts[0]);
                    double doubleValue = double.Parse(parts[1]);
                    District enumValue = (District)Enum.Parse(typeof(District), parts[2]);
                    DateTime dateTimeValue = DateTime.Parse(parts[3] + " " + parts[4]);

                    CreatePackage(guid, doubleValue, enumValue, dateTimeValue);
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
                {
                    sw.WriteLine("<ERROR> Ошибка при обработке файла: " + ex.Message);
                }
            }
        }

        void OutSortedPackageArray(List<Package> packageArray)
        {
            Console.WriteLine("Список отсортированных заказов:");
            using (StreamWriter sw = new StreamWriter(_deliveryOrder))
            {
                
            }
            foreach (var package in packageArray)
            {
                using (StreamWriter sw = new StreamWriter(_deliveryOrder, true))
                {
                    sw.WriteLine($"PackageID: {package.PackageId}. Weight: {package.PackageWeight}. " +
                    $"To {package.PackageDistrict} by {package.DeliveryTime}.");
                }
                Console.WriteLine($"PackageID: {package.PackageId}. Weight: {package.PackageWeight}. " +
                    $"To {package.PackageDistrict} by {package.DeliveryTime}.");
            }
        }
    }
}



/*
 * string dataFormat = "yyyy-MM-dd HH:mm:ss";
 * filePath = "D:\VS works\workApp\DeliveryServiceConsoleApp\DeliveryServiceConsoleApp\Files\data.txt"
*/