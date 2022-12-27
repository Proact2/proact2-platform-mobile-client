using System;
namespace Proact.Mobile.Core {

    public enum MedicalTeamState {
        Open,
        Closed
    }
    public class MedicalTeamModel {

        public Guid MedicalTeamId { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string StateOrProvince { get; set; }

        public string RegionCode { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string TimeZone { get; set; }
        
        public MedicalTeamState State { get; set; }
        
        public bool IsClosed {
            get {
                return State == MedicalTeamState.Closed;
            }
        }
    }
}
