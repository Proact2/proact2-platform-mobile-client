using System;
using System.Collections.Generic;
using Proact.Mobile.Core.ViewModels;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class SurveyChartLegendView : StackLayout {
        public SurveyChartLegendView() {
            InitializeComponent();
        }

        public static readonly BindableProperty EntriesProperty
           = BindableProperty.Create(
           propertyName: nameof( Entries ),
           returnType: typeof( List<LegendEntry> ),
           declaringType: typeof( SurveyChartLegendView ),
           defaultValue: new List<LegendEntry>(),
           propertyChanged: ( bindable, oldValue, newValue ) => {
               if ( newValue != null ) {
                   var stackLayout = ( bindable as SurveyChartLegendView );
                   var items = newValue as List<LegendEntry>;
                   BindableLayout.SetItemsSource( stackLayout, items );
               }
           }
       );

        public List<LegendEntry> Entries {
            get { return ( List<LegendEntry> )GetValue( EntriesProperty ); }
            set { SetValue( EntriesProperty, value ); }
        }
    }
}
