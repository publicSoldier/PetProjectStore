using PetProjectStore.DAL.Entities;
using System.Collections.Generic;

namespace PetProjectStore.Api.Dtos
{
    public class OrderDto
    {

        public IReadOnlyCollection<long> ProductIds { get; set; }

        public string Address { get; set; }

        public DeliveryType DeliveryType { get; set; }
    }
}
