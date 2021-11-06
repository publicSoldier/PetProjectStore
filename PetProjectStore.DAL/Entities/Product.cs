using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetProjectStore.DAL.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Size { get; set; }

        public decimal Cost { get; set; }

        public bool IsFragile { get; set; }

        public IReadOnlyCollection<Order> Orders { get; set; }
    }
}