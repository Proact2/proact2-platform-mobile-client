using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class MediaUploadStatusBar : Grid {
        public MediaUploadStatusBar() {
            InitializeComponent();
        }

        public static readonly BindableProperty ActivityIndicatorIsRunningProperty =
           BindableProperty.Create(
               nameof( ActivityIndicatorIsRunning ),
               typeof( bool ),
               typeof( MediaUploadStatusBar ),
               true,
               propertyChanged: ( bindable, oldValue, newValue ) => {

                   ( bindable as MediaUploadStatusBar ).uploadActivityIndicator
                       .IsVisible = ( bool )newValue;

                   ( bindable as MediaUploadStatusBar ).uploadActivityIndicator
                       .IsRunning = ( bool )newValue;
               } );

        public bool ActivityIndicatorIsRunning {
            get { return ( bool )GetValue( ActivityIndicatorIsRunningProperty ); }
            set { SetValue( ActivityIndicatorIsRunningProperty, value ); }
        }
    }
}
