using System;
using System.Collections.Generic;

namespace Proact.Mobile.Core {

    public enum UserSubscriptionState {
        Active,
        Suspended,
        Deactivated
    }

    public class UserModel {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public string Title { get; set; }
        public string AccountId { get; set; }
        public UserSubscriptionState State { get; set; }
        public List<string> Roles { get; set; }
        public List<MedicalTeamModel> MedicalTeam { get; set; }
        public Guid InstituteId { get; set; }

        public bool UserIsInRole(string role ) {
            return Roles.Contains( role );
        }

        public bool IsMedicOrNurse {
            get {
                return UserIsInRole( UserRolesModel.MedicalProfessional )
                    || UserIsInRole( UserRolesModel.Nurse );
            }
        }

        public bool IsActive {
            get { return State == UserSubscriptionState.Active; }
        }

        public bool IsDeactivated {
            get { return State == UserSubscriptionState.Deactivated; }
        }

        public bool IsSuspended {
            get { return State == UserSubscriptionState.Suspended; }
        }
    }
}
