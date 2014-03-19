using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Sigillo.App.Common;

namespace Sigillo.App.Models
{
    public class ConsentModel
    {
        [Required]
        public Guid ConsentId { get; set; }

        public PatientModel Patient { get; set; }

        public ClinicianModel Clinician { get; set; }

        public InterpreterModel Interpreter { get; set; }

        public WitnessModel Witness { get; set; }

        public RepresentativeModel Representative { get; set; }

        public List<ProcedureModel> Procedures { get; set; }

        public ConsentType Type { get; set; }

        public ConsentStatus Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid? ModifiedBy { get; set; }

        public bool IsComplete { get; set; }

        [StringLength(2000)]
        public string CancelledReason { get; set; }

        [StringLength(2000)]
        public string WithdrawnReason { get; set; }
    }
}
