using SimpleAuth.Entities;


namespace SimpleAuth.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserInfo> UserInfos { get; set; }
        public Role()
        {
            UserInfos = new List<UserInfo>();
        }
    }
}
