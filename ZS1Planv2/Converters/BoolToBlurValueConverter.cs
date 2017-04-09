using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZS1Planv2.Converters
{
    public class BoolToBlurValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => (bool)value ? 2 : 0;

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => (int)value;
    }
}
