using System;
using System.ComponentModel;

namespace Proact.Mobile.Core.Validators {
	public class IsDoubleValueTrueRule<T> : IValidationRule<ValidablePair<T>> {
		public string ValidationMessage { get; set; }

		public bool Check( ValidablePair<T> value ) {
			return bool.Parse( value.Item1.Value.ToString() )
				&& bool.Parse( value.Item2.Value.ToString() );
		}
	}
}
