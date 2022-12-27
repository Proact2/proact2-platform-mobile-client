using System;
namespace Proact.Mobile.Core.Validators {
    public interface IValidationRule<T> {
        string ValidationMessage { get; set; }
        bool Check( T value );
    }
}
