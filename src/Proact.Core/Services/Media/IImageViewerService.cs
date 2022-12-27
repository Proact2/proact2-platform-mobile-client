using System;
using System.Collections.Generic;

namespace Proact.Mobile.Core {
    public interface IImageViewerService {
        void ShowImage( string url );
        void ShowImagesGallery( List<string> urls );
    }
}
