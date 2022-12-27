using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Proact.Mobile.Core.Models;
using Xamarin.Forms;
namespace Proact.Mobile.Core.ViewModels {
    public class AnalysisListViewModel : BaseViewModel<AnalysisPageParams> {
        public Command ListRefreshCommand { get; private set; }
        public Command AddAnalysisCommand { get; private set; }
        public Command<AnalysisModel> EditAnalysisCommand { get; set; }
        public Command<AnalysisModel> DeleteAnalysisCommand { get; set; }
        
        private ObservableCollection<AnalysisModel> _analysis;
        public ObservableCollection<AnalysisModel> Analysis {
            get => _analysis;
            set => SetProperty( ref _analysis, value );
        }
        
        private bool _isRefreshing;
        public bool IsRefreshing {
            get => _isRefreshing;
            set => SetProperty( ref _isRefreshing, value );
        }

        private bool _isListEmpty;
        public bool IsLisEmpty {
            get => _isListEmpty;
            set => SetProperty( ref _isListEmpty, value );
        }

        private AnalysisPageParams _pageParams;
        private ILocalDataReadService _localDataReadService;
        private AnalysisResumeModel _analysisResumeModel;
        private IAnalysisService _analysisService;
        private IListScrollingBehaviour _listScrollingBehaviour;

        public AnalysisListViewModel(
            ILocalDataReadService localDataReadService,
            IAnalysisService analysisService ) {
            _analysisService = analysisService;
            _localDataReadService = localDataReadService;
        }

        public async override void Prepare(AnalysisPageParams pageParams) {
            base.Prepare();

            _pageParams = pageParams;
            InitUi();
            InitCommand();
            MessageSubscribe();
            await LoadAnalysisAsync();
        }
       
        private void InitUi() {
            PageTitle = Resources.AppResources.AnalysisListPageTitle;
        }
        
        private void InitCommand() {
            AddAnalysisCommand 
                = new Command( PerformAddAnalysis );
            ListRefreshCommand
                = new Command( RefreshList );
            EditAnalysisCommand 
                = new Command<AnalysisModel>( PerformEditAnalysis );
            DeleteAnalysisCommand 
                = new Command<AnalysisModel>( PerformDeleteAnalysis );
        }
        
        private async Task LoadAnalysisAsync() {
            if ( _pageParams.MessageModel.MessageId != null ) {
                IsBusy = true;
                Guid messageId = ( Guid )_pageParams.MessageModel.MessageId;
                var result = await _analysisService.GetAnalysisResume(
                    _pageParams.ProjectId,
                    _pageParams.MedicalTeamId,
                    messageId );

                IsBusy = false;
                if ( result.Success ) {
                    _analysisResumeModel = result.data;
                    Analysis = new ObservableCollection<AnalysisModel>( _analysisResumeModel.Analysis );
                    IsLisEmpty = Analysis.Count == 0;
                }
            }
        }
        
        private async void RefreshList() {
            if ( IsBusy ) {
                return;
            }
            IsRefreshing = true;
            await LoadAnalysisAsync();
            IsRefreshing = false;
        }
        
        private async void PerformAddAnalysis() {
            _pageParams.AnalysisToEdit = null;
            await _navigationService.Navigate<AddAnalysisViewModel, AnalysisPageParams>( _pageParams );
        }
         
        private async void PerformEditAnalysis( AnalysisModel analysis ) {
            _pageParams.AnalysisToEdit = analysis;
            await _navigationService.Navigate<AddAnalysisViewModel, AnalysisPageParams>( _pageParams );
        }

        private async void PerformDeleteAnalysis( AnalysisModel analysis ) {
            var delete = await Application.Current.MainPage.DisplayAlert(
                Resources.AppResources.AnalysisDeleteAlertTitle,
                Resources.AppResources.AnalysisDeleteAlertMessage,
                Resources.AppResources.Delete,
                Resources.AppResources.Cancel);

            if ( delete ) {
                _popupService.OpenLoadingPopup();
                var result = await _analysisService.DeleteAnalysis(
                    _pageParams.ProjectId,
                    _pageParams.MedicalTeamId,
                    analysis.AnalysisId );

                if ( result.Success ) {
                    await _popupService.CloseAllPopup();
                    Analysis.Remove( analysis );
                    IsLisEmpty = Analysis.Count == 0;
                }
                else {
                   ShowDeleteErrorPopup();
                }
            }
        }
        
        private void MessageSubscribe() {
            MessagingCenter.Subscribe<AddAnalysisViewModel,AnalysisModel>( 
                this, AddAnalysisViewModel.MSG_ANALYSIS_CREATED, UpdateUiOnAnalysisCreated );
            
            MessagingCenter.Subscribe<AddAnalysisViewModel,AnalysisModel>( 
                this, AddAnalysisViewModel.MSG_ANALYSIS_UPDATED, UpdateUiOnAnalysisUpdated );
        }
        
        private void UpdateUiOnAnalysisCreated( AddAnalysisViewModel sender, AnalysisModel newAnalysis ) {
            var index = 0;
            Analysis.Insert( index, newAnalysis );
            IsLisEmpty = Analysis.Count == 0;
            ScrollToIndex( index );
        }
        
        private void UpdateUiOnAnalysisUpdated( AddAnalysisViewModel sender, AnalysisModel updatedAnalysis ) {
            ListRefreshCommand.Execute( null );
        }

        public void SetListScrollingBehaviour( IListScrollingBehaviour listScrollingBehaviour ) {
            _listScrollingBehaviour = listScrollingBehaviour;
        }

        public void ScrollToIndex( int index ) {
            InvokeOnMainThread( () => {
                try {
                    if ( _listScrollingBehaviour != null ) {
                        _listScrollingBehaviour.ScrollToIndex( index );
                    }
                }
                catch ( Exception e ) {
                    Console.WriteLine( e.Message );
                }
            } );
        }

        private void ShowDeleteErrorPopup() {
            var model = new PopupMessageModel() {
                MessageText = Resources.AppResources.AnalysisDeleteErrorMessage,
                Type = PopupMessageType.ERROR
            };
            _popupService.OpenMessagePopup( model );
        }
    }
}
