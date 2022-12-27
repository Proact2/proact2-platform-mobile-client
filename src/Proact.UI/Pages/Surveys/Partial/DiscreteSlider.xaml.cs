using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class DiscreteSlider : Grid {

        private double _stepValue = 1.0;
        private List<int> _values;

        public DiscreteSlider() {
            InitializeComponent();        
        }

        void OnSliderValueChanged( object sender, ValueChangedEventArgs e ) {
            var newStep = Math.Round( e.NewValue / _stepValue );
            var discreteValue = newStep * _stepValue;
            MainSlider.Value = discreteValue;
            Value = discreteValue;
        }

        void BuildSlider() {
            if ( _values != null ) {
                TicksStack.Children.Clear();
                MainSlider.Maximum = _values[_values.Count - 1];
                MainSlider.Minimum = _values[0];

                var ballSize = MainSlider.Height;
                var labelWidth = ( MainSlider.Width - ballSize ) / ( _values.Count - 1 );

                var paddingOffset = 16 / _values.Count;

                for ( var i = 0; i < _values.Count; i++ ) {
                    var textWidth = 10;
                    var margin = ( ballSize / 2 ) - ( textWidth / 2 )  - paddingOffset;
                    margin = margin > 0 ? margin : 0;
                    var label = new Label {
                        Text = _values[i].ToString(),
                        WidthRequest = i == _values.Count - 1 ? (ballSize - margin ) - paddingOffset : (labelWidth - margin) - paddingOffset,
                        HorizontalTextAlignment = i == _values.Count - 1 ? TextAlignment.End : TextAlignment.Start,
                        Margin = i == _values.Count - 1 ? new Thickness( 0, 0, margin, 0 ) : new Thickness( margin, 0, 0, 0 ),
                        LineBreakMode = LineBreakMode.NoWrap
                    };

                    TicksStack.Children.Add( label );
                }
            }
        }

        public static readonly BindableProperty TickValuesProperty
            = BindableProperty.Create(
            propertyName: nameof( TickValues ),
            returnType: typeof( List<int> ),
            declaringType: typeof( DiscreteSlider ),
            defaultValue: new List<int>(),
            propertyChanged: ( bindable, oldValue, newValue ) => {
                if ( newValue != null ) {
                    ( bindable as DiscreteSlider )._values = ( List<int> )newValue;
                    ( bindable as DiscreteSlider ).BuildSlider();
                }
            }
        );

        public List<int> TickValues {
            get { return ( List<int> )GetValue( TickValuesProperty ); }
            set { SetValue( TickValuesProperty, value ); }
        }

        public static readonly BindableProperty MinLabelTextProperty
            = BindableProperty.Create(
            propertyName: nameof( MinLabelText ),
            returnType: typeof( string ),
            declaringType: typeof( DiscreteSlider ),
            defaultValue: string.Empty,
            propertyChanged: ( bindable, oldValue, newValue ) => {
                if ( newValue != null ) {
                    ( bindable as DiscreteSlider )
                    .MinLabel.Text = newValue.ToString(); ;
                }
            }
        );

        public string MinLabelText {
            get { return ( string )GetValue( MinLabelTextProperty ); }
            set { SetValue( MinLabelTextProperty, value ); }
        }

        public static readonly BindableProperty MaxLabelTextProperty
              = BindableProperty.Create(
              propertyName: nameof( MaxLabelText ),
              returnType: typeof( string ),
              declaringType: typeof( DiscreteSlider ),
              defaultValue: string.Empty,
              propertyChanged: ( bindable, oldValue, newValue ) => {
                  if ( newValue != null ) {
                      ( bindable as DiscreteSlider )
                      .MaxLabel.Text = newValue.ToString(); ;
                  }
              }
          );

        public string MaxLabelText {
            get { return ( string )GetValue( MaxLabelTextProperty ); }
            set { SetValue( MaxLabelTextProperty, value ); }
        }

        public static readonly BindableProperty ValueProperty =
               BindableProperty.Create( "Value",
                   typeof( double ),
                   typeof( DiscreteSlider ),
                   0.0,
                   BindingMode.TwoWay,
                   null,
                   OnValuePropertyChanged,
                   null,
                   null,
                   null );

        public static void OnValuePropertyChanged( BindableObject bindable, object oldValue, object newValue ) {
            var control = bindable as DiscreteSlider;
            if ( control == null ) return;
            if ( newValue == null ) return;

            var newStep = Math.Round( ( double )newValue / control._stepValue );
            control.MainSlider.Value = newStep * control._stepValue;
        }

        public double Value {
            get {
                return ( double )GetValue( ValueProperty );
            }
            set {
                SetValue( ValueProperty, value );
            }
        }


    }
}
