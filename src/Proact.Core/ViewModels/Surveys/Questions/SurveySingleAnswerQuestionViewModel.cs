using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class SurveySingleAnswerQuestionViewModel : SurveyQuestionViewModel {

        public Command SelectionChangedCommand { get; set; }
        public List<SurveysSelectableAnswer> Answers { get; set; }
        public SurveysSelectableAnswer SelectedAnswer { get; set; }
        public int AnswersCollectionHeight { get; set; }

        private int _answerCollectionCellHeight = 40;

        protected override void UIInitialized() {
            InitModel();
            InitAnswersList();
            SetAnswersCollectionHeight();
            InitUICommand();         
        }

        private void InitModel() {
            if ( !EditMode ) {
                QuestionModel.Answers = new SurveyMultipleAnswerModel();
            }
        }

        private void InitAnswersList() {
            Answers = ( ( SurveysSingleChoiceQuestionModelAnswersContainer )QuestionModel
                .AnswersContainer ).SelectableAnswers;
            SelectedAnswer = Answers.Find( x => x.Selected );
            RaisePropertyChanged( () => Answers );
        }

        private void SetAnswersCollectionHeight() {
            AnswersCollectionHeight = Answers.Count * _answerCollectionCellHeight;
            RaisePropertyChanged( () => AnswersCollectionHeight );
        }

        private void InitUICommand() {
            SelectionChangedCommand = new Command( UpdateUIOnSelectionChanged );
        }

        private void UpdateUIOnSelectionChanged() {
            foreach ( var selectableAnswer in Answers ) {
                selectableAnswer.Selected = false;
                if ( selectableAnswer.AnswerId == SelectedAnswer.AnswerId ) {
                    selectableAnswer.Selected = true;
                }
            }
            Answers = new List<SurveysSelectableAnswer>( Answers );
            RaisePropertyChanged( () => Answers );
        }

        protected override bool Validate() {
            bool isValid = Answers.FindAll( x => x.Selected ).Count > 0;
            if ( isValid ) {
                UpdateModel();
            }
            return isValid;
        }

        private void UpdateModel() {
            ( ( SurveyMultipleAnswerModel )QuestionModel.Answers )
                     .SelectedAnswers = new List<Guid>() { SelectedAnswer.AnswerId };

        }
    }
}
