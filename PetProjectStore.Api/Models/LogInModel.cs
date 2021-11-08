using System.ComponentModel.DataAnnotations;

namespace PetProjectStore.Api.Models
{
    public class LogInModel
    {
        [Required(ErrorMessage = "Укажите e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
