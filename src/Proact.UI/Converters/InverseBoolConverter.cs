using System;
using System.Globalization;
using MvvmCross.Converters;
using MvvmCross.Forms.Converters;
using Xamarin.Forms;

namespace Proact.UI.Converters {
    public class InverseBoolConverter : MvxValueConverter<bool, bool> {
        //public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {
        //    if( !( value is bool ) ) {
        //        throw new InvalidOperationException( "The target must be a boolean" );
        //    }

        //    return !( bool )value;
        //}

        //public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
        //    return null;
        //}
        protected override bool Convert( bool value, Type targetType, object parameter, CultureInfo culture ) {
            return !value;
        }
    }

    public class NativeInverseBoolConverter
    : MvxNativeValueConverter<InverseBoolConverter> {
    }
}