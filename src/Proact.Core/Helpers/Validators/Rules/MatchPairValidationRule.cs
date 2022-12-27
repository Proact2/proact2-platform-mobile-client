using System;
using System.ComponentModel;

namespace Proact.Mobile.Core.Validators {
	public class MatchPairValidationRule<T> : IValidationRule<ValidablePair<T>> {
		public string ValidationMessage { get; set; }

		public bool Check( ValidablePair<T> value ) {
			return value.Item1.Value.Equals( value.Item2.Value );
		}
	}
}
