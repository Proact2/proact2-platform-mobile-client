using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class MessageReplyCounterView : Grid {
        public MessageReplyCounterView() {
            InitializeComponent();
        }

        public static readonly BindableProperty MessageCountProperty =
          BindableProperty.Create(
              nameof( MessageCount ),
              typeof( string ),
              typeof( MessageReplyCounterView ),
              "0",
              propertyChanged: ( bindable, oldValue, newValue ) => {
                  ( bindable as MessageReplyCounterView ).CounterTextLabel
                        .Text = newValue.ToString();
              }
              );

        public string MessageCount {
            get { return ( string )GetValue( MessageCountProperty ); }
            set { SetValue( MessageCountProperty, value ); }
        }

    }
}
