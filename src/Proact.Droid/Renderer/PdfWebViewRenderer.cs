using Android.Content;
using Proact.Mobile.Droid;
using Proact.UI;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PdfWebView), typeof(PdfWebViewRenderer))]
namespace Proact.Mobile.Droid {
	public class PdfWebViewRenderer : WebViewRenderer {
		public PdfWebViewRenderer( Context context ) : base( context ) {
		}

		protected override void OnElementChanged( ElementChangedEventArgs<WebView> e ) {
			base.OnElementChanged( e );

			if ( e.NewElement != null ) {
				Control.Settings.AllowFileAccess = true;
				Control.Settings.AllowFileAccessFromFileURLs = true;
				Control.Settings.AllowUniversalAccessFromFileURLs = true;
			}
		}
	}
}