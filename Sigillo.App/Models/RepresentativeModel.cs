using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Sigillo.App.Models
{
    public class RepresentativeModel
    {
        [StringLength(200)]
        public string RoleOrRelationship { get; set; }
    }
}
