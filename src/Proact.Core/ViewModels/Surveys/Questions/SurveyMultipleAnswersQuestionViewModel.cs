using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class SurveyMultipleAnswersQuestionViewModel : SurveyQuestionViewModel {

        public Command SelectionChangedCommand { get; set; }
        public List<SurveysSelectableAnswer> Answers { get; set; }
        public ObservableCollection<object> SelectedAnswers { get; set; }
        public int AnswersCollectionHeight { get; set; }

        private int _answerCollectionCellHeight = 40;

        protected override void UIInitialized() {
            InitModel();
            InitAnswersList();
            SetAnswersCollectionHeight();
            InitUICommand();
            SelectedAnswers = new ObservableCollection<object>();
        }

        private void InitModel() {
            if ( !EditMode ) {
                QuestionModel.Answers = new SurveyMultipleAnswerModel();
            }
        }

        private void InitAnswersList() {
            Answers = ( ( SurveysMultipleChoiceQuestionModelAnswersContainer )QuestionModel
             .AnswersContainer ).SelectableAnswers;

            var selectedAnswers = ( ( SurveyMultipleAnswerModel )QuestionModel
            .Answers ).SelectedAnswers;

            if ( selectedAnswers != null ) {
                foreach ( Guid selectedAnswerIndex in selectedAnswers ) {
                    Answers.Find( x => x.AnswerId == selectedAnswerIndex ).Selected = true;
                }
            }
            RaisePropertyChanged( () => Answers );
        }

        private void SetAnswersCollectionHeight() {
            AnswersCollectionHeight = Answers.Count * _answerCollectionCellHeight;
            RaisePropertyChanged( () => AnswersCollectionHeight );
        }

        private void InitUICommand() {
            SelectionChangedCommand = new Command( SelectionCHangedActionHandle );
        }

        private void SelectionCHangedActionHandle() {
            UnselectAllAnswers();
            UpdateAnswersUIOnSelectionChanged();
        }

        private void UnselectAllAnswers() {
            foreach ( var answer in Answers ) {
                answer.Selected = false;
            }
        }

        private void UpdateAnswersUIOnSelectionChanged() {
            foreach ( var answerModel in SelectedAnswers ) {
                var index = Answers.FindIndex( x =>
                x.AnswerId == ( ( SurveysSelectableAnswer )answerModel ).AnswerId );

                Answers[index].Selected = true;
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
            var selectedAnswers = new List<Guid>();
            foreach ( var answer in Answers ) {
                if ( answer.Selected ) {
                    selectedAnswers.Add( answer.AnswerId );
                }
            }
            ( ( SurveyMultipleAnswerModel )QuestionModel.Answers )
                .SelectedAnswers = selectedAnswers;

        }
    }
}
