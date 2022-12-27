using System;
using MvvmCross.Commands;

namespace Proact.Mobile.Core.ViewModels {
    public class PatientDetailsViewModel : BaseViewModel<string> {

        public IMvxCommand OpenCompletedSurveysCommand { get; private set; }
        public IMvxCommand OpenPatientMediaListCommand { get; private set; }
        public IMvxCommand SendMessageToPatientCommand { get; private set; }

        private UserModel _userModel;
        public UserModel UserModel {
            get => _userModel;
            private set => SetProperty( ref _userModel, value );
        }

        private Guid _userId;
        private IPatientsService _patientsService;

        public PatientDetailsViewModel( IPatientsService patientsService ) {
            _patientsService = patientsService;
        }

        public override void Prepare( string userId ) {
            _userId = new Guid( userId );
            InitUICommands();
            LoadUserDetails();
        }

        private void InitUICommands() {
            OpenCompletedSurveysCommand
                = new MvxCommand( OpenCompletedSurveysActionHandle );
            OpenPatientMediaListCommand
                = new MvxCommand( OpenPatientMediaListActionHandle );
            SendMessageToPatientCommand
                = new MvxCommand( OpenPrivateMessagePage );
        }

        private async void LoadUserDetails() {
            IsBusy = true;
            var result = await _patientsService.GetPatientDetails( _userId.ToString() );
            if ( result.Success ) {
                IsBusy = false;
                UserModel = result.data;
            }
            else {
                OpenErrorMessagePopup();
            }
        }

        private async void OpenCompletedSurveysActionHandle() {
            await _navigationService
                .Navigate<SurveysAdminListViewModel, Guid>( _userId );
        }

        private void OpenPatientMediaListActionHandle() {
            Console.WriteLine( "Media list" );
        }

        private void OpenPrivateMessagePage() {
            Console.WriteLine( "Send message to patient" );
        }

        private void OpenErrorMessagePopup() {
            try {
                var model = new PopupMessageModel() {
                    Type = PopupMessageType.ERROR,
                    MessageText = Resources.AppResources.GenericLoadingError
                };
                _popupService.OpenMessagePopup( model );
            }
            catch ( Exception ) { }
        }
    }
}
