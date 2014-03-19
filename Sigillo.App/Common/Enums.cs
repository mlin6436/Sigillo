using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigillo.App.Common
{
    public enum UserRole
    {
        Patient = 1,
        HealthProfessional = 2,
        Consultant = 3,
        Relative = 4,
        Interpreter = 5,
        Witness = 6,
    }

    public enum ConsentType
    {
        GeneralConsent = 1,
        OneStageConsent = 2,
        Assent = 3,
    }

    public enum ConsentStatus
    {
        Created = 1,
        Presented = 2,
        Cancelled = 3,
        Withdrawn = 4,
        Consented = 5,
    }

    public enum AnaesthesiaType
    {
        GeneralOrRegional = 1,
        Local = 2,
        Sedation = 3,
        Other = 4,
    }
}
