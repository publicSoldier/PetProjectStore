using PetProjectStore.Api.Dtos;
using PetProjectStore.Api.Models;
using System.Threading.Tasks;

namespace PetProjectStore.Api.Interfaces
{
    public interface IAccountService
    {
        public Task RegistrationAsync(RegistrationModel registrationModel);

        public Task<LogInResultDto> LogInAsync(LogInModel logInModel);
    }
}
