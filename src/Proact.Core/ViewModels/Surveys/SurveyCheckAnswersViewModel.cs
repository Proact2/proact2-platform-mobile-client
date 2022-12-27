using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Presenters.Hints;

namespace Proact.Mobile.Core.ViewModels {
    public class SurveyCheckAnswersViewModel : BaseViewModel<SurveyModel>, ISurveyResultsPopupCallback {

        public IMvxCommand SendSurveyCommand { get; private set; }
        public IMvxCommand<SurveyQuestionReviewModel> EditAnswerCommand { get; private set; }
        public SurveyModel SurveyModel { get; set; }

        public ObservableCollection<SurveyQuestionReviewModel> _questions;
        public ObservableCollection<SurveyQuestionReviewModel> Questions {
            get => _questions;
            set => SetProperty( ref _questions, value );
        }

        private ISurveysService _surveysService;

        public SurveyCheckAnswersViewModel( ISurveysService surveysService ) {
            PageTitle = Resources.AppResources.SurveyCheckPageTitle;
            _surveysService = surveysService;
        }

        public override void Prepare() {
            base.Prepare();
            InitUICommand();
        }

        public override void Prepare( SurveyModel parameter ) {
            SurveyModel = parameter;
            InitQuestionListUI();
        }

        private void InitUICommand() {
            SendSurveyCommand = new MvxCommand( SendSurveyActionHandle );
            EditAnswerCommand = new MvxCommand<SurveyQuestionReviewModel>( OpenEditQuestionPage );
        }

        private async void SendSurveyActionHandle() {
            await SubmitSurveyAsync();
        }

        private async Task SubmitSurveyAsync() {
            _popupService.OpenLoadingPopup();
            var request = GenerateRequest();
            var result = await _surveysService.CompileSurvey( request );
            if ( result.Success ) {
                ShowGoodResultPopup();
            }
            else {
                OpenErrorMessagePopup();
            }

        }

        private async void OpenEditQuestionPage( SurveyQuestionReviewModel question ) {
            var parameter = new SurveyQuestionParameter() {
                Survey = SurveyModel,
                EditMode = true,
                QuestionOrder = question.QuestionModel.Order
            };

            var edited = false;
            switch ( SurveyModel.Questions[question.QuestionModel.Order].Properties.Type ) {
                case SurveyQuestionType.OPEN_ANSWER:
                    edited = await _navigationService
                        .Navigate<SurveyOpenAnswerQuestionViewModel,
                        SurveyQuestionParameter,
                        bool>( parameter );
                    break;

                case SurveyQuestionType.SINGLE_ANSWER:
                    edited = await _navigationService
                     .Navigate<SurveySingleAnswerQuestionViewModel,
                     SurveyQuestionParameter, bool>( parameter );
                    break;

                case SurveyQuestionType.MULTIPLE_ANSWERS:
                    edited = await _navigationService
                     .Navigate<SurveyMultipleAnswersQuestionViewModel,
                     SurveyQuestionParameter, bool>( parameter );
                    break;

                case SurveyQuestionType.BOOLEAN:
                    edited = await _navigationService
                     .Navigate<SurveyBooleanQuestionViewModel,
                     SurveyQuestionParameter, bool>( parameter );
                    break;

                case SurveyQuestionType.RATING:
                    edited = await _navigationService
                     .Navigate<SurveyRatingQuestionViewModel,
                     SurveyQuestionParameter, bool>( parameter );
                    break;

                case SurveyQuestionType.MOOD:
                    edited = await _navigationService
                     .Navigate<SurveyQuestionMoodAnswerViewModel,
                     SurveyQuestionParameter, bool>( parameter );
                    break;
            }

            if ( edited ) {
                UpdateQuestionAfterEdit( question );
            }
        }

        private void InitQuestionListUI() {
            Questions = new ObservableCollection<SurveyQuestionReviewModel>();
            foreach ( var question in SurveyModel.Questions ) {
                Questions.Add( new SurveyQuestionReviewModel( question ) );
            }
        }

        private void UpdateQuestionAfterEdit( SurveyQuestionReviewModel question ) {
            Questions[question.QuestionModel.Order] = question;
        }

        private async void CloseSurveyAndReturnToMainPage() {
            await _navigationService.ChangePresentation( new MvxPopToRootPresentationHint() );
            await _navigationService.Close( this );
        }

        private void ShowGoodResultPopup() {
            _popupService.OpenSurveyResultsPopup( false, this );
        }

