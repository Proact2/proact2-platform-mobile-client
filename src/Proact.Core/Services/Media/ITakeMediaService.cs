using System;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;

namespace Proact.Mobile.Core {
    public interface ITakeMediaService {

        Task<MediaFile> TakePhoto();
        Task<MediaFile> PickPhoto();

        Task<MediaFile> TakeVideoAsync();

        Task<string> GetBase64( MediaFile iMediaFile );
    }
}
