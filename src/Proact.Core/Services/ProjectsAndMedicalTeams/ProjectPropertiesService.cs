using System;
using Proact.Mobile.Core.Models;
namespace Proact.Mobile.Core {
    public class ProjectPropertiesService : IProjectPropertiesService {

        private ILocalDataReadService _localDataReadService;
        
        public ProjectPropertiesService( ILocalDataReadService localDataReadService ) {
            _localDataReadService = localDataReadService;
        }
        
        public bool MessageCanBeDeleted( MessageModel messageModel ) {
             return ( DateTime.Now - messageModel.CreatedDatetime.ToLocalTime() )
                 .TotalMinutes <= MinutesAfterWitchMessageCantBeDeleted();
        }
        
        public bool MessageCanBeReplied( MessageModel messageModel ) {
            var x = ( DateTime.Now - messageModel.CreatedDatetime.ToLocalTime() )
                .TotalMinutes;

            if ( CurrentUserIsMedicalProfessionalOrNurse() ) {
                return ( DateTime.Now - messageModel.CreatedDatetime.ToLocalTime() )
                    .TotalMinutes > MinutesAfterWitchMessageCantBeReplied();
            }
            return true;
        }

        public bool MessageCanBeAnalyzed( MessageModel messageModel ) {
            if ( CurrentUserIsMedicalProfessionalOrNurse() || CurrentUserIsResearcher() ) {
                return ( DateTime.Now - messageModel.CreatedDatetime.ToLocalTime() )
                    .TotalMinutes > MinutesAfterWitchMessageCanBeAnalyzed();
            }
            return false;
        }
        
        public bool MedicsCanSeeOtherAnalisys() {
            return _localDataReadService.GetProjectModel()
                .Properties.MedicsCanSeeOtherAnalisys;
        }

        private bool CurrentUserIsMedicalProfessionalOrNurse() {
            return _localDataReadService.GetUserData()
                    .UserIsInRole( UserRolesModel.MedicalProfessional )
                    || _localDataReadService.GetUserData()
                    .UserIsInRole( UserRolesModel.Nurse );
        }

        private bool CurrentUserIsResearcher() {
            return _localDataReadService.GetUserData()
                    .UserIsInRole( UserRolesModel.Researcher );
        }

        private bool CurrentUserIsPatient() {
            return _localDataReadService.GetUserData()
                .UserIsInRole( UserRolesModel.Patient );
        }

        public int MinutesAfterWitchMessageCantBeDeleted() {
            var props 
                = _localDataReadService.GetProjectModel().Properties;
            return props?.MessageCanNotBeDeletedAfterMinutes ?? 0;
        }
        
        public int MinutesAfterWitchMessageCantBeReplied() {
            var props 
                = _localDataReadService.GetProjectModel().Properties;
            return props?.MessageCanBeRepliedAfterMinutes ?? 0;
        }
        
        public bool IsAnalystConsoleActive() {
            var props 
                = _localDataReadService.GetProjectModel().Properties;
            return props?.IsAnalystConsoleActive ?? false;
        }
        
        public int MinutesAfterWitchMessageCanBeAnalyzed() {
            var props 
                = _localDataReadService.GetProjectModel().Properties;
            return props?.MessageCanBeAnalizedAfterMinutes ?? 0;
        }

        public bool IsSurveysSystemActive() {
            var props 
                = _localDataReadService.GetProjectModel().Properties;
            return props?.IsSurveysSystemActive ?? false;
        }

        public bool IsMessagingActive() {
            var props
                = _localDataReadService.GetProjectModel().Properties;
            return props?.IsMessagingActive ?? false;
        }
    }
}
