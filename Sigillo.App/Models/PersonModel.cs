using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Sigillo.App.Common;

namespace Sigillo.App.Models
{
    public class PersonModel
    {
        [Required]
        public Guid PersonId { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        public UserRole UserRole { get; set; }
    }
}
