using System;
using Proact.Mobile.Core.Models;

namespace Proact.Mobile.Core {
    public interface ILandingViewModel {

        void OpenTermsAndConditionsPage();
        void OpenLicensePage();
        void OpenPrivacyPolicyPage();
        void TermsAndConditionAcceptedActionHandle();
        void UrgentMedicalAdviceAcceptedActionHandle();
        DocumentModel GetTermsAndConditionsModel();
        DocumentModel GetPrivacyPolicyModel();
    }
}
