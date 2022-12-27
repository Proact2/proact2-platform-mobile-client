using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proact.UI {
    [XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class ReplyMessageContentView : ContentView {
        public ReplyMessageContentView() {
            InitializeComponent();
        }

        public static readonly BindableProperty IconSizeProperty =
            BindableProperty.Create(
            nameof( MaxLines ),
            typeof( int ),
            typeof( ReplyMessageContentView ),
            0,
            propertyChanged: ( bindable, oldValue, newValue ) => {

                ( bindable as ReplyMessageContentView ).MessageLabel
                            .MaxLines = ( int )newValue;
            } );

        public int MaxLines {
            get { return ( int )GetValue( IconSizeProperty ); }
            set { SetValue( IconSizeProperty, value ); }
        }
    }
}
