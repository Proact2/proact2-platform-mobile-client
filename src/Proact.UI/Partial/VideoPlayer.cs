using System;
using Proact.Mobile.Core.Models;
using Xamarin.Forms;

namespace Proact.UI {
    public class VideoPlayer : View{

        public static readonly BindableProperty MediaFileDecryptProperty =
            BindableProperty.Create(
                nameof( MediaFileDecrypt ),
                typeof( MediaFileDecryptModel ),
                typeof( VideoPlayer )
                );

        public MediaFileDecryptModel MediaFileDecrypt {
            get => ( MediaFileDecryptModel )GetValue( MediaFileDecryptProperty );
            set => SetValue( MediaFileDecryptProperty, value );
        }

    }
}
