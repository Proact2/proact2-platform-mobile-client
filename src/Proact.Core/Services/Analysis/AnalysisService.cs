using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proact.Mobile.Core.Models;
namespace Proact.Mobile.Core {
    public class AnalysisService : IAnalysisService{

        private const string _endpointLexicon = "Lexicons";
        private const string _endpointAnalysis = "MessageAnalysis";
        
        private INetworkRequestService _networkRequestService;
        
        public AnalysisService(
            INetworkRequestService networkRequestService ) {
            _networkRequestService = networkRequestService;
        }
        
        public async Task<ResponseResult<LexiconModel>> GetLexicon( Guid lexiconId ) {
            string url = $"{_endpointLexicon}/{lexiconId}";
            var result = await _networkRequestService.GetRequestAsync<LexiconModel>(
                ProactServerConfigurations.ApiUrl, url );
            return result;
        }
        public async Task<ResponseResult<AnalysisResumeModel>> GetAnalysisResume( Guid projectId, Guid medicalTeamId, Guid messageId ) {
            string url = $"{_endpointAnalysis}/{projectId}/{medicalTeamId}/{messageId}";
            var result = await _networkRequestService.GetRequestAsync<AnalysisResumeModel>(
                ProactServerConfigurations.ApiUrl, url );
            return result;
        }
        public async Task<ResponseResult<AnalysisModel>> AddAnalysis( Guid projectId, Guid medicalTeamId, AddAnalysisRequest request ) {
            string url = $"{_endpointAnalysis}/{projectId}/{medicalTeamId}/create";
            var result = await _networkRequestService.PostRequestAsync<AnalysisModel>(
                ProactServerConfigurations.ApiUrl, url, request );

            return result;
        }
        public async Task<ResponseResult> DeleteAnalysis( Guid projectId, Guid medicalTeamId, Guid analysisId ) {
            string url = $"{_endpointAnalysis}/{projectId}/{medicalTeamId}/{analysisId}";
            var result = await _networkRequestService.DeleteRequestAsync<AnalysisModel>( ProactServerConfigurations.ApiUrl, url );
            return result;
        }
        public async Task<ResponseResult<AnalysisModel>> UpdateAnalysis( Guid projectId, Guid medicalTeamId, Guid analysisId, AddAnalysisRequest request ) {
            string url = $"{_endpointAnalysis}/{projectId}/{medicalTeamId}/{analysisId}";
            var result = await _networkRequestService.PutRequestAsync<AnalysisModel>(
                ProactServerConfigurations.ApiUrl, url, request );
            return result;
        }
    }
}
