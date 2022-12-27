using System;
using System.Threading.Tasks;
using Proact.Mobile.Core.Models;

namespace Proact.Mobile.Core {
    public interface IInstituteService {
        Task<ResponseResult<InstituteModel>> GetMyInstitute();
    }
}
