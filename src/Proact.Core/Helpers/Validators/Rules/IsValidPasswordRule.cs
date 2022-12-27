using System.Text.RegularExpressions;

namespace Proact.Mobile.Core.Validators {
    public class IsValidPasswordRule<T> : IValidationRule<T> {
        public string ValidationMessage { get; set; }
        public Regex RegexPassword { get; set; } = new Regex( "(?=.*[A-Z])(?=.*\\d)(?=.*[¡!@#$%*¿?\\-_.\\(\\)])[A-Za-z\\d¡!@#$%*¿?\\-\\(\\)&]{6,20}" );

        public bool Check( T value ) {
            return ( RegexPassword.IsMatch( $"{value}" ) );
        }
    }
}
