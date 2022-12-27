using Proact.Mobile.Core.Models;
using Proact.UI;
using Xamarin.Forms;
namespace Proact.UI {
    public class AddLexiconViewTemplateSelector : DataTemplateSelector{

        public DataTemplate SingleSelectionTemplate { get; set; }
        public DataTemplate MultipleSelectionTemplate { get; set; }

        public AddLexiconViewTemplateSelector() {
            SingleSelectionTemplate = new DataTemplate( typeof( AddSingleSelectionLexiconView ) );
            MultipleSelectionTemplate = new DataTemplate( typeof( AddMultipleSelectionLexiconView ) );
        }
        
        protected override DataTemplate OnSelectTemplate( object item, BindableObject container ) {
            var analysisModel = ( LexiconCategoryModel )item;
            if ( analysisModel.MultipleSelection ) {
                return MultipleSelectionTemplate;
            }
            return SingleSelectionTemplate;
        }
    }
}
