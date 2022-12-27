using Proact.Mobile.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Proact.Mobile.Core.Services {
    public class MessagesService : IMessagesService {
        private INetworkRequestService _networkRequestService;
        private ILocalDataReadService _localDataReadService;

        private const string _createNewMessageEndPoint = "Messages/{0}/{1}/create";
        private const string _createBroadcastNewMessageEndPoint = "Messages/{0}/{1}/createbroadcast";
        private const string _replyToMessageEndPoint = "Messages/{0}/{1}/replyTo/{2}";
        private const string _uploadAttachmentEndPoint = "Attachments/{0}/{1}/{2}/{3}/upload/noencryption";
        private const string _uploadAsyncAttachmentEndPoint = "Attachments/{0}/{1}/{2}/{3}/upload/async/noencryption";
        private const string _editMessageEndPoint = "Messages/{0}/{1}/{2}/edit";
        private const string _deleteMessageEndPoint = "Messages/{0}/{1}/{2}";
        private const string _messagesApiEndPoint = "Messages/{0}/{1}/{2}";
        private const string _messageApiEndPoint = "Messages/{0}/{1}/{2}";
        private const string _unrepliedMessagesApiEndPoint = "Messages/{0}/{1}/unreplied/{2}";
        private const string _mediaFileParamName = "mediaFile";
        private const string _mediaDecryptToken = "Attachments/{0}";
        private const string _patientSearchMessageEndPoint = "Messages/{0}/{1}/patient/search";
        private const string _medicSearchMessageEndPoint = "Messages/{0}/{1}/medic/search";

        public MessagesService( 
            INetworkRequestService networkRequestService, 
            ILocalDataReadService localDataReadService ) {
            _networkRequestService = networkRequestService;
            _localDataReadService = localDataReadService;
        }

        private MessageRequestModel CreateMessageRequest( 
            MessageMood patientMood, string title, string body, bool hasHattachment, MessageScope messageScope ) {

            var messageRequest = new MessageRequestModel() {
                Body = body,
                Title = title,
                Emotion = patientMood,
                HasAttachment = hasHattachment,
                MessageScope = messageScope
            };

            return messageRequest;
        } 

        private ReplyMessageRequestModel CreateReplyMessageRequest(
            MessageMood patientMood, string body, bool hasAttachment, Guid originalMessageId ) {

            var messageRequest = new ReplyMessageRequestModel() {
                Body = body, Title = "", Emotion = patientMood, 
                HasAttachment = hasAttachment, OriginalMessageId = originalMessageId
            };

            return messageRequest;
        }

        private string GetProjectId() {
            return _localDataReadService.GetProjectModel().ProjectId.ToString();
        }

        private string GetMedicalTeamId() {
            return _localDataReadService.GetMedicalTeamModel().MedicalTeamId.ToString();
        }

        public async Task<ResponseResult<MessageModel>> CreateNewMessage( 
            MessageMood patientMood, string body, bool hasAttachment, MessageScope messageScope ) {
            var messageRequest = CreateMessageRequest( patientMood, "", body, hasAttachment, messageScope );

            string url = string.Format( _createNewMessageEndPoint, GetProjectId(), GetMedicalTeamId() );

            var result = await _networkRequestService.PostRequestAsync<MessageModel>(
                ProactServerConfigurations.ApiUrl, url, messageRequest );

            return result;
        }

        public async Task<ResponseResult<MessageModel>> CreateBroadcastMessage( string title, string body ) {
            var encryptedMessage = CreateMessageRequest( MessageMood.None, title, body, false, MessageScope.None );

            string url = string.Format( _createBroadcastNewMessageEndPoint, GetProjectId(), GetMedicalTeamId() );

            var result = await _networkRequestService.PostRequestAsync<MessageModel>(
                ProactServerConfigurations.ApiUrl, url, encryptedMessage );

            return result;
        }

        public async Task<ResponseResult<MessageModel>> CreateNewMessageWithVideoAttached( 
            Stream mediaFileStream ) {
            return await CreateNewMessageWithMediaFileAttached( 
                MessageMood.None, mediaFileStream, AttachmentType.VIDEO );
        }

        public async Task<ResponseResult<MessageModel>> CreateNewMessageWithAudioAttached( 
            Stream mediaFileStream ) {
            return await CreateNewMessageWithMediaFileAttached( 
                MessageMood.None, mediaFileStream, AttachmentType.VOICE );
        }

        private async Task<ResponseResult<MessageModel>> CreateNewMessageWithMediaFileAttached(
            MessageMood patientMood, Stream mediaFileStream, AttachmentType attachmentType  ) {
            var messageContainerResult = await CreateNewMessage( patientMood, "", true, MessageScope.Health );

            if ( messageContainerResult.Success ) {
                return await UploadAttachmentToMessage(
                    (Guid)messageContainerResult.data.MessageId, mediaFileStream, attachmentType );
            }
            else {
                return messageContainerResult;
            }
        }

        public async Task<ResponseResult<MessageModel>> CreateNewMessageWithImageAttached( 
            MessageMood patientMood, string body, Stream mediaFileStream, MessageScope messageScope ) {
            var messageContainerResult = await CreateNewMessage( patientMood, body, true, messageScope );

            if ( messageContainerResult.Success ) {
                return await UploadAttachmentToMessage( 
                    (Guid)messageContainerResult.data.MessageId, mediaFileStream, AttachmentType.IMAGE );
            }
            else {
                return messageContainerResult;
            }
        }

        //private async Task<ResponseResult<MessageModel>> PerformUploadEncodedMediaFile(
        //    Guid messageId, Stream fileStream, AttachmentType attachmentType ) {
        //    //var stopWatch = new Stopwatch();
        //    //stopWatch.Start();
        //    //var streamEncoded = await _mediaEncodingService
        //    //    .EncodeMediaAsync( ( fileStream as FileStream ).Name, attachmentType );

        //    //var attachmentResponseResult = await UploadAttachmentToMessage(
        //    //    messageId, streamEncoded, attachmentType );

        //    var attachmentResponseResult = await UploadAttachmentToMessage(
        //        messageId, fileStream, attachmentType );

  
        //    //_mediaEncodingService.DeleteMedia( ( streamEncoded as FileStream ).Name );
        //    //_mediaEncodingService.DeleteMedia( ( fileStream as FileStream ).Name );

        //    //stopWatch.Stop();
        //    //Console.WriteLine( $">>> UPLOAD TIME: {stopWatch.Elapsed}" );

        //    return attachmentResponseResult;
        //}

        public async Task<ResponseResult<MessageModel>> ReplyToMessage(
            MessageMood patientMood, string body, bool hasAttachment, Guid originalMessageId ) {
            var replyMessageRequest = CreateReplyMessageRequest( 
                patientMood, body, hasAttachment, originalMessageId );

            string url = string.Format(
                _replyToMessageEndPoint, GetProjectId(), GetMedicalTeamId(), originalMessageId );

            var result = await _networkRequestService.PostRequestAsync<MessageModel>(
                ProactServerConfigurations.ApiUrl, url, replyMessageRequest );

            return result;
        }

        public async Task<ResponseResult<MessageModel>> ReplyToMessageWithVideoAttached( 
            Guid originalMessageId, Stream mediaFileStream ) {
            var messageContainerResult = await ReplyToMessage(
                MessageMood.None, "", true, originalMessageId );

            if ( messageContainerResult.Success ) {
                return await UploadAttachmentToMessage( 
                    (Guid)messageContainerResult.data.MessageId, mediaFileStream, AttachmentType.VIDEO );
            }

            return messageContainerResult;
        }

        public async Task<ResponseResult<MessageModel>> ReplyToMessageWithAudioAttached( 
            Guid originalMessageId, Stream mediaFileStream ) {
            var messageContainerResult = await ReplyToMessage(
                MessageMood.None, "",true, originalMessageId );

            if ( messageContainerResult.Success ) {
                return await UploadAttachmentToMessage(
                    (Guid)messageContainerResult.data.MessageId, mediaFileStream, AttachmentType.VOICE );
            }

            return messageContainerResult;
        }

        public async Task<ResponseResult<MessageModel>> ReplyToMessageWithImageAttached(
            string body, Guid originalMessageId, Stream mediaFileStream ) {
            var messageContainerResult = await ReplyToMessage(
                MessageMood.None, body, true, originalMessageId );

            if ( messageContainerResult.Success ) {
                return await UploadAttachmentToMessage(
                    (Guid)messageContainerResult.data.MessageId, mediaFileStream, AttachmentType.IMAGE );
            }

            return messageContainerResult;
        }

        private async Task<ResponseResult<MessageModel>> UploadAttachmentToMessage(
            Guid messageId, Stream mediaFileStream, AttachmentType attachmentType ) {
            string url = $"Attachments/{messageId}/{( int )attachmentType}";
            return await PerformUpload( url, mediaFileStream );
        }

        private async Task<ResponseResult<MessageModel>> PerformUpload( string url, Stream mediaFileStream ) {
            var result = await _networkRequestService.PostRequestWithMultipartFormData<MessageModel>(
                    ProactServerConfigurations.ApiUrl, url, _mediaFileParamName, mediaFileStream );

            return result;
        }

        public async Task<ResponseResult> DeleteMessage( Guid messageId ) {
            string url = string.Format( _deleteMessageEndPoint, GetProjectId(), GetMedicalTeamId(), messageId );

            var result = await _networkRequestService
                .DeleteRequestAsync<MessageModel>( ProactServerConfigurations.ApiUrl, url );

            return result;
        }

        public async Task<ResponseResult<MessageModel>> EditMessage(
            MessageMood patientMood, string title, string body, Guid messageId, MessageScope messageScope ) {
            var encryptedMessage = CreateMessageRequest( patientMood, title, body, false, messageScope );

            string url = string.Format( _editMessageEndPoint, GetProjectId(), GetMedicalTeamId() );

            var result = await _networkRequestService.PutRequestAsync<MessageModel>(
                ProactServerConfigurations.ApiUrl, url, encryptedMessage );

            return result;
        }

        public async Task<ResponseResult<List<MessagesContainer>>> GetMessages( int pagingCount ) {
            string url = string.Format( _messagesApiEndPoint, 
                GetProjectId(), GetMedicalTeamId(), pagingCount );

            var result = await _networkRequestService
                .GetRequestAsync<List<MessagesContainer>>( ProactServerConfigurations.ApiUrl, url );

            return result;
        }

        public async Task<ResponseResult<MessagesContainer>> GetMessage( Guid messageId ) {
            string url = string.Format( _messageApiEndPoint, GetProjectId(), GetMedicalTeamId(), messageId );

            var result = await _networkRequestService
                .GetRequestAsync<MessagesContainer>( ProactServerConfigurations.ApiUrl, url );

            return result;
        }

        public async Task<ResponseResult<List<MessagesContainer>>> GetUnrepliedMessages( int pagingCount ) {
            string url = string.Format( 
                _unrepliedMessagesApiEndPoint, GetProjectId(), GetMedicalTeamId(), pagingCount );

            var result = await _networkRequestService
                .GetRequestAsync<List<MessagesContainer>>( ProactServerConfigurations.ApiUrl, url );

            return result;
        }

        public async Task<ResponseResult<MediaFileDecryptModel>> GetMediaDecryptTokenKey( Guid messageId ) {
            string url = string.Format( _mediaDecryptToken, messageId );
            var result = await _networkRequestService
                .GetRequestAsync<MediaFileDecryptModel>( ProactServerConfigurations.ApiUrl, url );

            return result;
        }

        public async Task<ResponseResult<List<MessageModel>>> PatientSearchMessage(
            string searchText, DateTime? fromDate, DateTime? toDate ) {

            var url = CreateSearchQueryUrl( false, searchText, fromDate, toDate );
            var result = await _networkRequestService
                .GetRequestAsync<List<MessageModel>>( ProactServerConfigurations.ApiUrl, url );

            return result;
        }

        public async Task<ResponseResult<List<MessageModel>>> MedicSearchMessage(
            string searchText, DateTime? fromDate, DateTime? toDate ) {

            var url = CreateSearchQueryUrl( true, searchText, fromDate, toDate );
            var result = await _networkRequestService
                .GetRequestAsync<List<MessageModel>>( ProactServerConfigurations.ApiUrl, url );

            return result;
        }

        private string CreateSearchQueryUrl(
            bool isMedic, string searchText, DateTime? fromDate, DateTime? toDate )  {

            string baseUrl = string.Empty;
            if ( isMedic ) {
                baseUrl = _medicSearchMessageEndPoint;
            }
            else {
                baseUrl = _patientSearchMessageEndPoint;
            }

            string url = string.Format( baseUrl,
             GetProjectId(), GetMedicalTeamId() );
            url += "?";

            if ( !string.IsNullOrEmpty( searchText ) ) {
                url += $"message={searchText}&";
            }

            if ( fromDate != null ) {
                url += $"fromdate={fromDate}&";
            }

            if ( toDate != null ) {
                url += $"fromdate={toDate}";
            }
            return url;
        }
    }
}
