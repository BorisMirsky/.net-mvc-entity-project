namespace SimpleAuth.Models
{
    public class UserPageModel
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Name { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? Date { get; set; }
        public string? Date { get; set; }

        public int? Salary { get; set; }

        public string? Position { get; set; }

        public string? Photo_Path { get; set; }

        public string? Immediate_Supervisor { get; set; }

        public Entities.UserInfo? userInfo { get; set; }
    }
}
