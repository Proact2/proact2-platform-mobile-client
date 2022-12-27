using System;
using System.Collections.Generic;
using MvvmCross.Commands;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class IconButton : Grid {
        public IconButton() {
            InitializeComponent();
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                "Text",
                typeof( string ),
                typeof( IconButton ),
                string.Empty,
                propertyChanged: ( bindable, oldValue, newValue ) => {

                    if ( newValue != null || string.IsNullOrEmpty( newValue.ToString() ) ) {

                        ( bindable as IconButton ).ButtonText
                            .IsVisible = string.IsNullOrEmpty( newValue.ToString() );
                    }
                    else {

                        ( bindable as IconButton ).ButtonText
                            .Text = newValue.ToString();
                    }
                } );

        public string Text {
            get { return ( string )GetValue( TextProperty ); }
            set { SetValue( TextProperty, value ); }
        }

        public static readonly BindableProperty TextColorProperty =
           BindableProperty.Create(
               "TextColor",
               typeof( Color ),
               typeof( IconButton ),
               Color.Black,
               propertyChanged: ( bindable, oldValue, newValue ) => {

                   ( bindable as IconButton ).ButtonText
                        .TextColor = ( Color )newValue;
               } );

        public Color TextColor {
            get { return ( Color )GetValue( TextColorProperty ); }
            set { SetValue( TextColorProperty, value ); }
        }


        public static readonly BindableProperty IconProperty =
            BindableProperty.Create(
                "Icon",
                typeof( string ),
                typeof( IconButton ),
                string.Empty,
                propertyChanged: ( bindable, oldValue, newValue ) => {

                ( bindable as IconButton ).ButtonIcon
                    .IsVisible = true;
                ( bindable as IconButton ).ButtonIcon
                    .Text = newValue.ToString();
            } );

        public string Icon {
            get { return ( string )GetValue( IconProperty ); }
            set { SetValue( IconProperty, value ); }
        }

        public static readonly BindableProperty IconColorProperty =
            BindableProperty.Create(
                "IconColor",
                typeof( Color ),
                typeof( IconButton ),
                Color.Black,
                propertyChanged: ( bindable, oldValue, newValue ) => {

                ( bindable as IconButton ).ButtonIcon
                    .TextColor = ( Color )newValue;
            } );

        public Color IconColor {
            get { return ( Color )GetValue( IconColorProperty ); }
            set { SetValue( IconColorProperty, value ); }
        }

        public static readonly BindableProperty IconSizeProperty =
            BindableProperty.Create(
                "IconSize",
                typeof( double ),
                typeof( IconButton ),
                20.0,
                propertyChanged: ( bindable, oldValue, newValue ) => {

                ( bindable as IconButton ).ButtonIcon
                    .FontSize = ( double )newValue;
            } );

        public double IconSize {
            get { return ( double )GetValue( IconSizeProperty ); }
            set { SetValue( IconSizeProperty, value ); }
        }


        public static readonly BindableProperty ButtonBackgroundColorProperty =
            BindableProperty.Create(
                "ButtonBackgroundColor",
                typeof( Color ),
                typeof( IconButton ),
                Color.Black,
                propertyChanged: ( bindable, oldValue, newValue ) => {

                ( bindable as IconButton ).ButtonShape
                    .Color = ( Color )newValue;
            } );

        public Color ButtonBackgroundColor {
            get { return ( Color )GetValue( ButtonBackgroundColorProperty ); }
            set { SetValue( ButtonBackgroundColorProperty, value ); }
        }

        public static readonly BindableProperty CornerRadiusProperty =
           BindableProperty.Create(
               "Text",
               typeof( CornerRadius ),
               typeof( IconButton ),
               new CornerRadius(10),
               propertyChanged: ( bindable, oldValue, newValue ) => {

                   ( bindable as IconButton ).ButtonShape
                        .CornerRadius = ( CornerRadius )newValue;
               } );

        public CornerRadius CornerRadius {
            get { return ( CornerRadius )GetValue( CornerRadiusProperty ); }
            set { SetValue( CornerRadiusProperty, value ); }
        }

        public static readonly BindableProperty SpacingProperty =
            BindableProperty.Create(
                 "Spacing",
                 typeof( double ),
                 typeof( IconButton ),
                 0.0,
                 propertyChanged: ( bindable, oldValue, newValue ) => {

                     ( bindable as IconButton ).WrapperStackLayout
                          .Spacing = ( double )newValue;
                 } );

        public CornerRadius Spacing {
            get { return ( CornerRadius )GetValue( SpacingProperty ); }
            set { SetValue( SpacingProperty, value ); }
        }


    }
}
