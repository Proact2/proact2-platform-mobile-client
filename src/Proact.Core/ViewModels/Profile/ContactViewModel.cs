using System;
using Proact.Mobile.Core.ViewModels;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class ContactViewModel : BaseViewModel {

        public HtmlWebViewSource _source;
        public HtmlWebViewSource Source {
            get => _source;
            set => SetProperty( ref _source, value );
        }

        public IProjectContactsService _projectContactsService { get; set; }

        public ContactViewModel( IProjectContactsService projectContactsService ) {
            _projectContactsService = projectContactsService;
        }

        public override void ViewAppeared() {
            base.ViewAppeared();
            UpdateContent();
        }

        private async void UpdateContent() {
            var fontsize = "+1";
            if(Device.RuntimePlatform == Device.iOS ) {
                fontsize = "+4";
            }

            IsBusy = true;
            var result = await _projectContactsService.GetContacts();
            if ( result.Success ) {
                Source = new HtmlWebViewSource();
                Source.Html = $"<font size=\"{ fontsize }\">{result.data.HtmlContent}</font>";
            }
            IsBusy = false;
        }
    }
}
