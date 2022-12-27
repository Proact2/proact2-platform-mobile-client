namespace Proact.Mobile.Core.Models {
    public class MessageRequestModel {
        public string Title { get; set; }
        public string Body { get; set; }
        public MessageMood Emotion { get; set; }
        public bool HasAttachment { get; set; }
        public MessageScope MessageScope { get; set; }
    }
}
