using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;
namespace MvdBackend.Models
{
    public class CitizenRequest
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Citizen))]
        public int CitizenId { get; set; }

        [JsonIgnore]
        public Citizen Citizen { get; set; } = null!;

        [ForeignKey(nameof(RequestType))]
        public int RequestTypeId { get; set; }

        [JsonIgnore]
        public RequestType RequestType { get; set; } = null!;

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [JsonIgnore]
        public Category Category { get; set; } = null!;

        public string Description { get; set; } = "";

        [ForeignKey(nameof(AcceptedBy))]
        public int AcceptedById { get; set; }

        [JsonIgnore]
        public Employee AcceptedBy { get; set; } = null!;

        [ForeignKey(nameof(AssignedTo))]
        public int AssignedToId { get; set; }

        [JsonIgnore]
        public Employee AssignedTo { get; set; } = null!;

        public DateTime IncidentTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string IncidentLocation { get; set; } = "";
        public string CitizenLocation { get; set; } = "";
        public string RequestNumber { get; set; } = "";
        public DateTime? UpdatedAt { get; set; }


        [ForeignKey(nameof(RequestStatus))]
        public int RequestStatusId { get; set; }

        [JsonIgnore]
        public RequestStatus RequestStatus { get; set; } = null!;


        [ForeignKey(nameof(District))]
        public int? DistrictId { get; set; }

        [JsonIgnore]
        public District? District { get; set; }

        public Point? Location { get; set; }


        public string? AiCategory { get; set; }
        public string? AiPriority { get; set; }
        public string? AiSummary { get; set; }
        public string? AiSuggestedAction { get; set; }
        public string? AiSentiment { get; set; }
        public DateTime? AiAnalyzedAt { get; set; }

        
        public bool IsAiCorrected { get; set; }
        public string? FinalCategory { get; set; } 
    }
}
