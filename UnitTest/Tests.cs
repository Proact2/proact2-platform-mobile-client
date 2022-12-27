using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UnitTest {
    /*[TestFixture( Platform.Android )]
    [TestFixture( Platform.iOS )]
    public class Tests {
        IApp app;
        Platform platform;

        public Tests( Platform platform ) {
            this.platform = platform;

            var platformServicesFake = A.Fake<IPlatformServices>();
            Device.PlatformServices = platformServicesFake;
        }

        [SetUp]
        public void BeforeEachTest() {
            app = AppInitializer.StartApp( platform );
        }

        private async Task<ResponseResult<UserModel>>
            PerformSignIn( string email, string password ) {
            LocalDataService localDataService = new LocalDataService();

            AuthNetworkRequestService authNetworkRequestService
                    = new AuthNetworkRequestService(
                        new NetworkRequestService( localDataService ), localDataService );

            return await authNetworkRequestService.SignIn( email, password );
        }

        [Test]
        public async Task SignIn_CorrectData() {
            string email = "utente1@utente.com";
            string password = "12345678";

            var loginResult
                    = await PerformSignIn( email, password );

            if ( loginResult.Success ) {
                LocalDataService localDataService = new LocalDataService();
                UserModel savedLocalUserData = localDataService.GetUserData();

                if ( string.IsNullOrEmpty( savedLocalUserData.JwtToken ) ) {
                    Assert.Fail( "JwtToken Empty" );
                }

                if ( string.IsNullOrEmpty( savedLocalUserData.ApiKey ) ) {
                    Assert.Fail( "ApiKey Empty" );
                }

                Assert.Pass();
            }
            else {
                Assert.Fail();
            }
        }

        [Test]
        public async Task SignIn_WrongEmail() {
            string email = "utenteWRONG@WRONG.com";
            string password = "12345678";

            var loginResult
                    = await PerformSignIn( email, password );

            if ( !loginResult.Success 
                && loginResult.code == 400 ) {
                Assert.Pass();
            }
            else {
                Assert.Fail();
            }
        }

        [Test]
        public async Task SignIn_WrongPassword() {
            string email = "utente1@utente.com";
            string password = "123456789";

            var loginResult
                    = await PerformSignIn( email, password );

            if ( !loginResult.Success
                && loginResult.code == 400 ) {
                Assert.Pass();
            }
            else {
                Assert.Fail();
            }
        }

        [Test]
        public void WelcomeTextIsDisplayed() {
            AppResult[] results = app.WaitForElement(c => c.Marked("Non hai un account?"));
            app.Screenshot( "Welcome screen." );

            Assert.IsTrue( results.Any() );
        }
    }*/
}
