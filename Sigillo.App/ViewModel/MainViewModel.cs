using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Text.RegularExpressions;
using Sigillo.App.Common;
using Sigillo.Resources;
using Sigillo.App.Models;

namespace Sigillo.App.ViewModel
{
    public class MainViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Members

        private const string LettersAndSpaceValidationRegex = @"^[a-zA-Z][a-zA-Z\s]*$";
        private const string TelephoneNumberValidationRegex = @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$";

        private const string ProceduresProposedIsExpandedPropertyName = "ProceduresProposedIsExpanded";
        private const string ProceduresProposedIsSelectedPropertyName = "ProceduresProposedIsSelected";
        private const string CurrentDateTimePropertyName = "CurrentDateTime";
        private const string InformationBarPropertyName = "StatusBar"; 
        private const string HealthProfessionalPropertyName = "HealthProfessional";
        private const string HealthProfessionalJobTitlePropertyName = "HealthProfessionalJobTitle";
        private const string HealthProfessionalContactNumberPropertyName = "HealthProfessionalContactNumber";
        private const string PatientPropertyName = "Patient";
        private const string PatientSurnamePropertyName = "PatientSurname";
        private const string PatientFirstNamePropertyName = "PatientFirstName";
        private const string PatientDOBPropertyName = "PatientDOB";
        private const string PatientNHSNumberPropertyName = "PatientNHSNumber";
        private const string RepresentativePropertyName = "Representative";
        private const string WitnessPropertyName = "Witness";
        private const string InterpreterPropertyName = "Interpreter";
        private const string InterpreterSurnamePropertyName = "InterpreterSurname";
        private const string InterpreterFirstNamePropertyName = "InterpreterFirstName";
        private const string InterpreterJobTitlePropertyName = "InterpreterJobTitle";
        private const string InterpreterRoleOrRelationshipPropertyName = "InterpreterRoleOrRelationship";
        private const string InterpreterIsPresentPropertyName = "InterpreterIsPresent";
        private const string ProceduresSearchResultPropertyName = "ProceduresSearchResult";
        private const string ProceduresSelectedPropertyName = "ProceduresSelected";
        private const string AlternativeProceduresSearchResultPropertyName = "AlternativeProceduresSearchResult";

        private const string CapacityPropertyName = "Capacity";
        private const string IsPatientConsciousPropertyName = "PatientIsUnconscious";
        private const string DoesPatientHaveASoundedMindPropertyName = "PatientHasASoundedMind";
        private const string DoesPatientUnderstandInformationProvidedPropertyName = "PatientCanUnderstandInformationProvided";
        private const string IsPatientAbleToRetainInformationProvidedPropertyName = "PatientCanRetainInformationProvided";
        private const string IsPatientAbleToUseInfromationProvidedPropertyName = "PatientCanUseInfromationProvided";
        private const string IsPatientAbleToCommunicateDecisionPropertyName = "PatientCanCommunicateDecision";
        private const string DoesPatientLackCapacityToDecidePropertyName = "PatientHasCapacityToDecide";
        private const string IsLackOfCapacityPermanentPropertyName = "PatientLacksOfCapacityPermanently";
        private const string CanTheDecisionWaitPropertyName = "DecisionCanWait";

        private const string AssentPropertyName = "Assent";
        private const string LPAIsAvailablePropertyName = "AssentLPAAvailable";
        private const string AdvanceDecisionIsInPlacePropertyName = "AssentAdvanceDecisionInPlace";
        private const string DOLSRequiredPropertyName = "AssentDOLSRequired";
        private const string PatientHasFriendFamilyToConsultPropertyName = "PatientHasFriendFamilyToConsult";
        private const string PatientFriendFamilyWillingToBeConsultedPropertyName = "PatientFriendFamilyWillingToBeConsulted";
        private const string IMCAHasBeenInstructedPropertyName = "AssentIMCAInstructed";
        private const string PatientHasExpressedAViewPropertyName = "PatientHasExpressedAView";

