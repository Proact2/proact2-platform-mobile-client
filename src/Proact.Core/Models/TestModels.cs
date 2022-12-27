using System;
using System.Collections.Generic;
using System.Text;

namespace Proact.Mobile.Core.Models {
    public class Test_UserModel {
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public int userId { get; set; }
    }

    public class Test_CommentItemModel {
        public int postId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string body { get; set; }
    }
}
