using System;
using System.Net.Http;
using System.Threading.Tasks;
using Proact.Mobile.Core.Models;

namespace Proact.Mobile.Core {
    public interface IPopupService {

        Task CloseAllPopup();
        void OpenLoadingPopup();
        void OpenMessagePopup( PopupMessageModel popupMessageModel );
        void OpenEncryptedAudioPlayerPopup( MediaFileDecryptModel mediaFileDecrypt );
        void OpenNotEncryptedAudioPlayerPopup( MediaFileDecryptModel mediaFileDecrypt );
        void OpenNewMessageTypeSelectionPopup( IOpenNewMessagePagesBehaviour openNewMessagePagesBehaviour );
        bool OnBackButtonPressed();
        void OpenDebugMessagePopup( HttpResponseMessage httpResponseMessage );
        void OpenSigninConditionsPopup( ILandingViewModel landingViewModel );
        void OpenUrgentMedicalAdvicePopup( ILandingViewModel landingViewModel );
        void OpenSurveyResultsPopup( bool badResults, ISurveyResultsPopupCallback callback );
        void OpenForceUpdatePopup();

    }
}
