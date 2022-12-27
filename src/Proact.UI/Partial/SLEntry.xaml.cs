using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Proact.UI {

    public partial class SLEntry : ContentView {
        public SLEntry() {
            InitializeComponent();

            customEntry.BindingContext = this;
            customEntry.Keyboard = Keyboard.Default;
            errorLabel.IsVisible = false;
            icon.FontSize = 15;
        }

        //-- Entry

        public static readonly BindableProperty TextProperty =
        BindableProperty.Create( "Text", typeof( string ), typeof( SLEntry ), string.Empty, BindingMode.TwoWay, null,
            propertyChanged: ( bindable, oldValue, newValue ) => {

                if ( newValue != null ) {
                    ( bindable as SLEntry ).customEntry.Text = newValue.ToString();
                }

            } );

        public string Text {
            get { return ( string )GetValue( TextProperty ); }
            set { SetValue( TextProperty, value ); }
        }

        public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create( "Placeholder", typeof( string ), typeof( SLEntry ), string.Empty,
            propertyChanged: ( bindable, oldValue, newValue ) => {

                if ( newValue != null ) {
                    ( bindable as SLEntry ).customEntry.Placeholder = newValue.ToString();
                }

            } );

        public string Placeholder {
            get { return ( string )GetValue( PlaceholderProperty ); }
            set { SetValue( PlaceholderProperty, value ); }
        }

        public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create( "Keyboard", typeof( Xamarin.Forms.Keyboard ), typeof( SLEntry ), Xamarin.Forms.Keyboard.Default,
            propertyChanged: ( bindable, oldValue, newValue ) => {

                if ( newValue != null ) {
                    ( bindable as SLEntry ).customEntry.Keyboard = ( Xamarin.Forms.Keyboard )newValue;
                }
            } );

        public Xamarin.Forms.Keyboard Keyboard {
            get { return ( Xamarin.Forms.Keyboard )GetValue( KeyboardProperty ); }
            set { SetValue( KeyboardProperty, value ); }
        }

        public static readonly BindableProperty EntryEnableProperty =
        BindableProperty.Create( "EntryEnable", typeof( bool ), typeof( SLEntry ), true,
           propertyChanged: ( bindable, oldValue, newValue ) => {

               if ( newValue != null ) {
                   ( bindable as SLEntry ).customEntry.IsEnabled = ( bool )newValue;
               }
           } );

        public bool EntryEnable {
            get { return ( bool )GetValue( EntryEnableProperty ); }
            set { SetValue( EntryEnableProperty, value ); }
        }

        public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create( "IsPassword", typeof( bool ), typeof( SLEntry ), false,
           propertyChanged: ( bindable, oldValue, newValue ) => {

               if ( newValue != null ) {
                   ( bindable as SLEntry ).customEntry.IsPassword = ( bool )newValue;
               }
           } );

        public bool IsPassword {
            get { return ( bool )GetValue( IsPasswordProperty ); }
            set { SetValue( IsPasswordProperty, value ); }
        }

        //-- Icon

        public static readonly BindableProperty IconProperty =
        BindableProperty.Create( "Icon", typeof( string ), typeof( SLEntry ), string.Empty,
            propertyChanged: ( bindable, oldValue, newValue ) => {

                if ( newValue != null ) {
                    ( bindable as SLEntry ).icon.Text = newValue.ToString();
                }

            } );

        public string Icon {
            get { return ( string )GetValue( IconProperty ); }
            set { SetValue( IconProperty, value ); }
        }

        public static readonly BindableProperty IconSizeProperty =
        BindableProperty.Create( "IconSize", typeof( double ), typeof( SLEntry ), 15.0,
            propertyChanged: ( bindable, oldValue, newValue ) => {

                if ( newValue != null ) {
                    ( bindable as SLEntry ).icon.FontSize = ( double )newValue;
                }

            } );

        public double IconSize {
            get { return ( double )GetValue( IconSizeProperty ); }
            set { SetValue( IconSizeProperty, value ); }
        }

        public static readonly BindableProperty IconColorProperty =
           BindableProperty.Create( "IconColor", typeof( Color ), typeof( SLEntry ), Color.Gray,
           propertyChanged: ( bindable, oldValue, newValue ) => {

               if ( newValue != null ) {
                   ( bindable as SLEntry ).icon.TextColor = ( Color )newValue;
               }

           } );

        public Color IconColor {
            get { return ( Color )GetValue( IconColorProperty ); }
            set { SetValue( IconColorProperty, value ); }
        }

        //-- Error

        public static readonly BindableProperty ErrorTextProperty =
        BindableProperty.Create( "ErrorText", typeof( string ), typeof( SLEntry ), string.Empty,
            propertyChanged: ( bindable, oldValue, newValue ) => {

                if ( newValue != null
                        && !string.IsNullOrEmpty( newValue.ToString() ) ) {

                    ( bindable as SLEntry ).errorLabel.IsVisible = true;
                    ( bindable as SLEntry ).errorLabel.Text = newValue.ToString();
                }
                else {

                    ( bindable as SLEntry ).errorLabel.IsVisible = false;
                }

            } );

        public string ErrorText {
            get { return ( string )GetValue( ErrorTextProperty ); }
            set { SetValue( ErrorTextProperty, value ); }
        }

    }
}
