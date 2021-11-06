namespace PetProjectStore.Api.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; }

        public string Size { get; set; }

        public decimal Cost { get; set; }

        public bool IsFragile { get; set; }
    }
}
