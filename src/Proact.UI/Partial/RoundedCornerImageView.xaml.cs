using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class RoundedCornerImageView : Frame {
        public RoundedCornerImageView() {
            InitializeComponent();
        }

        public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create( "ImageSource", typeof( string ), typeof( RoundedCornerImageView ), string.Empty, propertyChanged: ( bindable, oldValue, newValue ) => {

            if ( newValue != null ) {
                ( bindable as RoundedCornerImageView ).Image.Source = newValue.ToString();
            }

        } );

        public string ImageSource {
            get { return ( string )GetValue( ImageSourceProperty ); }
            set { SetValue( ImageSourceProperty, value ); }
        }

        public static readonly BindableProperty ImageHeightProperty =
        BindableProperty.Create( "ImageHeight", typeof( double ), typeof( RoundedCornerImageView ), 0.0, propertyChanged: ( bindable, oldValue, newValue ) => {

            if ( newValue != null ) {
                ( bindable as RoundedCornerImageView ).Image.HeightRequest = ( double )newValue;
                ( bindable as RoundedCornerImageView ).HeightRequest = ( double )newValue;
            }
        } );

        public double ImageHeight {
            get { return ( double )GetValue( ImageHeightProperty ); }
            set { SetValue( ImageHeightProperty, value ); }
        }

        public static readonly BindableProperty ImageWidthProperty =
        BindableProperty.Create( "ImageWidth", typeof( double ), typeof( RoundedCornerImageView ), 0.0, propertyChanged: ( bindable, oldValue, newValue ) => {

            if ( newValue != null ) {
                ( bindable as RoundedCornerImageView ).Image.WidthRequest = ( double )newValue;
                ( bindable as RoundedCornerImageView ).WidthRequest = ( double )newValue;
            }
        } );

        public double ImageWidth {
            get { return ( double )GetValue( ImageWidthProperty ); }
            set { SetValue( ImageWidthProperty, value ); }
        }

        //public static readonly BindableProperty CornerRadiusProperty =
        //  BindableProperty.Create( "CornerRadius", typeof( float ), typeof( RoundedCornerImageView ), 0f, propertyChanged: ( bindable, oldValue, newValue ) => {

        //      if ( newValue != null ) {
        //          ( bindable as RoundedCornerImageView ).CornerRadius = ( float )newValue;
        //      }
        //  } );

        //public float CornerRadius {
        //    get { return ( float )GetValue( CornerRadiusProperty ); }
        //    set { SetValue( CornerRadiusProperty, value ); }
        //}
    }
}
