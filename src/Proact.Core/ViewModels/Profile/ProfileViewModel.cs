using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross;
using MvvmCross.Commands;
using Plugin.Media.Abstractions;
using Proact.Mobile.Core.Models;
using Proact.Mobile.Core.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class ProfileViewModel : BaseViewModel {

        public IMvxCommand<string> OptionsCommand { get; private set; }
        public IMvxCommand TakeImageProfileCommand { get; private set; }
        public Command<bool> EnableNotificationsCommand { get; set; }

        private bool _isPatientProfile;
        public bool IsPatientProfile {
            get => _isPatientProfile;
            set => SetProperty( ref _isPatientProfile, value );
        }

        private bool _isMedicOrNurseProfile;
        public bool IsMedicOrNurseProfile {
            get => _isMedicOrNurseProfile;
            set => SetProperty( ref _isMedicOrNurseProfile, value );
        }

        private bool _isMedicalProfessional;
        public bool IsMedicalProfessional {
            get => _isMedicalProfessional;
            set => SetProperty( ref _isMedicalProfessional, value );
        }

        private string _completeName;
        public string CompleteName {
            get => _completeName;
            set => SetProperty( ref _completeName, value );
        }

        private string _userImageSource;
        public string UserImageSource {
            get => _userImageSource;
            set => SetProperty( ref _userImageSource, value );
        }

        private string _surveysToBeFillCount;
        public string SurveysToBeFillCount {
            get => _surveysToBeFillCount;
            set => SetProperty( ref _surveysToBeFillCount, value );
        }

        private string _surveysFilledInCount;
        public string SurveysFilledInCount {
            get => _surveysFilledInCount;
            set => SetProperty( ref _surveysFilledInCount, value );
        }

        private string _protocolName;
        public string ProtocolName {
            get => _protocolName;
            set => SetProperty( ref _protocolName, value );
        }

        private string _protocolInternalCode;
        public string ProtocolInternalCode {
            get => _protocolInternalCode;
            set => SetProperty( ref _protocolInternalCode, value );
        }

        private string _protocolInternationalCode;
        public string ProtocolInternationalCode {
            get => _protocolInternationalCode;
            set => SetProperty( ref _protocolInternationalCode, value );
        }

        private string _proactAppVersion;
        public string ProactAppVersion {
            get => _proactAppVersion;
            set => SetProperty( ref _proactAppVersion, value );
        }

        private bool _notificationsEnable;
        public bool NotificationsEnable {
            get => _notificationsEnable;
            set => SetProperty( ref _notificationsEnable, value );
        }

        private string _notificationTimeLabelText;
        public string NotificationTimeLabelText {
            get => _notificationTimeLabelText;
            set => SetProperty( ref _notificationTimeLabelText, value );
        }

        private bool _notificationsSettingsLoading;
        public bool NotificationsSettingsLoading {
            get => _notificationsSettingsLoading;
            set => SetProperty( ref _notificationsSettingsLoading, value );
        }

        private bool _termsAndConditionsIsVisible;
        public bool TermsAndConditionsIsVisible {
            get => _termsAndConditionsIsVisible;
            set => SetProperty( ref _termsAndConditionsIsVisible, value );
        }

        private bool _privacyPolicyIsVisible;
        public bool PrivacyPolicyIsVisible {
            get => _privacyPolicyIsVisible;
            set => SetProperty( ref _privacyPolicyIsVisible, value );
        }

        private string _termsAndConditionsTitle;
        public string TermsAndConditionsTitle {
            get => _termsAndConditionsTitle;
            set => SetProperty( ref _termsAndConditionsTitle, value );
        }

        private string _privacyPolicyTitle;
        public string PrivacyPolicyTitle {
            get => _privacyPolicyTitle;
            set => SetProperty( ref _privacyPolicyTitle, value );
        }

        private bool _closeButtonIsVisible;
        public bool CloseButtonIsVisible {
            get => _closeButtonIsVisible;
            set => SetProperty( ref _closeButtonIsVisible, value );
        }

        private bool _surveysGroupIsVisible;
        public bool SurveysGroupIsVisible {
            get => _surveysGroupIsVisible;
            set => SetProperty( ref _surveysGroupIsVisible, value );
        }

        private ILocalDataReadService _localDataReadService;
        private IAuthService _authService;
        private ITakeMediaService _takeMediaService;
        private IAvatarUploaderService _avatarUploaderService;
        private IUserProfileRequestService _userProfileRequestService;
        private ILocalDataWriteService _localDataWriteService;
        private IPushNotificationsService _pushNotificationService;
        private ISurveysService _surveysService;
        private IProtocolsService _protocolsService;
        private IInstituteService _instituteService;
        private IProjectPropertiesService _projectPropertiesService;

        private bool _profileImageAlreadyTaked;
        private MediaFile _attachImageMediaFile;
        private PushNotificationsSettingsModel _notificationsSettingsModel;
        private PatientProtocolsModel _patientProtocolsModel;

        private DocumentModel termsAndConditionDocument;
        private DocumentModel privacyPolicyDocument;

        public ProfileViewModel(
            IUserProfileRequestService userProfileRequestService,
            ILocalDataWriteService localDataWriteService,
            ILocalDataReadService localDataReadService,
            IAuthService authService,
            ITakeMediaService takeMediaService,
            IAvatarUploaderService avatarUploaderService,
            IPushNotificationsService pushNotificationService,
            ISurveysService surveysService,
            IProtocolsService protocolsService,
            IInstituteService instituteService,
            IProjectPropertiesService projectPropertiesService ) {
            _userProfileRequestService = userProfileRequestService;
            _localDataWriteService = localDataWriteService;
            _localDataReadService = localDataReadService;
            _authService = authService;
            _takeMediaService = takeMediaService;
            _avatarUploaderService = avatarUploaderService;
            _pushNotificationService = pushNotificationService;
            _surveysService = surveysService;
            _protocolsService = protocolsService;
            _instituteService = instituteService;
            _projectPropertiesService = projectPropertiesService;

            _authService.Init();
        }

        public override void Prepare() {
            base.Prepare();
            InitUICommands();
            
            LoadProtocols();
            LoadTermsAndPrivacy();
        }

        public async override Task Initialize() {
            await base.Initialize();

            UpdateUIWithUserData();
            UpdateUISuveysBadge();
            UpdateUIWithAppVersion();
            UpdateUIWithProjectProperties();
            PushNotificationServiceRegistration();
        }

        private void InitUICommands() {
            OptionsCommand
                = new MvxCommand<string>( OptionCommandActionHandle );
            TakeImageProfileCommand
                = new MvxCommand( TakeImageProfileActionHandle );
            EnableNotificationsCommand
                = new Command<bool>( EnableNotificationsToggle );
        }

        private void OptionCommandActionHandle( string parameter ) {
            switch ( parameter ) {
                case "signout":
                    DisplaySignoutAlert();
                    break;
                case "surveysToBeFill":
                    OpenToBeCompleteSurveys();
                    break;
                case "surveysFilledIn":
                    OpenCompleteSurveys();
                    break;
                case "projectSurveys":
                    OpenProjectSurveys();
                    break;
                case "protocol":
                    OpenProtocol();
                    break;
                case "termsAndConditions":
                    OpenDocument( termsAndConditionDocument );
                    break;
                case "privacyPolicy":
                    OpenDocument( privacyPolicyDocument );
                    break;
                case "yourPatients":
                    OpenPatientsPage();
                    break;
                case "changeMedicalTeam":
                    OpenProjectPage();
                    break;
                case "openPushNotificationSettings":
                    OpenPushNotificationsSettingsPage();
                    break;
                case "contact":
                    OpenContactPage();
                    break;
            }
        }

        private void UpdateUIWithUserData() {
            var localUserData = _localDataReadService
                .GetUserData();

            CompleteName = localUserData.Name;
            UserImageSource = localUserData.AvatarUrl;
            IsPatientProfile = localUserData
                .UserIsInRole( UserRolesModel.Patient );

            IsMedicOrNurseProfile = localUserData
                .IsMedicOrNurse;
            IsMedicalProfessional = localUserData
                .UserIsInRole( UserRolesModel.MedicalProfessional );
        }

        private async void UpdateUISuveysBadge() {
            SurveysFilledInCount = "0";
            int count = await _surveysService.GetNotCompiledSurveysCount();
            SurveysToBeFillCount = count.ToString();
        }

        private void UpdateUIWithProtocolData() {
            if ( _patientProtocolsModel == null ) {
                return;
            }

            if ( _patientProtocolsModel.UserProtocol != null ) {
                ProtocolName = _patientProtocolsModel
                    .UserProtocol.Name;
                ProtocolInternationalCode = _patientProtocolsModel
                    .UserProtocol.InternationalCode;
                ProtocolInternalCode = _patientProtocolsModel
                    .UserProtocol.InternalCode;
            }
            else if ( _patientProtocolsModel.ProjectProtocol != null ) {
                ProtocolName = _patientProtocolsModel
                    .ProjectProtocol.Name;
                ProtocolInternationalCode = _patientProtocolsModel
                    .ProjectProtocol.InternationalCode;
                ProtocolInternalCode = _patientProtocolsModel
                    .ProjectProtocol.InternalCode;
            }
        }

        private void UpdateUIWithAppVersion() {
            ProactAppVersion = string.Format(
                Resources.AppResources.AppVersion,
                VersionTracking.CurrentVersion,
                VersionTracking.CurrentBuild );
        }

        private void UpdateUIWithProjectProperties() {
            CloseButtonIsVisible =
                _projectPropertiesService.IsMessagingActive();
            SurveysGroupIsVisible = IsPatientProfile
                && _projectPropertiesService.IsSurveysSystemActive();
        }

        private async void DisplaySignoutAlert() {
            bool signoutConfirm
                = await Application.Current.MainPage.DisplayAlert(
                    Resources.AppResources.SignoutAlertTitle,
                    Resources.AppResources.SignoutAlertMessage,
                    Resources.AppResources.SignoutAlertOkButton,
                    Resources.AppResources.SignoutAlertCancelButton );

            if ( signoutConfirm ) {
                PerformSignout();
            }
        }

        private async void PerformSignout() {
            _pushNotificationService.RemovePlayerId();
            _localDataWriteService.SetSigninConditionsAccepted( false );
            await _authService.RemoveAuthenticationAsync();
            await _navigationService.Navigate<LandingViewModel, bool>( true );
        }

        private async void OpenToBeCompleteSurveys() {
            var parameter = new SurveysListParameter() {
                SurveysListType = SurveysListType.TO_BE_COMPLETED
            };

            await _navigationService
                .Navigate<SurveysListViewModel, SurveysListParameter>( parameter );
        }

        private async void OpenCompleteSurveys() {
            var parameter = new SurveysListParameter() {
                SurveysListType = SurveysListType.COMPLETED
            };

            await _navigationService
                .Navigate<SurveysListViewModel, SurveysListParameter>( parameter );
        }

        private async void OpenProjectSurveys() {
            await _navigationService.Navigate<ProjectSurveyListViewModel>();
        }

        private async void OpenProjectPage() {
            await _navigationService.Navigate<ProjectsListViewModel>();
        }

        private async void OpenPatientsPage() {
            await _navigationService.Navigate<PatientsAssociatedWithTeamViewModel>();
        }

        private async void OpenPushNotificationsSettingsPage() {
            var notificationsSettingsModel = await _navigationService
                .Navigate<PushNotificationsSettingsViewModel,
                PushNotificationsSettingsModel,
                PushNotificationsSettingsModel>( _notificationsSettingsModel );

            if ( notificationsSettingsModel != null ) {
                _notificationsSettingsModel = notificationsSettingsModel;
                UpdateNotificationsSettingsUI();
            }
        }

        private async void TakeImageProfileActionHandle() {
            string resetString = null;
            if ( _profileImageAlreadyTaked ) {
                resetString = Resources.AppResources.RemoveProfileImage;
            }

            var response = await Application.Current.MainPage
                .DisplayActionSheet(
                    Resources.AppResources.SelectProfileImage,
                    Resources.AppResources.Cancel,
                    resetString,
                    Resources.AppResources.TakePhoto,
                    Resources.AppResources.PickPhoto );

            if ( response == Resources.AppResources.TakePhoto ) {
                _attachImageMediaFile = await _takeMediaService.TakePhoto();
            }
            else if ( response == Resources.AppResources.PickPhoto ) {
                _attachImageMediaFile = await _takeMediaService.PickPhoto();
            }
            else if ( response == Resources.AppResources.RemoveAttachedImage ) {
                ResetImageProfileAsync();
            }

            if ( _attachImageMediaFile != null ) {
                UpdateImageProfileAsync();
            }
        }

        private async void UpdateImageProfileAsync() {
            _popupService.OpenLoadingPopup();

            var imageStream = _attachImageMediaFile.GetStream();
            var result = await _avatarUploaderService.UploadAvatar( imageStream );
            if ( !result.Success ) {
                var messageModel = new PopupMessageModel() {
                    Type = PopupMessageType.ERROR,
                    MessageText = Resources.AppResources.UploadProfileImageError
                };
                _popupService.OpenMessagePopup( messageModel );
            }
            else {
                UserImageSource = _attachImageMediaFile.Path;
                _profileImageAlreadyTaked = true;

                UpdateLocalUserData();

                var messageModel = new PopupMessageModel() {
                    Type = PopupMessageType.SUCCESS,
                    MessageText = Resources.AppResources.UploadProfileImageSuccess
                };
                _popupService.OpenMessagePopup( messageModel );
            }
        }

        private async void ResetImageProfileAsync() {

            _popupService.OpenLoadingPopup();

            var result = await _avatarUploaderService.ResetAvatar();

            if ( !result.Success ) {
                var messageModel = new PopupMessageModel() {
                    Type = PopupMessageType.ERROR,
                    MessageText = Resources.AppResources.UploadProfileImageError
                };
                _popupService.OpenMessagePopup( messageModel );
            }
            else {

                UpdateLocalUserData();

                UserImageSource = _localDataReadService.GetUserData().AvatarUrl;
                _profileImageAlreadyTaked = false;
                _attachImageMediaFile = null;

                var messageModel = new PopupMessageModel() {
                    Type = PopupMessageType.SUCCESS,
                    MessageText = Resources.AppResources.UploadProfileImageSuccess
                };
                _popupService.OpenMessagePopup( messageModel );
            }
        }

        private async void UpdateLocalUserData() {
            var userDataResponse = await _userProfileRequestService
                .GetCurrenUserProfileData();

            if ( userDataResponse.Success ) {
                var userData = userDataResponse.data;
                _localDataWriteService.SetUserData( userData );
            }
        }

        private void EnableNotificationsToggle( bool notificationsEnable ) {
            NotificationsEnable = notificationsEnable;
            _notificationsSettingsModel.Active = notificationsEnable;
            SaveNotificationsSettings();
        }

        private async void LoadNotificationSettings() {
            NotificationsSettingsLoading = true;
            _notificationsSettingsModel
                = await _pushNotificationService.GetSettings();
            NotificationsSettingsLoading = false;
            UpdateNotificationsSettingsUI();
        }

        private async void SaveNotificationsSettings() {
            await _pushNotificationService
                .ActiveNotifications( _notificationsSettingsModel.Active );
        }

        private void PushNotificationServiceRegistration() {
            _pushNotificationService.RegisterPlayerId( LoadNotificationSettings );
        }

        private void UpdateNotificationsSettingsUI() {
            if ( _notificationsSettingsModel == null ) {
                _notificationsSettingsModel = new PushNotificationsSettingsModel();
            }
            NotificationsEnable = _notificationsSettingsModel.Active;

            if ( !_notificationsSettingsModel.AllDay ) {
                var fromText = $"{Resources.AppResources.NotificationsTimeFrom} {_notificationsSettingsModel.FormattedTimeFrom}";
                var ToText = $"{Resources.AppResources.NotificationsTimeTo} {_notificationsSettingsModel.FormattedTimeTo}";

                NotificationTimeLabelText = $"{fromText} {ToText}";
            }
            else {
                NotificationTimeLabelText = Resources.AppResources.UnlimitedNotificationsTime;
            }
        }

        private async void OpenContactPage() {
            await _navigationService.Navigate<ContactViewModel>();
        }

        private async void LoadProtocols() {
            var response = await _protocolsService.GetProtocols();
            if ( response.Success ) {
                _patientProtocolsModel = response.data;
            }

            UpdateUIWithProtocolData();
        }

        private async void OpenProtocol() {
            if ( _patientProtocolsModel == null ) {
                return;
            }

            if ( _patientProtocolsModel.UserProtocol != null ) {
                var model = new DocumentViewerModel() {
                    Title = _patientProtocolsModel.UserProtocol.Name,
                    Url = _patientProtocolsModel.UserProtocol.Url
                };

                await _navigationService
                    .Navigate<DocumentViewerViewModel, DocumentViewerModel>( model );

            }
            else if ( _patientProtocolsModel.ProjectProtocol != null ) {
                var model = new DocumentViewerModel() {
                    Title = _patientProtocolsModel.ProjectProtocol.Name,
                    Url = _patientProtocolsModel.ProjectProtocol.Url
                };

                await _navigationService
                    .Navigate<DocumentViewerViewModel, DocumentViewerModel>( model );
            }
        }

        private async void LoadTermsAndPrivacy() {
            var result = await _instituteService.GetMyInstitute();
            if ( result.Success ) {
                var instituteModel = result.data;

                if ( instituteModel.Properties == null ) {
                    return;
                }

                if ( instituteModel.Properties.TermsAndConditions != null ) {
                    TermsAndConditionsIsVisible = true;
                    TermsAndConditionsTitle = instituteModel
                        .Properties.TermsAndConditions.Title;
                    termsAndConditionDocument = instituteModel
                        .Properties.TermsAndConditions;
                }

                if ( instituteModel.Properties.PrivacyPolicy != null ) {
                    PrivacyPolicyIsVisible = true;
                    PrivacyPolicyTitle = instituteModel
                        .Properties.PrivacyPolicy.Title;
                    privacyPolicyDocument = instituteModel
                        .Properties.PrivacyPolicy;
                }
            }
        }

        public async void OpenDocument( DocumentModel documentModel ) {
            await _popupService.CloseAllPopup();
            var model = new DocumentViewerModel() {
                Title = documentModel.Title,
                Url = documentModel.Url
            };

            await _navigationService
                .Navigate<DocumentViewerViewModel, DocumentViewerModel>( model );
        }
    }
}

