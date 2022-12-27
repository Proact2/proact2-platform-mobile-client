using System;
using System.Net.Http;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Proact.Mobile.Core.Resources;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public abstract class BaseViewModel : MvxViewModel {

        public IMvxAsyncCommand ClosePageCommand { get; private set; }
        protected IPopupService _popupService;

        public IPopupService PopupService {
            get => _popupService;
        }

        private bool _isBusy;
        public bool IsBusy {
            get => _isBusy;
            set => SetProperty( ref _isBusy, value );
        }

        private string _pageTitle;
        public string PageTitle {
            get => _pageTitle;
            set => SetProperty( ref _pageTitle, value );
        }

        private MessageBoxModel _messageBoxModel;
        public MessageBoxModel MessageBoxModel {
            get => _messageBoxModel;
            set => SetProperty( ref _messageBoxModel, value );
        }

        protected IMvxNavigationService _navigationService;

        public BaseViewModel() {
            _navigationService
                = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
            ClosePageCommand
                = new MvxAsyncCommand( ClosePage );
        }

        protected void ExecuteAsyncCall(Func<Task> iAsyncAction ) {
            IsBusy = true;
            MvxNotifyTask.Create( iAsyncAction );
        }

        protected void ExecuteCallEnd() {
            IsBusy = false;
        }

        protected void ProcessLoadDataFromServerFail( ResponseResult responseResult ) {
            int code
                = (int)responseResult.httpResponseMessage.StatusCode;
        }

        public virtual async Task ClosePage() {
            await _navigationService.Close( this );
        }

        public void SetPopupService( IPopupService popupService ) {
            _popupService = popupService;
        }

        protected void ShowDebugHttpErrorMessage( HttpResponseMessage httpResponseMessage ) {
#if DEBUG
            _popupService.OpenDebugMessagePopup( httpResponseMessage );
#endif
        }

        //-- ci permette di mettere in binding il file delle risorse con le stringhe
        public string this[string index]
            => AppResources.ResourceManager.GetString( index );
    }

}
