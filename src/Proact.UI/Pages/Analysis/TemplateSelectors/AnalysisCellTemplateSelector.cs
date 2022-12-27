using Proact.Mobile.Core.Models;
using Proact.UI;
using Xamarin.Forms;
namespace Proact.UI {
    public class AnalysisCellTemplateSelector : DataTemplateSelector{

        public DataTemplate VisibleTemplate { get; set; }
        public DataTemplate NotVisibleTemplate { get; set; }

        public AnalysisCellTemplateSelector() {
            VisibleTemplate = new DataTemplate( typeof( AnalysisViewCell ) );
            NotVisibleTemplate = new DataTemplate( typeof( AnalysisNotVisibleViewCell ) );
        }
        
        protected override DataTemplate OnSelectTemplate( object item, BindableObject container ) {
            var analysisModel = ( AnalysisModel )item;
            if ( analysisModel.ResultsVisible ) {
                return VisibleTemplate;
            }
            return NotVisibleTemplate;
        }
    }
}
