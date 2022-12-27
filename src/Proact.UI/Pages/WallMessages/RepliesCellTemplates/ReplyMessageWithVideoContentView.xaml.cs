using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proact.UI {
    [XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class ReplyMessageWithVideoContentView : ContentView {
        public ReplyMessageWithVideoContentView() {
            InitializeComponent();
        }

        public static readonly BindableProperty TapGesturePropery =
           BindableProperty.Create(
           nameof( TapGesture ),
           typeof( ReplyMessageAttachmentType ),
           typeof( ReplyMessageWithVideoContentView ),
           ReplyMessageAttachmentType.NONE,
           propertyChanged: ( bindable, oldValue, newValue ) => {

               if ( ( ReplyMessageAttachmentType )newValue
                        == ReplyMessageAttachmentType.WITH_GESTURE ) {
                   ( bindable as ReplyMessageWithVideoContentView )
                   .MainGrid.Children.Add( new VideoPlayButtonWithTapGesture(), 1, 1 );
               }
               else if ( ( ReplyMessageAttachmentType )newValue
                        == ReplyMessageAttachmentType.WITHOUT_GESTURE ) {

                   ( bindable as ReplyMessageWithVideoContentView )
                  .MainGrid.Children.Add( new VideoPlayButton(), 1, 1 );
               }
           }
       );

        public ReplyMessageAttachmentType TapGesture {
            get { return ( ReplyMessageAttachmentType )GetValue( TapGesturePropery ); }
            set { SetValue( TapGesturePropery, value ); }
        }
    }
}
