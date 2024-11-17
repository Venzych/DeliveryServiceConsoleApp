using DeliveryServiceConsoleApp;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.IO;

public class Program
{
    static void Main(string[] args)
    {
        DeliveryData deliveryData;
        #region Валидация вводимых аргументов
        if (args.Length > 0)
        {
            #region Обработка файла логирования
            string _deliveryLog = args[0];

            if (File.Exists(_deliveryLog))
            {
                using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
                {
                    sw.WriteLine($"<DONE> Файл {_deliveryLog} найден");
                }
            }
            else
            {
                Console.WriteLine("Файл для логирования не найден: " + _deliveryLog);
                return;
            }
            #endregion

            #region Обработка результирующего файла
            string _deliveryOrder = args[1];

            if (File.Exists(_deliveryOrder))
            {
                using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
                {
                    sw.WriteLine($"<DONE> Файл {_deliveryOrder} найден");
                }
            }
            else
            {
                Console.WriteLine("Результирующий файл не найден: " + _deliveryOrder);
                using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
                {
                    sw.WriteLine($"<ERROR> Результирующий файл не найден: {_deliveryOrder}");
                }
                return;
            }
            #endregion

            #region Обработка выбранного района
            string _cityDistrictArg = args[2];
            District _cityDistrict;

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

            #region Обработка выбранного времени
            string _firstDeliveryDateTimeArg = args[3];
            string dataFormat = "yyyy-MM-dd HH:mm:ss";
            DateTime _firstDeliveryDateTime;

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

            // Создание всех компонентов для удобства (файл логирования, результирующий файл, выбранный район, выбранное время)
            deliveryData = new DeliveryData(_deliveryLog, _deliveryOrder, _cityDistrict, _firstDeliveryDateTime);
        }
        else
        {
            Console.WriteLine("Не указаны аргументы командной строки.");
            return;
        }
        #endregion

        // Путь файла с данными
        string filePath = @"D:\VS works\workApp\DeliveryServiceConsoleApp\DeliveryServiceConsoleApp\Files\data.txt";

        Reader reader = new Reader(filePath);
        Logger logger = new Logger(deliveryData.DeliveryLog);
        Writer writer = new Writer(deliveryData.DeliveryOrder);



        List<Package>? packageArray = new List<Package>();

        try
        {
            foreach (var line in reader.ReadLines())
            {
                var parts = line.Split(' ');
                if (parts.Length < 4)
                {
                    logger.Error("Недостаточно данных в строке: " + line);
                    continue;
                }

                // Преобразуем каждый элемент в соответствующий тип
                Guid guid = Guid.Parse(parts[0]);
                double doubleValue = double.Parse(parts[1]);
                District enumValue = (District)Enum.Parse(typeof(District), parts[2]);
                DateTime dateTimeValue = DateTime.Parse(parts[3] + " " + parts[4]);


                // Валидация данных
                Package package = DeliveryService.ToPackage(guid, doubleValue, enumValue, dateTimeValue);
                var context = new ValidationContext(package);
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(package, context, results, true))
                {
                    foreach (var error in results)
                    {
                        logger.Error($" PackageId: {package.PackageId}. {error.ErrorMessage}.");
                    }
                }
                else
                {
                    logger.Done($"Объект Package успешно создан. PackageId: {package.PackageId}");
                    packageArray.Add(package);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error("Ошибка при обработке файла: " + ex.Message);
        }


        if (packageArray.Count() == 0)
        {
            logger.Error("Нет данных.");
        }
        packageArray = DeliveryService.FilterByDistrict(packageArray, deliveryData.CityDistrict);
        if (packageArray.Count != 0)
        {
            packageArray = DeliveryService.SortByDateTime(packageArray, deliveryData.FirstDeliveryDateTime);
        }
        if (packageArray.Count != 0)
        {
            writer.Print(packageArray);
        }
        else
        {
            Console.WriteLine($"No one package in the {deliveryData.CityDistrict} from {deliveryData.FirstDeliveryDateTime} to {deliveryData.FirstDeliveryDateTime.AddMinutes(30)}.");
        }
        logger.Done($"Программа завершена." + "\n-----------------------------");        
    }
}



/*
 * string dataFormat = "yyyy-MM-dd HH:mm:ss";
*/