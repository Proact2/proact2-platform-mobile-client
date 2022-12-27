using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Proact.Mobile.Core.Models;

namespace Proact.Mobile.Core {
    public class SurveyQuestionModel {

        public Guid Id { get; set; }
        public Guid QuestionsSetId { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public int Order { get; set; }
        public SurveyState State { get; set; }

        [JsonConverter( typeof( SurveyQuestionPropertiesJsonConverter ) )]
        public ISurveysQuestionModelProperties Properties { get; set; }

        [JsonConverter( typeof( SurveyQuestionAnswersContainerJsonConverter ) )]
        public ISurveyQuestionModelAnswersContainer AnswersContainer { get; set; }


        //Binding properties
        public bool Selected { get; set; }
        public ISurveyAnswersModel Answers { get; set; }
    }
}
