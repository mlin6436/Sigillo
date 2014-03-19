using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Sigillo.App.Models
{
    public class ClinicianModel : PersonModel
    {
        [StringLength(100)]
        public string JobTitle { get; set; }

        [StringLength(50)]
        public string ContactNumber { get; set; }
    }
}
