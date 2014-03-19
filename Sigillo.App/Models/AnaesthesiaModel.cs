using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Sigillo.App.Common;

namespace Sigillo.App.Models
{
    public class AnaesthesiaModel
    {
        [Required]
        public int AnaesthesiaId { get; set; }

        public AnaesthesiaType Type { get; set; }

        public string Risks { get; set; }
    }
}
