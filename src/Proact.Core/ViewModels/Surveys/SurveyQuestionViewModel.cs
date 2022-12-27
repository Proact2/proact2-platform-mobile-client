using System;
using System.Threading.Tasks;
using MvvmCross.Commands;

namespace Proact.Mobile.Core.ViewModels {
    public abstract class SurveyQuestionViewModel : BaseViewModel<SurveyQuestionParameter, bool> {

        public IMvxCommand ShowNextQuesionPageCommand { get; private set; }
        public IMvxCommand EditQuestionCommand { get; private set; }

        public SurveyModel SurveyModel { get; private set; }
        public SurveyQuestionModel QuestionModel { get; private set; }

        public string QuestionOrderLabelText { get; private set; }
        public string VersionLabelText { get; private set; }
        public bool ErrorIsVisible { get; private set; }
        public bool EditMode { get; private set; }
        
        public override void Prepare( SurveyQuestionParameter parameter ) {
            SurveyModel = parameter.Survey;
            QuestionModel = SurveyModel.Questions[parameter.QuestionOrder];
            EditMode = parameter.EditMode;
            InitUI();
            InitUICommand();
        }

        private void InitUI() {
            HideErrorMessage();
            VersionLabelText = string.Format(
                Resources.AppResources.SurveyQuestionsVersionLabel,
                SurveyModel.Version );

            QuestionOrderLabelText = string.Format(
                Resources.AppResources.SurveyQuestionOrderLabel,
                QuestionModel.Order + 1,
                SurveyModel.Questions.Count );

            UIInitialized();
        }

        private void InitUICommand() {
            ShowNextQuesionPageCommand
                = new MvxCommand( ShowNextQuestion );

            EditQuestionCommand
                = new MvxCommand( ClosePageAfterEditQuestion );
        }

        private async void ClosePageAfterEditQuestion() {
            HideErrorMessage();
            if ( !Validate() ) {
                ShowErrorMessage();
                return;
            }
            await _navigationService.Close( this, true );
        }

        protected abstract bool Validate();
        protected abstract void UIInitialized();

        private async void ShowNextQuestion() {
            HideErrorMessage();
            if ( !Validate() ) {
                ShowErrorMessage();
                return;
            }

            if ( IsLastQuestion() ) {
                await _navigationService
                    .Navigate<SurveyCheckAnswersViewModel,
                    SurveyModel>( SurveyModel );
            }
            else {

                var parameter = new SurveyQuestionParameter() {
                    Survey = SurveyModel,
                    QuestionOrder = QuestionModel.Order + 1
                };

                switch ( SurveyModel.Questions[parameter.QuestionOrder].Properties.Type ) {
                    case SurveyQuestionType.OPEN_ANSWER:
                        await _navigationService
                            .Navigate<SurveyOpenAnswerQuestionViewModel,
                            SurveyQuestionParameter>( parameter );
                        break;

                    case SurveyQuestionType.SINGLE_ANSWER:
                        await _navigationService
                         .Navigate<SurveySingleAnswerQuestionViewModel,
                         SurveyQuestionParameter>( parameter );
                        break;

                    case SurveyQuestionType.MULTIPLE_ANSWERS:
                        await _navigationService
                         .Navigate<SurveyMultipleAnswersQuestionViewModel,
                         SurveyQuestionParameter>( parameter );
                        break;

                    case SurveyQuestionType.BOOLEAN:
                        await _navigationService
                         .Navigate<SurveyBooleanQuestionViewModel,
                         SurveyQuestionParameter>( parameter );
                        break;

                    case SurveyQuestionType.RATING:
                        await _navigationService
                         .Navigate<SurveyRatingQuestionViewModel,
                         SurveyQuestionParameter>( parameter );
                        break;

                    case SurveyQuestionType.MOOD:
                        await _navigationService
                         .Navigate<SurveyQuestionMoodAnswerViewModel,
                         SurveyQuestionParameter>( parameter );
                        break;
                }
            }            
        }

        private void ShowErrorMessage() {
            ErrorIsVisible = true;
            RaisePropertyChanged( () => ErrorIsVisible );
        }

        private void HideErrorMessage() {
            ErrorIsVisible = false;
            RaisePropertyChanged( () => ErrorIsVisible );
        }

        private bool IsLastQuestion() {
            return SurveyModel.Questions.Count == QuestionModel.Order + 1;
        }
    }
}
