using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetProjectStore.DAL.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string CustomerId { get; set; }

        public IReadOnlyCollection<Product> Products { get; set; }

        public string Address { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalCost { get; set; }

        public DeliveryType DeliveryType { get; set; }

    }
}
