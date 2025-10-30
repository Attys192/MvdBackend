using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MvdBackend.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [MaxLength(25)]
        [Column(TypeName = "varchar")]
        public string LastName { get; set; } = "";

        [MaxLength(25)]
        [Column(TypeName = "varchar")]
        public string FirstName { get; set; } = "";

        [MaxLength(25)]
        [Column(TypeName = "varchar")]
        public string Patronymic { get; set; } = "";

        [JsonIgnore]
        public List<CitizenRequest> AcceptedRequests { get; set; } = new();

        [JsonIgnore]
        public List<CitizenRequest> AssignedRequests { get; set; } = new();
    }
}