using Android.Content;
using AndroidX.Core.Content;
using Proact.Mobile.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(Xamarin.Forms.SearchBar), typeof(LexiconSearchbarRenderer))]
namespace Proact.Mobile.Droid {
    class LexiconSearchbarRenderer : SearchBarRenderer {
        public LexiconSearchbarRenderer( Context context ) : base( context ) {
        }

        protected override void OnElementChanged( ElementChangedEventArgs<Xamarin.Forms.SearchBar> e ) {
            base.OnElementChanged( e );
            var plateId = Resources.GetIdentifier( "android:id/search_plate", null, null );
            var plate = Control.FindViewById( plateId );
            plate?.SetBackgroundColor( Android.Graphics.Color.Transparent );

            if ( Control != null ) {
                Control.Background = ContextCompat.GetDrawable(Context, Resource.Drawable.CustomSearchBarBackground);  
            }

        }
    }
}