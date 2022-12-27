using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Forms.Views;
using Proact.Mobile.Core;
using Proact.Mobile.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proact.UI.Pages {
    [XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class DocumentViewerPage : MvxContentPage<DocumentViewerViewModel> {

        public DocumentViewerPage() {
            InitializeComponent();

            actIndicator.IsVisible = true;
            actIndicator.IsRunning = true;
        }

        void WebViewNavigating( System.Object sender, Xamarin.Forms.WebNavigatingEventArgs e ) {
            actIndicator.IsVisible = true;
            actIndicator.IsRunning = true;
        }

        void WebViewNavigated( System.Object sender, Xamarin.Forms.WebNavigatedEventArgs e ) {

            actIndicator.IsVisible = false;
            actIndicator.IsRunning = false;

        }

        void ReloadButtonClicked( object sender, EventArgs e ) {
            pdfWebView.Reload();
        }


    }
}

