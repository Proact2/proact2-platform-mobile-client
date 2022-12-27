using System;
using System.Collections.Generic;

namespace Proact.Mobile.Core.Models {

    public enum InstituteState {
        Open,
        Closed,
    }

    public class InstituteModel {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public InstitutePropertiesModel Properties { get; set; }
        public InstituteState State { get; set; }
        public List<UserModel> Admins { get; set; }
    }

    public class InstitutePropertiesModel {
        public DocumentModel TermsAndConditions { get; set; }
        public DocumentModel PrivacyPolicy { get; set; }
    }
}