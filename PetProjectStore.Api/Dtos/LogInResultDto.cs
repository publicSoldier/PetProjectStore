using PetProjectStore.DAL.Entities;

namespace PetProjectStore.Api.Dtos
{
    public class LogInResultDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public UserRole UserRole { get; set; }
    }
}
