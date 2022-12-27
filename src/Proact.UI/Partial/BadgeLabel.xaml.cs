using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class BadgeLabel : Grid {
        public BadgeLabel() {
            InitializeComponent();
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
              nameof( Text ),
              typeof( string ),
              typeof( BadgeLabel ),
              string.Empty,
              propertyChanged: ( bindable, oldValue, newValue ) => {
                  ( bindable as BadgeLabel ).TextLabel
                        .Text = newValue.ToString();
              }
          );

        public string Text {
            get { return ( string )GetValue( TextProperty ); }
            set { SetValue( TextProperty, value ); }
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(
           nameof( TextColor ),
           typeof( Color ),
           typeof( BadgeLabel ),
           Color.White,
           propertyChanged: ( bindable, oldValue, newValue ) => {

               ( bindable as BadgeLabel ).TextLabel
                    .TextColor = ( Color )newValue;
           } );

        public Color TextColor {
            get { return ( Color )GetValue( TextColorProperty ); }
            set { SetValue( TextColorProperty, value ); }
        }

        public static readonly BindableProperty BadgecolorProperty =
            BindableProperty.Create(
           nameof( BadgeColor ),
           typeof( Color ),
           typeof( BadgeLabel ),
           Color.White,
           propertyChanged: ( bindable, oldValue, newValue ) => {

               ( bindable as BadgeLabel ).BackgroundView
                    .BackgroundColor = ( Color )newValue;
           } );

        public Color BadgeColor {
            get { return ( Color )GetValue( BadgecolorProperty ); }
            set { SetValue( BadgecolorProperty, value ); }
        }

    }
}
