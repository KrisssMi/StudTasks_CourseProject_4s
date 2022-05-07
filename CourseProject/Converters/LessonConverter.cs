using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CourseProject.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    class LessonConverter : IValueConverter
    {
        public string[] Lessons = { "8:00-9:20", "9:35-11:55", "11:10-13:30", "13:45-15:05" };
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return Lessons[((int)value) - 1];
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            for (int i = 0; i < Lessons.Count(); i++)
            {
                if ((string)value == Lessons[i])
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
