using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public class SurveysService : ISurveysService {

        private INetworkRequestService _networkRequestService;

        private const string _baseEndPoint = "SurveyAssegnations";
        private const string _endpointSurveyDetails = "Survey";

        public SurveysService(
            INetworkRequestService networkRequestService ) {
            _networkRequestService = networkRequestService;
        }

        public async Task<ResponseResult<List<SurveyAssignationModel>>> GetMineCompiledSurveys() {
            var url = $"{ _baseEndPoint}/completed/me";
            var result = await _networkRequestService
                .GetRequestAsync<List<SurveyAssignationModel>>(
                ProactServerConfigurations.ApiUrl, url );
            return result;
        }

        public async Task<ResponseResult<List<SurveyAssignationModel>>> GetMineNotCompiledSurveys() {
            var url = $"{ _baseEndPoint}/notcompleted/me";
            var result = await _networkRequestService
                .GetRequestAsync<List<SurveyAssignationModel>>(
                ProactServerConfigurations.ApiUrl, url );
            return result;
        }

        public async Task<ResponseResult<SurveyModel>> GetSurveyDetails( Guid id ) {
            string url = $"{_endpointSurveyDetails}/{id}";
            var result = await _networkRequestService.GetRequestAsync<SurveyModel>(
                 ProactServerConfigurations.ApiUrl, url );
            return result;
        }

        public async Task<ResponseResult> CompileSurvey( SurveyCompileRequest request ) {
            var url = $"{_baseEndPoint}/{request.SurveyId}/{request.AssegnationId}/compile";
            var result = await _networkRequestService
                .PostRequestAsync<string>(
                    ProactServerConfigurations.ApiUrl, url, request );
            return result;
        }

        public async Task<ResponseResult<SurveyCompiledModel>> GetMineCompiledSurvey( Guid assignationId ) {
            string url = $"{_baseEndPoint}/{assignationId}/compiled/me";
            var result = await _networkRequestService.GetRequestAsync<SurveyCompiledModel>(
                 ProactServerConfigurations.ApiUrl, url );
            return result;
        }

        public async Task<int> GetNotCompiledSurveysCount() {
            var surveys = await GetMineNotCompiledSurveys();
            if(surveys.Success ) {
                return surveys.data.Count;
            }
            else {
                return 0;
            }
        }

        public async Task<ResponseResult<List<SurveyAssignationModel>>> GetCompiledSurveys( Guid userId ) {
            var endpoint = $"{_baseEndPoint}/{userId}/completed";
            var result = await _networkRequestService
               .GetRequestAsync<List<SurveyAssignationModel>>(
               ProactServerConfigurations.ApiUrl, endpoint );
            return result;
        }

        public async Task<ResponseResult<SurveyCompiledModel>> GetCompiledSurvey( Guid assignationId ) {
            string url = $"{_baseEndPoint}/{assignationId}/compiled";
            var result = await _networkRequestService.GetRequestAsync<SurveyCompiledModel>(
                 ProactServerConfigurations.ApiUrl, url );
            return result;
        }

        public async Task<ResponseResult<List<SurveyModel>>> GetSurveysOfProject( Guid projectId ) {
            var url = $"{ _endpointSurveyDetails}/{projectId}/all";
            var result = await _networkRequestService
                .GetRequestAsync<List<SurveyModel>>(
                ProactServerConfigurations.ApiUrl, url );
            return result;
        }

        public async Task<ResponseResult<SurveyStatsModel>> GetSurveysStats( Guid surveyId ) {
            var url = $"{ _endpointSurveyDetails}/{surveyId}/stats";
            var result = await _networkRequestService
                .GetRequestAsync<SurveyStatsModel>(
                ProactServerConfigurations.ApiUrl, url );
            return result;
        }

        public async Task<ResponseResult<List<SurveyAssignationModel>>> GetPatientsAssignedToASurvey( Guid surveyId ) {
            var url = $"{ _baseEndPoint}/{surveyId}/patients";
            var result = await _networkRequestService
                  .GetRequestAsync<List<SurveyAssignationModel>>(
                  ProactServerConfigurations.ApiUrl, url );
            return result;
        }
    }
}
