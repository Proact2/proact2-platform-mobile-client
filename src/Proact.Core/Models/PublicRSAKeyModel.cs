namespace Proact.Mobile.Core {
    public class PublicRSAKeyModel {
        public string PublicKey { get; set; }
        public string Exponent { get; set; }

        public string XmlFormat {
            get {
                return @"<RSAKeyValue>" +
                     "<Exponent>" + Exponent + "</Exponent>" + 
                     "<Modulus>" + PublicKey + "</Modulus>" +
                     "</RSAKeyValue>";
            }
        }
    }
}
