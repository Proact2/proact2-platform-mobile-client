using System;
using System.IO;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public interface INetworkRequestService {
        Task<ResponseResult<T>> GetRequestAsync<T>
            ( string baseUrl, string endPoint );
        Task<ResponseResult<T>> PostRequestAsync<T>
            ( string baseUrl, string endPoint, object objectToSend );
        Task<ResponseResult<T>> PutRequestAsync<T>
            ( string baseUrl, string endPoint, object objectToSend );
        Task<ResponseResult<T>>
            DeleteRequestAsync<T>( string baseUrl, string endPoint );
        Task<ResponseResult<T>> PostRequestWithMultipartFormData<T>(
            string baseUrl, string endPoint, string mediaFileParamName, Stream mediaFileStream );
    }
}