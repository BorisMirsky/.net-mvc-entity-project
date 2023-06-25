using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace SimpleAuth.Models
{
    public class UserProfileEditModel
    {
        public int Id { get; set; }
        public string? Email { get; set; }    // текущий email, он же логин
        public string? Password { get; set; } // текущий пароль

        [Display(Name = "Новый Email")]
        //[DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid email format.")]
        public string? NewEmail { get; set; }

        [Display(Name = "Новый Пароль")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Пароль слишком короткий", MinimumLength = 6)]
        public string? NewPassword { get; set; }

        [Display(Name = "Подтвердить Новый Пароль")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Пароль слишком короткий", MinimumLength = 6)]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string? ConfirmNewPassword { get; set; }

        [Display(Name = "Новая Заработная Плата")]
        [Range(30000, 1000000, ErrorMessage = "У нас получают от 30k до 1000k")]
        public int? NewSalary { get; set; }

        [Display(Name = "Новая Должность")]
        public string? NewPosition { get; set; }

        [Display(Name = "Новый Непосредственный Начальник")]
        public string? NewImmediate_Supervisor { get; set; }

        [Display(Name = "Загрузить Новую Фотографию")]
        public string? New_Photo_Path { get; set; }

        public Entities.UserInfo? userInfo { get; set; }

    }
}
