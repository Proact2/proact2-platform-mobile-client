using System;
using System.Collections.Generic;
using FontAwesome;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class OptionRowItemView : ContentView {

        public OptionRowItemView() {
            InitializeComponent();
            IconR.Text = FontAwesomeIcons.ChevronRight;
            Badge.IsVisible = false;
            GroupTitleContainer.IsVisible = false;
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof( Text ),
                typeof( string ),
                typeof( OptionRowItemView ),
                string.Empty,
                propertyChanged: ( bindable, oldValue, newValue ) => {

                    if ( newValue != null ) {
                        ( bindable as OptionRowItemView )
                            .TitleLabel.Text = newValue.ToString();
                    }
                } );

        public string Text {
            get { return ( string )GetValue( TextProperty ); }
            set { SetValue( TextProperty, value ); }
        }

        public static readonly BindableProperty SubtitleTextProperty =
            BindableProperty.Create(
                nameof( SubtitleLabel ),
                typeof( string ),
                typeof( OptionRowItemView ),
                string.Empty,
                propertyChanged: ( bindable, oldValue, newValue ) => {

                    if ( newValue != null ) {
                        ( bindable as OptionRowItemView ).SubtitleLabel
                            .IsVisible = !string.IsNullOrEmpty( newValue.ToString() );
                        ( bindable as OptionRowItemView ).SubtitleLabel
                            .Text = newValue.ToString();
                    }
                    else {
                        ( bindable as OptionRowItemView ).SubtitleLabel
                            .IsVisible = false;
                    }

                } );

        public string SubtitleText {
            get { return ( string )GetValue( SubtitleTextProperty ); }
            set { SetValue( SubtitleTextProperty, value ); }
        }

        public static readonly BindableProperty CaptionTextProperty =
           BindableProperty.Create(
               nameof( CaptionText ),
               typeof( string ),
               typeof( OptionRowItemView ),
               string.Empty,
               propertyChanged: ( bindable, oldValue, newValue ) => {

                   if ( newValue != null ) {
                       ( bindable as OptionRowItemView ).CaptionLabel
                           .IsVisible = !string.IsNullOrEmpty( newValue.ToString() );
                       ( bindable as OptionRowItemView ).CaptionLabel
                           .Text = newValue.ToString();
                   }
                   else {
                       ( bindable as OptionRowItemView ).CaptionLabel
                           .IsVisible = false;
                   }
               } );

        public string CaptionText {
            get { return ( string )GetValue( CaptionTextProperty ); }
            set { SetValue( CaptionTextProperty, value ); }
        }

        public static readonly BindableProperty BadgeTextProperty =
         BindableProperty.Create(
             nameof( BadgeText ),
             typeof( string ),
             typeof( OptionRowItemView ),
             string.Empty,
             propertyChanged: ( bindable, oldValue, newValue ) => {

                 if ( newValue != null ) {
                     ( bindable as OptionRowItemView ).Badge
                         .IsVisible = ( !string.IsNullOrEmpty( newValue.ToString() )
                         && newValue.ToString() != "0" );
                     ( bindable as OptionRowItemView ).BadgeLabel
                         .Text = newValue.ToString();
                 }
                 else {
                     ( bindable as OptionRowItemView ).Badge
                         .IsVisible = false;
                 }
             } );

        public string BadgeText {
            get { return ( string )GetValue( BadgeTextProperty ); }
            set { SetValue( BadgeTextProperty, value ); }
        }

        public static readonly BindableProperty GroupTitleTextProperty =
     BindableProperty.Create(
         nameof( GroupTitleText ),
         typeof( string ),
         typeof( OptionRowItemView ),
         string.Empty,
         propertyChanged: ( bindable, oldValue, newValue ) => {

             if ( newValue != null ) {
                 ( bindable as OptionRowItemView ).GroupTitleContainer
                     .IsVisible = !string.IsNullOrEmpty( newValue.ToString() );
                 ( bindable as OptionRowItemView ).GroupTitleLabel
                     .Text = newValue.ToString();
             }
             else {
                 ( bindable as OptionRowItemView ).GroupTitleContainer
                     .IsVisible = false;
             }
         } );

        public string GroupTitleText {
            get { return ( string )GetValue( GroupTitleTextProperty ); }
            set { SetValue( GroupTitleTextProperty, value ); }
        }

        public static readonly BindableProperty LeftIconProperty =
            BindableProperty.Create(
                nameof( LeftIcon ),
                typeof( string ),
                typeof( OptionRowItemView ),
                string.Empty,
                propertyChanged: ( bindable, oldValue, newValue ) => {

                    ( bindable as OptionRowItemView ).IconL.IsVisible = true;
                    ( bindable as OptionRowItemView ).IconL.Text = newValue.ToString();
                } );

        public string LeftIcon {
            get { return ( string )GetValue( LeftIconProperty ); }
            set { SetValue( LeftIconProperty, value ); }
        }

        public static readonly BindableProperty RightIconProperty =
        BindableProperty.Create(
            nameof( RightIcon ),
            typeof( string ),
            typeof( OptionRowItemView ),
            string.Empty,
            propertyChanged: ( bindable, oldValue, newValue ) => {

                ( bindable as OptionRowItemView ).IconR.IsVisible = true;

                string newValueAsString = newValue.ToString();
                ( bindable as OptionRowItemView ).IconR.Text = newValue.ToString();

            } );

        public string RightIcon {
            get { return ( string )GetValue( RightIconProperty ); }
            set { SetValue( RightIconProperty, value ); }
        }

        public static readonly BindableProperty IconColorProperty =
            BindableProperty.Create(
                nameof( IconColor ),
                typeof( Color ),
                typeof( OptionRowItemView ),
                Color.Black,
                propertyChanged: ( bindable, oldValue, newValue ) => {

                    ( bindable as OptionRowItemView ).IconL
                        .TextColor = ( Color )newValue;
                } );

        public Color IconColor {
            get { return ( Color )GetValue( IconColorProperty ); }
            set { SetValue( IconColorProperty, value ); }
        }

        public static readonly BindableProperty RightIconIsVisibleProperty =
            BindableProperty.Create(
                nameof( RightIconIsVisible ),
                typeof( bool ),
                typeof( OptionRowItemView ),
                true,
                propertyChanged: ( bindable, oldValue, newValue ) => {

                    ( bindable as OptionRowItemView ).IconR
                        .IsVisible = ( bool )newValue;
                } );

        public bool RightIconIsVisible {
            get { return ( bool )GetValue( RightIconIsVisibleProperty ); }
            set { SetValue( RightIconIsVisibleProperty, value ); }
        }

        public static readonly BindableProperty SeparatorColorProperty =
            BindableProperty.Create(
                nameof( SeparatorColor ),
                typeof( Color ),
                typeof( OptionRowItemView ),
                Color.Black,
                propertyChanged: ( bindable, oldValue, newValue ) => {

                    ( bindable as OptionRowItemView ).Separator
                        .BackgroundColor = ( Color )newValue;

                    ( bindable as OptionRowItemView ).GroupSeparator
                    .BackgroundColor = ( Color )newValue;
                } );

        public Color SeparatorColor {
            get { return ( Color )GetValue( SeparatorColorProperty ); }
            set { SetValue( SeparatorColorProperty, value ); }
        }

        public static readonly BindableProperty RightIconColorProperty =
            BindableProperty.Create(
                nameof( RightIconColor ),
                typeof( Color ),
                typeof( OptionRowItemView ),
                Color.Black,
                propertyChanged: ( bindable, oldValue, newValue ) => {

                    ( bindable as OptionRowItemView ).IconR
                        .TextColor = ( Color )newValue;

                } );

        public Color RightIconColor {
            get { return ( Color )GetValue( RightIconColorProperty ); }
            set { SetValue( RightIconColorProperty, value ); }
        }

        public static readonly BindableProperty TextColorProperty =
          BindableProperty.Create(
              nameof( TextColor ),
              typeof( Color ),
              typeof( OptionRowItemView ),
              Color.Black,
              propertyChanged: ( bindable, oldValue, newValue ) => {

                  ( bindable as OptionRowItemView ).TitleLabel
                      .TextColor = ( Color )newValue;

              } );

        public Color TextColor {
            get { return ( Color )GetValue( TextColorProperty ); }
            set { SetValue( TextColorProperty, value ); }
        }

        public static readonly BindableProperty RightIconSizeProperty =
           BindableProperty.Create(
               nameof( RightIconSize ),
               typeof( double ),
               typeof( OptionRowItemView ),
               (double)16,
               propertyChanged: ( bindable, oldValue, newValue ) => {

                   ( bindable as OptionRowItemView ).IconR
                       .FontSize = ( double )newValue;
               } );

        public double RightIconSize {
            get { return ( double )GetValue( RightIconSizeProperty ); }
            set { SetValue( RightIconSizeProperty, value ); }
        }
    }
}