        private bool _proceduresProposedIsExpanded;
        private bool _proceduresProposedIsSelected;
        private DateTime _currentDateTime;
        private string _statusBar;
        private ConsentModel _consent;
        private ClinicianModel _healthProfessional;
        private PatientModel _patient;
        private RepresentativeModel _representative;
        private WitnessModel _witnessModel;
        private InterpreterModel _interpreterModel;
        private ObservableCollection<ProcedureModel> _proceduresSearchResult;
        private ObservableCollection<ProcedureModel> _proceduresSelected;
        private ObservableCollection<AlternativeProcedureModel> _alternativeProceduresSearchResult;
        private CapacityModel _capacity;
        private AssentModel _assent;

        #endregion

        #region Properties

        public bool ProceduresProposedIsExpanded
        {
            get { return _proceduresProposedIsExpanded; }
            set
            {
                if (!Equals(value, _proceduresProposedIsExpanded))
                {
                    _proceduresProposedIsExpanded = value;
                    RaisePropertyChanged(ProceduresProposedIsExpandedPropertyName);
                }
            }
        }

        public bool ProceduresProposedIsSelected
        {
            get { return _proceduresProposedIsSelected; }
            set
            {
                if (!Equals(value, _proceduresProposedIsSelected))
                {
                    _proceduresProposedIsSelected = value;
                    RaisePropertyChanged(ProceduresProposedIsSelectedPropertyName);
                }
            }
        }

        public DateTime CurrentDateTime
        {
            get { return DateTime.Now; }
            set
            {
                if (!Equals(value, _currentDateTime))
                {
                    _currentDateTime = value;
                    RaisePropertyChanged(CurrentDateTimePropertyName);
                }
            }
        }

        public string StatusBar
        {
            get { return _statusBar; }
            set
            {
                if (!Equals(value, _statusBar))
                {
                    _statusBar = value;
                    RaisePropertyChanged(InformationBarPropertyName);
                }
            }
        }

        public ClinicianModel HealthProfessional
        {
            get { return _healthProfessional; }
            set
            {
                if (!Equals(value, _healthProfessional))
                {
                    _healthProfessional = value;
                    RaisePropertyChanged(HealthProfessionalPropertyName);
                }
            }
        }

        public string HealthProfessionalSurname
        {
            get { return HealthProfessional.Surname; }
            set { HealthProfessional.Surname = value; }
        }

        public string HealthProfessionalFirstName
        {
            get { return HealthProfessional.FirstName; }
            set { HealthProfessional.FirstName = value; }
        }

        public string HealthProfessionalJobTitle
        {
            get { return HealthProfessional.JobTitle; }
            set
            {
                if (!Equals(value, HealthProfessional.JobTitle))
                {
                    HealthProfessional.JobTitle = value;
                    RaisePropertyChanged(HealthProfessionalJobTitlePropertyName);
                }
            }
        }

        public string HealthProfessionalContactNumber
        {
            get { return HealthProfessional.ContactNumber; }
            set
            {
                if (!Equals(value, HealthProfessional.ContactNumber))
                {
                    HealthProfessional.ContactNumber = value;
                    RaisePropertyChanged(HealthProfessionalContactNumberPropertyName);
                }
            }
        }

        public PatientModel Patient
        {
            get { return _patient; }
            set
            {
                if (!Equals(value, _patient))
                {
                    _patient = value;
                    RaisePropertyChanged(PatientPropertyName);
                }
            }
        }

        public string PatientSurname
        {
            get { return Patient.Surname; }
            //set { Patient.Surname = value; }
            set // DEMO
            {
                if (!Equals(value, Patient.Surname))
                {
                    Patient.Surname = value;
                    RaisePropertyChanged(PatientSurnamePropertyName);
                }
            }
        }

        public string PatientFirstName
        {
            get { return Patient.FirstName; }
            //set { Patient.FirstName = value; }
            set // DEMO
            {
                if (!Equals(value, Patient.FirstName))
                {
                    Patient.FirstName = value;
                    RaisePropertyChanged(PatientFirstNamePropertyName);
                }
            }
        }

