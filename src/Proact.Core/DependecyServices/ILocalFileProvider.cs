using System;
using System.IO;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public interface ILocalFileProvider {
        Task<string> SaveFileToDisk( Stream stream, string fileName );
    }
}
