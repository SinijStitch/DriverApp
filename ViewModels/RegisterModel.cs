using System.ComponentModel.DataAnnotations;

namespace SearchForDriversWebApp.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не вказаний Username")]
        public string Username { get; set; }
        public string Phone { get; set; }

        [Required(ErrorMessage = "Не вказаний Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не вказаний пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введено неправильно")]
        public string ConfirmPassword { get; set; }
    }
}
