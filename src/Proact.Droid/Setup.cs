using Android.App;
using MvvmCross.Forms.Platforms.Android.Core;
using Proact.UI;

#if DEBUG
[assembly: Application(Debuggable = true)]
#else
[assembly: Application(Debuggable = false)]
#endif

namespace Proact.Mobile.Droid {
    public class Setup : MvxFormsAndroidSetup<Core.App, App> {
    }
}
