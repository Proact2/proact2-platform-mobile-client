using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MvvmHelpers;
using Proact.Mobile.Core.Models;
using Proact.Mobile.Core.Services;
using Xamarin.Forms;
namespace Proact.Mobile.Core.ViewModels {
    public class LexiconLabelsListViewModel : BaseViewModel<LexiconCategoryModel> {

        public const string MSG_UPDATE_LABEL = "MSG_UPDATE_LABEL";

        private readonly ObservableRangeCollection<Grouping<string,LexiconLabelModel >> _labels 
            = new ObservableRangeCollection<Grouping<string, LexiconLabelModel>>();
        public ObservableCollection<Grouping<string, LexiconLabelModel>> Labels => _labels;
        public Command<LexiconLabelModel> ItemSelectedCommand { get; set; }
        public Command<string> PerformSearchCommand { get; set; }
        public Command OpenGroupsListPageCommand { get; set; }
        public Command RemoveGroupFilterCommand { get; set; }

        private string _searchQuery;
        public string SearchQuery {
            get => _searchQuery;
            set => SetProperty( ref _searchQuery, value );
        }
        
        public bool GroupFilterIsActive { get; private set; }
        public string GroupFilterMessage { get; private set; } 
        
        private LexiconCategoryModel _category;
        private String _selectedGroupFilter;

        public override void Prepare( LexiconCategoryModel parameter ) {
            _category = parameter;
            InitUi();
            InitCommand();

        }

        private void InitUi() {
            PageTitle = _category.Description;
            PerformAlphabeticalGrouping( _category.Labels );
        }
 
        private void InitCommand() {
            ItemSelectedCommand 
                = new Command<LexiconLabelModel>( OnItemSelected );
            PerformSearchCommand
                = new Command<string>( PerformSearch );
            OpenGroupsListPageCommand 
                = new Command( OpenGroupListPage );
            RemoveGroupFilterCommand 
                = new Command( RemoveGroupFilter );
        }
     
        private async void OnItemSelected( LexiconLabelModel itemSelected ) {
            if ( !_category.MultipleSelection ) {
                _category.SelectedLabels = new ObservableCollection<LexiconLabelModel>();
            }

            if ( !await ValidateSelection( itemSelected ) )
                return;

            _category.SelectedLabels.Add( itemSelected );
            MessagingCenter.Send( this,MSG_UPDATE_LABEL, _category );
            await _navigationService.Close( this );
        }

        private async Task<bool> ValidateSelection( LexiconLabelModel itemSelected ) {
            if ( _category.SelectedLabels
                    .FirstOrDefault( x => x.Id == itemSelected.Id ) == null ) {
                return true;
            }
           
            await Application.Current.MainPage.DisplayAlert(
                Resources.AppResources.AnalysisLabelAlreadyExistAlertMessage,
                string.Empty,
                Resources.AppResources.Ok );
            return false;
        }
        
        private void PerformSearch( string searchQuery ) {
            var searchList = _category.Labels
                .Where( x => x.Label.ToLowerInvariant()
                    .Contains( searchQuery.ToLowerInvariant() ) ).ToList();

            if ( !String.IsNullOrEmpty( _selectedGroupFilter ) ) {
                searchList = searchList
                    .Where( c => c.groupName == _selectedGroupFilter )
                    .ToList();
            }

            PerformAlphabeticalGrouping( searchList );
        }

        private void ResetSearch() {
            SearchQuery = string.Empty;
            PerformSearch( SearchQuery );
        }

        private void PerformAlphabeticalGrouping( List<LexiconLabelModel> list ) {
            var sorted = from item in list
                orderby item.Label
                group item by item.Label[0].ToString().ToUpperInvariant() into itemGroup
                select new Grouping<string, LexiconLabelModel>(itemGroup.Key, itemGroup);

            _labels.ReplaceRange(sorted);
        }

        private async void OpenGroupListPage() {
            _selectedGroupFilter = await _navigationService
                .Navigate<LexiconGroupsListViewModel, LexiconCategoryModel, String>( _category );
           ApplyGroupFilter();
        }

        private void ApplyGroupFilter() {
            ResetSearch();
            GroupFilterIsActive = true;
            GroupFilterMessage = String.Format( 
                Resources.AppResources.AnalysisGroupFilterActiveMessage, 
                _selectedGroupFilter );
            
            RaisePropertyChanged( () => GroupFilterIsActive );
            RaisePropertyChanged( () => GroupFilterMessage );
        }

        private void RemoveGroupFilter() {
            _selectedGroupFilter = string.Empty;
            GroupFilterIsActive = false;
            ResetSearch();
            RaisePropertyChanged( () => GroupFilterIsActive );
        }
        
    }
}
