using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proact.UI {
    [XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class ReplyMessageWithImageContentView : ContentView {
        public ReplyMessageWithImageContentView() {
            InitializeComponent();
        }

        public static readonly BindableProperty IconSizeProperty =
           BindableProperty.Create(
           nameof( MaxLines ),
           typeof( int ),
           typeof( ReplyMessageWithImageContentView ),
           0,
           propertyChanged: ( bindable, oldValue, newValue ) => {

               ( bindable as ReplyMessageWithImageContentView ).MessageLabel
                           .MaxLines = ( int )newValue;
           } );

        public int MaxLines {
            get { return ( int )GetValue( IconSizeProperty ); }
            set { SetValue( IconSizeProperty, value ); }
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
                  ( bindable as ReplyMessageWithImageContentView )
                  .ImageWrapperGrid.Children.Add( new ImageGalleryButtonWithTapGesture(), 0, 0 );
              }
              else if ( ( ReplyMessageAttachmentType )newValue
                       == ReplyMessageAttachmentType.WITHOUT_GESTURE ) {

                  ( bindable as ReplyMessageWithImageContentView )
                 .ImageWrapperGrid.Children.Add( new ImageGalleryButton(), 0, 0 );
              }
          }
        );

        public ReplyMessageAttachmentType TapGesture {
            get { return ( ReplyMessageAttachmentType )GetValue( TapGesturePropery ); }
            set { SetValue( TapGesturePropery, value ); }
        }
    }
}
