using System;
using System.Threading.Tasks;
using Proact.Mobile.Core.Models;
namespace Proact.Mobile.Core {
    public interface IAnalysisService {
        Task<ResponseResult<LexiconModel>>  GetLexicon( Guid lexiconId );
        Task<ResponseResult<AnalysisResumeModel>> GetAnalysisResume( Guid projectId, Guid medicalTeamId, Guid messageId );
        Task<ResponseResult<AnalysisModel>> AddAnalysis( Guid projectId, Guid medicalTeamId, AddAnalysisRequest request );
        Task<ResponseResult> DeleteAnalysis( Guid projectId, Guid medicalTeamId, Guid analysisId );
        Task<ResponseResult<AnalysisModel>> UpdateAnalysis( Guid projectId, Guid medicalTeamId, Guid analysisId, AddAnalysisRequest request );
    }
}
