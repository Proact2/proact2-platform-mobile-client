using System;
using System.Collections.Generic;

namespace Proact.Mobile.Core.Models {
    public class AddAnalysisRequest {
        public Guid MessageId { get; set; }
        public List<AddAnalysisResultModel> AnalysisResults { get; set; }
    }

    public class AddAnalysisResultModel {
        public Guid LabelId { get; set; }
    }
}
