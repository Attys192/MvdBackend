using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MvdBackend.Models
{
    public class Role
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string Name { get; set; } = "";

        [JsonIgnore]
        public List<User> Users { get; set; } = new();
    }
}