using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OpenBullet.Converters
{
	// Token: 0x02000159 RID: 345
	public class BooleanBrushConverter : IValueConverter
	{
		// Token: 0x060009D6 RID: 2518 RVA: 0x00032858 File Offset: 0x00030A58
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return new SolidColorBrush(((value != null) ? new bool?(value.Equals(true)) : null).Value ? Colors.Tomato : Colors.Yellow);
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0003289F File Offset: 0x00030A9F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return false;
		}
	}
}
