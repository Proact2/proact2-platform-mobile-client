using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Proact.Mobile.Core.Services;

namespace Proact.Mobile.Core {
    public class SigninService : ISigninService {

        public event EventHandler<ErrorModel> OnSigninErrorError;

        private IAuthService _authService;

        private ILocalDataWriteService _localDataWriteService;
        private ILocalDataReadService _localDateReadService;
        private IUserProfileRequestService _userProfileRequestService;
        private IProjectsRequestService _projectsRequestService;
        private IMedicalTeamRequestService _medicalTeamRequestService;
        private ISettingsRequestService _settingsRequestService;

        private AuthDataModel _authDataModel;

        public SigninService(
            IAuthService authService,
            ILocalDataWriteService localDataWriteService,
            ILocalDataReadService localDataReadService,
            IUserProfileRequestService userProfileRequestService,
            IProjectsRequestService projectsRequestService,
            IMedicalTeamRequestService medicalTeamRequestService,
            ISettingsRequestService settingsRequestService ) {

            _authService = authService;
            _localDataWriteService = localDataWriteService;
            _localDateReadService = localDataReadService;
            _userProfileRequestService = userProfileRequestService;
            _projectsRequestService = projectsRequestService;
            _medicalTeamRequestService = medicalTeamRequestService;
            _settingsRequestService = settingsRequestService;
        }

        public void Init() {
            _authService.Init();
            _authService.OnAuthenticationError += _authService_OnAuthenticationError;
        }

        public async Task PerformAuthenticationAsync() {
            _authDataModel = await _authService
                  .PerformAuthenticationAsync();
        }

        public async Task<SigninResult> PerformSigninAsync() {
            var signinResult = new SigninResult();
            signinResult.authData = _authDataModel;
            if ( _authDataModel.Authorized ) {
                signinResult.userData = await GetAndSaveUserData();
            }

            return signinResult;
        }

        public Task<AuthDataModel> PerformAutoSigninAsync() {
            return _authService.PerformSilentAuthenticationAsync();
        }

        public async Task RetrieveAndSavePatientProjectAndMedicalTeamData() {
            var projectResult = await _projectsRequestService.GetCurrentUserProjects();

            if ( projectResult.Success ) {
                var projectsList = projectResult.data;

                if ( projectsList != null && projectsList.Count != 0 ) {

                    var projectModel = projectsList.First();
                    var medicalTeamResult = await _medicalTeamRequestService
                        .GetCurrentUserMedicalTeam( projectModel.ProjectId );

                    if ( medicalTeamResult.Success ) {
                        var medicalTeamsListModel = medicalTeamResult.data;
                        var medicalTeamModel = medicalTeamsListModel.First();

                        if ( medicalTeamModel != null ) {
                            _localDataWriteService.SetProjectData( projectModel );
                            _localDataWriteService.SetMedicalTeamData( medicalTeamModel );
                        }
                        else {
                            PerformSigninError(
                                Resources.AppResources.AuthErrorTitle,
                                Resources.AppResources.AuthErrorNoMedicalTeamAssociated );
                        }
                    }
                    else {
                        PerformSigninError(
                            Resources.AppResources.AuthErrorTitle,
                            Resources.AppResources.AuthErrorInvalidMedicalTeam );
                    }
                }
                else {
                    PerformSigninError(
                        Resources.AppResources.AuthErrorTitle,
                        Resources.AppResources.AuthErrorNoProjectAssociated );
                }
            }
            else {
                PerformSigninError(
                    Resources.AppResources.AuthErrorTitle,
                    Resources.AppResources.AuthErrorInvalidProject );
            }
        }

        private async Task<UserModel> GetAndSaveUserData() {
            var userDataResult = await _userProfileRequestService
                       .GetCurrenUserProfileData();

            if ( userDataResult.Success ) {
                var userModel = userDataResult.data;

                if ( userModel.State == UserSubscriptionState.Active ) {
                    if ( userModel.UserIsInRole( UserRolesModel.MedicalTeamAdmin )
                        || userModel.UserIsInRole( UserRolesModel.MedicalProfessional )
                        || userModel.UserIsInRole( UserRolesModel.Nurse )
                        || userModel.UserIsInRole( UserRolesModel.Patient )
                        || userModel.UserIsInRole( UserRolesModel.Researcher ) ) {

                        _localDataWriteService.SetUserData( userModel );
                        return userModel;
                    }
                    else {
                        PerformSigninError(
                                Resources.AppResources.AuthErrorTitle,
                                Resources.AppResources.AuthErrorUserNotAuthorized );
                        return null;
                    }
                }
                else {
                    PerformSigninError(
                                Resources.AppResources.AuthErrorTitle,
                                Resources.AppResources.AuthErrorUserInactive );
                    return null;
                }
            }
            else {
                PerformSigninError(
                Resources.AppResources.AuthErrorTitle, string.Empty );
                return null;
            }
        }

        public async Task UpdateLocalProjectAndMedicalTeamData() {
            var project = _localDateReadService.GetProjectModel();
            var medicalTeam = _localDateReadService.GetMedicalTeamModel();

            if ( project != null ) {
                var projectsResult = await _projectsRequestService
                    .GetCurrentUserProjects();

                if ( projectsResult.Success && projectsResult.data.Count > 0 ) {
                    var updatedProject = projectsResult.data
                        .FirstOrDefault( p => p.ProjectId == project.ProjectId );
                    _localDataWriteService.SetProjectData( updatedProject );
                }
            }

            if( medicalTeam != null ) {
                var medicalTeamResult = await _medicalTeamRequestService
                    .GetCurrentUserMedicalTeam( project.ProjectId );

                if ( medicalTeamResult.Success && medicalTeamResult.data.Count > 0 ) {
                    var updatedMedicalTeam = medicalTeamResult.data
                        .FirstOrDefault( m => m.MedicalTeamId == medicalTeam.MedicalTeamId );
                    _localDataWriteService.SetMedicalTeamData( updatedMedicalTeam );
                }
            }
        }

        private void _authService_OnAuthenticationError( object sender, ErrorModel e ) {
            PerformSigninError( e.Code, e.Message );
        }

        private void PerformSigninError( string code, string message ) {
            if ( OnSigninErrorError != null ) {
                var errorModel = new ErrorModel() {
                    Code = code,
                    Message = message
                };

                OnSigninErrorError( this, errorModel );
            }
        }

        public async Task PerformSignout() {
            await _authService.RemoveAuthenticationAsync();
        }
    }
}