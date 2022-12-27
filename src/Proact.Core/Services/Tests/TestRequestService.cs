using Proact.Mobile.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Proact.Mobile.Core.Services {
    public class TestRequestService : ITestRequestService {

        private INetworkRequestService _networkRequestService;

        private string _baseUrl = "https://jsonplaceholder.typicode.com/";

        public TestRequestService( INetworkRequestService networkRequestService ) {
            _networkRequestService = networkRequestService;
        }

        public async Task<ResponseResult<Test_UserModel>> CreateResourse( Test_UserModel test_UserModel ) {
            return await _networkRequestService.PostRequestAsync
                <Test_UserModel>( _baseUrl, "posts", test_UserModel );
        }

        public async Task<ResponseResult<List<Test_CommentItemModel>>> GetComments() {
            return await _networkRequestService.GetRequestAsync
                <List<Test_CommentItemModel>>( _baseUrl, "comments" );
        }

        public async Task<ResponseResult<List<Test_CommentItemModel>>> GetCommentWithId( int id ) {
            return await _networkRequestService.GetRequestAsync
                <List<Test_CommentItemModel>>( _baseUrl, "comments?id=" + id );
        }
    }
}
