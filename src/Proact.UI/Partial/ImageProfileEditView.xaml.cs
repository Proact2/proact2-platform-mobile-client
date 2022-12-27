using System;
using System.Collections.Generic;
using MvvmCross.Commands;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class ImageProfileEditView : Grid {
 
        public ImageProfileEditView() {
            InitializeComponent();

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += ( sender, e ) => {
                if ( EditImageCommand != null )
                    EditImageCommand.Execute( CommandParam );
            };

            EditImageButton.GestureRecognizers.Add( tapGestureRecognizer );
        }

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(

            propertyName: nameof( ImageSource ),
            returnType: typeof( string ),
            declaringType: typeof( ImageProfileEditView ),
            defaultValue: string.Empty,
            propertyChanged: ( bindable, oldValue, newValue ) => {

                ( bindable as ImageProfileEditView ).ImageView
                                .ImageSource = ( string )newValue;
            }
        );

        public string ImageSource {
            get { return ( string )GetValue( ImageSourceProperty ); }
            set { SetValue( ImageSourceProperty, value ); }
        }

        #region Command

        public static readonly BindableProperty EditImageCommandProperty = BindableProperty.Create(

            propertyName: nameof( EditImageCommand ),
            returnType: typeof( IMvxCommand ),
            declaringType: typeof( ImageProfileEditView ),
            defaultValue: null
        );

        public IMvxCommand EditImageCommand {
            get { return ( IMvxCommand )GetValue( EditImageCommandProperty ); }
            set { SetValue( EditImageCommandProperty, value ); }
        }

        public static readonly BindableProperty CommandParamProperty = BindableProperty.Create(
          nameof( CommandParam ),
          typeof( string ),
          typeof( ImageProfileEditView ),
          string.Empty
        );

        public string CommandParam {
            get { return ( string )GetValue( CommandParamProperty ); }
            set { SetValue( CommandParamProperty, value ); }
        }

        #endregion
    }
}
