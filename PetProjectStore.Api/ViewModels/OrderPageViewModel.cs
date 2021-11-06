using System.Collections.Generic;

namespace PetProjectStore.Api.ViewModels
{
    public class OrderPageViewModel
    {
        public PageViewModel PageViewModel { get; set; }

        public IReadOnlyCollection<OrderViewModel> OrderViewModel { get; set; }
    }
}
