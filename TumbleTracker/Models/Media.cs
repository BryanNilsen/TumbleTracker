using System.ComponentModel.DataAnnotations;

namespace TumbleTracker.Models
{
    public class Media
    {
        [Key]
        public int MediaId { get; set; }

        [Required]
        [Display(Name = "File URL")]
        public string FileUrl { get; set; }

        // Related Data

        [Required]
        public int GymnastId { get; set; }

        [Required]
        public Gymnast Gymnast { get; set; }


        public int? MeetId { get; set; }
        public Meet Meet { get; set; }
    }
}