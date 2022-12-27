using System.ComponentModel;
using Android.Content;
using Android.Net;
using Android.OS;
using Com.Google.Android.Exoplayer2;
using Com.Google.Android.Exoplayer2.Source;
using Com.Google.Android.Exoplayer2.Source.Dash;
using Com.Google.Android.Exoplayer2.Source.Dash.Manifest;
using Com.Google.Android.Exoplayer2.Source.Hls;
using Com.Google.Android.Exoplayer2.Source.Smoothstreaming;
using Com.Google.Android.Exoplayer2.Trackselection;
using Com.Google.Android.Exoplayer2.UI;
using Com.Google.Android.Exoplayer2.Upstream;
using Java.IO;
using Java.Lang;
using Proact.Mobile.Droid;
using Proact.UI;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(VideoPlayer), typeof(VideoPlayerRenderer))]
namespace Proact.Mobile.Droid {
    public class VideoPlayerRenderer : ViewRenderer<VideoPlayer, PlayerView> {

        private PlayerView _playerView;
        private SimpleExoPlayer _player;

        public VideoPlayerRenderer( Context context ) : base( context ) { }

        protected override void OnElementChanged( ElementChangedEventArgs<VideoPlayer> e ) {
            base.OnElementChanged( e );

            if ( _player == null ) {
                InitializePlayer();
            }
            Play();
        }

        protected override void OnElementPropertyChanged( object sender, PropertyChangedEventArgs e ) {
            base.OnElementPropertyChanged( sender, e );

            if(e.PropertyName == "MediaFileDecrypt" ) {
                var videoPlayer = ( VideoPlayer )sender;
                if(videoPlayer.MediaFileDecrypt == null ) {
                    Stop();
                }
            }
        }

        private void InitializePlayer() {
            DefaultTrackSelector trackSelector = new DefaultTrackSelector( Context );
            trackSelector.SetParameters( trackSelector.BuildUponParameters().SetMaxVideoSizeSd() );
            DefaultRenderersFactory renderersFactory = new DefaultRenderersFactory( Context );

            _player = new SimpleExoPlayer
                .Builder( Context, renderersFactory )
                .SetTrackSelector( trackSelector )
                .Build();

            _player.PlayWhenReady = true;
            _playerView = new PlayerView( Context ) { Player = _player };

            SetNativeControl( _playerView );
        }

        private void Play() {
            Uri sourceUri = Uri.Parse( Element.MediaFileDecrypt.ContentUrl );
            IMediaSource mediaSource = BuildHlsMediaSource( sourceUri );
            _player.Prepare( mediaSource, false, false );

        }

        private void Stop() {
            _player.Stop();
        }

        private IMediaSource BuildMediaSource( Uri uri ) {
            DefaultDataSourceFactory dataSourceFactory = new DefaultDataSourceFactory( Context, "ExoplayerTest" );
            return new ProgressiveMediaSource.Factory( dataSourceFactory ).CreateMediaSource( uri );
        }

        private IMediaSource BuildDashMediaSource( Uri uri ) {
            DefaultDataSourceFactory dataSourceFactory = new DefaultDataSourceFactory( Context, "ExoplayerTest" );
            DashMediaSource.Factory mediaSourceFactory = new DashMediaSource.Factory( dataSourceFactory );
            return mediaSourceFactory.CreateMediaSource( uri );
        }

        private IMediaSource BuildHlsMediaSource( Uri uri ) {
            DefaultHttpDataSourceFactory dataSourceFactory = new DefaultHttpDataSourceFactory( "ExoplayerTest" );

            var token = Element.MediaFileDecrypt.DecryptToken;
            dataSourceFactory.DefaultRequestProperties.Set( "Authorization", $"Bearer {token}" );
            IMediaSource hlsMediaSource = new HlsMediaSource.Factory( dataSourceFactory ).SetAllowChunklessPreparation( true ).CreateMediaSource( uri );
            return hlsMediaSource;
        }

    }
}