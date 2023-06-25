namespace SimpleAuth.Models
{
    public class IndexModel
    {
        public IEnumerable<Entities.UserInfo>? Users { get; set; }
        public Entities.UserInfo? User { get; set; }
        
    }
}
