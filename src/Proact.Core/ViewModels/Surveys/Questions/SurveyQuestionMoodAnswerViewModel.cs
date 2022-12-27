using System;
using MvvmCross.Commands;
using Proact.Mobile.Core.Models;

namespace Proact.Mobile.Core.ViewModels {
    public class SurveyQuestionMoodAnswerViewModel : SurveyQuestionViewModel {

        public IMvxCommand<string> MoodButtonPressedCommand { get; private set; }

        public MessageMood MoodAnswer { get; set; }

        private double _veryGoodButtonScale;
        public double VeryGoodButtonScale {
            get => _veryGoodButtonScale;
            set => SetProperty( ref _veryGoodButtonScale, value );
        }

        private double _goodButtonScale;
        public double GoodButtonScale {
            get => _goodButtonScale;
            set => SetProperty( ref _goodButtonScale, value );
        }

        private double _badButtonScale;
        public double BadButtonScale {
            get => _badButtonScale;
            set => SetProperty( ref _badButtonScale, value );
        }

        private double _veryBadButtonScale;
        public double VeryBadButtonScale {
            get => _veryBadButtonScale;
            set => SetProperty( ref _veryBadButtonScale, value );
        }

        private double _unselectedButtonScale = 0.8;
        private double _selectedButtonScale = 1.2;

        public SurveyQuestionMoodAnswerViewModel() {
        }

        protected override void UIInitialized() {
            InitModel();
            UnselectMoodButtons();
            UpdateMoodButton();
            InitUICommands();
        }

        private void InitModel() {
            if ( QuestionModel.Answers == null ) {
                QuestionModel.Answers = new SurveyMoodAnswerModel();
            }
            MoodAnswer = ( ( SurveyMoodAnswerModel )QuestionModel.Answers )
              .MoodAnswer;
            UpdateMoodButton();
        }

        protected override bool Validate() {
          
            if ( MoodAnswer != MessageMood.None ) {
                ( ( SurveyMoodAnswerModel )QuestionModel.Answers )
                    .MoodAnswer = MoodAnswer;
                return true;
            }
            else {
                return false;
            }
        }

        private void InitUICommands() {
            MoodButtonPressedCommand
                = new MvxCommand<string>( MoodButtonPressedActioonHandle );
        }

        private void MoodButtonPressedActioonHandle( string mood ) {
            UnselectMoodButtons();
            var patientMood
                = ( MessageMood )Enum.Parse( typeof( MessageMood ), mood );

            if ( patientMood == MoodAnswer ) {
                MoodAnswer = MessageMood.None;
            }
            else {
                MoodAnswer = patientMood;
                UpdateMoodButton();
            }
        }

        private void UpdateMoodButton() {
            switch ( MoodAnswer ) {

                case MessageMood.VeryGood:
                    VeryGoodButtonScale = _selectedButtonScale;
                    break;

                case MessageMood.Good:
                    GoodButtonScale = _selectedButtonScale;
                    break;

                case MessageMood.Bad:
                    BadButtonScale = _selectedButtonScale;
                    break;

                case MessageMood.VeryBad:
                    VeryBadButtonScale = _selectedButtonScale;
                    break;
            }
        }

        private void UnselectMoodButtons() {
            VeryGoodButtonScale = _unselectedButtonScale;
            GoodButtonScale = _unselectedButtonScale;
            BadButtonScale = _unselectedButtonScale;
            VeryBadButtonScale = _unselectedButtonScale;
        }
    }
}
