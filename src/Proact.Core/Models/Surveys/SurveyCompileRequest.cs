using System;
using System.Collections.Generic;

namespace Proact.Mobile.Core {

    public class SurveyAnswerCompileRequest {
        public Guid AnswerId { get; set; }
        public string Value { get; set; }
    }

    public class SurveyQuestionCompileRequest {
        public Guid QuestionId { get; set; }
        public List<SurveyAnswerCompileRequest> Answers { get; set; }
    }

    public class SurveyCompileRequest {
        public Guid SurveyId { get; set; }
        public Guid AssegnationId { get; set; }
        public List<SurveyQuestionCompileRequest> QuestionsCompiled { get; set; }
    }
}
