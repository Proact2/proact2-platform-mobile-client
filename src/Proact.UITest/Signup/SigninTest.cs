using System;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Proact.UITest {
    public class SigninTest : BaseTest {

        public SigninTest( Platform platform ) : base( platform ) {
        }

        [Test]
        public void SignIn_OpenPage() {
            app.Tap( x => x.Button( "NoResourceEntry-6" ) );
            AppResult[] results = app.WaitForElement( c => c.Marked( "Login" ) );
            Assert.IsTrue( results.Any() );
        }

        [Test]
        public void SignIn_Success() {

            app.Tap( x => x.Button( "NoResourceEntry-6" ) );
            app.EnterText( x => x.Id( "NoResourceEntry-22" ), "utente1@utente.com" );
            app.EnterText( x => x.Id( "NoResourceEntry-26" ), "12345678" );
            app.DismissKeyboard();
            app.Tap( "NoResourceEntry-31" );

            AppResult[] results = app.WaitForElement( x => x.Marked( "Main page" ) );
            Assert.IsTrue( results.Any() );
        }

        [Test]
        public void SignIn_InvalidEmail() {
            app.Tap( x => x.Button( "NoResourceEntry-6" ) );
            app.EnterText( x => x.Id( "NoResourceEntry-22" ), "utente1utente.com" );
            app.Tap( "NoResourceEntry-31" );

            AppResult[] results = app.WaitForElement( x => x.Marked( "L'indirizzo email non è valido" ) );
            Assert.IsTrue( results.Any() );
        }


    }
}
