using System;
namespace Proact.Mobile.Core {
    public class RequiredUpdateModel {
        public int AndroidLastBuildRequired { get; set; }
        public int IosLastBuildRequired { get; set; }
        public string AndroidStoreUrl { get; set; }
        public string IosStoreUrl { get; set; }
    }
}
