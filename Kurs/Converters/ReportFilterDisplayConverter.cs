using Kurs.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.Converters
{
    public class ReportFilterDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is ReportFilter filter ? filter switch
            {
                ReportFilter.All => "Все",
                ReportFilter.OnlyMine => "Только мои",
                ReportFilter.ToReview => "На проверку",
                ReportFilter.NotCompleted => "Незавершённые",
                _ => filter.ToString()
            } : value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
