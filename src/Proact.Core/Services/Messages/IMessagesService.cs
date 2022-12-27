using Proact.Mobile.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Proact.Mobile.Core.Services {
    public interface IMessagesService {
        Task<ResponseResult<List<MessagesContainer>>> GetMessages( int pagingCount );

        Task<ResponseResult<MessagesContainer>> GetMessage( Guid messageId );

        Task<ResponseResult<List<MessagesContainer>>> GetUnrepliedMessages( int pagingCount );

        Task<ResponseResult<MessageModel>> CreateNewMessage(
            MessageMood mood, string body, bool hasAttachment, MessageScope messageScope );

        Task<ResponseResult<MessageModel>> CreateNewMessageWithVideoAttached( Stream mediaFileStream );

        Task<ResponseResult<MessageModel>> CreateNewMessageWithAudioAttached( Stream mediaFileStream );

        Task<ResponseResult<MessageModel>> CreateNewMessageWithImageAttached(
            MessageMood mood, string body, Stream mediaFileStream, MessageScope messageScope );

        Task<ResponseResult<MessageModel>> CreateBroadcastMessage(
            string title, string body );

        Task<ResponseResult<MessageModel>> ReplyToMessage( 
            MessageMood mood, string body, bool hasAttachment, Guid originalMessageId );

        Task<ResponseResult<MessageModel>> ReplyToMessageWithVideoAttached(
            Guid originalMessageId, Stream mediaFileStream );

        Task<ResponseResult<MessageModel>> ReplyToMessageWithAudioAttached(
            Guid originalMessageId, Stream mediaFileStream );

        Task<ResponseResult<MessageModel>> ReplyToMessageWithImageAttached(
            string body, Guid originalMessageId, Stream mediaFileStream );

        Task<ResponseResult<MessageModel>> EditMessage(
            MessageMood patientMood, string title, string body, Guid messageId, MessageScope messageScope );

        Task<ResponseResult> DeleteMessage( Guid messageId );

        Task<ResponseResult<MediaFileDecryptModel>> GetMediaDecryptTokenKey( Guid messageId );

        Task<ResponseResult<List<MessageModel>>> PatientSearchMessage(
            string searchText, DateTime? fromDate, DateTime? toDate );

        Task<ResponseResult<List<MessageModel>>> MedicSearchMessage(
            string searchText, DateTime? fromDate, DateTime? toDate );
    }
}
