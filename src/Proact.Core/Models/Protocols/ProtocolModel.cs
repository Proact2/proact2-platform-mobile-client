using System;
namespace Proact.Mobile.Core {
    public class ProtocolModel {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string InternalCode { get; set; }
        public string InternationalCode { get; set; }
        public string Url { get; set; }
    }

    public class PatientProtocolsModel {
        public ProtocolModel ProjectProtocol { get; set; }
        public ProtocolModel UserProtocol { get; set; }
    }
}
