using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class SurveysListViewModel : BaseViewModel<SurveysListParameter> {

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

        private SurveysListParameter _parameter;
        private ISurveysService _surveyService;

        public SurveysListViewModel( ISurveysService surveysService ) {
            _surveyService = surveysService;
        }

        public async override void Prepare( SurveysListParameter parameter ) {
            _parameter = parameter;
            InitUI();
            SetUICommands();
            await GetSurveysAsync();
            OpenSurveyOnPushNotificationReceived();
        }

        private void InitUI() {
            if ( _parameter.SurveysListType == SurveysListType.COMPLETED ) {
                PageTitle = Resources.AppResources.CompletedSurveysPageTitle;
                DatePrefixText = Resources.AppResources.SurveyCompleted;
            }
            else {
                PageTitle = Resources.AppResources.ToBeCompletedSurveysPageTitle;
                DatePrefixText = Resources.AppResources.SurveyExpires;
            }
        }

        private void SetUICommands() {
            SelectionChangedCommand = new Command( SelectionChangedActionHandle );
            ListRefreshCommand = new Command( ListRefreshActionHandle );
        }

        private async void SelectionChangedActionHandle() {
            if ( SelectedSurvey == null ) {
                return;
            }

            if ( _parameter.SurveysListType == SurveysListType.TO_BE_COMPLETED ) {

                var surveyModel = await GetNotCompletedSurveyDetailsAsync( SelectedSurvey.SurveyId );
                surveyModel.AssignationModel = SelectedSurvey;
                await _navigationService
                    .Navigate<SurveyLandingViewModel, SurveyModel>( surveyModel );
            }
            else {

                var surveyModel = await GetCompiledSurveyDetailsAsync( SelectedSurvey.Id );
                await _navigationService
                    .Navigate<SurveyCompiledViewModel, SurveyCompiledModel>( surveyModel );
            }
        }

        private async void ListRefreshActionHandle() {
            await GetSurveysAsync();
        }

        private async Task GetSurveysAsync() {
            IsBusy = true;
            EmptyListIsVisible = false;

            if ( _parameter.SurveysListType == SurveysListType.COMPLETED ) {
                var result = await _surveyService.GetMineCompiledSurveys();
                if ( result.Success ) {
                    IsBusy = false;
                    Surveys = result.data;
                }
                else {
                    OpenErrorMessagePopup();
                }
            }
            else {
                var result = await _surveyService.GetMineNotCompiledSurveys();
                if ( result.Success ) {
                    IsBusy = false;
                    Surveys = result.data;
                }
                else {
                    OpenErrorMessagePopup();
                }
            }

            if(Surveys.Count == 0 ) {
                EmptyListIsVisible = true;
            }
            IsBusy = false;
        }

        private async Task<SurveyModel> GetNotCompletedSurveyDetailsAsync( Guid id ) {
            _popupService.OpenLoadingPopup();
            var model = await _surveyService.GetSurveyDetails( id );
            await _popupService.CloseAllPopup();

            if ( !model.Success ) {
                OpenErrorMessagePopup();
            }
            return model.data;
        }

        private async Task<SurveyCompiledModel> GetCompiledSurveyDetailsAsync( Guid assignationId ) {
            _popupService.OpenLoadingPopup();
            var model = await _surveyService.GetMineCompiledSurvey( assignationId );
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

        private void OpenSurveyOnPushNotificationReceived( ) {
            if(_parameter.SurveysListType == SurveysListType.TO_BE_COMPLETED
                || _parameter.SurveyAssignationIdToOpen != null ) {

                SelectedSurvey =
                    Surveys.Find( x => x.Id == _parameter.SurveyAssignationIdToOpen );

                SelectionChangedActionHandle();
            }
        }
    }
}
