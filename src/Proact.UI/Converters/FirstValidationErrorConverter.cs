using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MvvmCross.Converters;
using MvvmCross.Forms.Converters;
using Xamarin.Forms;

namespace Proact.UI.Converters {
    public class FirstValidationErrorConverter : MvxValueConverter {
        //public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {
        //	ICollection<string> errors = value as ICollection<string>;
        //	return errors != null && errors.Count > 0 ? errors.ElementAt( 0 ) : null;
        //}

        //public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
        //	return null;
        //}
        public override object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {
            ICollection<string> errors = value as ICollection<string>;
            return errors != null && errors.Count > 0 ? errors.ElementAt( 0 ) : string.Empty;
        }


    }

    public class NativeFirstValidationErrorConverter
    : MvxNativeValueConverter<FirstValidationErrorConverter> {
    }
}