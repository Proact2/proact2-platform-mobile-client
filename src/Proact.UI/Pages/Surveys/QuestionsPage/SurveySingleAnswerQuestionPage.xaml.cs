using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Proact.Mobile.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace Proact.UI.Pages {
    [XamlCompilation( XamlCompilationOptions.Compile )]
    [MvxContentPagePresentation( WrapInNavigationPage = true )]
    public partial class SurveySingleAnswerQuestionPage : MvxContentPage<SurveySingleAnswerQuestionViewModel> {

        public SurveySingleAnswerQuestionPage() {
            InitializeComponent();
        }
    }
}