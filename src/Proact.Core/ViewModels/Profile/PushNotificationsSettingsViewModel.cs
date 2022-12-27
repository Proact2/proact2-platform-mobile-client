using System;
using System.Windows.Input;
using MvvmCross.Commands;
using Proact.Mobile.Core.Services;
using Proact.Mobile.Core.ViewModels;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class PushNotificationsSettingsViewModel
        : BaseViewModel<PushNotificationsSettingsModel, PushNotificationsSettingsModel> {

        public Command SaveCommand { get; private set; }
        public Command RemoveTimeLimitCommand { get; private set; }

        public TimeSpan StartAt { get; set; }
        public TimeSpan StopAt { get; set; }

        public PushNotificationsSettingsModel SettingsModel { get; set; }

        private bool _errorIsVisible;
        public bool ErrorIsVisible {
            get => _errorIsVisible;
            set => SetProperty( ref ( _errorIsVisible ), value );
        }

        private IPushNotificationsService _pushNotificationsService;
        private ILocalDataWriteService _localDataWriteService;

        public PushNotificationsSettingsViewModel(
            IPushNotificationsService pushNotificationsService,
            ILocalDataWriteService localDataWriteService ) {
            _pushNotificationsService = pushNotificationsService;
            _localDataWriteService = localDataWriteService;
        }

        public override void Prepare( PushNotificationsSettingsModel parameter ) {
            SettingsModel = parameter;
            InitUI();
            SetUICommands();
        }

        private void InitUI() {
            ErrorIsVisible = false;

            if ( SettingsModel.AllDay ) {
                StartAt = TimeSpan.Zero;
                StopAt = TimeSpan.Zero;
            }
            else {
                StartAt = SettingsModel.LocalStartAt;
                StopAt = SettingsModel.LocalStopAt;
            }          
        }

        private void SetUICommands() {
            SaveCommand = new Command( SaveAndClosePage );
            RemoveTimeLimitCommand = new Command( ResetAndClosePage );
        }

        private async void ResetAndClosePage() {
            _popupService.OpenLoadingPopup();
            SettingsModel.AllDay = true;
            await _pushNotificationsService.ResetSettings();
            await _popupService.CloseAllPopup();
            await _navigationService.Close( this, SettingsModel );
        }

        private async void SaveAndClosePage() {
            _popupService.OpenLoadingPopup();

            SettingsModel.StartAtUtc = _pushNotificationsService.ConvertToUTC( StartAt );
            SettingsModel.StopAtUtc = _pushNotificationsService.ConvertToUTC( StopAt );
            SettingsModel.AllDay = false;
            await _pushNotificationsService.SetNotificationTimeRangeForAllDays(
                SettingsModel.StartAtUtc, SettingsModel.StopAtUtc );
            await _popupService.CloseAllPopup();
            await _navigationService.Close( this, SettingsModel );
        }

    }
}
