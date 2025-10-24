
namespace MvdBackend.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public ICollection<CitizenRequest>? CitizenRequests { get; set; }
    }
}
