using System.Text.Json.Serialization;

namespace MvdBackend.Models
{
    public class Citizen
    {
        public int Id { get; set; }
        public string LastName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string Patronymic { get; set; } = "";
        public string Phone { get; set; } = "";

        [JsonIgnore]
        public List<CitizenRequest> Requests { get; set; } = new();
    }
}
