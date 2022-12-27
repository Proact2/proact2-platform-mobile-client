using System;
using System.Net.Http;
using System.Threading.Tasks;
using Proact.Mobile.Core;
using Proact.Mobile.Core.Models;
using Proact.UI;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Proact.Mobile.UI {
    public class PopupService : IPopupService {

        public async Task CloseAllPopup() {
            if ( PopupNavigation.Instance.PopupStack.Count > 0 ) {
                await PopupNavigation.Instance.PopAllAsync();
            }
        }

        public async void OpenLoadingPopup() {
            await CloseAllPopup();
            await PopupNavigation.Instance
                .PushAsync( new LoadingPopup() );
        }

        public async void OpenMessagePopup( PopupMessageModel popupMessageModel ) {
            await CloseAllPopup();
            await PopupNavigation.Instance
                .PushAsync( new MessagePopup( popupMessageModel ) );
        }

        public async void OpenEncryptedAudioPlayerPopup( MediaFileDecryptModel mediaFileDecrypt ) {
            await CloseAllPopup();

            if ( Device.RuntimePlatform == Device.Android ) {
                await PopupNavigation.Instance
                    .PushAsync( new AndroidAudioPlayerPopup( mediaFileDecrypt ) );
            }
            else if ( Device.RuntimePlatform == Device.iOS ) {
                await PopupNavigation.Instance
                    .PushAsync( new AudioPlayerPopup( mediaFileDecrypt, true ) );
            }
        }

        public async void OpenNotEncryptedAudioPlayerPopup( MediaFileDecryptModel mediaFileDecrypt ) {
            await PopupNavigation.Instance
                    .PushAsync( new AudioPlayerPopup( mediaFileDecrypt, false ) );
        }

        public async void OpenSigninConditionsPopup( ILandingViewModel landingViewModel ) {
            await CloseAllPopup();
            await PopupNavigation.Instance
                .PushAsync( new SigninConditionsPopup( landingViewModel ) );
        }

        public async void OpenUrgentMedicalAdvicePopup( ILandingViewModel landingViewModel ) {
            await CloseAllPopup();
            await PopupNavigation.Instance
                .PushAsync( new UrgentMedicalAdvicePopup( landingViewModel ) );
        }

        public async void OpenNewMessageTypeSelectionPopup( IOpenNewMessagePagesBehaviour openNewMessagePagesBehaviour ) {
            await PopupNavigation.Instance
                .PushAsync( new NewMessageTypeSelectionPopup( openNewMessagePagesBehaviour ) );
        }

        public async void OpenDebugMessagePopup( HttpResponseMessage httpResponseMessage ) {
            await CloseAllPopup();
            await PopupNavigation.Instance
                .PushAsync( new DebugMessagePopup( httpResponseMessage ) );
        }

        public async void OpenSurveyResultsPopup( bool badResults, ISurveyResultsPopupCallback callback ) {
            var results = badResults? SurveyResults.BAD: SurveyResults.GOOD;
            await CloseAllPopup();
            await PopupNavigation.Instance
                .PushAsync( new SurveyResultsPopup( results, callback ) );
        }

        public async void OpenForceUpdatePopup() {
            await CloseAllPopup();
            await PopupNavigation.Instance
                .PushAsync( new ForceUpdatePopup() );
        }

        public bool OnBackButtonPressed() {
            if ( !CheckIfModalMopupIsVisible() ) {
                if ( PopupNavigation.Instance.PopupStack.Count > 0 ) {
                    CloseAllPopup();
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return true;
            }
        }

        private bool CheckIfModalMopupIsVisible() {
            if( PopupNavigation.Instance.PopupStack.Count > 0 ) {
                var count = PopupNavigation.Instance.PopupStack.Count;
                var popupPage = PopupNavigation.Instance.PopupStack[count - 1];

                if ( popupPage != null ) {
                    return !popupPage.CloseWhenBackgroundIsClicked;
                }
            }
            return false;
        }
    }
}
