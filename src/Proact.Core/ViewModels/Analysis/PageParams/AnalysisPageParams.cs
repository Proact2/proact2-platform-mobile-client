using System;
using Proact.Mobile.Core.Models;
namespace Proact.Mobile.Core.ViewModels {
    public class AnalysisPageParams {

        public MessageModel MessageModel { get; set; }
        public Guid ProjectId { get; set; }
        public Guid MedicalTeamId { get; set; }

        public AnalysisModel AnalysisToEdit { get; set; }

        public bool IsEditMode {
            get => AnalysisToEdit != null;
        }
    }
}
