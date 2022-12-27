using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Presenters.Hints;

namespace Proact.Mobile.Core.ViewModels {
    public class SurveyCompiledViewModel : BaseViewModel<SurveyCompiledModel>, ISurveyResultsPopupCallback {

        public SurveyCompiledModel SurveyModel { get; set; }

        public ObservableCollection<SurveyQuestionReviewModel> _questions;
        public ObservableCollection<SurveyQuestionReviewModel> Questions {
            get => _questions;
            set => SetProperty( ref _questions, value );
        }

        public override void Prepare() {
            base.Prepare();
        }

        public override void Prepare( SurveyCompiledModel parameter ) {
            SurveyModel = parameter;
            InitUI();
            InitQuestionListUI();
        }

        private void InitUI() {
            PageTitle = SurveyModel.Title;
        }

        private void InitQuestionListUI() {
            Questions = new ObservableCollection<SurveyQuestionReviewModel>();
            foreach ( var question in SurveyModel.Questions ) {
                Questions.Add( new SurveyQuestionReviewModel( question ) );
            }
        }

        private async void CloseSurveyAndReturnToMainPage() {
            await _navigationService.ChangePresentation( new MvxPopToRootPresentationHint() );
            await _navigationService.Close( this );
        }

        public async void OnPopupClosed() {
            await _popupService.CloseAllPopup();
            CloseSurveyAndReturnToMainPage();
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