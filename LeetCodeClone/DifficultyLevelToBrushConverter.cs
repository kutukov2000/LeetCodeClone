using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LeetCodeClone
{
    public class DifficultyLevelToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int difficultyLevel = (int)value;

            if (difficultyLevel == 1)
                return Brushes.Green;
            else if (difficultyLevel == 2)
                return Brushes.Orange;
            else if (difficultyLevel == 3)
                return Brushes.Red;

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
