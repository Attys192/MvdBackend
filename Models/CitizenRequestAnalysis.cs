using System.ComponentModel.DataAnnotations.Schema;

namespace MvdBackend.Models
{
    public class CitizenRequestAnalysis
    {
        public int Id { get; set; }

        [ForeignKey(nameof(CitizenRequest))]
        public int CitizenRequestId { get; set; }

        public CitizenRequest CitizenRequest { get; set; } = null!;

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
