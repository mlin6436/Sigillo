using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Sigillo.App.Models
{
    public class MaterialModel
    {
        [Required]
        public int MaterialId { get; set; }

        [StringLength(200)]
        public string Name { get; set; }
    }
}