        public DateTime PatientDOB
        {
            get { return Patient.DOB; }
            set
            {
                if (!Equals(value, Patient.DOB))
                {
                    Patient.DOB = value;
                    RaisePropertyChanged(PatientDOBPropertyName);
                }
            }
        }

        public string PatientNHSNumber
        {
            get { return Patient.NHSNumber; }
            set
            {
                if (!Equals(value, Patient.NHSNumber))
                {
                    Patient.NHSNumber = value;
                    RaisePropertyChanged(PatientNHSNumberPropertyName);
                }
            }
        }

        public RepresentativeModel Representative
        {
            get { return _representative; }
            set
            {
                if (!Equals(value, _representative))
                {
                    _representative = value;
                    RaisePropertyChanged(RepresentativePropertyName);
                }
            }
        }

        public WitnessModel Witness
        {
            get { return _witnessModel; }
            set
            {
                if (!Equals(value, _witnessModel))
                {
                    _witnessModel = value;
                    RaisePropertyChanged(WitnessPropertyName);
                }
            }
        }

        public InterpreterModel Interpreter
        {
            get { return _interpreterModel; }
            set
            {
                if (!Equals(value, _interpreterModel))
                {
                    _interpreterModel = value;
                    RaisePropertyChanged(InterpreterPropertyName);
                }
            }
        }

        public string InterpreterSurname
        {
            get { return Interpreter.Surname; }
            set
            {
                if (!Equals(value, Interpreter.Surname))
                {
                    Interpreter.Surname = value;
                    RaisePropertyChanged(InterpreterSurnamePropertyName);
                }
            }
        }

        public string InterpreterFirstName
        {
            get { return Interpreter.FirstName; }
            set
            {
                if (!Equals(value, Interpreter.FirstName))
                {
                    Interpreter.FirstName = value;
                    RaisePropertyChanged(InterpreterFirstNamePropertyName);
                }
            }
        }

        public string InterpreterJobTitle
        {
            get { return Interpreter.JobTitle; }
            set
            {
                if (!Equals(value, Interpreter.JobTitle))
                {
                    Interpreter.JobTitle = value;
                    RaisePropertyChanged(InterpreterJobTitlePropertyName);
                }
            }
        }

        public string InterpreterRoleOrRelationship
        {
            get { return Interpreter.RoleOrRelationship; }
            set
            {
                if (!Equals(value, Interpreter.RoleOrRelationship))
                {
                    Interpreter.RoleOrRelationship = value;
                    RaisePropertyChanged(InterpreterRoleOrRelationshipPropertyName);
                }
            }
        }

        public bool InterpreterIsPresent
        {
            get { return Interpreter.IsPresent; }
            set
            {
                if (!Equals(value, Interpreter.IsPresent))
                {
                    Interpreter.IsPresent = value;
                    RaisePropertyChanged(InterpreterIsPresentPropertyName);
                }
            }
        }

        public ObservableCollection<ProcedureModel> Procedures { get; set; }

        public ObservableCollection<ProcedureModel> ProceduresSearchResult
        {
            get { return _proceduresSearchResult; }
            set
            {
                if (!Equals(value, _proceduresSearchResult))
                {
                    _proceduresSearchResult = value;
                    RaisePropertyChanged(ProceduresSearchResultPropertyName);
                }
            }
        }

        public ObservableCollection<ProcedureModel> ProceduresSelected
        {
            get { return _proceduresSelected; }
            set
            {
                if (!Equals(value, _proceduresSelected))
                {
                    _proceduresSelected = value;
                    RaisePropertyChanged(ProceduresSelectedPropertyName);
                }
            }
        }

        public ObservableCollection<AlternativeProcedureModel> AlternativeProcedures { get; set; }

        public ObservableCollection<AlternativeProcedureModel> AlternativeProceduresSearchResult
        {
            get { return _alternativeProceduresSearchResult; }
            set
            {
                if (!Equals(value, _alternativeProceduresSearchResult))
                {
                    _alternativeProceduresSearchResult = value;
                    RaisePropertyChanged(AlternativeProceduresSearchResultPropertyName);
                }
            }
        }

