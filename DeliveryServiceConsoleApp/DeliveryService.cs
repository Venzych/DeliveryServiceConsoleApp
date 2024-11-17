using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceConsoleApp
{
    public static class DeliveryService
    {
        public static Package? ToPackage(string data)
        {
            try
            {
                    var parts = data.Split(' ');
                    if (parts.Length < 4)
                    {
                        /*using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
                        {
                            sw.WriteLine("<ERROR> Недостаточно данных в строке: " + data);
                        }
                        continue;*/
                    }

                    // Преобразуем каждый элемент в соответствующий тип
                    Guid guid = Guid.Parse(parts[0]);
                    double doubleValue = double.Parse(parts[1]);
                    District enumValue = (District)Enum.Parse(typeof(District), parts[2]);
                    DateTime dateTimeValue = DateTime.Parse(parts[3] + " " + parts[4]);
                    return new Package(guid, doubleValue, enumValue, dateTimeValue);
            }
            catch (Exception ex)
            {
                /*using (StreamWriter sw = new StreamWriter(_deliveryLog, true))
                {
                    sw.WriteLine("<ERROR> Ошибка при обработке файла: " + ex.Message);
                }*/
                return null;
            }
        }
        public static Package ToPackage(Guid packageId, double packageWeight, District packageDistrict, DateTime deliveryTime)
        {
            return new Package(packageId, packageWeight, packageDistrict, deliveryTime);
        }
        // Фильтрация по району
        public static List<Package> FilterByDistrict(List<Package> packageArray, District _cityDistrict)
        {
            return packageArray.Where(p => p.PackageDistrict == _cityDistrict).ToList();
        }
        // Доставка в течении 30 минут
        public static List<Package> SortByDateTime(List<Package> packageArray, DateTime _firstDeliveryDateTime)
        {
            var _lastDeliveryDateTime = _firstDeliveryDateTime.AddMinutes(30);
            return packageArray.OrderBy(p => p.DeliveryTime).Where(p => p.DeliveryTime <= _lastDeliveryDateTime && p.DeliveryTime >= _firstDeliveryDateTime).ToList();
        }
        // Доставка от _firstDeliveryDateTime и до _lastDeliveryDateTime
        public static List<Package> SortByDateTime(List<Package> packageArray, DateTime _firstDeliveryDateTime, DateTime _lastDeliveryDateTime)
        {
            return packageArray.OrderBy(p => p.DeliveryTime).Where(p => p.DeliveryTime <= _lastDeliveryDateTime && p.DeliveryTime >= _firstDeliveryDateTime).ToList();
        }
    }
}
