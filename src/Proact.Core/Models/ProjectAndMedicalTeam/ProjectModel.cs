using System;
namespace Proact.Mobile.Core {

    public enum ProjectState {
        Open,
        Closed,
    }

    public class ProjectModel {
        public Guid ProjectId { get; set; }
        public ProjectState Status { get; set; }
        public string Name { get; set; }
        public string SponsorName { get; set; }
        public string Description { get; set; }
        public ProjectPropertiesModel Properties { get; set; }
        public bool IsClosed {
            get {
                return Status == ProjectState.Closed;
            }
        }
    }
}
