using System.ComponentModel.DataAnnotations;


namespace SimpleAuth.Models
{
    public class NewUserModel
    {
        public Entities.UserInfo? User { get; set; }
        public int Id { get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        [Display(Name = "Email (login)")]
        //[DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(18, ErrorMessage = "Надо от 6 до 18 символов", MinimumLength = 6)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        [Display(Name = "Подтвердить Новый Пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [StringLength(18, ErrorMessage = "Надо от 6 до 18 символов", MinimumLength = 6)]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        [Display(Name = "Имя")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        [Display(Name = "Дата приёма на работу")]
        //public DateTime Date { get; set; }
        public string? Date { get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        [Display(Name = "Заработная Плата")]
        [Range(30000, 1000000, ErrorMessage = "У нас зарплаты от 30k до 1000k")]
        public int? Salary { get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        [Display(Name = "Должность")]
        public string? Position { get; set; }

        //[Required(ErrorMessage = "Заполните поле")]
        [Display(Name = "Юзерпик")]
        public string? Photo_Path { get; set; } // = "anonim.png";

        [Required(ErrorMessage = "Заполните поле")]
        [Display(Name = "Непосредственный начальник")]
        public string? Immediate_Supervisor { get; set; }
    }
}
