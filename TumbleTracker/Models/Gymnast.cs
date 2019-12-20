using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TumbleTracker.Models
{
    public class Gymnast
    {
        [Key]
        public int GymnastId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "DOB")]
        public DateTime DOB { get; set; }

        [NotMapped]
        public int Age
        {
            get
            {

                DateTime today = DateTime.Today;

                int age = today.Year - DOB.Year;

                if (DOB > today.AddYears(-age))
                {

                    return age--;
                }
                return age;
            }
        }

        // Related Data

        [Required]
        public string UserId { get; set; }

        [Required]
        public ApplicationUser User { get; set; }


        public List<Media> Medias { get; set; }

    }
}
