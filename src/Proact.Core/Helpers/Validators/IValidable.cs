using System.Collections.Generic;
using System.ComponentModel;

namespace Proact.Mobile.Core.Validators {
    public interface IValidable<T> : INotifyPropertyChanged {
        List<IValidationRule<T>> Validations { get; }

        List<string> Errors { get; set; }

        bool Validate();

        bool IsValid { get; set; }
    }
}