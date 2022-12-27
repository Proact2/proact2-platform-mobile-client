using System;
using System.Threading.Tasks;
using MvvmCross.Commands;

namespace Proact.Mobile.Core.ViewModels {
    public class SurveyLandingViewModel : BaseViewModel<SurveyModel> {

        public IMvxCommand StartSurveyCommand { get; private set; }
        public SurveyModel SurveyModel { get; set; }
        public string QuestionsCounterLabelText { get; private set; }
        public string VersionLabelText { get; private set; }
        public string ExpiredDateLabelText { get; private set; }

        public override void Prepare( SurveyModel parameter ) {
            SurveyModel = parameter;
            InitUI();
            InitUICommand();
        }

        private void InitUI() {
            QuestionsCounterLabelText = string.Format(
                Resources.AppResources.SurveyQuestionsCounterLabel,
                SurveyModel.Questions.Count );

            VersionLabelText = string.Format(
             Resources.AppResources.SurveyQuestionsVersionLabel,
             SurveyModel.Version );

            ExpiredDateLabelText
                = $"{Resources.AppResources.SurveyExpires} {SurveyModel.AssignationModel.FormattedEndDate}";
        }

        private void InitUICommand() {
            StartSurveyCommand = new MvxCommand( StartSurveyActionHandle );
        }

        private async void StartSurveyActionHandle() {
            await OpenQuestionPage();
        }

        private async Task OpenQuestionPage() {

            var parameter = new SurveyQuestionParameter() {
                Survey = SurveyModel,
                QuestionOrder = 0
            };

            switch ( SurveyModel.Questions[0].Properties.Type ) {
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
}
