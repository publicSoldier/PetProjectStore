using PetProjectStore.Api.Dtos;
using PetProjectStore.DAL.Entities;
using System;
using System.Collections.Generic;

namespace PetProjectStore.Api.ViewModels
{
    public class OrderViewModel
    {
        public long Id { get; set; }

        public string CustomerId { get; set; }

        public IReadOnlyCollection<ProductDto> Products { get; set; }

        public string Address { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalCost { get; set; }

        public DeliveryType DeliveryType { get; set; }

    }
}