        public CapacityModel Capacity
        {
            get { return _capacity; }
            set
            {
                if (!Equals(value, _capacity))
                {
                    _capacity = value;
                    RaisePropertyChanged(CapacityPropertyName);
                }
            }
        }

        public bool PatientIsUnconscious
        {
            get { return Capacity.PatientIsUnconscious; }
            set
            {
                if (!Equals(value, Capacity.PatientIsUnconscious))
                {
                    Capacity.PatientIsUnconscious = value;
                    RaisePropertyChanged(IsPatientConsciousPropertyName);
                }
            }
        }

        public bool PatientHasASoundedMind
        {
            get { return Capacity.PatientHasASoundedMind; }
            set
            {
                if (!Equals(value, Capacity.PatientHasASoundedMind))
                {
                    Capacity.PatientHasASoundedMind = value;
                    RaisePropertyChanged(DoesPatientHaveASoundedMindPropertyName);
                }
            }
        }

        public bool PatientCanUnderstandInformationProvided
        {
            get { return Capacity.PatientCanUnderstandInformationProvided; }
            set
            {
                if (!Equals(value, Capacity.PatientCanUnderstandInformationProvided))
                {
                    Capacity.PatientCanUnderstandInformationProvided = value;
                    RaisePropertyChanged(DoesPatientUnderstandInformationProvidedPropertyName);
                }
            }
        }

        public bool PatientCanRetainInformationProvided
        {
            get { return Capacity.PatientCanRetainInformationProvided; }
            set
            {
                if (!Equals(value, Capacity.PatientCanRetainInformationProvided))
                {
                    Capacity.PatientCanRetainInformationProvided = value;
                    RaisePropertyChanged(IsPatientAbleToRetainInformationProvidedPropertyName);
                }
            }
        }

        public bool PatientCanUseInfromationProvided
        {
            get { return Capacity.PatientCanUseInfromationProvided; }
            set
            {
                if (!Equals(value, Capacity.PatientCanUseInfromationProvided))
                {
                    Capacity.PatientCanUseInfromationProvided = value;
                    RaisePropertyChanged(IsPatientAbleToUseInfromationProvidedPropertyName);
                }
            }
        }

        public bool PatientCanCommunicateDecision
        {
            get { return Capacity.PatientCanCommunicateDecision; }
            set
            {
                if (!Equals(value, Capacity.PatientCanCommunicateDecision))
                {
                    Capacity.PatientCanCommunicateDecision = value;
                    RaisePropertyChanged(IsPatientAbleToCommunicateDecisionPropertyName);
                }
            }
        }

        public bool PatientHasCapacityToDecide
        {
            get { return Capacity.PatientHasCapacityToDecide; }
            set
            {
                if (!Equals(value, Capacity.PatientHasCapacityToDecide))
                {
                    Capacity.PatientHasCapacityToDecide = value;
                    RaisePropertyChanged(DoesPatientLackCapacityToDecidePropertyName);
                }
            }
        }

        public bool PatientLacksOfCapacityPermanently
        {
            get { return Capacity.PatientLacksOfCapacityPermanently; }
            set
            {
                if (!Equals(value, Capacity.PatientLacksOfCapacityPermanently))
                {
                    Capacity.PatientLacksOfCapacityPermanently = value;
                    RaisePropertyChanged(IsLackOfCapacityPermanentPropertyName);
                }
            }
        }

        public bool DecisionCanWait
        {
            get { return Capacity.DecisionCanWait; }
            set
            {
                if (!Equals(value, Capacity.DecisionCanWait))
                {
                    Capacity.DecisionCanWait = value;
                    RaisePropertyChanged(CanTheDecisionWaitPropertyName);
                }
            }
        }

        public AssentModel Assent
        {
            get { return _assent; }
            set
            {
                if (!Equals(value, _assent))
                {
                    _assent = value;
                    RaisePropertyChanged(AssentPropertyName);
                }
            }
        }

