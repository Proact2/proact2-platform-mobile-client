using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace Proact.Mobile.Core.ViewModels {
   
    public class MedicalTeamListViewModel : BaseViewModel<ProjectModel>{
        public Command ListRefreshCommand { get; private set; }
        public Command SelectionChanged { get; set; }
        public List<MedicalTeamModel> MedicalTeams { get; set; }
        public MedicalTeamModel SelectedMedicalTeam { get; set; }
        public bool ListIsEmpty { get; set; }
        
        private IMedicalTeamRequestService _medicalTeamService;
        private ILocalDataWriteService _localDataWriteService;
        private IProjectPropertiesService _projectPropertiesService;

        private ProjectModel _selectedProject;
        
        public MedicalTeamListViewModel(
            IMedicalTeamRequestService medicalTeamService,
            ILocalDataWriteService localDataWriteService,
            IProjectPropertiesService projectPropertiesService ) {
            _medicalTeamService = medicalTeamService;
            _localDataWriteService = localDataWriteService;
            _projectPropertiesService = projectPropertiesService;
        }

        public override void Prepare( ProjectModel projectModel ) {
            base.Prepare();
            _selectedProject = projectModel;
            InitUi();
            InitCommands();
        }

        public override void ViewAppeared() {
            base.ViewAppeared();
            LoadDataAsync();
        }

        private void InitUi() {
            PageTitle = Resources.AppResources.MedicalTeamsListPageTitle;
        }

        private void InitCommands() {
            SelectionChanged
                = new Command( OnMedicalTeamSelected );
            ListRefreshCommand
                = new Command( LoadDataAsync );
        }
        private async void LoadDataAsync() {
            IsBusy = true;
            var response = await _medicalTeamService.GetCurrentUserMedicalTeam( _selectedProject.ProjectId );
            if ( response.Success ) {
                MedicalTeams = response.data
                    .Where(mt => !mt.IsClosed  ).ToList();
                await RaisePropertyChanged( () => MedicalTeams );
            }
            else {
                OpenErrorMessagePopup(
                    Resources.AppResources.MedicalTeamsListRequestError );
            }
            IsBusy = false;
            await UpdateEmptyListMessage();
        }
        private void OnMedicalTeamSelected() {
            if ( SelectedMedicalTeam != null ) {
                SaveSelectedProject();
                SaveMedicalTeam();
                OpenMainPage();
            }
        }
        
        private void SaveMedicalTeam() {
            _localDataWriteService.SetMedicalTeamData( SelectedMedicalTeam );
        }

        private void SaveSelectedProject() {
            _localDataWriteService.SetProjectData( _selectedProject );
        }

        private async Task OpenMainPage() {
            var isMessagingActive = _projectPropertiesService.IsMessagingActive();

            if ( isMessagingActive ) {
                await _navigationService.Navigate<WallMessagesViewModel>();
            }
            else {
                await _navigationService.Navigate<ProfileViewModel>();
            }
        }

        private async Task UpdateEmptyListMessage() {
            ListIsEmpty = ( MedicalTeams == null || MedicalTeams.Count == 0 );
            await RaisePropertyChanged( () => ListIsEmpty );
        }
        
        private void OpenErrorMessagePopup( string errorMessage ) {
            var message = new PopupMessageModel();
            message.Type = PopupMessageType.ERROR;
            message.MessageText = errorMessage;

            _popupService.OpenMessagePopup( message );
        }
    }
}
