using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace EngApp
{
    public class ToRandomColorConverter : IValueConverter
    {
        static int i = 0;
        SolidColorBrush[] brushes = new SolidColorBrush[]
        {
            new SolidColorBrush(Colors.Blue),
            new SolidColorBrush(Colors.Magenta),
            new SolidColorBrush(Colors.Purple),
            new SolidColorBrush(Colors.Red),
            new SolidColorBrush(Colors.Brown),
        };

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return brushes[(int)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
