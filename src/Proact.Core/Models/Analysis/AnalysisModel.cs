using System;
using System.Collections.Generic;
namespace Proact.Mobile.Core.Models {
 
    public enum AnalysisState {
        Current,
        Deprecated,
        Deleted
    }
    
    public class AnalysisResumeModel {
        public DateTime LastUpdate { get; set; }
        public UserModel LastUpdateBy { get; set; }
        public int AnalysisCount { get; set; }
        public List<AnalysisModel> Analysis { get; set; }
    }

    public class AnalysisModel {
        public Guid AnalysisId { get; set; }
        public UserModel Author { get; set; }
        public DateTime CreationDate { get; set; }
        public AnalysisState State { get; set; }
        public List<AnalysisResultGroupByCategoryModel> ResultsGroupByCategories { get; set; }
        public bool ResultsVisible { get; set; }
        public bool IsMine { get; set; }
        
        public string FormattedCreationDatetime => CreationDate.ToLocalTime().ToString( "g" );
    }
    
    public class AnalysisResultModel {
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }
        public string ResultLabel { get; set; }
        public Guid LabelId { get; set; }
    }

    public class AnalysisResultGroupByCategoryModel {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<AnalysisResultModel> Results { get; set; }

        public string AllResultsLabels {
            get {
                var separator = " - ";
                var labels = string.Empty;
                if ( Results != null ) {
                    foreach ( var resultModel in Results ) {
                        labels += separator + resultModel.ResultLabel;
                    }
                }
                return labels.Substring( separator.Length );
            }
        }
    }
}
