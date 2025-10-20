namespace MvdBackend.Models
{
    public class District
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Навигационное свойство
        public ICollection<CitizenRequest> CitizenRequests { get; set; } = new List<CitizenRequest>();
    }
}