using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using MvvmCross.Commands;
using Plugin.AudioRecorder;
using Proact.Mobile.Core.Models;
using Proact.Mobile.Core.Services;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class VoiceRecordingViewModel : BaseViewModel<MessageActionModel,bool> {

        public IMvxCommand StartVoiceRecordingCommand { get; set; }
        public string ActionLabelText { get; private set; }
        public string TotalRecordingSecondsAvailable { get; private set; }
        public bool RecordingLabelIsVisible { get; private set; }
        public bool PulseAnimaionActive { get; private set; } 

        private Timer _timer;
        private int _totalRecordingSecondsAvailable = 60;

        private int _currentRecordingSeconds;
        public int CurrentRecordingSeconds {
            get => _currentRecordingSeconds;
            set {
                _currentRecordingSeconds = value;
                RaisePropertyChanged( () => CurrentRecordingSecondsFormatted );
            }
        }

        private MessageActionModel _messageActionModel;
        private AudioRecorderService _audioRecorderecorder;
        private AudioPlayer _audioPlayer;
        private IMessagesService _messagesService;
        private IMicrophonePermissionService _microphonePermissionService;
        private IPulseViewBehaviour _pulseViewBehaviour;

        public string CurrentRecordingSecondsFormatted {
            get {
                TimeSpan time = TimeSpan.FromSeconds( _currentRecordingSeconds );
                return time.ToString( @"mm\.ss" );
            }
        }

        public VoiceRecordingViewModel(
            IMessagesService messageService,
            IMicrophonePermissionService microphonePermissionService ) {
            _messagesService = messageService;
            _microphonePermissionService = microphonePermissionService;
        }

        public override void Prepare( MessageActionModel messageAction ) {
            _messageActionModel = messageAction;
            InitUICommand();
            SetUIOnNotRecording();
            InitRecorder();
            InitAudioPlayer();
        }

        public override void ViewDestroy( bool viewFinishing = true ) {
            base.ViewDestroy( viewFinishing );
            RemoveAudioReceiver();
        }

        private void InitUICommand() {
            StartVoiceRecordingCommand
                = new MvxCommand( StartVoiceRecordingActionHandle );
        }

        private async void StartVoiceRecordingActionHandle() {
            if ( !_audioRecorderecorder.IsRecording ) {
                await StartRecordAudio();
            }
            else {
                await StopRecordAudio();
            }
        }

        private void InitRecorder() {
            _audioRecorderecorder = new AudioRecorderService {
                StopRecordingAfterTimeout = true,
                StopRecordingOnSilence = false,
                TotalAudioTimeout = TimeSpan.FromSeconds( _totalRecordingSecondsAvailable )
            };

            AddAudioReceiver();
        }

        private async Task StartRecordAudio() {
            DependencyService.Get<IAudioSession>().StartSession();
            var micPermissionGranted = await CheckAndRequestMicrophonePermission();
            if ( micPermissionGranted ) {
                try {
                    SetUIOnRecording();
                    StartCountdownTimer();
                    var audioRecordTask = await _audioRecorderecorder.StartRecording();
                }
                catch ( Exception e ) {
                    Console.WriteLine( e.Message );
                    await StopRecordAudio();
                }
            }
        }

        private async Task StopRecordAudio() {
            DependencyService.Get<IAudioSession>().StopSession();
            try {
                SetUIOnNotRecording();
                StopCountdownTimer();
                await _audioRecorderecorder.StopRecording();
            }
            catch ( Exception e ) {
                Console.WriteLine( e.Message );
            }
        }

        private async void RecorderAudioInputReceived( object sender, string e ) {
            SetUIOnNotRecording();
            await ShowConfirmAlertDialogue();
        }

        private IBackgroundServicesManager GetBackgroundServicesManager() {
            var bgServiceManager = DependencyService.Get<IBackgroundServicesManager>();
            bgServiceManager.InitializeStrings(
                Resources.AppResources.NotificationMediaUploadTitle,
                Resources.AppResources.NotificationMessageMediaUploading,
                Resources.AppResources.NotificationMessageMediaVerification,
                Resources.AppResources.NotificationMessageMediaUploaded
            );

            return bgServiceManager;
        }

        private async void CreateNewMessage() {
            try {
                var audioStream = (FileStream)_audioRecorderecorder.GetAudioFileStream();
                if ( audioStream != null ) {

                    var bgServiceManager = GetBackgroundServicesManager();
                    bgServiceManager.CreateNewMessageWithAttachment(
                        audioStream, AttachmentType.VOICE );
                    await _navigationService.Close( this, true );
                }
            }
            catch ( Exception ex ) {
                Console.WriteLine( ex.Message );
            }
        }

        private async void ReplyToMessage() {
            try {
                var audioStream = (FileStream)_audioRecorderecorder.GetAudioFileStream();
                var originalMessageId = (Guid)_messageActionModel.Container
                    .OriginalMessage.MessageId;

                if ( audioStream != null ) {

                    var bgServiceManager = GetBackgroundServicesManager();
                    bgServiceManager.CreateReplyMessageWithAttachment(
                        originalMessageId, audioStream, AttachmentType.VOICE );

                    await _navigationService.Close( this, true );
                }
            }
            catch ( Exception ex ) {
                Console.WriteLine( ex.Message );
            }
        }

        private void StartCountdownTimer() {
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += OnCountdownTimedEvent;
            CurrentRecordingSeconds = _totalRecordingSecondsAvailable;
            _timer.Enabled = true;
        }

        private void OnCountdownTimedEvent( object sender, ElapsedEventArgs e ) {
            CurrentRecordingSeconds--;
            if ( CurrentRecordingSeconds == 0 ) {
                StopCountdownTimer();
            }
        }

        private void StopCountdownTimer() {
            _timer.Stop();
        }

        private void SetUIOnNotRecording() {
            StopPulseAnimation();
            TimeSpan time = TimeSpan.FromSeconds( _totalRecordingSecondsAvailable );
            TotalRecordingSecondsAvailable = time.ToString( @"mm\.ss" );

            CurrentRecordingSeconds = _totalRecordingSecondsAvailable;
            ActionLabelText = Resources.AppResources.TapToStartRecording;
            RecordingLabelIsVisible = false;
            RaisePropertyChanged( () => ActionLabelText );
            RaisePropertyChanged( () => RecordingLabelIsVisible );
            RaisePropertyChanged( () => TotalRecordingSecondsAvailable );
        }

        private void SetUIOnRecording() {
            StartPulseAnimation();
            ActionLabelText = Resources.AppResources.TapToStopRecording;
            RecordingLabelIsVisible = true;
            RaisePropertyChanged( () => ActionLabelText );
            RaisePropertyChanged( () => RecordingLabelIsVisible );
        }

        private async Task ShowConfirmAlertDialogue() {
            await InvokeOnMainThreadAsync( async () => {
                string selected = await Application.Current.MainPage.DisplayActionSheet(
               Resources.AppResources.SendVoiceMessageAlertMessage,
               Resources.AppResources.Cancel, null,
               Resources.AppResources.SendVoiceMessageConfirmButton );

                if ( selected == Resources.AppResources.SendVoiceMessageConfirmButton ) {
                    if ( _messageActionModel.ActionType == MessageActionType.CREATE ) {
                        CreateNewMessage();
                    }
                    else if ( _messageActionModel.ActionType == MessageActionType.REPLY ) {
                        ReplyToMessage();
                    }
                }
            } );
        }

        private async Task<bool> CheckAndRequestMicrophonePermission() {
            var micPermissionGranted = await _microphonePermissionService
                .CheckMicrophonePermissionsGranted();

            if ( !micPermissionGranted ) {
                micPermissionGranted = await _microphonePermissionService
                    .RequestMicrophonePermission();

                if ( !micPermissionGranted ) {
                    _microphonePermissionService.ShowSettingsUIOnPermissionDenied();
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                return true;
            }
        }

        private void AddAudioReceiver() {
            _audioRecorderecorder.AudioInputReceived += RecorderAudioInputReceived;
        }

        private void RemoveAudioReceiver() {
            _audioRecorderecorder.AudioInputReceived -= RecorderAudioInputReceived;
        }

        public async override Task ClosePage() {
            if ( _audioRecorderecorder.IsRecording ) {
                bool close = await Application.Current.MainPage.DisplayAlert(
                    Resources.AppResources.VoiceRecordingClosePageTitle,
                    string.Empty,
                    Resources.AppResources.VoiceRecordingCloseButton,
                    Resources.AppResources.VoiceRecordingCancelButton );

                if ( close ) {
                    RemoveAudioReceiver();
                    await StopRecordAudio();
                    await _navigationService.Close( this, false );
                }
            }
            else {
                await _navigationService.Close( this, false );
            }
        }

        private void InitAudioPlayer() {
            _audioPlayer = new AudioPlayer();
        }

        private void PlayAudio() {
            try {
                var filePath = _audioRecorderecorder.GetAudioFilePath();
                if ( filePath != null ) {
                    _audioPlayer.Play( filePath );
                }
            }
            catch ( Exception ex ) {
                Console.WriteLine( ex.Message );
            }
        }

        public void SetPulseViewBehaviour( IPulseViewBehaviour pulseViewBehaviour ) {
            _pulseViewBehaviour = pulseViewBehaviour;
        }

        private void StartPulseAnimation() {
            _pulseViewBehaviour?.StartPulse();
        }

        private void StopPulseAnimation() {
            _pulseViewBehaviour?.StopPulse();
        }

    }
}
