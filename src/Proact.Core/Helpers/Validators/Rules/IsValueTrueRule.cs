namespace Proact.Mobile.Core.Validators {
    public class IsValueTrueRule<T> : IValidationRule<T> {
        public string ValidationMessage { get; set; }

        public bool Check( T value ) {
            return bool.Parse( $"{value}" );
        }
    }
}


