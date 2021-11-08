using System.ComponentModel.DataAnnotations;

namespace PetProjectStore.Api.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Укажите e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
    }
}
