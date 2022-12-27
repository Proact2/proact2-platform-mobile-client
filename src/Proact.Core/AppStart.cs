using System.Threading.Tasks;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Proact.Mobile.Core.ViewModels;

namespace Proact.Mobile.Core {
    public class AppStart : MvxAppStart {

        private readonly ILocalDataService _localDataService;

        public AppStart( 
            IMvxApplication application, 
            IMvxNavigationService navigationService,
            ILocalDataService localDataService ) : base( application, navigationService ) {

            _localDataService = localDataService;
        }

        protected override Task NavigateToFirstViewModel( object hint = null ) {
            return NavigationService.Navigate<LandingViewModel, bool>( false );
        }
    }
}
