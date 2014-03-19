using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Sigillo.App.Models
{
    public class InterpreterModel : PersonModel
    {
        [StringLength(200)]
        public string JobTitle { get; set; }

        [StringLength(200)]
        public string RoleOrRelationship { get; set; }

        public bool IsPresent { get; set; }
    }
}
