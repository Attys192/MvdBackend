
namespace MvdBackend.Models
{
    public class RequestType
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public List<CitizenRequest> Requests { get; set; } = new();
    }
}
