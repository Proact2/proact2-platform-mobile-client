using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Proact.Mobile.Core.Models;
using Proact.Mobile.Core.Services;
using Xamarin.Forms;
namespace Proact.Mobile.Core.ViewModels {
    public class AddAnalysisViewModel : BaseViewModel<AnalysisPageParams> {

        public const string MSG_ANALYSIS_CREATED = "MSG_ANALYSIS_CREATED";
        public const string MSG_ANALYSIS_UPDATED = "MSG_ANALYSIS_UPDATED";
        public  Command<LexiconCategoryModel> OpenLexiconLabelSelectionPageCommand { get; set; }
        public  Command<LexiconLabelModel> RemoveLabelFromMultipleSelectionCategoryCommand  { get; set; }
        public Command SubmitAnalysisCommand { get; set; }

        private ObservableCollection<LexiconCategoryModel> _categories;
        public ObservableCollection<LexiconCategoryModel> Categories {
            get => _categories;
            set => SetProperty( ref _categories, value );
        }

        private AnalysisPageParams _pageParams;
        private LexiconModel _lexiconModel;
        private AddAnalysisRequest _addAnalysisRequest;
        
        private IAnalysisService _analysisService;
        private ILocalDataReadService _localDataReadService;
        private AnalysisModel _newAnalysis;

        public AddAnalysisViewModel(
            IAnalysisService analysisService,
            ILocalDataReadService localDataReadService ) {
            _analysisService = analysisService;
            _localDataReadService = localDataReadService;
        }

        public override void Prepare(AnalysisPageParams pageParams) {
            base.Prepare();

            _pageParams = pageParams;
            InitUi();
            InitCommand();
            LoadLexicon();
            RegisterMessages();
        }

        private void InitUi() {
            if ( _pageParams.IsEditMode ) {
                PageTitle = Resources.AppResources.EditAnalysisPageTitle;
            }
            else {
                PageTitle = Resources.AppResources.AddAnalysisPageTitle;
            }
        }

        private void InitCommand() {
            SubmitAnalysisCommand 
                = new Command( PerformSubmit );
            OpenLexiconLabelSelectionPageCommand 
                = new Command<LexiconCategoryModel>( OpenLexiconLabelSelectionPage );
            RemoveLabelFromMultipleSelectionCategoryCommand
                = new Command<LexiconLabelModel>( RemoveLabelFromMultipleSelectionCategory );
        }

        private void RegisterMessages() {
            MessagingCenter.Subscribe<LexiconLabelsListViewModel, LexiconCategoryModel>(
                this, LexiconLabelsListViewModel.MSG_UPDATE_LABEL, OnCategoryUpdated );
        }

        private async void LoadLexicon() {
            IsBusy = true;

            var lexiconId = _localDataReadService.GetProjectModel().Properties.Lexicon.Id;
            if ( lexiconId == null )    
                return;

            var responseResult = await _analysisService
                .GetLexicon( ( Guid )lexiconId );

            if ( responseResult.Success ) {
                _lexiconModel = responseResult.data;
                UpdateUiOnLexiconLoaded();
            }
            IsBusy = false;
        }

        private void UpdateUiOnLexiconLoaded() {
                Categories = new ObservableCollection<LexiconCategoryModel>( _lexiconModel.Categories );
            foreach ( var category in Categories ) {
                foreach ( var label in category.Labels ) {
                    label.LexiconCategoryId = category.Id;
                }
            }
            
            if ( _pageParams.IsEditMode ) {
                UpdateUiInEditMode();
            }
        }

        private async Task<bool> Validate() {
            if ( _addAnalysisRequest.AnalysisResults.Count == 0 ) {
                await Application.Current.MainPage.DisplayAlert(
                    Resources.AppResources.AddAnalysisValidationError,
                    string.Empty,
                    Resources.AppResources.Ok );
                return false;
            }
            
            return true;
        }

        private void PerformSubmit() {
            if ( !_pageParams.IsEditMode ) {
                PerformAddAnalysis();
            }
            else {
                PerformUpdateAnalysis();
            }
        }
        
        private async void PerformAddAnalysis() {
            PrepareRequest();
            
            if ( !await Validate() )
                return;

            bool accepted = await Application.Current.MainPage.DisplayAlert(
                Resources.AppResources.AddAnalysisAlertTitle,
                Resources.AppResources.AddAnalysisAlertMessage,
                Resources.AppResources.AddAnalysisAlertConfirmButton,
                Resources.AppResources.Cancel );

            if ( !accepted )
                return;

            _popupService.OpenLoadingPopup();
            
            var result = await _analysisService.AddAnalysis( 
                _pageParams.ProjectId, _pageParams.MedicalTeamId, _addAnalysisRequest );

            await _popupService.CloseAllPopup();
            if ( result.Success ) {
               _newAnalysis = result.data;
               await ClosePageWhenAnalysisIsCreated();
            }
            else {
                ShowErrorPopupMessage();
            }
        }
        