        public bool AssentLPAAvailable
        {
            get { return Assent.AssentLPAAvailable; }
            set
            {
                if (!Equals(value, Assent.AssentLPAAvailable))
                {
                    Assent.AssentLPAAvailable = value;
                    RaisePropertyChanged(LPAIsAvailablePropertyName);
                }
            }
        }

        public bool AssentAdvanceDecisionInPlace
        {
            get { return Assent.AssentAdvanceDecisionInPlace; }
            set
            {
                if (!Equals(value, Assent.AssentAdvanceDecisionInPlace))
                {
                    Assent.AssentAdvanceDecisionInPlace = value;
                    RaisePropertyChanged(AdvanceDecisionIsInPlacePropertyName);
                }
            }
        }

        public bool AssentDOLSRequired
        {
            get { return Assent.AssentDOLSRequired; }
            set
            {
                if (!Equals(value, Assent.AssentDOLSRequired))
                {
                    Assent.AssentDOLSRequired = value;
                    RaisePropertyChanged(DOLSRequiredPropertyName);
                }
            }
        }

        public bool PatientHasFriendFamilyToConsult
        {
            get { return Assent.PatientHasFriendFamilyToConsult; }
            set
            {
                if (!Equals(value, Assent.PatientHasFriendFamilyToConsult))
                {
                    Assent.PatientHasFriendFamilyToConsult = value;
                    RaisePropertyChanged(PatientHasFriendFamilyToConsultPropertyName);
                }
            }
        }

        public bool PatientFriendFamilyWillingToBeConsulted
        {
            get { return Assent.PatientFriendFamilyWillingToBeConsulted; }
            set
            {
                if (!Equals(value, Assent.PatientFriendFamilyWillingToBeConsulted))
                {
                    Assent.PatientFriendFamilyWillingToBeConsulted = value;
                    RaisePropertyChanged(PatientFriendFamilyWillingToBeConsultedPropertyName);
                }
            }
        }

        public bool AssentIMCAInstructed
        {
            get { return Assent.AssentIMCAInstructed; }
            set
            {
                if (!Equals(value, Assent.AssentIMCAInstructed))
                {
                    Assent.AssentIMCAInstructed = value;
                    RaisePropertyChanged(IMCAHasBeenInstructedPropertyName);
                }
            }
        }

        public bool PatientHasExpressedAView
        {
            get { return Assent.PatientHasExpressedAView; }
            set
            {
                if (!Equals(value, Assent.PatientHasExpressedAView))
                {
                    Assent.PatientHasExpressedAView = value;
                    RaisePropertyChanged(PatientHasExpressedAViewPropertyName);
                }
            }
        }

        #endregion

        #region Commands

        public RelayCommand ConfirmQualification { get; private set; }
        public RelayCommand OpenProcedures { get; private set; }
        public RelayCommand OpenAlternativeProcedures { get; private set; }
        public RelayCommand OpenPanelCollection { get; private set; }
        public RelayCommand OpenGeneralConsent { get; private set; }
        public RelayCommand SectionPatientDetailsNext { get; private set; }
        public RelayCommand SectionProceduresDetailsNext { get; private set; }
        public RelayCommand SectionDeclarationDetailsNext { get; private set; }
        public RelayCommand SectionFirstConsentAdultNext { get; private set; }
        public RelayCommand SectionFirstConsentProxyNext { get; private set; }
        public RelayCommand SectionFinalConsentNext { get; private set; }
        public RelayCommand SectionCapacityTestNext { get; private set; }
        public RelayCommand SectionAssentDetailsNext { get; private set; }
        public RelayCommand FlyoutDemoInfo16OrOver { get; private set; }
        public RelayCommand FlyoutDemoInfoBelow16 { get; private set; }
        public RelayCommand FlyoutLanguageAssessmentYes { get; private set; }
        public RelayCommand FlyoutLanguageAssessmentNo { get; private set; }
        public RelayCommand FlyoutInterpreterDetailsSave { get; private set; }
        public RelayCommand FlyoutPatientCapacityYes { get; private set; }
        public RelayCommand FlyoutPatientCapacityNo { get; private set; }
        public RelayCommand FlyoutConsentStageNormal { get; private set; }
        public RelayCommand FlyoutConsentStageEmergency { get; private set; }
        public RelayCommand FlyoutAgeOverthrownYes { get; private set; }
        public RelayCommand FlyoutAgeOverthrownNo { get; private set; }
        public RelayCommand<TextChangedEventArgs> SearchProcedure { get; private set; }
        public RelayCommand<TextChangedEventArgs> SearchAlternativeProcedure { get; private set; }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            Patient = GetPatient();
            HealthProfessional = GetHealthProfessional();
            Witness = new WitnessModel();
            Interpreter = new InterpreterModel();
            Procedures = GetProcedures();
            ProceduresSearchResult = GetProcedures();
            ProceduresSelected = new ObservableCollection<ProcedureModel>();
            AlternativeProcedures = GetAlternativeProcedures();
            AlternativeProceduresSearchResult = GetAlternativeProcedures();
            Capacity = new CapacityModel();
            Assent = new AssentModel();

