using System.Text.Json.Serialization;

namespace MvdBackend.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string LastName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string Patronymic { get; set; } = "";

        [JsonIgnore]
        public List<CitizenRequest> AcceptedRequests { get; set; } = new();

        [JsonIgnore]
        public List<CitizenRequest> AssignedRequests { get; set; } = new();
    }
}
