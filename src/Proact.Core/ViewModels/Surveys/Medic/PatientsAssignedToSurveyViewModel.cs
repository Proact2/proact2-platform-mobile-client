using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proact.Mobile.Core.ViewModels;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class PatientsAssignedToSurveyViewModel : BaseViewModel<Guid> {

        public Command SelectionChangedCommand { get; private set; }
        public Command ListRefreshCommand { get; private set; }
       
        public bool _emptyListIsVisible;
        public bool EmptyListIsVisible {
            get => _emptyListIsVisible;
            set => SetProperty( ref _emptyListIsVisible, value );
        }

        private SurveyAssignationModel _selectedAssignation;
        public SurveyAssignationModel SelectedAssignation {
            get => _selectedAssignation;
            set => SetProperty( ref _selectedAssignation, value );
        }

        private List<SurveyAssignationModel> _assignations;
        public List<SurveyAssignationModel> Assignations {
            get => _assignations;
            set => SetProperty( ref _assignations, value );
        }

        private ISurveysService _surveyService;
        private Guid _surveyId;


        public PatientsAssignedToSurveyViewModel( ISurveysService surveysService ) {
            _surveyService = surveysService;
        }

        public async override void Prepare( Guid surveyId ) {
            _surveyId = surveyId;
            PageTitle = Resources.AppResources.YourPatientsTitle;
            SetUICommands();
            await GetPatientsAsync();
        }

        private void SetUICommands() {
            SelectionChangedCommand = new Command( SelectionChangedActionHandle );
            ListRefreshCommand = new Command( ListRefreshActionHandle );
        }

        private async Task GetPatientsAsync() {
            IsBusy = true;
            EmptyListIsVisible = false;

            var result
                = await _surveyService.GetPatientsAssignedToASurvey( _surveyId );
            if ( result.Success ) {
                IsBusy = false;
                Assignations = result.data;
            }
            else {
                OpenErrorMessagePopup( Resources.AppResources.GenericLoadingError );
            }

            if ( Assignations.Count == 0 ) {
                EmptyListIsVisible = true;
            }
            IsBusy = false;
        }

        private async void ListRefreshActionHandle() {
            await GetPatientsAsync();
        }

        private async void SelectionChangedActionHandle() {
            if ( SelectedAssignation == null ) {
                return;
            }

            if(SelectedAssignation.Completed ) {
                await OpenSurveyDetails();
            }
            else {
                OpenErrorMessagePopup(Resources.AppResources.SurveysNotCompiledError);
            }

            SelectedAssignation = null;
        }

        private async Task OpenSurveyDetails() {
            var surveyModel = await GetCompiledSurveyDetailsAsync( SelectedAssignation.Id );
            await _navigationService
                .Navigate<SurveyCompiledViewModel, SurveyCompiledModel>( surveyModel );
        }

        private async Task<SurveyCompiledModel> GetCompiledSurveyDetailsAsync( Guid assignationId ) {
            _popupService.OpenLoadingPopup();
            var model = await _surveyService.GetCompiledSurvey( assignationId );
            await _popupService.CloseAllPopup();

            if ( !model.Success ) {
                OpenErrorMessagePopup( Resources.AppResources.GenericLoadingError );
            }
            return model.data;
        }

        private void OpenErrorMessagePopup(string error) {
            var model = new PopupMessageModel() {
                Type = PopupMessageType.ERROR,
                MessageText = error
            };
            _popupService.OpenMessagePopup( model );
        }
    }
}
