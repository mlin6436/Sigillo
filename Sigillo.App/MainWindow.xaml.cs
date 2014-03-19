using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sigillo.App.Common;
using MahApps.Metro;
using Sigillo.App.Models;
using Sigillo.App.UserControls;

namespace Sigillo.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region Members

        private const string ThemeColourBlue = "Blue";
        private const string ThemeColourRed = "Red";
        private const string ThemeColourGreen = "Green";
        private const string ThemeColourPurple = "Purple";
        private const string ThemeColourOrange = "Orange";
        private const Theme ThemeBrightnessLight = Theme.Light;
        private const Theme ThemeBrightnessDark = Theme.Dark;

        private bool _isQualifiedToTakeConsent;
        private bool _hasCapacity;
        private bool _agedSixteenOrOver;
        private bool _consentByParants;
        private bool _understandEnglish;
        private bool _isSingleStageConsent;
        private bool _interpreterIsPresent;

        #endregion

        #region Constructor

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                GetTheme(ThemeColourBlue, ThemeBrightnessLight);

                _hasCapacity = true;
                _understandEnglish = true;
                _consentByParants = false;
                _isSingleStageConsent = false;
                _interpreterIsPresent = true;

                SectionWorkingArea.HideAllTabItems();
                SectionPatientDetails.ShowTabItem();

                MessengerRegister();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }
        }

        #endregion

        #region Private Methods

        private void GetTheme(string themeColour, Theme theme)
        {
            ThemeManager.ChangeTheme(this, ThemeManager.DefaultAccents.First(x => x.Name.Equals(themeColour)), theme);
        }

        #endregion

        #region Messenger Register

        private void MessengerRegister()
        {
            Messenger.Default.Register<NotificationMessage<bool>>(this, message =>
            {
                if (message.Notification.Equals("SectionCapacityTestNext"))
                {
                    SectionCapacityTestNext(message.Content);
                    return;
                }
            });

            Messenger.Default.Register<NotificationMessage>(this, message =>
            {
                // Main
                if (message.Notification.Equals("ConfirmQualification"))
                {
                    PanelQualification.FadeOutPanel(0);
                    PanelQualificationOverlay.FadeOutPanel(0);
                    FlyoutDemoInfo.ChangeFlyoutStatus();  // DEMO
                    return;
                }
                if (message.Notification.Equals("OpenProcedures"))
                {
                    FlyoutProcedureSearch.ChangeFlyoutStatus();
                    return;
                }
                if (message.Notification.Equals("OpenAlternativeProcedures"))
                {
                    FlyoutAlternativeProcedureSearch.ChangeFlyoutStatus();
                    return;
                }
                if (message.Notification.Equals("OpenPanelCollection"))
                {
                    PanelCollection.FadeInPanel(1);
                    PanelCollectionOverlay.FadeInPanel(0.5);
                    return;
                }
                if (message.Notification.Equals("OpenGeneralConsent"))
                {
                    PanelCollection.FadeOutPanel();
                    PanelCollectionOverlay.FadeOutPanel();
                    PanelQualification.FadeInPanel(1);
                    PanelQualificationOverlay.FadeInPanel(0.5);
                    return;
                }
                if (message.Notification.Equals("SectionPatientDetailsNext"))
                {
                    SectionPatientDetailsNext();
                    return;
                }
                if (message.Notification.Equals("SectionProceduresDetailsNext"))
                {
                    SectionProceduresDetailsNext();
                    return;
                }
                if (message.Notification.Equals("SectionDeclarationDetailsNext"))
                {
                    SectionDeclarationDetailsNext();
                    return;
                }
                if (message.Notification.Equals("SectionFirstConsentAdultNext"))
                {
                    SectionFirstConsentAdultNext();
                    return;
                }
                if (message.Notification.Equals("SectionFirstConsentProxyNext"))
                {
                    SectionFirstConsentAdultNext();
                    return;
                }
                if (message.Notification.Equals("SectionAssentDetailsNext"))
                {
                    SectionAssentDetailsNext();
                    return;
                }
                if (message.Notification.Equals("FlyoutDemoInfo16OrOver"))
                {
                    FlyoutDemoInfoNext(true);
                }
                if (message.Notification.Equals("FlyoutDemoInfoBelow16"))
                {
                    FlyoutDemoInfoNext(false);
                }
                if (message.Notification.Equals("FlyoutLanguageAssessmentYes"))
                {
                    FlyoutLanguageAssessmentNext(true);
                    return;
                }
                if (message.Notification.Equals("FlyoutLanguageAssessmentNo"))
                {
                    FlyoutLanguageAssessmentNext(false);
                    return;
                }
                if (message.Notification.Equals("FlyoutInterpreterDetailsSaveInterpreterIsPresent"))
                {
                    FlyoutInterpreterDetailsNext(true);
                    return;
                }
                if (message.Notification.Equals("FlyoutInterpreterDetailsSaveInterpreterIsNotPresent"))
                {
                    FlyoutInterpreterDetailsNext();
                    return;
                }
                if (message.Notification.Equals("FlyoutPatientCapacityYes"))
                {
                    FlyoutCapacityTestNext(true);
                    return;
                }
                if (message.Notification.Equals("FlyoutPatientCapacityNo"))
                {
                    FlyoutCapacityTestNext(false);
                    return;
                }
                if (message.Notification.Equals("FlyoutConsentStageNormal"))
                {
                    FlyoutConsentStageNext(false);
                    return;
                }
                if (message.Notification.Equals("FlyoutConsentStageEmergency"))
                {
                    FlyoutConsentStageNext(true);
                    return;
                }
                if (message.Notification.Equals("FlyoutAgeOverthrownYes"))
                {
                    FlyoutAgeOfConsentNext(false);
                    return;
                }
                if (message.Notification.Equals("FlyoutAgeOverthrownNo"))
                {
                    FlyoutAgeOfConsentNext(true);
                    return;
                }
                if (message.Notification.Equals("AddNewRelativeAndFriendInfo"))
                {
                    var userControl = new RelativeAndFriendUserControl();
                    StackPanelRelativeAndFriend.Children.Add(userControl);
                }
                if (message.Notification.Equals("RemoveRelativeAndFriendInfo"))
                {
                }
            });
        }

        private void SectionPatientDetailsNext()
        {
            FlyoutLanguageAssessment.ChangeFlyoutStatus();
        }

        private void SectionProceduresDetailsNext()
        {
            if (_agedSixteenOrOver)
            {
                FlyoutPatientCapacity.ChangeFlyoutStatus();
            }
            else
            {
                FlyoutAgeOfConsent.ChangeFlyoutStatus();
            }
        }

        private void SectionCapacityTestNext(bool goToAssent)
        {
            if (goToAssent)
            {
                SectionWorkingArea.HideAllTabItems();
                SectionPatientDetails.ShowTabItem();
                SectionProceduresDetails.ShowTabItem();
                SectionCapacityTest.ShowTabItem();
                SectionAssentDetails.ShowTabItem();
            }
            else
            {
                SectionWorkingArea.HideAllTabItems();
                SectionPatientDetails.ShowTabItem();
                SectionProceduresDetails.ShowTabItem();
                SectionCapacityTest.ShowTabItem();
                SectionDeclarationDetails.ShowTabItem();
            }
        }

        private void SectionDeclarationDetailsNext()
        {
            FlyoutConsentStage.ChangeFlyoutStatus();
        }

        private void SectionFirstConsentAdultNext()
        {
            SectionFinalConsent.ShowTabItem();
        }

        private void SectionAssentDetailsNext()
        {
            SectionAssentDecision.ShowTabItem();
        }

        private void FlyoutDemoInfoNext(bool agedSixteenOrOver)
        {
            _agedSixteenOrOver = agedSixteenOrOver;
            FlyoutDemoInfo.ChangeFlyoutStatus();
            SectionWorkingArea.HideAllTabItems();
            SectionPatientDetails.ShowTabItem();
        }

        private void FlyoutLanguageAssessmentNext(bool requireInterpreter)
        {
            _understandEnglish = requireInterpreter;
            FlyoutLanguageAssessment.ChangeFlyoutStatus();
            if (_understandEnglish)
            {
                SectionProceduresDetails.ShowTabItem();
            }
            else
            {
                FlyoutInterpreterDetails.ChangeFlyoutStatus();
            }
        }

        private void FlyoutInterpreterDetailsNext(bool interpreterIsPresent = false)
        {
            _interpreterIsPresent = interpreterIsPresent;
            FlyoutInterpreterDetails.ChangeFlyoutStatus();
            SectionProceduresDetails.ShowTabItem();
        }

        private void FlyoutCapacityTestNext(bool hasCapacity)
        {
            _hasCapacity = hasCapacity;
            FlyoutPatientCapacity.ChangeFlyoutStatus();
            SectionWorkingArea.HideAllTabItems();
            SectionPatientDetails.ShowTabItem();
            SectionProceduresDetails.ShowTabItem();
            if (_hasCapacity)
            {
                SectionDeclarationDetails.ShowTabItem();
            }
            else
            {
                SectionCapacityTest.ShowTabItem();
            }
        }

        private void FlyoutAgeOfConsentNext(bool consentByParants)
        {
            _consentByParants = consentByParants;
            FlyoutAgeOfConsent.ChangeFlyoutStatus();
            if (_consentByParants)
            {
                SectionDeclarationDetails.ShowTabItem();
            }
            else
            {
                SectionCapacityTest.ShowTabItem();
            }
        }

        private void FlyoutConsentStageNext(bool isSingleStageConsent)
        {
            _isSingleStageConsent = isSingleStageConsent;
            FlyoutConsentStage.ChangeFlyoutStatus();
            SectionFirstConsentAdult.HideTabItem();
            SectionFirstConsentAdultWitnessSignature.Visibility = Visibility.Collapsed;
            SectionFirstConsentAdultInterpreterSignature.Visibility = Visibility.Collapsed;
            SectionFirstConsentProxy.HideTabItem();
            SectionFirstConsentProxyInterpreterSignature.Visibility = Visibility.Collapsed;
            SectionFinalConsent.HideTabItem();

            if (_isSingleStageConsent)
            {
                SectionFinalConsent.ShowTabItem();
                if (!_understandEnglish && _interpreterIsPresent)
                {
                    SectionFinalConsentInterpreterSignature.Visibility = Visibility.Visible;
                }
                return;
            }

            if (!_isSingleStageConsent && _agedSixteenOrOver)
            {
                SectionFirstConsentAdult.ShowTabItem();
                if (!_understandEnglish && _interpreterIsPresent)
                {
                    SectionFirstConsentAdultInterpreterSignature.Visibility = Visibility.Visible;
                }
                return;
            }

            if (!_isSingleStageConsent && !_agedSixteenOrOver && _consentByParants)
            {
                SectionFirstConsentProxy.ShowTabItem();
                if (!_understandEnglish && _interpreterIsPresent)
                {
                    SectionFirstConsentProxyInterpreterSignature.Visibility = Visibility.Visible;
                }
                return;
            }

            if (!_isSingleStageConsent && !_agedSixteenOrOver && !_consentByParants)
            {
                SectionFirstConsentAdult.ShowTabItem();
                SectionFirstConsentAdultWitnessSignature.Visibility = Visibility.Visible;
                if (!_understandEnglish && _interpreterIsPresent)
                {
                    SectionFirstConsentAdultInterpreterSignature.Visibility = Visibility.Visible;
                }
                return;
            }
        }

        #endregion

        #region Events

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = ((TreeView)sender).SelectedItem;

            if (selectedItem == null)
            {
                ProcedureSelectedName.Text = "Procedure Name";
                ProcedureSelectedDescription.Text = String.Empty;
                ProcedureSelectedBenefits.Text = String.Empty;
                ProcedureSelectedRisks.Text = String.Empty;
                ProcedureSelectedFollowup.Text = String.Empty;
                return;
            }

            if (selectedItem.GetType().Name.Equals((typeof(ProcedureModel)).Name))
            {
                ProcedureSelectedName.Text = ((ProcedureModel)selectedItem).Name;
                ProcedureSelectedDescription.Text = ((ProcedureModel)selectedItem).Description;
                ProcedureSelectedBenefits.Text = ((ProcedureModel)selectedItem).Benefits;
                ProcedureSelectedRisks.Text = ((ProcedureModel)selectedItem).Risks;
                ProcedureSelectedFollowup.Text = ((ProcedureModel)selectedItem).Followup;
                return;
            }

            if (selectedItem.GetType().Name.Equals((typeof(AlternativeProcedureModel)).Name))
            {
                ProcedureSelectedName.Text = ((AlternativeProcedureModel)selectedItem).Name;
                ProcedureSelectedDescription.Text = ((AlternativeProcedureModel)selectedItem).Description;
                ProcedureSelectedBenefits.Text = ((AlternativeProcedureModel)selectedItem).Benefits;
                ProcedureSelectedRisks.Text = ((AlternativeProcedureModel)selectedItem).Risks;
                ProcedureSelectedFollowup.Text = ((AlternativeProcedureModel)selectedItem).Followup;
                return;
            }
        }

        private void TreeView_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = (TreeView) sender;
            var selectedItem = treeViewItem.SelectedItem;
            var optionMenu = new ContextMenu();

            // TODO: list overload when adding more procedures
            if (selectedItem.GetType().Name.Equals((typeof(ProcedureModel)).Name))
            {
                var item = new MenuItem {Header = String.Format("Delete - {0}", ((ProcedureModel) selectedItem).Name)};
                item.Click += (o, args) => Messenger.Default.Send(new NotificationMessage<int>(((ProcedureModel) selectedItem).ProcedureId, "DeleteProcedure"));
                optionMenu.Items.Add(item);
            }

            if (selectedItem.GetType().Name.Equals((typeof(AlternativeProcedureModel)).Name))
            {
                var item = new MenuItem {Header = String.Format("Delete - {0}", ((AlternativeProcedureModel) selectedItem).Name)};
                item.Click += (o, args) => Messenger.Default.Send(new NotificationMessage<int>(((AlternativeProcedureModel) selectedItem).ProcedureId, "DeleteAlternativeProcedure"));
                optionMenu.Items.Add(item);
            }

            treeViewItem.ContextMenu = optionMenu;
        }

        #endregion
    }
}
