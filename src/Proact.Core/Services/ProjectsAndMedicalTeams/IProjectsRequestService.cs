using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public interface IProjectsRequestService {
        Task<ResponseResult<List<ProjectModel>>> GetCurrentUserProjects();
    }
}
