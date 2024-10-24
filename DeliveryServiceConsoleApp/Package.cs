using System.ComponentModel.DataAnnotations;

namespace DeliveryServiceConsoleApp
{
    public class Package
    {
        [Required]
        public Guid PackageId { get; }
        [Required]
        [Range(0.01, 100)]
        public double PackageWeight { get; }
        [Required]
        public District PackageDistrict { get; }
        [Required]
        public DateTime DeliveryTime { get; }

        public Package(Guid packageId, double packageWeight, District packageDistrict, DateTime deliveryTime)
        {
            PackageId = packageId;

            PackageWeight = packageWeight;

            PackageDistrict = packageDistrict;

            DeliveryTime = deliveryTime;
        }
    }
}
