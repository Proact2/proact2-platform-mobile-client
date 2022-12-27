using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class SurveysAdminListViewModel : BaseViewModel<Guid> {

        public Command SelectionChangedCommand { get; private set; }
        public Command ListRefreshCommand { get; private set; }
        public SurveyAssignationModel SelectedSurvey { get; set; }
        public string DatePrefixText { get; set; }

        public bool _emptyListIsVisible;
        public bool EmptyListIsVisible {
            get => _emptyListIsVisible;
            set => SetProperty( ref _emptyListIsVisible, value );
        }

        private List<SurveyAssignationModel> _surveys;
        public List<SurveyAssignationModel> Surveys {
            get => _surveys;
            set => SetProperty( ref _surveys, value );
        }

        private Guid _patientId;
        private ISurveysService _surveyService;

        public SurveysAdminListViewModel( ISurveysService surveysService ) {
            _surveyService = surveysService;
        }

        public async override void Prepare( Guid patientId ) {
            _patientId = patientId;
            InitUI();
            SetUICommands();
            await GetSurveysAsync();
        }

        private void InitUI() {
            PageTitle = Resources.AppResources.CompletedSurveysPageTitle;
        }

        private void SetUICommands() {
            SelectionChangedCommand = new Command( SelectionChangedActionHandle );
            ListRefreshCommand = new Command( ListRefreshActionHandle );
        }

        private async void SelectionChangedActionHandle() {
            if ( SelectedSurvey == null ) {
                return;
            }

            var surveyModel = await GetCompiledSurveyDetailsAsync( SelectedSurvey.Id );
            await _navigationService
                .Navigate<SurveyCompiledViewModel, SurveyCompiledModel>( surveyModel );
        }

        private async void ListRefreshActionHandle() {
            await GetSurveysAsync();
        }

        private async Task GetSurveysAsync() {
            IsBusy = true;
            EmptyListIsVisible = false;

            var result = await _surveyService.GetCompiledSurveys( _patientId );
            if ( result.Success ) {
                IsBusy = false;
                Surveys = result.data;
            }
            else {
                OpenErrorMessagePopup();
            }

            
            if ( Surveys.Count == 0 ) {
                EmptyListIsVisible = true;
            }
            IsBusy = false;
        }

        private async Task<SurveyCompiledModel> GetCompiledSurveyDetailsAsync( Guid assignationId ) {
            _popupService.OpenLoadingPopup();
            var model = await _surveyService.GetCompiledSurvey( assignationId );
            await _popupService.CloseAllPopup();

            if ( !model.Success ) {
                OpenErrorMessagePopup();
            }
            return model.data;
        }

        private void OpenErrorMessagePopup() {
            var model = new PopupMessageModel() {
                Type = PopupMessageType.ERROR,
                MessageText = Resources.AppResources.GenericLoadingError
            };
            _popupService.OpenMessagePopup( model );
        }

       
    }
}
