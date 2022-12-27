using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class SurveyBooleanButton : Grid {

        private Color SelectedColor;
        private Color UnselectedColor;

        public SurveyBooleanButton() {
            InitializeComponent();

            UnselectedColor = ( Color )Application.Current.Resources["PrimaryColor"];
            SelectedColor = Color.White;
        }

        void OnYesButtonTapped( System.Object sender, System.EventArgs e ) {
            SetTrue();
        }

        void OnNoButtonTapped( System.Object sender, System.EventArgs e ) {
            SetFalse();
        }

        private void SetTrue() {
            Value = true;
            YesButtonImage.Source = "btn_boolYesSelected";
            NoButtonImage.Source = "btn_boolNoUnselected";
            YesButtonLabel.TextColor = SelectedColor;
            NoButtonLabel.TextColor = UnselectedColor;
        }

        private void SetFalse() {
            Value = false;
            YesButtonImage.Source = "btn_boolYesUnselected";
            NoButtonImage.Source = "btn_boolNoSelected";
            YesButtonLabel.TextColor = UnselectedColor;
            NoButtonLabel.TextColor = SelectedColor;
        }

        public static readonly BindableProperty ValueProperty =
           BindableProperty.Create( "Value",
               typeof( bool ),
               typeof( SurveyBooleanButton ),
               true,
               BindingMode.TwoWay,
               null,
               OnValuePropertyChanged,
               null,
               null,
               null );

        public static void OnValuePropertyChanged( BindableObject bindable, object oldValue, object newValue ) {
            var control = bindable as SurveyBooleanButton;
            if ( control == null ) return;
            if ( newValue == null ) return;

            var selected = ( bool )newValue;

            if ( selected ) {
                control.SetTrue();
            }
            else {
                control.SetFalse();
            }
        }

        public bool Value {
            get {
                return ( bool )GetValue( ValueProperty );
            }
            set {
                SetValue( ValueProperty, value );
            }
        }
    }
}
