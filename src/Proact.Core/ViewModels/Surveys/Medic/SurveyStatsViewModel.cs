using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microcharts;
using Proact.Mobile.Core.Models;
using Proact.Mobile.Core.ViewModels;
using SkiaSharp;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class SurveyStatsViewModel : BaseViewModel<Guid> {

        public Command ListRefreshCommand { get; private set; }
        private List<QuestionChart> _questionCharts;
        public List<QuestionChart> QuestionCharts {
            get => _questionCharts;
            set => SetProperty( ref _questionCharts, value );
        }

        public DonutChart PartecipantChart { get; set; }

        private ISurveysService _surveyService;
        private Guid _surveyId;
        private SurveyStatsModel _surveyStatsModels;

        public SurveyStatsViewModel( ISurveysService surveysService ) {
            _surveyService = surveysService;
        }

        public async override void Prepare( Guid surveyId ) {
            _surveyId = surveyId;
            PageTitle = Resources.AppResources.SurveysStats;
            SetUICommands();
            await GetSurveyStatsAsync();
            CreateChartsData();
        }

        private void SetUICommands() {
            ListRefreshCommand = new Command( ListRefreshActionHandle );
        }

        private async void ListRefreshActionHandle() {
            await GetSurveyStatsAsync();
            CreateChartsData();
        }

        private async Task GetSurveyStatsAsync() {
            IsBusy = true;
            var result
                = await _surveyService.GetSurveysStats( _surveyId );
            if ( result.Success ) {
                IsBusy = false;
                _surveyStatsModels = result.data;
            }
            else {
                OpenErrorMessagePopup();
            }

            IsBusy = false;
        }

        private void CreateChartsData() {
            var charts = new List<QuestionChart>();
            _surveyStatsModels.QuestionWithAnswers
                .RemoveAll( x => x.Type == SurveyQuestionType.OPEN_ANSWER );


            var partecipantsChart = CreatePartecipantsChart();
            charts.Add( partecipantsChart );

            foreach ( var question in _surveyStatsModels.QuestionWithAnswers ) {

                var questionChart = CreateQuestionChart( question );
                AddEntriesToQuestionChart( questionChart, question );

                charts.Add( questionChart );
            }

            QuestionCharts = charts;
        }

        private QuestionChart CreatePartecipantsChart() {
            List<ChartEntry> chartEntries = new List<ChartEntry>();
            chartEntries.Add(
                new ChartEntry( _surveyStatsModels.Completed ) {
                    ValueLabel = _surveyStatsModels.Completed.ToString(),
                    Label = Resources.AppResources.CompletedSurvey,
                    Color = SKColor.Parse( GetChartColor( 0 ) )
                }
            );

            chartEntries.Add(
               new ChartEntry( _surveyStatsModels.Incompleted ) {
                   ValueLabel = _surveyStatsModels.Incompleted.ToString(),
                   Label = Resources.AppResources.IncompletedSurvey,
                   Color = SKColor.Parse( GetChartColor( 1 ) )
               }
           );

            List<LegendEntry> legendEntries = new List<LegendEntry>();
            legendEntries.Add( new LegendEntry() {
                Label = Resources.AppResources.CompletedSurvey,
                Value = _surveyStatsModels.Completed.ToString(),
                Color = Color.FromHex( GetChartColor( 0 ) )
            } );
            legendEntries.Add( new LegendEntry() {
                Label = Resources.AppResources.IncompletedSurvey,
                Value = _surveyStatsModels.Incompleted.ToString(),
                Color = Color.FromHex( GetChartColor( 1 ) )
            } );

            var partecipantChart = new QuestionChart() {
                Title = Resources.AppResources.SurveysPartecipants,
                Entries = chartEntries,
                LegendEntries = legendEntries,
                Chart = new PieChart {
                    Entries = chartEntries,
                    LabelMode = LabelMode.None
                }
            };

            return partecipantChart;
        }

        private QuestionChart CreateQuestionChart( QuestionWithAnswers question ) {
            QuestionChart questionChart = new QuestionChart() {
                Title = question.Title,
                Question = question.Question,
                Type = question.Type
            };

            return questionChart;
        }

        private void AddEntriesToQuestionChart( QuestionChart questionChart, QuestionWithAnswers question ) {
            var chartEntries = new List<ChartEntry>();
            var legendEntries = new List<LegendEntry>();
            for ( int i = 0; i < question.Answers.Count; i++ ) {
                var answer = question.Answers[i];

                var chartEntry = CreateChartEntry( answer, i );
                var legendEntry = CreateLegendEntry( answer, question.Type, i );
                chartEntries.Add( chartEntry );
                legendEntries.Add( legendEntry );
            }

            questionChart.LegendEntries = legendEntries;
            questionChart.Entries = chartEntries;
            questionChart.Chart = new DonutChart {
                Entries = chartEntries,
                LabelMode = LabelMode.None
            };
        }

        private ChartEntry CreateChartEntry(QuestionAnswer answer, int answerIndex ) {
            var chartEntry = new ChartEntry( answer.Count ) {
                Label = answer.Value,
                ValueLabel = answer.Count.ToString(),
                Color = SKColor.Parse( GetChartColor( answerIndex ) )
            };
            return chartEntry;
        }

        private LegendEntry CreateLegendEntry( QuestionAnswer answer, SurveyQuestionType type, int answeIndex ) {
            var label = answer.Value;
            if ( type == SurveyQuestionType.MOOD ) {
                int moodIndex = 0;
                int.TryParse( answer.Value, out moodIndex );
                MessageMood moodValue = ( MessageMood )moodIndex;
                label = moodValue.ToString().ToLowerInvariant();
            }

            var legendEntry = new LegendEntry() {
                Label = label,
                Value = answer.Count.ToString(),
                Color = Color.FromHex( GetChartColor( answeIndex ) )
            };
            return legendEntry;
        }

        private void OpenErrorMessagePopup() {
            var model = new PopupMessageModel() {
                Type = PopupMessageType.ERROR,
                MessageText = Resources.AppResources.GenericLoadingError
            };
            _popupService.OpenMessagePopup( model );
        }

        private string GetChartColor( int index ) {
            List<string> hexColors
                = new List<string> {
                "#00a65a",
                "#f56954",
                "#f39c12",
                "#00c0ef",
                "#3c8dbc",
                "#d2d6de",
                "#97a3eb",
                "#b3245a",
                "#670f8a",
                "#e03297",
                "#0b73dd",
                "#5cbb73" };

            int baseIndex
                = index % hexColors.Count;

            return hexColors[baseIndex];
        }
    }

    public class QuestionChart {
        public string Title { get; set; }
        public string Question { get; set; }
        public SurveyQuestionType Type { get; set; }
        public Chart Chart { get; set; }
        public List<ChartEntry> Entries { get; set; }
        public List<LegendEntry> LegendEntries { get; set; }
     }

    public class LegendEntry {
        public string Label { get; set; }
        public string Value { get; set; }
        public Color Color { get; set; }
    }
    

}
