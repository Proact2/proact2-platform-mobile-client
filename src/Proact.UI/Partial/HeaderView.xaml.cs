using System;
using System.Collections.Generic;
using MvvmCross.Commands;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class HeaderView : Grid {
        public HeaderView() {
            InitializeComponent();

            var profileTapGestureRecognizer = new TapGestureRecognizer();
            profileTapGestureRecognizer.Tapped += ( sender, e ) => {
                if ( ProfileTapCommand != null )
                    ProfileTapCommand.Execute();
            };
            ProfileTapAnchor.GestureRecognizers.Add( profileTapGestureRecognizer );

            var searchTapGestureRecognizer = new TapGestureRecognizer();
            searchTapGestureRecognizer.Tapped += ( sender, e ) => {
                if ( SearchTapCommand != null )
                    SearchTapCommand.Execute();
            };
            SearchButton.GestureRecognizers.Add( searchTapGestureRecognizer );
        }

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
            propertyName: nameof( ImageSource ),
            returnType: typeof( string ),
            declaringType: typeof( HeaderView ),
            defaultValue: string.Empty,
            propertyChanged: ( bindable, oldValue, newValue ) => {
                ( bindable as HeaderView ).profileImage
                    .ImageSource = ( string )newValue;
            }
        );

        public string ImageSource {
            get { return ( string )GetValue( ImageSourceProperty ); }
            set { SetValue( ImageSourceProperty, value ); }
        }

        public static readonly BindableProperty BadgeIsVisibleProperty = BindableProperty.Create(
            propertyName: nameof( BadgeIsVisible ),
            returnType: typeof( bool ),
            declaringType: typeof( HeaderView ),
            defaultValue: false,
            propertyChanged: ( bindable, oldValue, newValue ) => {
                ( bindable as HeaderView ).sideMenuBadge
                    .IsVisible = ( bool )newValue;
            }
        );

        public bool BadgeIsVisible {
            get { return ( bool )GetValue( BadgeIsVisibleProperty ); }
            set { SetValue( BadgeIsVisibleProperty, value ); }
        }

        public static readonly BindableProperty SearchIsVisibleProperty = BindableProperty.Create(
            propertyName: nameof( SearchIsVisible ),
            returnType: typeof( bool ),
            declaringType: typeof( HeaderView ),
            defaultValue: true,
            propertyChanged: ( bindable, oldValue, newValue ) => {
                ( bindable as HeaderView ).SearchButton
                    .IsVisible = ( bool )newValue;
            }
        );

        public bool SearchIsVisible {
            get { return ( bool )GetValue( SearchIsVisibleProperty ); }
            set { SetValue( SearchIsVisibleProperty, value ); }
        }


        #region Command

        public static readonly BindableProperty ProfileTapCommandProperty = BindableProperty.Create(
            propertyName: nameof( ProfileTapCommand ),
            returnType: typeof( IMvxCommand ),
            declaringType: typeof( HeaderView ),
            defaultValue: null
        );

        public IMvxCommand ProfileTapCommand {
            get { return ( IMvxCommand )GetValue( ProfileTapCommandProperty ); }
            set { SetValue( ProfileTapCommandProperty, value ); }
        }


        public static readonly BindableProperty SearchTapCommandProperty = BindableProperty.Create(
        propertyName: nameof( SearchTapCommand ),
        returnType: typeof( IMvxCommand ),
        declaringType: typeof( HeaderView ),
        defaultValue: null
    );

        public IMvxCommand SearchTapCommand {
            get { return ( IMvxCommand )GetValue( SearchTapCommandProperty ); }
            set { SetValue( SearchTapCommandProperty, value ); }
        }

        //public static readonly BindableProperty CommandParamProperty = BindableProperty.Create(
        //  nameof( CommandParam ),
        //  typeof( string ),
        //  typeof( ImageProfileEditView ),
        //  string.Empty
        //);

        //public string CommandParam {
        //    get { return ( string )GetValue( CommandParamProperty ); }
        //    set { SetValue( CommandParamProperty, value ); }
        //}

        #endregion
    }
}
