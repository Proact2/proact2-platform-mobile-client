using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public interface ISurveysService {
        Task<ResponseResult<List<SurveyAssignationModel>>> GetMineCompiledSurveys();
        Task<ResponseResult<List<SurveyAssignationModel>>> GetMineNotCompiledSurveys();
        Task<ResponseResult<SurveyModel>> GetSurveyDetails( Guid id );
        Task<ResponseResult<SurveyCompiledModel>> GetMineCompiledSurvey( Guid assignationId );
        Task<ResponseResult> CompileSurvey( SurveyCompileRequest request );
        Task<int> GetNotCompiledSurveysCount();

        Task<ResponseResult<List<SurveyAssignationModel>>> GetCompiledSurveys( Guid userId );
        Task<ResponseResult<SurveyCompiledModel>> GetCompiledSurvey( Guid assignationId );

        Task<ResponseResult<List<SurveyModel>>> GetSurveysOfProject( Guid projectId );
        Task<ResponseResult<SurveyStatsModel>> GetSurveysStats( Guid surveyId );

        Task<ResponseResult<List<SurveyAssignationModel>>> GetPatientsAssignedToASurvey( Guid surveyId );
    }
}
