using System;
namespace Proact.Mobile.Core {
    public class SurveyAssignationModel {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public string SurveyDescription { get; set; }
        public string SurveyVersion { get; set; }
        public bool Completed { get; set; }
        public SurveyState SurveyState { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime? CompletedDateTime { get; set; }
        public SurveyReccurence Reccurrence { get; set; }
        public bool Expired { get; set; }
        public UserModel User { get; set; }


        public string FormattedStartDate {
            get {
                return StartTime.ToShortDateString();
            }
        }

        public string FormattedEndDate {
            get {

                if(ExpireTime.Date == DateTime.Now.Date ) {
                    return Resources.AppResources.Today;
                }
                else if(ExpireTime.Date == DateTime.Now.AddDays( 1 ).Date ) {
                    return Resources.AppResources.Tomorrow;
                }

                return ExpireTime.ToShortDateString();
            }
        }

        public string FormattedCompletedDate {
            get {
                return CompletedDateTime?.ToShortDateString();
            }
        }

       public string DateToShow {
            get {
                if ( Completed ) {
                    return FormattedCompletedDate;
                }
                else {
                    return FormattedEndDate;
                }
            }
        }

        public bool ExpiresSoon {
            get {

                if ( Completed ) {
                    return false;
                }

                if ( ExpireTime.Date == DateTime.Now.Date
                     || ExpireTime.Date == DateTime.Now.AddDays( 1 ).Date ) {
                    return true;
                }

                return false;
            }
        }

     
    }
}

