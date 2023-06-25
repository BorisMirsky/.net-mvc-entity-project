namespace SimpleAuth.Models
{
    public class SearchResultModel
    {
        public IEnumerable<Entities.UserInfo> Users { get; set; }
        public Entities.UserInfo User { get; set; }
    }
}
