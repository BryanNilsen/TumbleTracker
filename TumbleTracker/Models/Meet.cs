using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TumbleTracker.Models
{
    public class Meet
    {
        [Key]
        public int MeetId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required]
        public string EventName { get; set; }
        public string HostGym { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public string Zip { get; set; }

        // Related Data

        [Required]
        public string UserId { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

    }
}
