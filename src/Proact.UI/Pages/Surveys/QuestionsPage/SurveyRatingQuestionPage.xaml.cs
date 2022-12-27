using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Proact.Mobile.Core;
using Proact.Mobile.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace Proact.UI.Pages {
    [XamlCompilation( XamlCompilationOptions.Compile )]
    [MvxContentPagePresentation( WrapInNavigationPage = true )]
    public partial class SurveyRatingQuestionPage : MvxContentPage<SurveyRatingQuestionViewModel> {

        public SurveyRatingQuestionPage() {
            InitializeComponent();
            HideSlider();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            InitSlider();
        }

        public void InitSlider() {
            var properties = (SurveysRatingQuestionModelProperties) ViewModel 
                .QuestionModel.Properties;
      
            var answer = ( SurveyRatingAnswerModel )ViewModel
            .QuestionModel.Answers;

            slider.MaxValue = properties.Max;
            slider.MinValue = properties.Min;

            if ( answer.RatingAnswer == 0 ) {
                slider.Value = slider.MinValue;
            }
            else {
                slider.Value = answer.RatingAnswer;
            }

            ShowSlider();
        }

        private void ShowSlider() {
            SliderWrapper.IsVisible = true;
        }

        private void HideSlider() {
            SliderWrapper.IsVisible = false;
        }
    }
}