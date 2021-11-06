using System.Collections.Generic;

namespace PetProjectStore.Api.ViewModels
{
    public class ProductPageViewModel
    {
        public PageViewModel PageViewModel { get; set; }

        public IReadOnlyCollection<ProductViewModel> ProductViewModel { get; set; } 
    }
}
