using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvdBackend.Models
{
    public class District
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        [Column(TypeName = "varchar")]
        public string Description { get; set; } = string.Empty;

        public ICollection<CitizenRequest> CitizenRequests { get; set; } = new List<CitizenRequest>();
    }
}