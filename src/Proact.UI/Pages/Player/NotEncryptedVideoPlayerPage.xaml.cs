using System;
using System.Collections.Generic;
using MediaManager;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Proact.Mobile.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proact.UI.Pages {

    [XamlCompilation( XamlCompilationOptions.Compile )]
    [MvxModalPresentation]
    public partial class NotEncryptedVideoPlayerPage : MvxContentPage<NotEncryptedVideoPlayerViewModel> {
        public NotEncryptedVideoPlayerPage() {
            InitializeComponent();
        }
    }
}
