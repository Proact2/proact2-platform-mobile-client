using Proact.Mobile.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Proact.Mobile.Core.Services {
    public interface ITestRequestService {
        Task<ResponseResult<List<Test_CommentItemModel>>> GetComments();
        Task<ResponseResult<List<Test_CommentItemModel>>> GetCommentWithId( int id );
        Task<ResponseResult<Test_UserModel>> CreateResourse( Test_UserModel test_UserModel );
    }
}
