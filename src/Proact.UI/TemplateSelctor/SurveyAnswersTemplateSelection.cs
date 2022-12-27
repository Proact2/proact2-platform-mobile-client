using System;
using Xamarin.Forms;
using Proact.Mobile.Core;

namespace Proact.UI {
    public class SurveyAnswersTemplateSelection : DataTemplateSelector {
        public DataTemplate OpenAnswerTemplate { get; set; }
        public DataTemplate MultipleAnswersTemplate { get; set; }
        public DataTemplate SingleAnswerTemplate { get; set; }
        public DataTemplate BooleanAnswerTemplate { get; set; }
        public DataTemplate RatingAnswerTemplate { get; set; }
        public DataTemplate MoodAnswerTemplate { get; set; }

        public SurveyAnswersTemplateSelection() {
            OpenAnswerTemplate = new DataTemplate( typeof( SurveyOpenAnswerViewCell ) );
            MultipleAnswersTemplate = new DataTemplate( typeof( SurveyMultipleAnswersViewCell ) );
            SingleAnswerTemplate = new DataTemplate( typeof( SurveySingleAnswerViewCell ) );
            BooleanAnswerTemplate = new DataTemplate( typeof( SurveyBooleanAnswersViewCell ) );
            RatingAnswerTemplate = new DataTemplate( typeof( SurveyRatingAnswerViewCell ) );
            MoodAnswerTemplate = new DataTemplate( typeof( SurveyMoodAnswerViewCell ) );
        }

        protected override DataTemplate OnSelectTemplate(
            object item, BindableObject container ) {

            var cellModel = ( SurveyQuestionReviewModel )item;
            if ( cellModel.QuestionType == SurveyQuestionType.OPEN_ANSWER ) {
                return OpenAnswerTemplate;
            }
            else if ( cellModel.QuestionType == SurveyQuestionType.MULTIPLE_ANSWERS ) {
                return MultipleAnswersTemplate;
            }
            else if ( cellModel.QuestionType == SurveyQuestionType.SINGLE_ANSWER ) {
                return SingleAnswerTemplate;
            }
            else if ( cellModel.QuestionType == SurveyQuestionType.BOOLEAN ) {
                return BooleanAnswerTemplate;
            }
            else if ( cellModel.QuestionType == SurveyQuestionType.RATING ) {
                return RatingAnswerTemplate;
            }
            else {
                return MoodAnswerTemplate;
            }
        }
    }
}
