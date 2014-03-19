using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Sigillo.App.Models
{
    public class PatientModel : PersonModel
    {
        [Required]
        public DateTime DOB { get; set; }

        [Required]
        public string NHSNumber { get; set; }
    }
}
