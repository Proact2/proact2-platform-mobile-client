    using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MvvmHelpers;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class DocumentViewerViewModel : BaseViewModel<DocumentViewerModel> {

        private UrlWebViewSource _documentSource;
        public UrlWebViewSource DocumentSource {
            get => _documentSource;
            set => SetProperty( ref _documentSource, value );
        }

        private DocumentViewerModel _model;

        public override void Prepare( DocumentViewerModel documentModel ) {
            _model = documentModel;
        }

        public async override Task Initialize() {
            await base.Initialize();
            PageTitle = _model.Title;
        }

        public override void ViewAppeared() {
            base.ViewAppeared();
            OpenPdfFile();
        }

        private void OpenPdfFile() {
            if ( Device.RuntimePlatform == Device.Android ) {

                Device.StartTimer( new TimeSpan( 0, 0, 1 ), () => {
                    OpenPdfFileOnAndroid();
                    return false;
                } );
            }
            else {
                OpenPdfFileOnIos();
            }
        }

        public void OpenPdfFileOnAndroid() {
            var localPath = string.Empty;
            var dependency = DependencyService.Get<ILocalFileProvider>();

            if ( dependency == null ) {
                return;
            }

            using ( var httpClient = new HttpClient() ) {
                try {
                    var pdfStream = Task
                   .Run( () => httpClient.GetStreamAsync( _model.Url ) )
                   .Result;

                    string filename = string.Empty;

                    Uri uri = new Uri( _model.Url );
                    filename = System.IO.Path.GetFileName( uri.LocalPath );

                    localPath = Task
                    .Run( () => dependency.SaveFileToDisk( pdfStream, filename ) )
                    .Result;
                }
                catch ( Exception ) { };
            }

            if ( string.IsNullOrWhiteSpace( localPath ) ) {
                return;
            }
            var source = new UrlWebViewSource() {
                Url = $"file:///android_asset/pdfjs/web/viewer.html?file={"file:" + localPath }"
            };
            DocumentSource = source;
        }

        private void OpenPdfFileOnIos() {
            var source = new UrlWebViewSource() {
                Url = _model.Url
            };
            DocumentSource = source;
        }
    }
}
