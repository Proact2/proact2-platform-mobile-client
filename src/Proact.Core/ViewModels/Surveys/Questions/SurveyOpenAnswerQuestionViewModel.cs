using System;
namespace Proact.Mobile.Core.ViewModels {
    public class SurveyOpenAnswerQuestionViewModel : SurveyQuestionViewModel {

        public string OpenAnswer { get; set; }

        protected override void UIInitialized() {
            InitModel();
            OpenAnswer = ( ( SurveyOpenAnswerModel )QuestionModel.Answers )
                  .OpenAnswer;
            RaisePropertyChanged( () => OpenAnswer );
        }

        protected override bool Validate() {
            if ( string.IsNullOrEmpty( OpenAnswer ) ) {
                return false;
            }
            else {
                ( ( SurveyOpenAnswerModel )QuestionModel.Answers )
                    .OpenAnswer = OpenAnswer;
                return true;
            }
        }

        private void InitModel() {
            if ( !EditMode ) {
                QuestionModel.Answers = new SurveyOpenAnswerModel();
            }
        }
    }
}
