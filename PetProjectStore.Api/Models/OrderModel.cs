using System.Collections.Generic;

namespace PetProjectStore.Api.Models
{
    public class OrderModel
    {

        public IReadOnlyCollection<long> ProductIds { get; set; }

        public string Address { get; set; }

        public DeliveryTypeModel DeliveryType { get; set; }
    }
}
