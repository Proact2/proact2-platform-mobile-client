using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Proact.Mobile.Core.Models {
    public class LexiconModel {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<LexiconCategoryModel> Categories { get; set; }
    }

    public class LexiconCategoryModel {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool MultipleSelection { get; set; }
        public List<LexiconLabelModel> Labels { get; set; }
        
        //Bindable properties

        public ObservableCollection<LexiconLabelModel> SelectedLabels { get; set; } 
            = new ObservableCollection<LexiconLabelModel>();

        public string SingleSelectionSelectedValue {
            get {
                if ( SelectedLabels == null || SelectedLabels.Count == 0 ) {
                    return "Select";
                }
                return SelectedLabels[0].Label;
            }
        }

        public List<string> LabelsGroups {
            get {
                var groups = from item in Labels
                    orderby item.groupName
                    group item by item.groupName
                    into itemGroup
                    select itemGroup.Key;

                return groups.ToList();
            }
        }
    }

    public class LexiconLabelModel {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string groupName { get; set; }
        
        //Bindable Props
        public Guid LexiconCategoryId { get; set; }
    }
}
