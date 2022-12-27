using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proact.UI {
    [XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class WallMessageWithImageViewCell : Grid {
        public WallMessageWithImageViewCell() {
            InitializeComponent();
        }
    }
}
