using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proact.UI {
    [XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class ReplyMessageWithVoiceContentView : ContentView {

        public ReplyMessageWithVoiceContentView() {
            InitializeComponent();
        }

        public static readonly BindableProperty TapGesturePropery =
           BindableProperty.Create(
           nameof( TapGesture ),
           typeof( ReplyMessageAttachmentType ),
           typeof( ReplyMessageWithVoiceContentView ),
           ReplyMessageAttachmentType.NONE,
           propertyChanged: ( bindable, oldValue, newValue ) => {

               if ( ( ReplyMessageAttachmentType )newValue
                        == ReplyMessageAttachmentType.WITH_GESTURE ) {
                   ( bindable as ReplyMessageWithVoiceContentView )
                   .MainGrid.Children.Add( new VoicePlayButtonWithTapGesture(), 1, 1 );
               }
               else if ( ( ReplyMessageAttachmentType )newValue
                        == ReplyMessageAttachmentType.WITHOUT_GESTURE ) {

                   ( bindable as ReplyMessageWithVoiceContentView )
                  .MainGrid.Children.Add( new VoicePlayButton(), 1, 1 );
               }
           } );

        public ReplyMessageAttachmentType TapGesture {
            get { return ( ReplyMessageAttachmentType )GetValue( TapGesturePropery ); }
            set { SetValue( TapGesturePropery, value ); }
        }
    }
}