            StatusBar = GetStatusBar();

            var timer = new Timer { Interval = 1000 };
            timer.Elapsed += (sender, args) => RaisePropertyChanged(CurrentDateTimePropertyName);
            timer.Start();

            MessengerSender();

            MessengerRegister();
        }

        #endregion

        #region Private Methods

        private PatientModel GetPatient()
        {
            return new PatientModel
            {
                PersonId = new Guid(),
                NHSNumber = "P19720101",
                FirstName = "Mike",
                Surname = "Wellburn",
                DOB = new DateTime(1972, 1, 1),
            };
        }

        private ClinicianModel GetHealthProfessional()
        {
            return new ClinicianModel
            {
                PersonId = new Guid(),
                Surname = "Hugh",
                FirstName = "Peter",
                JobTitle = "Dentist"
            };
        }

        private ObservableCollection<ProcedureModel> GetProcedures()
        {
            var procedures = new ObservableCollection<ProcedureModel>();
            for (var i = 0; i < 10; i++)
            {
                procedures.Add(new ProcedureModel { ProcedureId = i, Name = String.Format("{0} {1}", "Procedure", i), Benefits = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.", Risks = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum."});
            }
            return procedures;
        }

        private ObservableCollection<AlternativeProcedureModel> GetAlternativeProcedures()
        {
            var alternativeProcedures = new ObservableCollection<AlternativeProcedureModel>();
            for (var i = 0; i < 10; i++)
            {
                alternativeProcedures.Add(new AlternativeProcedureModel { ProcedureId = i, Name = String.Format("{0} {1}", "Alternative", i), Benefits = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.", Risks = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum." });
            }
            return alternativeProcedures;
        }

        private string GetStatusBar(bool displayLanguage = false, bool understandsEnglish = false, bool displayCapacity = false, bool hasCapacity = false)
        {
            var result = String.Format("Hello, {0} {1}. You are currently consenting for {2} {3}, aged {4}",
                                       HealthProfessionalFirstName,
                                       HealthProfessionalSurname,
                                       PatientFirstName,
                                       PatientSurname,
                                       (DateTime.Now.Year - PatientDOB.Year).ToString());

            if (displayLanguage && displayCapacity)
            {
                if (understandsEnglish && hasCapacity)
                {
                    return result + ", whom understands English, shown to have capacity";
                }
                else if (!understandsEnglish && hasCapacity)
                {
                    return result + ", whom doesn't understand English, shown to have capacity";
                }
                else if (understandsEnglish && !hasCapacity)
                {
                    return result + ", whom understands English, NOT shown to have capacity";
                }
                else if (!understandsEnglish && !hasCapacity)
                {
                    return result + ", whom doesn't understand English, NOT shown to have capacity";
                }
            }
            else if (!displayLanguage && displayCapacity)
            {
                if (hasCapacity)
                {
                    return result + ", whom shown to have capacity";
                }
                else if (understandsEnglish)
                {
                    return result + ", whom NOT shown to have capacity";
                }
            }
            else if (displayLanguage && !displayCapacity)
            {
                if (understandsEnglish)
                {
                    return result + ", whom understands English";
                }
                else if (!understandsEnglish)
                {
                    return result + ", whom doesn't understand English";
                }
            }

            return result;
        }

        private bool GoToAssent()
        {
            if (PatientIsUnconscious)
            {
                return true;
            }

            if (!PatientHasCapacityToDecide && PatientLacksOfCapacityPermanently)
            {
                return true;
            }

            if (!PatientHasCapacityToDecide && !PatientLacksOfCapacityPermanently && !DecisionCanWait)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Messenger Sender

        private void MessengerSender()
        {
            ConfirmQualification =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "ConfirmQualification")));
            OpenProcedures =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "OpenProcedures")));
            OpenAlternativeProcedures =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "OpenAlternativeProcedures")));
            OpenPanelCollection =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "OpenPanelCollection")));
            OpenGeneralConsent =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "OpenGeneralConsent")));
            SectionPatientDetailsNext =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "SectionPatientDetailsNext")));
            SectionProceduresDetailsNext =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "SectionProceduresDetailsNext")));
            SectionDeclarationDetailsNext =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "SectionDeclarationDetailsNext")));
            SectionFirstConsentAdultNext =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "SectionFirstConsentAdultNext")));
            SectionFirstConsentProxyNext =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "SectionFirstConsentProxyNext")));
            SectionFinalConsentNext =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "SectionFinalConsentNext")));
            SectionCapacityTestNext =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage<bool>(GoToAssent(), "SectionCapacityTestNext")));
            SectionAssentDetailsNext =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "SectionAssentDetailsNext")));
            FlyoutDemoInfo16OrOver =
                new RelayCommand(() =>
                    {
                        PatientFirstName = "Mike";
                        PatientSurname = "Wellburn";
                        PatientDOB = new DateTime(1972, 1, 1);
                        PatientNHSNumber = "P19720101";
                        StatusBar = GetStatusBar();
                        Messenger.Default.Send(new NotificationMessage(this, "FlyoutDemoInfo16OrOver"));
                    });
            FlyoutDemoInfoBelow16 =
                new RelayCommand(() =>
                    {
                        PatientFirstName = "Joseph";
                        PatientSurname = "Taylor";
                        PatientDOB = new DateTime(2000, 1, 1);
                        StatusBar = GetStatusBar();
                        Messenger.Default.Send(new NotificationMessage(this, "FlyoutDemoInfoBelow16"));
                    });
            FlyoutLanguageAssessmentYes =
                new RelayCommand(() =>
                    {
                        StatusBar = GetStatusBar(true, true);
                        Messenger.Default.Send(new NotificationMessage(this, "FlyoutLanguageAssessmentYes"));
                    });
            FlyoutLanguageAssessmentNo =
                new RelayCommand(() =>
                    {
                        StatusBar = GetStatusBar(true, false);
                        Messenger.Default.Send(new NotificationMessage(this, "FlyoutLanguageAssessmentNo"));
                    });
            FlyoutInterpreterDetailsSave =
                new RelayCommand(() =>
                    {
                        if (InterpreterIsPresent)
                        {
                            Messenger.Default.Send(new NotificationMessage(this, "FlyoutInterpreterDetailsSaveInterpreterIsPresent"));
                        }
                        else
                        {
                            Messenger.Default.Send(new NotificationMessage(this, "FlyoutInterpreterDetailsSaveInterpreterIsNotPresent"));
                        }
                    });
            FlyoutPatientCapacityYes =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "FlyoutPatientCapacityYes")));
            FlyoutPatientCapacityNo =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "FlyoutPatientCapacityNo")));
            FlyoutConsentStageNormal =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "FlyoutConsentStageNormal")));
            FlyoutConsentStageEmergency =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "FlyoutConsentStageEmergency")));
            FlyoutAgeOverthrownYes =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "FlyoutAgeOverthrownYes")));
            FlyoutAgeOverthrownNo =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "FlyoutAgeOverthrownNo")));
            SearchProcedure =
                new RelayCommand<TextChangedEventArgs>(e =>
                {
                    if (e == null || e.Source == null)
                    {
                        return;
                    }
                    var txtProcedureSearch = e.Source as TextBox;
                    if (txtProcedureSearch == null)
                    {
                        return;
                    }
                    ProceduresSearchResult.Clear();
                    foreach (var setup in Procedures.Where(s => s.Name.ToLower().Contains(txtProcedureSearch.Text.ToLower()))) // DEMO
                    {
                        ProceduresSearchResult.Add(setup);
                    }
                });
            SearchAlternativeProcedure =
                new RelayCommand<TextChangedEventArgs>(e =>
                {
                    if (e == null || e.Source == null)
                    {
                        return;
                    }
                    var txtAlternativeProcedureSearch = e.Source as TextBox;
                    if (txtAlternativeProcedureSearch == null)
                    {
                        return;
                    }
                    AlternativeProceduresSearchResult.Clear();
                    foreach (var setup in AlternativeProcedures.Where(s => s.Name.ToLower().Contains(txtAlternativeProcedureSearch.Text.ToLower()))) // DEMO
                    {
                        AlternativeProceduresSearchResult.Add(setup);
                    }
                });
        }

        #endregion

        #region Messenger Register

        private void MessengerRegister()
        {
            Messenger.Default.Register<NotificationMessage<int>>(this, message =>
            {
                if (message.Notification.Equals("DeleteProcedure"))
                {
                    var item = ProceduresSelected.First(p => p.ProcedureId == message.Content);
                    ProceduresSelected.Remove(item);
                }

                if (message.Notification.Equals("DeleteAlternativeProcedure"))
                {
                    foreach (var procedure in ProceduresSelected.ToList())
                    {
                        foreach (var alternativeProcedure in procedure.AlternativeProcedures.ToList())
                        {
                            if (alternativeProcedure.ProcedureId == message.Content)
                            {
                                procedure.AlternativeProcedures.Remove(alternativeProcedure);
                            }
                        }
                    }
                }
            });
        }

        #endregion

        #region Validation

        public string Error { get; private set; }

        public string this[string columnName]
        {
            get
            {
                if (columnName == HealthProfessionalContactNumberPropertyName)
                {
                    return TelephoneNumberValidator(HealthProfessionalContactNumber);
                }
                if (columnName == InterpreterSurnamePropertyName)
                {
                    return RequiredFieldValidator(InterpreterSurname) ?? LettersAndSpaceValidator(InterpreterSurname);
                }
                if (columnName == InterpreterFirstNamePropertyName)
                {
                    return RequiredFieldValidator(InterpreterFirstName) ?? LettersAndSpaceValidator(InterpreterFirstName);
                }
                if (columnName == InterpreterJobTitlePropertyName)
                {
                    return RequiredFieldValidator(InterpreterJobTitle) ?? LettersAndSpaceValidator(InterpreterJobTitle);
                }
                if (columnName == InterpreterRoleOrRelationshipPropertyName)
                {
                    return RequiredFieldValidator(InterpreterRoleOrRelationship) ?? LettersAndSpaceValidator(InterpreterRoleOrRelationship);
                }
                return null;
            }
        }

        private string RequiredFieldValidator(string input)
        {
            return string.IsNullOrEmpty(input) ? Strings.ValidationMessage_RequiredField : null;
        }

        private string TelephoneNumberValidator(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return !Regex.IsMatch(input, TelephoneNumberValidationRegex) ? Strings.ValidationMessage_TelephoneNumberValidation : null;
            }
            return null;
        }

        private string LettersAndSpaceValidator(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return !Regex.IsMatch(input, LettersAndSpaceValidationRegex) ? Strings.ValidationMessage_LettersAndSpaceValidation : null;
            }
            return null;
        }

        #endregion
    }
}