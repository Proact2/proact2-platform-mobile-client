using System;
using System.Collections.Generic;
using MediaManager;
using Proact.Mobile.Core;
using Proact.Mobile.Core.Models;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class AudioPlayerPopup : PopupPage {

        private const string _headerAuthorizationKey = "Authorization";
        private MediaFileDecryptModel _mediaFileDecrypt;
        private bool _encryptedAudio;

        public AudioPlayerPopup( MediaFileDecryptModel mediaFileDecrypt, bool encryptedAudio ) {
            InitializeComponent();

            _mediaFileDecrypt = mediaFileDecrypt;
            _encryptedAudio = encryptedAudio;

            SetPlayIcon();
            SetIsBusy( true );
            SetTimer( TimeSpan.Zero );
            ShowErrorMessage( false );
        }

        protected override void OnAppearingAnimationEnd() {
            base.OnAppearingAnimationEnd();
            CrossMediaManager.Current.StateChanged += MediaPlayerStateChanged;
            CrossMediaManager.Current.PositionChanged += Positionchanged;

            PlayAudio();
        }

        protected override void OnDisappearingAnimationEnd() {
            base.OnDisappearingAnimationEnd();
            AudioStop();
            CrossMediaManager.Current.StateChanged -= MediaPlayerStateChanged;
            CrossMediaManager.Current.PositionChanged -= Positionchanged;
        }

        private void PlayAudio() {
            if ( _encryptedAudio ) {
                PlayEncryptedAudio();
            }
            else {
                PlayNotEncryptedAudio();
            }            
        }

        private async void PlayEncryptedAudio() {
            AddTokenTorequestHeader();

            var mediaitem = await CrossMediaManager.Current.Extractor
                .CreateMediaItem( _mediaFileDecrypt.ContentUrl );
            mediaitem.MediaType = MediaManager.Library.MediaType.Hls;

            await CrossMediaManager.Current.Play( mediaitem );
        }

        private async void PlayNotEncryptedAudio() {
            var mediaitem = await CrossMediaManager.Current.Extractor
                .CreateMediaItem( _mediaFileDecrypt.ContentUrl );
            mediaitem.MediaType = MediaManager.Library.MediaType.Audio;

            await CrossMediaManager.Current.Play( mediaitem );
        }

        private async void AudioPlayPause() {
            await CrossMediaManager.Current.PlayPause();
        }

        private async void AudioStop() {
            await CrossMediaManager.Current.Stop();
        }

        private void PlayPauseButtonTapped( System.Object sender, System.EventArgs e ) {
            if ( CrossMediaManager.Current.State
                    == MediaManager.Player.MediaPlayerState.Stopped ) {
                PlayAudio();
            }
            else {
                AudioPlayPause();
            }
        }

        private void MediaPlayerStateChanged(
            object sender, MediaManager.Playback.StateChangedEventArgs e ) {
            switch ( e.State ) {
                case MediaManager.Player.MediaPlayerState.Buffering:
                case MediaManager.Player.MediaPlayerState.Loading:
                    SetIsBusy( true );
                    ShowErrorMessage( false );
                    break;
                case MediaManager.Player.MediaPlayerState.Playing:
                    SetIsBusy( false );
                    SetPauseIcon();
                    break;
                case MediaManager.Player.MediaPlayerState.Stopped:
                    SetTimer( CrossMediaManager.Current.Duration );
                    SetIsBusy( false );
                    SetPlayIcon();
                    break;
                case MediaManager.Player.MediaPlayerState.Paused:
                    SetIsBusy( false );
                    SetPlayIcon();
                    break;
                case MediaManager.Player.MediaPlayerState.Failed:
                    SetIsBusy( false );
                    SetPlayIcon();
                    ShowErrorMessage( true );
                    break;
            }
        }

        private void Positionchanged(
        object sender, MediaManager.Playback.PositionChangedEventArgs e ) {
            try {
                if ( CrossMediaManager.Current.State
                    == MediaManager.Player.MediaPlayerState.Playing ) {
                    var time = CrossMediaManager.Current.Duration - e.Position;
                    SetTimer( time );
                }
            }
            catch ( Exception ) { }
        }

        private void SetPlayIcon() {
            Device.BeginInvokeOnMainThread( () => {
                ButtonIcon.Text = FontAwesome.FontAwesomeIcons.PlayCircle;
            } );
        }

        private void SetPauseIcon() {
            Device.BeginInvokeOnMainThread( () => {
                ButtonIcon.Text = FontAwesome.FontAwesomeIcons.PauseCircle;
            } );
        }

        private void SetTimer( TimeSpan time ) {
            Device.BeginInvokeOnMainThread( () => {
                TimeLabel.Text = time.ToString( @"mm\:ss" );
            } );
        }


        private void ShowErrorMessage( bool errorIsVisible ) {
            Device.BeginInvokeOnMainThread( () => {
                ErrorLabel.IsVisible = errorIsVisible;
            } );
        }

        private void SetIsBusy( bool isBusy ) {
            Device.BeginInvokeOnMainThread( () => {
                ActivityIndicator.IsVisible = isBusy;
                ActivityIndicator.IsRunning = isBusy;
            } );
        }

        private void AddTokenTorequestHeader() {
            CrossMediaManager.Current.RequestHeaders
                .Remove( _headerAuthorizationKey );

            CrossMediaManager.Current
               .RequestHeaders.Add(
               _headerAuthorizationKey,
               $"Bearer {_mediaFileDecrypt.DecryptToken}" );
        }
    }
}
