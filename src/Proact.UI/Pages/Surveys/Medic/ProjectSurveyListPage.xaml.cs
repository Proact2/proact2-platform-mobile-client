﻿using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Proact.Mobile.Core.ViewModels;
using Proact.Mobile.UI;
using Xamarin.Forms.Xaml;

namespace Proact.UI.Pages {
    [XamlCompilation( XamlCompilationOptions.Compile )]
    [MvxContentPagePresentation( WrapInNavigationPage = true )]
    public partial class ProjectSurveyListPage : MvxContentPage<ProjectSurveyListViewModel> {

        private PopupService _popupService;

        public ProjectSurveyListPage() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            SetPopupService();
        }

        private void SetPopupService() {
            _popupService = new PopupService();
            ViewModel.SetPopupService( _popupService );
        }
    }
}