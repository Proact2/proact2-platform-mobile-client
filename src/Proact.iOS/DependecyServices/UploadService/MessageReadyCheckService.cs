using System;
using System.Threading.Tasks;
using MvvmCross;
using Proact.Mobile.Core.Models;
using Proact.Mobile.Core.Services;

namespace Proact.Mobile.iOS {
    public class MessageReadyCheckService {

        private readonly int CHECK_DELAY_IN_MILLISECONDS = 5000;
        private readonly int MAX_CHECK_COUNT = 100;
        private IMessagesService _messagesService;
        private int _currentCheckCounter = 0;

        public MessageReadyCheckService() {
            InitCoreServices();
        }

        private void InitCoreServices() {
            _messagesService = Mvx.IoCProvider.Resolve<IMessagesService>();
        }

        public async void PerformCheckIfMessageIsReady(
            MessageModel messageMoodel, Action OnCheckSuccess, Action OnCheckError ) {
            if ( messageMoodel == null ) {
                OnCheckError();
            }

            await Task.Factory.StartNew( async () => {
                bool checkComplete = false;
                _currentCheckCounter = 0;
                while ( !checkComplete ) {
               
                    var responseResult = await _messagesService
                        .GetMessage( ( Guid )messageMoodel.MessageId );

                    if ( responseResult.Success ) {
                        if ( responseResult.data.OriginalMessage.AttachmentIsReady ) {
                            checkComplete = true;                
                            OnCheckSuccess();
                        }
                    }

                    _currentCheckCounter++;
                    if(_currentCheckCounter > MAX_CHECK_COUNT ) {
                        OnCheckError();
                        break;
                    }

                    await Task.Delay( CHECK_DELAY_IN_MILLISECONDS );
                }
            } );
        }

    }
}
