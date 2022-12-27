using MvvmCross;
using Proact.Mobile.Core.Models;
using Proact.Mobile.Core.Services;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Proact.Mobile.Droid {
    public class MessageReadyCheckRequester {
		private Timer _timerToCallCheckApi;
		private readonly int _callDelayInMilliseconds = 5000;
		private readonly int _maxAttempts = 90;
		private int _attemptCurrent = 0;
		private Guid _messageIdToCheck;
		private bool _isWaitingForAResponse;

		private IMessagesService _messagesService;
		public Action<MessagesContainer> OnMessageReady;
		public Action OnMessageTimeOut;

		public MessageReadyCheckRequester() {
			_messagesService = Mvx.IoCProvider.Resolve<IMessagesService>();
			_timerToCallCheckApi = new Timer();
		}

		private void CheckIfRequestIsTimeOut() {
			++_attemptCurrent;

			if ( _attemptCurrent > _maxAttempts ) {
				StopCheck();
				OnMessageTimeOut();
            }
		}

		private void PerformCheckIfMessageOk( Object source, ElapsedEventArgs e ) {
			if ( _isWaitingForAResponse ) {
				return;
            }

			Task.Run( async () => {
				_isWaitingForAResponse = true;
				var responseResult = await _messagesService.GetMessage( _messageIdToCheck );
				
				_isWaitingForAResponse = false;

				if ( responseResult.Success ) {
					var messageContainer = responseResult.data;

					if ( messageContainer.OriginalMessage.AttachmentIsReady ) {
						OnMessageReady( messageContainer );
					}
				}

				CheckIfRequestIsTimeOut();
			} );
		}

		public void StartCheck( Guid messageId ) {
			StopCheck();

			_timerToCallCheckApi.Interval = _callDelayInMilliseconds;
			_timerToCallCheckApi.Elapsed += PerformCheckIfMessageOk;
			_timerToCallCheckApi.Start();

			_messageIdToCheck = messageId;
			_attemptCurrent = 0;
		}

		public void StopCheck() {
			_timerToCallCheckApi.Stop();
			_timerToCallCheckApi.Elapsed -= PerformCheckIfMessageOk;
		}
    }
}