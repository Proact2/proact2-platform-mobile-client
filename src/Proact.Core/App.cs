using Microsoft.Identity.Client;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace Proact.Mobile.Core {
    public class App : MvxApplication {

      public static object UIParent { get; set; } = null;

        public override void Initialize() {
            CreatableTypes()
                .EndingWith( "Service" )
                .AsInterfaces()
                .RegisterAsDynamic();

            CreatableTypes()
                .EndingWith( "SingleFeed" )
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterCustomAppStart<AppStart>();
        }

    }
}