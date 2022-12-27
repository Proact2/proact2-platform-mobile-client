using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class PatientsAssociatedWithTeamViewModel : BaseViewModel {

        public Command ListRefreshCommand { get; private set; }
        public Command SelectionChanged { get; set; }
        public List<UserModel> Patients { get; set; }
        public UserModel SelectedPatient { get; set; }
        public bool ListIsEmpty { get; set; }

        private IPatientsService _patientService;

        public PatientsAssociatedWithTeamViewModel(
            IPatientsService patientService) {
            _patientService = patientService;
        }

        public override void Prepare() {
            base.Prepare();
            InitUI();
            InitUICommands();
            LoadPatientsAsync();
        }

        private void InitUI() {
            PageTitle = Resources.AppResources.YourPatientsTitle;
        }

        private void InitUICommands() {
            SelectionChanged
                = new Command( PatientSelectionActionHandle );
            ListRefreshCommand
                = new Command( LoadPatientsAsync );
        }

        private async void LoadPatientsAsync() {
            IsBusy = true;
            var response = await _patientService.GetPatientsAssociatedWithMedicalTeam();
            if ( response.Success ) {
                Patients = response.data;
                await RaisePropertyChanged( () => Patients );
                await UpdateEmptyListMessage();
            }
            else {
                OpenErrorMessagePopup(
                    Resources.AppResources.PatientsListRequestError );
            }
            IsBusy = false;
        }

        private async void PatientSelectionActionHandle( ) {
            await _navigationService
                .Navigate<PatientDetailsViewModel, string>( SelectedPatient.UserId.ToString() );

            SelectedPatient = null;
        }

        private void OpenErrorMessagePopup( string errorMessage ) {
            var message = new PopupMessageModel();
            message.Type = PopupMessageType.ERROR;
            message.MessageText = errorMessage;
            _popupService.OpenMessagePopup( message );
        }

        private async Task UpdateEmptyListMessage() {
            ListIsEmpty = ( Patients == null || Patients.Count == 0 );
            await RaisePropertyChanged( () => ListIsEmpty );
        }
    }
}
