namespace PetProjectStore.Api.ViewModels
{
    public class ProductViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Size { get; set; }

        public decimal Cost { get; set; }

        public bool IsFragile { get; set; }

    }
}
