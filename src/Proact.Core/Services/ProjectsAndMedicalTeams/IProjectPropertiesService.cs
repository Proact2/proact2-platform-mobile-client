using Proact.Mobile.Core.Models;
namespace Proact.Mobile.Core {
    public interface IProjectPropertiesService {
        bool MessageCanBeDeleted( MessageModel messageModel);
        bool MessageCanBeReplied( MessageModel messageModel );
        bool MessageCanBeAnalyzed( MessageModel messageModel );
        bool MedicsCanSeeOtherAnalisys();
        int MinutesAfterWitchMessageCantBeDeleted();
        int MinutesAfterWitchMessageCanBeAnalyzed();
        bool IsAnalystConsoleActive();
        bool IsSurveysSystemActive();
        bool IsMessagingActive();
    }
}
