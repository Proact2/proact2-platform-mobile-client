using System;
namespace Proact.Mobile.Core {

    public enum SurveysListType {
        COMPLETED,
        TO_BE_COMPLETED
    }

    public class SurveysListParameter {
        public SurveysListType SurveysListType { get; set; }
        public Guid? SurveyAssignationIdToOpen { get; set; }
    }
}
