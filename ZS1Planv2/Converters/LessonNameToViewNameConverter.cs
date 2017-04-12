using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using ZS1Planv2.Model.Application;

namespace ZS1Planv2.Converters
{
    public class LessonNameToViewNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is string))
                return value;
            return $"{Text.GetText(Text.TextId.TimetablePage_Title_Text_1)} - {value}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
