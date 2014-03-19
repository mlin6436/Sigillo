using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigillo.App.Models
{
    public class CapacityModel
    {
        public Guid CapacityId { get; set; }

        public bool PatientIsUnconscious { get; set; }

        public bool PatientHasASoundedMind { get; set; }

        public bool PatientCanUnderstandInformationProvided { get; set; }

        public bool PatientCanRetainInformationProvided { get; set; }

        public bool PatientCanUseInfromationProvided { get; set; }

        public bool PatientCanCommunicateDecision { get; set; }

        public bool PatientHasCapacityToDecide { get; set; }

        public bool PatientLacksOfCapacityPermanently { get; set; }

        public bool DecisionCanWait { get; set; }

        public CapacityModel()
        {
            CapacityId = new Guid();
            PatientIsUnconscious = true;
            PatientHasASoundedMind = true;
            PatientCanUnderstandInformationProvided = true;
            PatientCanRetainInformationProvided = true;
            PatientCanUseInfromationProvided = true;
            PatientCanCommunicateDecision = true;
            PatientHasCapacityToDecide = true;
            PatientLacksOfCapacityPermanently = true;
            DecisionCanWait = true;
        }
    }
}
