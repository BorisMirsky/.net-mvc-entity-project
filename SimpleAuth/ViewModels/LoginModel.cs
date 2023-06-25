using System.ComponentModel.DataAnnotations;


 
namespace SimpleAuth.ViewModels
{
    public class LoginModel
    {
        //[Required(ErrorMessage = "Не указан Email")]
        //public string? Email { get; set; }

        //[Required(ErrorMessage = "Не указан пароль")]
        //[DataType(DataType.Password)]
        //public string? Password { get; set; }

        [Required(ErrorMessage = "Не указан Email")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }


        [Required(ErrorMessage = "Не указан пароль")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Пароль от 6 до 20 знаков", MinimumLength = 6)]
        public string? Password { get; set; }

        public IEnumerable<Entities.UserInfo>? Users { get; set; }
    }
}
