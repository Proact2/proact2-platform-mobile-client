using System;
using System.Collections.Generic;
using Stormlion.PhotoBrowser;

namespace Proact.Mobile.Core {
    public class ImageViewerService : IImageViewerService {
      
        public void ShowImage( string url ) {
            ShowImagesGallery( new List<string>() { url } );
        }

        public void ShowImagesGallery( List<string> urls ) {
            var photoBrowser = new PhotoBrowser();
            photoBrowser.Photos = new List<Photo>();
            foreach( string url in urls ) {
                photoBrowser.Photos.Add( new Photo {
                    URL = url
                } );
            }
            photoBrowser.Show();
        }
    }
}
