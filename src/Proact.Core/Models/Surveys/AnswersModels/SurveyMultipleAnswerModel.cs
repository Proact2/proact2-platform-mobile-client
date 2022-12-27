using System;
using System.Collections.Generic;

namespace Proact.Mobile.Core {
    public class SurveyMultipleAnswerModel : ISurveyAnswersModel {
        public List<Guid> SelectedAnswers { get; set; }
    }
}
