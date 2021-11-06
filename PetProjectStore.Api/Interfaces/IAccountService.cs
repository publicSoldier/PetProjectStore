using PetProjectStore.Api.Dtos;
using System.Threading.Tasks;

namespace PetProjectStore.Api.Interfaces
{
    public interface IAccountService
    {
        public Task RegistrationAsync(RegistrationDto registrationDto);

        public Task<LogInResultDto> LogInAsync(LogInDto logInDto);
    }
}
