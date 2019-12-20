using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TumbleTracker.Models
{
    public class MeetGymnast
    {
        [Key]
        public int MeetGymnastId { get; set; }
        public string Group { get; set; }

        // Scores
        [Required]
        [Column(TypeName = "decimal(18,4)")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal ScoreBars { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal ScoreBeam { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal ScoreFloor { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal ScoreVault { get; set; }

        // Additional info
        [StringLength(255)]
        public string Notes { get; set; }

        public int? Place { get; set; }


        // Related Data

        [Required]
        public int GymnastId { get; set; }
        [Required]
        public Gymnast Gymnast { get; set; }

        [Required]
        public int MeetId { get; set; }
        [Required]
        public Meet Meet { get; set; }
    }
}