        private void ShowBadResultPopup() {
            _popupService.OpenSurveyResultsPopup( true, this );
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

        private SurveyCompileRequest GenerateRequest() {
            var request = new SurveyCompileRequest();
            request.AssegnationId = SurveyModel.AssignationModel.Id;
            request.SurveyId = SurveyModel.Id;
            request.QuestionsCompiled = new List<SurveyQuestionCompileRequest>();

            foreach ( var question in SurveyModel.Questions ) {

                var questionRequest = new SurveyQuestionCompileRequest();
                questionRequest.QuestionId = question.Id;
                questionRequest.Answers = new List<SurveyAnswerCompileRequest>();

                switch ( question.Properties.Type ) {
                    case SurveyQuestionType.OPEN_ANSWER:

                        GenerateOpenAnswerRequest( question, questionRequest );
                        break;
                    case SurveyQuestionType.BOOLEAN:

                        GenerateBooleanAnswerRequest( question, questionRequest );
                        break;
                    case SurveyQuestionType.MOOD:

                        GenerateMoodAnswerRequest( question, questionRequest );
                        break;
                    case SurveyQuestionType.RATING:

                        GenerateRatingAnswerRequest( question, questionRequest );

                        break;
                    case SurveyQuestionType.SINGLE_ANSWER:

                        GenerateSingleChoiceAnswerRequest( question, questionRequest );
                        break;
                    case SurveyQuestionType.MULTIPLE_ANSWERS:

                        GenerateMultipleChoiceAnswerRequest( question, questionRequest );
                        break;
                }
                request.QuestionsCompiled.Add( questionRequest );
            }
            return request;
        }

        private void GenerateOpenAnswerRequest(
            SurveyQuestionModel questionModel, SurveyQuestionCompileRequest questionRequest ) {
            var openAnswerRequest = new SurveyAnswerCompileRequest();
            openAnswerRequest.Value
                = ( ( SurveyOpenAnswerModel )questionModel.Answers )
                .OpenAnswer;
            questionRequest.Answers.Add( openAnswerRequest );
        }

        private void GenerateBooleanAnswerRequest(
            SurveyQuestionModel questionModel, SurveyQuestionCompileRequest questionRequest ) {
            var booleanAnswerRequest = new SurveyAnswerCompileRequest();
            booleanAnswerRequest.Value
                = ( ( SurveyBooleanAnswerModel )questionModel.Answers )
                .BooleanAnswer.ToString().ToLower();
            questionRequest.Answers.Add( booleanAnswerRequest );
        }

        private void GenerateMoodAnswerRequest(
           SurveyQuestionModel questionModel, SurveyQuestionCompileRequest questionRequest ) {
            int selectedMood
                = ( int )( ( SurveyMoodAnswerModel )questionModel.Answers )
                .MoodAnswer;

            var moodAnswerRequest = new SurveyAnswerCompileRequest();
            moodAnswerRequest.Value = selectedMood.ToString();
            questionRequest.Answers.Add( moodAnswerRequest );
        }

        private void GenerateRatingAnswerRequest(
         SurveyQuestionModel questionModel, SurveyQuestionCompileRequest questionRequest ) {
            var ratingAnswerRequest = new SurveyAnswerCompileRequest();
            ratingAnswerRequest.Value
                = ( ( SurveyRatingAnswerModel )questionModel.Answers )
                .RatingAnswer.ToString();
            questionRequest.Answers.Add( ratingAnswerRequest );
        }

        private void GenerateSingleChoiceAnswerRequest(
         SurveyQuestionModel questionModel, SurveyQuestionCompileRequest questionRequest ) {
            var singleAnswerRequest = new SurveyAnswerCompileRequest();
            singleAnswerRequest.AnswerId
                = ( ( SurveyMultipleAnswerModel )questionModel.Answers )
                .SelectedAnswers[0];
            questionRequest.Answers.Add( singleAnswerRequest );
        }

        private void GenerateMultipleChoiceAnswerRequest(
         SurveyQuestionModel questionModel, SurveyQuestionCompileRequest questionRequest ) {
            var selectedAnswers = ( ( SurveyMultipleAnswerModel )questionModel.Answers ).SelectedAnswers;

            foreach ( var selectedAnswer in selectedAnswers ) {
                var multipleAnswerRequest = new SurveyAnswerCompileRequest();
                multipleAnswerRequest.AnswerId = selectedAnswer;
                questionRequest.Answers.Add( multipleAnswerRequest );
            }
        }
    }
}