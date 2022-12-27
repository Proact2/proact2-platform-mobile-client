using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Proact.Mobile.Core.Models;
using Proact.Mobile.Core.Services;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {

    public class SearchMessagesViewModel : BaseViewModel {

        public Command SelectionChangedCommand { get; set; }
        public Command<string>  PerformSearchCommand { get; set; }

        private ObservableCollection<MessageModel> _messages;
        public ObservableCollection<MessageModel> Messages {
            get => _messages;
            set => SetProperty( ref _messages, value );
        }

        public bool _messageListIsEmpty;
        public bool MessageListIsEmpty {
            get => _messageListIsEmpty;
            set => SetProperty( ref _messageListIsEmpty, value );
        }

        public bool _placeholderLabelIsVisible;
        public bool PlaceholderLabelIsVisible {
            get => _placeholderLabelIsVisible;
            set => SetProperty( ref _placeholderLabelIsVisible, value );
        }

        private bool _searching;
        public bool Searching {
            get => _searching;
            set {
                _searching = value;
                IsBusy = value;
                if ( _searching ) {
                    PlaceholderLabelIsVisible = false;
                    MessageListIsEmpty = false;
                    Messages = null;
                }
            }
        }

        public MessageModel SelectedMessage { get; set; }

        private IMessagesService _messagesService;

        public SearchMessagesViewModel(IMessagesService messagesService) {
            _messagesService = messagesService;
        }

        public override void Prepare() {
            base.Prepare();
            InitUICommand();
            InitUI();
        }

        private void InitUI() {
            PageTitle = Resources.AppResources.SearchPageTitle;
            PlaceholderLabelIsVisible = true;
        }

        private void InitUICommand() {
            SelectionChangedCommand
                = new Command( SelectionChanged );
            PerformSearchCommand
                = new Command<string>( PerformSearch  );
        }

        private async void PerformSearch( string searchText ) {
            Searching = true;
            var result = await _messagesService
                .MedicSearchMessage( searchText, null, null );
            Searching = false;
            if ( result.Success ) {
                Messages = new ObservableCollection<MessageModel>( result.data );
            }
            CheckEmptyList();
        }

        private void CheckEmptyList() {
            MessageListIsEmpty = ( Messages == null || Messages.Count == 0 );
        }

        private async void SelectionChanged( ) {
            if ( SelectedMessage.IsBroadcastMessage ) {
                return;
            }

            _popupService.OpenLoadingPopup();
            var messageContainer = await GetMessageDetailsAsync();
            await _popupService.CloseAllPopup();

            UnselectMessage();
            await _navigationService
                  .Navigate<WallMessageDetailsViewModel,
                  MessagesContainer, MessagesContainer>( messageContainer );
        }

        private async Task<MessagesContainer> GetMessageDetailsAsync() {
            Guid originalMessageId;
            if ( SelectedMessage.OriginalMessageId == null
                || SelectedMessage.OriginalMessageId == Guid.Empty ) {
                originalMessageId = ( Guid )SelectedMessage.MessageId;
            }
            else {
                originalMessageId = ( Guid )SelectedMessage.OriginalMessageId;
            }

            var result = await _messagesService.GetMessage( originalMessageId );
            return result.data;
        }

        private async void UnselectMessage() {
            //SelectedMessage = null;
            //await RaisePropertyChanged( () => SelectedMessage );
        }
    }
}
