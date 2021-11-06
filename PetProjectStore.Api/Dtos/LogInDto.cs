using System.ComponentModel.DataAnnotations;

namespace PetProjectStore.Api.Dtos
{
    public class LogInDto
    {
        [Required(ErrorMessage = "Укажите e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
