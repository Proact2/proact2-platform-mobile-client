using System;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Proact.UITest {
    public class SignupTest : BaseTest {
        public SignupTest( Platform platform ) : base( platform ) {
        }

        [Test]
        public void OpenSignupPage() {
            app.Tap( "Registrati ora" );
            AppResult[] results = app.WaitForElement( c => c.Marked( "Registrazione" ) );
            Assert.IsTrue( results.Any() );
        }
    }
}
