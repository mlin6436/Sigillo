using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Sigillo.App.Models
{
    public class AssentModel
    {
        [Required]
        public Guid AssentId { get; set; }

        public bool AssentLPAAvailable { get; set; }

        public bool AssentAdvanceDecisionInPlace { get; set; }

        public bool AssentDOLSRequired { get; set; }

        public bool PatientHasFriendFamilyToConsult { get; set; }

        public bool PatientFriendFamilyWillingToBeConsulted { get; set; }

        public bool AssentIMCAInstructed { get; set; }

        public bool PatientHasExpressedAView { get; set; }

        public AssentModel()
        {
            AssentId = new Guid();
            AssentLPAAvailable = true;
            AssentAdvanceDecisionInPlace = true;
            AssentDOLSRequired = true;
            PatientHasFriendFamilyToConsult = true;
            PatientFriendFamilyWillingToBeConsulted = true;
            AssentIMCAInstructed = true;
            PatientHasExpressedAView = true;
        }
    }
}
