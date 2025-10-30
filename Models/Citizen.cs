using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MvdBackend.Models
{
    public class Citizen
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

        [MaxLength(12)]
        [Column(TypeName = "varchar")]
        public string Phone { get; set; } = "";

        [JsonIgnore]
        public List<CitizenRequest> Requests { get; set; } = new();
    }
}