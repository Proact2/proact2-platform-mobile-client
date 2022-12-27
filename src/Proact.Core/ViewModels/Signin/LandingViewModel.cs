using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using Proact.Mobile.Core.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class LandingViewModel : BaseViewModel<bool>, ILandingViewModel {

        public IMvxCommand ContinueButtonCommand { get; private set; }
       
        private Style _ButtonStyle;
        public Style ButtonStyle {
            get => _ButtonStyle;
            set => SetProperty( ref _ButtonStyle, value );
        }

        private string _proactAppVersion;
        public string ProactAppVersion {
            get => _proactAppVersion;
            set => SetProperty( ref _proactAppVersion, value );
        }

        public bool OpenAfterLogout { get; private set; }

        private ISigninService _signinService;
        private ILocalDataReadService _localDataReadService;
        private ILocalDataWriteService _localDataWriteService;
        private IInstituteService _instituteService;
        private IProjectPropertiesService _projectPropertiesService;

        private bool _autologinIsEnable = true;
     
        private bool _openTermsAndConditionsPopup;
        private InstituteModel _instituteModel;

        public LandingViewModel(
            ISigninService signinService,
            ILocalDataReadService localDataReadService,
            ILocalDataWriteService localDataWriteService,
            IInstituteService instituteService,
            IProjectPropertiesService projectPropertiesService ) {

            _localDataReadService = localDataReadService;
            _localDataWriteService = localDataWriteService;
            _signinService = signinService;
            _instituteService = instituteService;
            _projectPropertiesService = projectPropertiesService;
            _signinService.Init();
        }

        public override void Prepare( bool openAfterLogout ) {
            base.Prepare();
            InitUICommands();
            UpdateUIWithAppVersion();
            LoadingBusy = false;
            OpenAfterLogout = openAfterLogout;
        }

        private void InitUICommands() {
            ContinueButtonCommand
                = new MvxCommand( ContinueButtonActionHandle );
        }

        private async void ContinueButtonActionHandle() {
            await PerformAzureActiveDirectorySignin();
        }

        private async Task PerformAzureActiveDirectorySignin() {
            if ( IsBusy ) {
                return;
            }

            await _signinService.PerformSignout();
            await _signinService.PerformAuthenticationAsync();

            LoadingBusy = true;
            var AuthDataModel = await _signinService
                .PerformSigninAsync();

            if ( AuthDataModel.Authorized ) {
                if ( AuthDataModel.userData
                    .UserIsInRole( UserRolesModel.Patient ) ) {
                    PatientFirstAccess();
                }
                else if ( AuthDataModel.userData
                    .UserIsInRole( UserRolesModel.MedicalProfessional ) ) {
                    MedicalProfessionalFirstAccess();
                }
                else if ( AuthDataModel.userData
                    .UserIsInRole( UserRolesModel.Nurse ) ) {
                    NurseFirstAccess();
                }
                else if ( AuthDataModel.userData
                   .UserIsInRole( UserRolesModel.Researcher ) ) {
                    ResearcherFirstAccess();
                }
            }
            else {
                LoadingBusy = false;
            }
        }

        private async void PerformSilentAuthentication() {
            try {
                if ( _autologinIsEnable ) {
                    LoadingBusy = true;
                    var AuthDataModel = await _signinService
                        .PerformAutoSigninAsync();

                    if ( AuthDataModel.Authorized ) {
                        await _signinService.UpdateLocalProjectAndMedicalTeamData();
                        LoadingBusy = false;
                     
                        if ( IsPatient() ) {
                            await OpenMainPage();
                        }
                        else {
                            if ( MedicalProfessionalFirstAccessCompleted() ) {
                                await OpenMainPage();
                            }
                            else {
                                await _navigationService.Navigate<ProjectsListViewModel>();
                            }
                        }
                    }
                    else {
                        LoadingBusy = false;
                    }
                }
            }
            catch ( Exception e) {
                Console.WriteLine( e.Message );
                LoadingBusy = false;
            }
        }

        public override void ViewAppeared() {
            base.ViewAppeared();
            _signinService
                .OnSigninErrorError += OnAuthenticationError;

            if ( _openTermsAndConditionsPopup ) {
                OpenConditionsPopup();
            }
            else if ( _localDataReadService.IsSigninConditionsAccepted() ) {
                PerformSilentAuthentication();
            }
        }

        public override void ViewDisappearing() {
            base.ViewDisappearing();
            _signinService
                .OnSigninErrorError -= OnAuthenticationError;
        }

        private async void PatientFirstAccess() {
            var result = await _instituteService.GetMyInstitute();
            if ( result.Success ) {
                _instituteModel = result.data;
                if ( _instituteModel.State == InstituteState.Open ) {
                    OpenConditionsPopup();
                }
                else {
                    ShowUserCantAccessAlert();
                }
            }
            else {
                ShowUserCantAccessAlert();
            }
        }

        private async void PatientAccessAfterConditionsAccepted() {
            await _signinService.RetrieveAndSavePatientProjectAndMedicalTeamData();
            LoadingBusy = false;

            _localDataWriteService.SetSigninConditionsAccepted( true );
            await OpenMainPage();
        }

        private async void MedicalProfessionalFirstAccess() {
            LoadingBusy = false;
            _localDataWriteService.SetSigninConditionsAccepted( true );
            await _navigationService.Navigate<ProjectsListViewModel>();
        }

        private async void NurseFirstAccess() {
            LoadingBusy = false;
            _localDataWriteService.SetSigninConditionsAccepted( true );
            await _navigationService.Navigate<ProjectsListViewModel>();
        }

        private async void ResearcherFirstAccess() {
            LoadingBusy = false;
            _localDataWriteService.SetSigninConditionsAccepted( true );
            await _navigationService.Navigate<ProjectsListViewModel>();
        }

        private void OnAuthenticationError( object sender, ErrorModel error ) {
            LoadingBusy = false;
            if ( error.Code == "authentication_canceled" ) {
                return;
            }

            string errorMessage = Resources.AppResources.AuthenticationError + error.Message;
            ShowAddMessageErrorPopup( errorMessage );
        }

        private bool LoadingBusy {
            set {
                IsBusy = value;
                if ( value ) {
                    ButtonStyle = ( Style )Application.Current
                        .Resources["LandingButtonDisabledStyle"];
                }
                else {
                    ButtonStyle = ( Style )Application.Current
                      .Resources["LandingButtonStyle"];
                }
            }
        }

        private bool IsPatient() {
            var userData = _localDataReadService.GetUserData();
            return userData.UserIsInRole( UserRolesModel.Patient );
        }

        private bool MedicalProfessionalFirstAccessCompleted() {
            var project = _localDataReadService.GetProjectModel();
            var medicalTeam = _localDataReadService.GetMedicalTeamModel();

            return ( project != null && medicalTeam != null );
        }

        private void OpenConditionsPopup() {
            if ( !ValidateConditionsAndPolicy() ) {
                IsBusy = false;
                LoadingBusy = false;
                return;
            }
            _openTermsAndConditionsPopup = true;
            _popupService.OpenSigninConditionsPopup( this );
        }

        private bool ValidateConditionsAndPolicy() {
          
            if( GetTermsAndConditionsModel() != null
                && GetPrivacyPolicyModel() != null ) {

                return true;
            }
            else {
                ShowAddMessageErrorPopup(
                    Resources.AppResources.LoginDocumentsUnavaliable );
                return false;
            }
        }


        public async void OpenTermsAndConditionsPage() {
            await _popupService.CloseAllPopup();
            var model = new DocumentViewerModel() {
                Title = _instituteModel.Properties.TermsAndConditions.Title,
                Url = _instituteModel.Properties.TermsAndConditions.Url
            };

            await _navigationService
                .Navigate<DocumentViewerViewModel, DocumentViewerModel>( model );
        }

        public async void OpenLicensePage() {
            await _popupService.CloseAllPopup();
            var model = new DocumentViewerModel() {
                Title = _instituteModel.Properties.PrivacyPolicy.Title,
                Url = _instituteModel.Properties.PrivacyPolicy.Url
            };

            await _navigationService
                .Navigate<DocumentViewerViewModel, DocumentViewerModel>( model );
        }

        public async void OpenPrivacyPolicyPage() {
            await _popupService.CloseAllPopup();
            var model = new DocumentViewerModel() {
                Title = _instituteModel.Properties.PrivacyPolicy.Title,
                Url = _instituteModel.Properties.PrivacyPolicy.Url
            };

            await _navigationService
                .Navigate<DocumentViewerViewModel, DocumentViewerModel>( model );
        }

        public void TermsAndConditionAcceptedActionHandle() {
            _popupService.OpenUrgentMedicalAdvicePopup( this );
        }

        public async void UrgentMedicalAdviceAcceptedActionHandle() {
            _openTermsAndConditionsPopup = false;
            await _popupService.CloseAllPopup();
            //await PerformAzureActiveDirectorySignin();
            PatientAccessAfterConditionsAccepted();
        }

        private void ShowAddMessageErrorPopup( string error ) {
            PopupService.OpenMessagePopup( new PopupMessageModel() {
                Type = PopupMessageType.ERROR,
                MessageText = error
            } );
        }

        private void ShowUserCantAccessAlert() {
            var message = new PopupMessageModel() {
                Type = PopupMessageType.ERROR,
                MessageText = Resources.AppResources.UserCantSignin
            };

            PopupService.OpenMessagePopup( message );
        }

        public DocumentModel GetTermsAndConditionsModel() {
            try {
                return _instituteModel.Properties.TermsAndConditions;
            }
            catch ( Exception ) { }
            return null;
        }

        public DocumentModel GetPrivacyPolicyModel() {
            try {
                return _instituteModel.Properties.PrivacyPolicy;
            }
            catch ( Exception ) { }
            return null;
        }

        private void UpdateUIWithAppVersion() {
            ProactAppVersion = string.Format(
                Resources.AppResources.AppVersion,
                VersionTracking.CurrentVersion,
                VersionTracking.CurrentBuild );
        }

        private async Task OpenMainPage() {
            var isMessagingActive = _projectPropertiesService.IsMessagingActive();

            if ( isMessagingActive ) {
                await _navigationService.Navigate<WallMessagesViewModel>();
            }
            else {
                await _navigationService.Navigate<ProfileViewModel>();
            }
        }
    }
}