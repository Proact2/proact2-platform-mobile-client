using System;
using Proact.Mobile.iOS;
using Proact.UI;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer( typeof( TwoLinesButton ), typeof( TwoLinesButtonRenderer ) )]
namespace Proact.Mobile.iOS {
    public class TwoLinesButtonRenderer : ButtonRenderer {

        protected override void OnElementChanged( ElementChangedEventArgs<Button> e ) {
            base.OnElementChanged( e );
            if ( Control != null ) {
                Control.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
                Control.TitleLabel.Lines = 0;
                Control.TitleLabel.TextAlignment = UITextAlignment.Center;
            }
        }
    }
}