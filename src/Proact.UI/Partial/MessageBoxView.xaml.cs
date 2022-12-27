using System;
using System.Collections.Generic;
using Proact.Mobile.Core;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class MessageBoxView : Frame {

        public MessageBoxView() {
            InitializeComponent();

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += ( sender, e ) => {
                IsVisible = false;
            };

            CloseButton.GestureRecognizers.Add( tapGestureRecognizer );

            TitleLabel.IsVisible = false;
            MessageLabel.IsVisible = false;
            CloseButton.IsVisible = false;
            IsVisible = false;
        }

        public static readonly BindableProperty MessageBoxModelProperty =
            BindableProperty.Create( "MessageBoxModel", typeof( MessageBoxModel ), typeof( MessageBoxView ),

                null, propertyChanged: ( bindable, oldValue, newValue ) => {

                    if ( newValue != null ) {

                        MessageBoxModel messageMoodel = ( MessageBoxModel )newValue;

                        if ( messageMoodel != null ) {
                            ( bindable as MessageBoxView ).IsVisible = true;
                            ( bindable as MessageBoxView ).TitleLabel.Text = messageMoodel.Title;
                            ( bindable as MessageBoxView ).TitleLabel.IsVisible = !string.IsNullOrEmpty( messageMoodel.Title );
                            ( bindable as MessageBoxView ).MessageLabel.Text = messageMoodel.Message;
                            ( bindable as MessageBoxView ).MessageLabel.IsVisible = !string.IsNullOrEmpty( messageMoodel.Message );
                            ( bindable as MessageBoxView ).SetType( bindable, messageMoodel.Type );
                            ( bindable as MessageBoxView ).CloseButton.IsVisible = messageMoodel.Closable;
                        }
                    }
                }
        );

        public MessageBoxModel MessageBoxModel {
            get { return ( MessageBoxModel )GetValue( MessageBoxModelProperty ); }
            set { SetValue( MessageBoxModelProperty, value ); }
        }

        private void SetType( BindableObject bindable, MessageBoxType messageBoxType ) {

            switch ( messageBoxType ) {

                case MessageBoxType.ERROR:

                    ( bindable as MessageBoxView ).BorderColor
                        = ( Color )Application.Current
                            .Resources["ErrorTextColor"];


                    ( bindable as MessageBoxView ).BackgroundColor
                        = ( Color )Application.Current
                            .Resources["ErrorBGColor"];

                    ( bindable as MessageBoxView ).TitleLabel.TextColor
                        = ( Color )Application.Current
                            .Resources["ErrorTextColor"];

                    ( bindable as MessageBoxView ).MessageLabel.TextColor
                        = ( Color )Application.Current
                            .Resources["ErrorTextColor"];

                    ( bindable as MessageBoxView ).CloseButton.TextColor
                       = ( Color )Application.Current
                           .Resources["ErrorTextColor"];

                    break;
                case MessageBoxType.SUCCESS:

                    ( bindable as MessageBoxView ).BorderColor
                        = ( Color )Application.Current
                            .Resources["SuccessTextColor"];


                    ( bindable as MessageBoxView ).BackgroundColor
                        = ( Color )Application.Current
                            .Resources["SuccessBGColor"];

                    ( bindable as MessageBoxView ).TitleLabel.TextColor
                        = ( Color )Application.Current
                            .Resources["SuccessTextColor"];

                    ( bindable as MessageBoxView ).MessageLabel.TextColor
                        = ( Color )Application.Current
                            .Resources["SuccessTextColor"];

                    ( bindable as MessageBoxView ).CloseButton.TextColor
                      = ( Color )Application.Current
                            .Resources["SuccessTextColor"];

                    break;
                case MessageBoxType.WARNING:

                    ( bindable as MessageBoxView ).BorderColor
                        = ( Color )Application.Current
                            .Resources["WarningColorDark"];

                    ( bindable as MessageBoxView ).BackgroundColor
                        = ( Color )Application.Current
                            .Resources["WarningColor"];

                    ( bindable as MessageBoxView ).TitleLabel.TextColor
                        = ( Color )Application.Current
                            .Resources["WarningColorDark"];

                    ( bindable as MessageBoxView ).MessageLabel.TextColor
                        = ( Color )Application.Current
                            .Resources["WarningColorDark"];

                    ( bindable as MessageBoxView ).CloseButton.TextColor
                       = ( Color )Application.Current
                           .Resources["WarningColorDark"];

                    break;
                case MessageBoxType.INFO:

                    ( bindable as MessageBoxView ).BorderColor
                        = ( Color )Application.Current
                            .Resources["InfoTextColor"];

                    ( bindable as MessageBoxView ).BackgroundColor
                        = ( Color )Application.Current
                            .Resources["InfoBGColor"];

                    ( bindable as MessageBoxView ).TitleLabel.TextColor
                        = ( Color )Application.Current
                            .Resources["InfoTextColor"];

                    ( bindable as MessageBoxView ).MessageLabel.TextColor
                        = ( Color )Application.Current
                            .Resources["InfoTextColor"];

                    ( bindable as MessageBoxView ).CloseButton.TextColor
                       = ( Color )Application.Current
                           .Resources["InfoTextColor"];

                    break;
            }
        }

    }
}


