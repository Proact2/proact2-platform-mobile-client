using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class SurveyRatingQuestionViewModel : SurveyQuestionViewModel {

        public double SliderValue { get; set; }
        public double Min { get;  set; }
        public double Max { get;  set; }
        public string MinLabel { get;  set; }
        public string MaxLabel { get;  set; }

        protected override void UIInitialized() {
            var surveyProperties
                = ( SurveysRatingQuestionModelProperties )QuestionModel.Properties;

            Min = surveyProperties.Min;
            Max = surveyProperties.Max;
            MinLabel = surveyProperties.MinLabel;
            MaxLabel = surveyProperties.MaxLabel;

            if ( QuestionModel.Answers == null ) {
                QuestionModel.Answers = new SurveyRatingAnswerModel();
                SliderValue = Min;
            }
        }

        protected override bool Validate() {
            ( ( SurveyRatingAnswerModel )QuestionModel.Answers )
                .RatingAnswer = ( int )SliderValue;
            return true;

        }

      
    }
}
