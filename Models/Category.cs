using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvdBackend.Models
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Column(TypeName = "varchar")]
        public string Name { get; set; } = "";

        [MaxLength(200)]
        [Column(TypeName = "varchar")]
        public string Description { get; set; } = "";

        public ICollection<CitizenRequest>? CitizenRequests { get; set; }
    }
}