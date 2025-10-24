using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MvdBackend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";

        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }

        [JsonIgnore]
        public Employee Employee { get; set; } = null!;

        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }

        [JsonIgnore]
        public Role Role { get; set; } = null!;
    }
}
