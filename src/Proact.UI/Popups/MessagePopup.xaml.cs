using System;
using System.Collections.Generic;
using Proact.Mobile.Core;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class MessagePopup : PopupPage {
        public MessagePopup( PopupMessageModel messagePopupModel ) {
            InitializeComponent();
            UpdateUI( messagePopupModel );
        }

        private void UpdateUI( PopupMessageModel messagePopupModel ) {
            MessageTextLabel.Text = messagePopupModel.MessageText;

            if ( messagePopupModel.Type == PopupMessageType.SUCCESS ) {
                IconBackground.BackgroundColor
                    = ( Color )Application.Current.Resources["SuccessBGColor"];
                MessageIcon.Text = FontAwesome.FontAwesomeIcons.Check;

            }
            else if ( messagePopupModel.Type == PopupMessageType.ERROR ) {
                IconBackground.BackgroundColor
                   = ( Color )Application.Current.Resources["ErrorBGColor"];
                MessageIcon.Text = FontAwesome.FontAwesomeIcons.Times;

            }
            else if ( messagePopupModel.Type == PopupMessageType.INFO ) {
                IconBackground.BackgroundColor
                   = ( Color )Application.Current.Resources["InfoBGColor"];
                MessageIcon.Text = FontAwesome.FontAwesomeIcons.InfoCircle;

            }
        }
    }
}
