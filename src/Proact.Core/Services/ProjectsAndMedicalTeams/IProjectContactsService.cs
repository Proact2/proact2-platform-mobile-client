using System;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public interface IProjectContactsService {
        Task<ResponseResult<ProjectContactsModel>> GetContacts();
    }
}
