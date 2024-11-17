using System.ComponentModel.DataAnnotations;

namespace DeliveryServiceConsoleApp
{
    public class Package
    {
        [Required]
        public Guid PackageId { get; private set; }
        [Required]
        [Range(0.01, 100)]
        public double PackageWeight { get; private set; }
        [Required]
        public District PackageDistrict { get; private set; }
        [Required]
        public DateTime DeliveryTime { get; private set; }

        public Package(Guid packageId, double packageWeight, District packageDistrict, DateTime deliveryTime)
        {
            PackageId = packageId;

            PackageWeight = packageWeight;

            PackageDistrict = packageDistrict;

            DeliveryTime = deliveryTime;
        }
        public void CreateRandom()
        {
            Random random = new Random();


            Guid packageId = Guid.NewGuid();


            double packageWeight;
            double min = 0.01;
            double max = 100.00;
            packageWeight = min + (max - min) * random.NextDouble();
            packageWeight = Math.Round(packageWeight, 2);


            District packageDistrict;
            Array enumValues = Enum.GetValues(typeof(District));
            int randomIndex = random.Next(enumValues.Length);
            packageDistrict = (District)enumValues.GetValue(randomIndex);


            DateTime deliveryTime;
            // Случайный год (для данной выборки год только 2024)
            int year = random.Next(2024, 2025);
            // Случайный месяц (для данной выборки месяц только 10)
            int month = random.Next(10, 11);
            // Случайный день в данном месяце
            int day = random.Next(1, DateTime.DaysInMonth(year, month) + 1);
            // Случайный час от 0 до 23
            int hour = random.Next(0, 24);
            // Случайная минута от 0 до 59
            int minute = random.Next(0, 60);
            // Случайная секунда от 0 до 59
            int second = random.Next(0, 60); 

            deliveryTime = new DateTime(year, month, day, hour, minute, second);

            PackageId = packageId;
            PackageWeight = packageWeight;
            PackageDistrict = packageDistrict;
            DeliveryTime = deliveryTime;
        }
    }
}