        private async void PerformUpdateAnalysis() {
            PrepareRequest();
            
            if ( !await Validate() )
                return;

            bool accepted = await Application.Current.MainPage.DisplayAlert(
                Resources.AppResources.UpdateAnalysisAlertTitle,
                Resources.AppResources.UpdateAnalysisAlertMessage,
                Resources.AppResources.UpdateAnalysisAlertConfirmButton,
                Resources.AppResources.Cancel );

            if ( !accepted )
                return;

            _popupService.OpenLoadingPopup();
     
            var result = await _analysisService.UpdateAnalysis( 
                _pageParams.ProjectId, 
                _pageParams.MedicalTeamId, 
                _pageParams.AnalysisToEdit.AnalysisId, 
                _addAnalysisRequest );

            await _popupService.CloseAllPopup();
            
            if ( result.Success ) {
                _newAnalysis = result.data;
                await ClosePageWhenAnalysisIsUpdated();
            }
            else {
                ShowErrorPopupMessage();
            }
        }
       
        private async void OpenLexiconLabelSelectionPage( LexiconCategoryModel selectedCategory ) {
            await _navigationService.Navigate<LexiconLabelsListViewModel, LexiconCategoryModel>( selectedCategory );
        }
        
        private void OnCategoryUpdated( LexiconLabelsListViewModel sender, LexiconCategoryModel selectedCategory ) {
            UpdateSelectedCategory( selectedCategory );
        }
        
        private void UpdateSelectedCategory( LexiconCategoryModel lexiconCategory ) {
            if ( lexiconCategory == null )
                return;

            try {
                var updatedCategory = Categories
                    .FirstOrDefault( x
                        => x.Id == lexiconCategory.Id );

                var index = Categories.IndexOf( updatedCategory );
                if ( index >= 0 ) {
                    Categories[index] = updatedCategory;
                }
            }
            catch(Exception e ) {
                Console.WriteLine( e.Message );
            }
          
        }
        
        private void RemoveLabelFromMultipleSelectionCategory( LexiconLabelModel selectedLabel ) {
            var updatedCategory = Categories
                .FirstOrDefault( x 
                    => x.Id == selectedLabel.LexiconCategoryId );
            updatedCategory?.SelectedLabels.Remove( selectedLabel );
            UpdateSelectedCategory( updatedCategory );
        }

        private void PrepareRequest() {
            if ( _pageParams?.MessageModel?.MessageId == null ) {
                return;
            }
            _addAnalysisRequest = new AddAnalysisRequest();
            _addAnalysisRequest.MessageId = (Guid)_pageParams.MessageModel.MessageId;
            _addAnalysisRequest.AnalysisResults = new List<AddAnalysisResultModel>();
            foreach ( var category in Categories  ) {
                foreach ( var label in category.SelectedLabels ) {
                    _addAnalysisRequest.AnalysisResults.Add( new AddAnalysisResultModel() {
                        LabelId = label.Id
                    } );
                }
            }
        }

        private async Task ClosePageWhenAnalysisIsCreated(  ) {
            await ShowCreationSuccessPopupMessage();
            MessagingCenter.Send( this, MSG_ANALYSIS_CREATED, _newAnalysis );
            await _navigationService.Close( this );
        }
        
        private async Task ClosePageWhenAnalysisIsUpdated(  ) {
            await ShowUpdateSuccessPopupMessage();
            MessagingCenter.Send( this, MSG_ANALYSIS_UPDATED, _newAnalysis );
            await _navigationService.Close( this );
        }

        private async Task ShowCreationSuccessPopupMessage() {
            await Application.Current.MainPage.DisplayAlert(
                Resources.AppResources.AddAnalysisSuccessAlertMessage,
                string.Empty,
                Resources.AppResources.Ok );
        }
        
        private async Task ShowUpdateSuccessPopupMessage() {
            await Application.Current.MainPage.DisplayAlert(
                Resources.AppResources.UpdateAnalysisSuccessAlertMessage,
                string.Empty,
                Resources.AppResources.Ok );
        }
        
        private void ShowErrorPopupMessage() {
            var model = new PopupMessageModel() {
                Type = PopupMessageType.ERROR,
                MessageText = Resources.AppResources.AddAnalysisMessageError
            };
            _popupService.OpenMessagePopup( model );
        }

        private void UpdateUiInEditMode() {
            foreach ( var category in Categories ) {
                var resultsGroup = _pageParams.AnalysisToEdit.ResultsGroupByCategories
                    .Find( x => x.CategoryId == category.Id );

                if ( resultsGroup != null ) {
                    foreach ( var result in resultsGroup.Results ) {
                        category.SelectedLabels.Add( new LexiconLabelModel() {
                            Id = result.LabelId,
                            LexiconCategoryId = result.CategoryId,
                            Label = result.ResultLabel
                        } );
                    }
                }
            }
        }


    }
}
