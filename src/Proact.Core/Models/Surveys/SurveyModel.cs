using System;
using System.Collections.Generic;

namespace Proact.Mobile.Core {

   
    public class SurveyModel {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public SurveyState State { get; set; }
        public List<SurveyQuestionModel> Questions { get; set; }

        public SurveyAssignationModel AssignationModel { get; set; }

        public Guid QuestionsSetId {
            get {
                if ( Questions.Count > 0 ) {
                    return Questions[0].QuestionsSetId;
                }
                else {
                    return Guid.NewGuid();
                }
            }
        }
    }
}
