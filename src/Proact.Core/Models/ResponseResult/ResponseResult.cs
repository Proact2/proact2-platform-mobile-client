using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Proact.Mobile.Core {

    public class ResponseResult {
        public bool Success {
            get {
                return errorException == null
                    && httpResponseMessage != null
                    && httpResponseMessage.StatusCode == HttpStatusCode.OK;
            }
        }

        public HttpResponseMessage httpResponseMessage { get; set; }
        public Exception errorException { get; set; }
    }

    public class ResponseResult<T> : ResponseResult {
        public T data { get; set; }
    }
}
