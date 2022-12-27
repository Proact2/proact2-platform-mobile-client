using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {

    public class ProjectsListViewModel : BaseViewModel {
        public Command ListRefreshCommand { get; private set; }
        public Command SelectionChanged { get; set; }
        public List<ProjectModel> Projects { get; set; }
        public ProjectModel SelectedProject { get; set; }
        public bool ListIsEmpty { get; set; }

        private IProjectsRequestService _projectsService;
        private IMedicalTeamRequestService _medicalTeamService;
        private ILocalDataWriteService _localDataWriteService;

        public ProjectsListViewModel(
            IProjectsRequestService projectsService,
            IMedicalTeamRequestService medicalTeamService,
            ILocalDataWriteService localDataWriteService ) {
            _projectsService = projectsService;
            _medicalTeamService = medicalTeamService;
            _localDataWriteService = localDataWriteService;
        }

        public override void Prepare() {
            base.Prepare();
            InitUi();
            InitUiCommands();
        }

        public override void ViewAppeared() {
            base.ViewAppeared();
            LoadProjectsAsync();
        }

        private void InitUi() {
            PageTitle = Resources.AppResources.ProjectsListPageTitle;
        }

        private void InitUiCommands() {
            SelectionChanged
                = new Command( OnProjectSelection );
            ListRefreshCommand
                = new Command( LoadProjectsAsync );
        }

        private async void LoadProjectsAsync() {
            IsBusy = true;
            var response = await _projectsService.GetCurrentUserProjects();
            if ( response.Success ) {
                Projects = response.data
                    .Where( p => !p.IsClosed ).ToList();
                await RaisePropertyChanged( () => Projects );
            }
            else {
                OpenErrorMessagePopup(
                    Resources.AppResources.ProjectsListRequestError );
            }
            IsBusy = false;
            await UpdateEmptyListMessage();
        }

        private async Task UpdateEmptyListMessage() {
            ListIsEmpty = ( Projects == null || Projects.Count == 0 );
            await RaisePropertyChanged( () => ListIsEmpty );
        }

        private async void OpenMedicalTeamsSelectionPage() {
            await _navigationService.Navigate<MedicalTeamListViewModel, ProjectModel>( SelectedProject );
        }

      
        private void OnProjectSelection() {
            OpenMedicalTeamsSelectionPage();
        }

        private void OpenErrorMessagePopup( string errorMessage ) {
            var message = new PopupMessageModel();
            message.Type = PopupMessageType.ERROR;
            message.MessageText = errorMessage;

            _popupService.OpenMessagePopup( message );
        }
    }
}
