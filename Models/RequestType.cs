using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvdBackend.Models
{
    public class RequestType
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Column(TypeName = "varchar")]
        public string Name { get; set; } = "";

        public List<CitizenRequest> Requests { get; set; } = new();
    }
}