using System.IO;
using System.Threading.Tasks;
using Proact.Mobile.Core;
using Proact.Mobile.Droid;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(LocalFileProvider))]
namespace Proact.Mobile.Droid {
    public class LocalFileProvider : ILocalFileProvider {
        private readonly string _rootDir = Path
            .Combine( AndroidApp.Context.GetExternalFilesDir( Android.OS.Environment.DirectoryDocuments ).Path, "proact" );

        public async Task<string> SaveFileToDisk( Stream pdfStream, string fileName ) {
            if ( !Directory.Exists( _rootDir ) ) {
                Directory.CreateDirectory( _rootDir );
            }

            var filePath = Path.Combine( _rootDir, fileName );
            if ( !File.Exists( filePath ) ) {
                using ( var memoryStream = new MemoryStream() ) {
                    await pdfStream.CopyToAsync( memoryStream );
                    File.WriteAllBytes( filePath, memoryStream.ToArray() );
                }
            }
            return filePath;
        }
    }
}