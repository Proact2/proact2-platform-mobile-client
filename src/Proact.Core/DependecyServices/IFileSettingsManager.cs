using System;
using System.Collections.Generic;
using System.Text;

namespace Proact.Mobile.Core {
    public interface IFileSettingsManager {
        void    SaveOnFile( string iFileName, string iContent );
        string  LoadFromFile( string iFileName );
        void    DeleteFile( string iFileName );
        string  GetLocalFolderPath();
    }
}
