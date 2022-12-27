using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proact.Mobile.Core.ViewModels;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class ProjectSurveyListViewModel : BaseViewModel {

        private List<SurveyModel> _surveys;
        public List<SurveyModel> Surveys {
            get => _surveys;
            set => SetProperty( ref _surveys, value );
        }

        private SurveyModel _selectedSurvey;
        public SurveyModel SelectedSurvey {
            get => _selectedSurvey;
            set => SetProperty( ref _selectedSurvey, value );
        }

        public Command SelectionChangedCommand { get; private set; }
        public Command ListRefreshCommand { get; private set; }
     
       
        public bool _emptyListIsVisible;
        public bool EmptyListIsVisible {
            get => _emptyListIsVisible;
            set => SetProperty( ref _emptyListIsVisible, value );
        }

        private ISurveysService _surveyService;
        private ILocalDataReadService _localDataReadService;
        private Guid projectId;

        public ProjectSurveyListViewModel(
            ISurveysService surveysService,
            ILocalDataReadService localDataReadService) {
            _surveyService = surveysService;
            _localDataReadService = localDataReadService;
        }

        public async override void Prepare() {
            InitUI();
            SetUICommands();
            ReadCurrentProjectId();        

            await GetSurveysAsync();
        }

        private void InitUI() {
            PageTitle = Resources.AppResources.SurveysListPageTitle;
        }

        private void SetUICommands() {
            SelectionChangedCommand = new Command( SelectionChangedActionHandle );
            ListRefreshCommand = new Command( ListRefreshActionHandle );
        }

        private async void ListRefreshActionHandle() {
            await GetSurveysAsync();
        }

        private void ReadCurrentProjectId() {
            projectId =
             _localDataReadService.GetProjectModel().ProjectId;
        }

        private async Task GetSurveysAsync() {
            IsBusy = true;
            EmptyListIsVisible = false;

            var result
                = await _surveyService.GetSurveysOfProject( projectId );
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

        private async void SelectionChangedActionHandle() {
            if ( SelectedSurvey == null ) {
                return;
            }

            var response = await Application.Current.MainPage
             .DisplayActionSheet(
                Resources.AppResources.SurveysResultsAndStatistics,
                Resources.AppResources.Cancel,
                null,
                Resources.AppResources.SurveysResults,
                Resources.AppResources.SurveysStats );

            if ( response == Resources.AppResources.SurveysStats ) {
                await _navigationService.Navigate<SurveyStatsViewModel, Guid>( SelectedSurvey.Id );
            }
            else if ( response == Resources.AppResources.SurveysResults ) {
                await _navigationService.Navigate<PatientsAssignedToSurveyViewModel, Guid>( SelectedSurvey.Id );
            }

            SelectedSurvey = null;
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
