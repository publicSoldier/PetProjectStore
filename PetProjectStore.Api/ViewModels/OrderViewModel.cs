using PetProjectStore.Api.Models;
using System;
using System.Collections.Generic;

namespace PetProjectStore.Api.ViewModels
{
    public class OrderViewModel
    {
        public long Id { get; set; }

        public string CustomerId { get; set; }

        public IReadOnlyCollection<ProductViewModel> Products { get; set; }

        public string Address { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalCost { get; set; }

        public DeliveryTypeModel DeliveryType { get; set; }

    }
}
