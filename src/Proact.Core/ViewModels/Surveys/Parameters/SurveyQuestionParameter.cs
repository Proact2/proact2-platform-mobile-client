using System;
namespace Proact.Mobile.Core {
    public class SurveyQuestionParameter {
        public SurveyModel Survey { get; set; }
        public int QuestionOrder { get; set; }
        public bool EditMode { get; set; }
    }
}
