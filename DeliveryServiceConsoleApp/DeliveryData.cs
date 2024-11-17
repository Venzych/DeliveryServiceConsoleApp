using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceConsoleApp
{
    public class DeliveryData
    {
        public string DeliveryLog { get; }
        public string DeliveryOrder { get; }
        public District CityDistrict { get; }
        public DateTime FirstDeliveryDateTime { get; }

        public DeliveryData(string deliveryLog, string deliveryOrder, District cityDistrict, DateTime firstDeliveryDateTime)
        {
            DeliveryLog = deliveryLog;
            DeliveryOrder = deliveryOrder;
            CityDistrict = cityDistrict;
            FirstDeliveryDateTime = firstDeliveryDateTime;
        }
    }
}
