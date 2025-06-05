using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentSingleModel.Models
{
    public class Student
    {
            public int Id { get; set; }

            [Required(ErrorMessage = "Name is required.")]
            [Display(Name = "Full Name")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Date of Birth is required.")]
            [Display(Name = "Date of Birth")]
            public DateTime DateOfBirth { get; set; }

            [Required(ErrorMessage = "Gender is required.")]
            [Display(Name = "Gender")]
            public string Gender { get; set; }

        [Display(Name = "Hobbies")]
        public string Hobbies { get; set; } 

        //[Required(ErrorMessage = "Profile picture is required.")]
        //[Display(Name = "Profile Picture")]
        public string ProfilePicture { get; set; }

            [NotMapped]
            public HttpPostedFileBase ProfilePicFile { get; set; }
        
    }
}