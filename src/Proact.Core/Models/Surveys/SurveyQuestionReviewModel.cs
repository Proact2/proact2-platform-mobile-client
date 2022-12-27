using System;
using System.Collections.Generic;
using System.Linq;
using Proact.Mobile.Core.Models;

namespace Proact.Mobile.Core {
    public class SurveyQuestionReviewModel {

        public SurveyQuestionModel QuestionModel { get; set; }
        public SurveyCompiledQuestionModel CompiledQuestionModel { get; private set; }
        public SurveyQuestionType QuestionType { get; private set; }
        public bool IsReviewMode { get; private set; }

        public SurveyQuestionReviewModel( SurveyQuestionModel questionModel ) {
            QuestionModel = questionModel;
            IsReviewMode = true;
            QuestionType = QuestionModel.Properties.Type;
        }

        public SurveyQuestionReviewModel( SurveyCompiledQuestionModel compiledQuestionModel ) {
            CompiledQuestionModel = compiledQuestionModel;
            IsReviewMode = false;
            QuestionType = CompiledQuestionModel.Type;
        }

        public string Title {
            get {
                if ( IsReviewMode ) {
                    return QuestionModel.Title;
                }
                else {
                    return CompiledQuestionModel.Title;
                }
            }
        }

        public string Question {
            get {
                if ( IsReviewMode ) {
                    return QuestionModel.Question;
                }
                else {
                    return CompiledQuestionModel.Question;
                }
            }
        }

        public string OpenAnswer {
            get {
                if ( IsReviewMode ) {
                    return ( ( SurveyOpenAnswerModel )QuestionModel.Answers )
                            .OpenAnswer;
                }
                else {
                    return CompiledQuestionModel.CompiledAnswers[0].Value;
                }
            }
        }

        public List<string> SelectedAnswersFromSingleChoice {
            get {
                List<string> selectedAnswers = new List<string>();
                if ( IsReviewMode ) {

                    var Answers
                        = ( ( SurveysSingleChoiceQuestionModelAnswersContainer )
                        QuestionModel.AnswersContainer )
                        .SelectableAnswers
                        .Where( x => x.Selected )
                        .ToList();

                    foreach(var answer in Answers ) {
                        selectedAnswers.Add( answer.Label );
                    }
                }
                else {
                    foreach(var answer in CompiledQuestionModel.CompiledAnswers ) {
                        selectedAnswers.Add( answer.Value );
                    }
                }

                return selectedAnswers;
            }
        }

        public List<string> SelectedAnswersFromMultipleChoice {
            get {
                List<string> selectedAnswers = new List<string>();
                if ( IsReviewMode ) {

                    var Answers
                        = ( ( SurveysMultipleChoiceQuestionModelAnswersContainer )
                        QuestionModel.AnswersContainer )
                        .SelectableAnswers
                        .Where( x => x.Selected )
                        .ToList();

                    foreach ( var answer in Answers ) {
                        selectedAnswers.Add( answer.Label );
                    }
                }
                else {
                    foreach ( var answer in CompiledQuestionModel.CompiledAnswers ) {
                        selectedAnswers.Add( answer.Value );
                    }
                }

                return selectedAnswers;
            }
        }

        public string MoodImageSource {
            get {
                try {
                    MessageMood mood = MessageMood.None;
                    if ( IsReviewMode ) {
                        if ( QuestionModel.Properties.Type == SurveyQuestionType.MOOD ) {
                            SurveyMoodAnswerModel moodModel
                                = ( SurveyMoodAnswerModel )QuestionModel.Answers;

                            mood = moodModel.MoodAnswer;
                        }
                        else {
                            return string.Empty;
                        }
                    }
                    else {
                        int moodAsInt = 0;
                        int.TryParse( CompiledQuestionModel.CompiledAnswers[0].Value, out moodAsInt );

                        mood = ( MessageMood )moodAsInt;
                    }

                    switch ( mood ) {
                        case MessageMood.VeryBad:
                            return "btn_moodVeryBad";
                        case MessageMood.Bad:
                            return "btn_moodBad";
                        case MessageMood.Good:
                            return "btn_moodGood";
                        case MessageMood.VeryGood:
                            return "btn_moodVeryGood";
                        default:
                            return "btn_moodVeryGood";
                    }
                }
                catch ( Exception ) {
                    return string.Empty;
                }
            }
        }

        public string BooleanAnswerFormatted {
            get {
                bool booleanValue = false;
                if ( IsReviewMode ) {
                    if ( QuestionModel.Properties.Type == SurveyQuestionType.BOOLEAN ) {
                        SurveyBooleanAnswerModel boolModel
                            = ( SurveyBooleanAnswerModel )QuestionModel.Answers;

                        booleanValue = boolModel.BooleanAnswer;
                       
                    }
                    else {
                        return Resources.AppResources.NoButton;
                    }
                }
                else {

                    bool.TryParse(
                        CompiledQuestionModel.CompiledAnswers[0].Value,
                        out booleanValue );
                }

                if ( booleanValue ) {
                    return Resources.AppResources.YesButton;
                }
                else {
                    return Resources.AppResources.NoButton;
                }
            }
        }

        public string RatingAnswerFormatted {
            get {
                if ( IsReviewMode ) {
                    if ( QuestionModel.Properties.Type == SurveyQuestionType.RATING ) {
                        var ratingAnswer
                          = ( SurveyRatingAnswerModel )QuestionModel.Answers;
                        var ratingProperties
                            = ( SurveysRatingQuestionModelProperties )QuestionModel.Properties;

                        return ratingAnswer.RatingAnswer + " / " + ratingProperties.Max;
                    }
                    else {
                        return string.Empty;
                    }
                }
                else {
                    var value = CompiledQuestionModel.CompiledAnswers[0].Value;
                    var ratingProperties
                           = ( SurveysRatingQuestionModelProperties )CompiledQuestionModel.Properties;

                    return value + " / " + ratingProperties.Max;
                }
            }
        }
    }
}
