using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class SurveyBooleanQuestionViewModel : SurveyQuestionViewModel {

        public bool BooleanAnswer { get; set; }

        protected override void UIInitialized() {
            InitModel();
            BooleanAnswer = ( ( SurveyBooleanAnswerModel )QuestionModel.Answers )
                .BooleanAnswer;
            RaisePropertyChanged( () => BooleanAnswer );
        }

        protected override bool Validate() {
            ( ( SurveyBooleanAnswerModel )QuestionModel.Answers )
                .BooleanAnswer = BooleanAnswer;
            return true;
        }

        private void InitModel() {
            if ( QuestionModel.Answers == null ) {
                QuestionModel.Answers = new SurveyBooleanAnswerModel();
            }
        }
    }
}
