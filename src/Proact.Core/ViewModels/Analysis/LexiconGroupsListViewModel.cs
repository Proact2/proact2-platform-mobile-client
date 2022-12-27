using System;
using System.Collections.ObjectModel;
using Proact.Mobile.Core.Models;
using Xamarin.Forms;
namespace Proact.Mobile.Core.ViewModels {
    public class LexiconGroupsListViewModel : BaseViewModel<LexiconCategoryModel, string> {

        public Command OnItemSelectedCommand { get; set; }
        public string SelectedGroup { get; set; }
        public ObservableCollection<string> Groups {
            get => _groups;
            set => SetProperty( ref _groups, value );
        }
        private ObservableCollection<string> _groups;
        private LexiconCategoryModel _category;


        public override void Prepare( LexiconCategoryModel category ) {
            _category = category;
            InitUi();
            InitCommand();
        }
        private void InitUi() {
            PageTitle = Resources.AppResources.AnalysisGroupPageTitle;
            try {
                Groups = new ObservableCollection<string>( _category.LabelsGroups );
            }
            catch ( Exception ) { };
        
        }

        private void InitCommand() {
            OnItemSelectedCommand = new Command( OnItemSelected );
        }
        private async void OnItemSelected() {
            await _navigationService.Close( this, SelectedGroup );
        }
    }
}
