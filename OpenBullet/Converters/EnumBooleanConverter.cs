using System;
using System.Globalization;
using System.Windows.Data;

namespace OpenBullet.Converters
{
	// Token: 0x0200015A RID: 346
	public class EnumBooleanConverter : IValueConverter
	{
		// Token: 0x060009D9 RID: 2521 RVA: 0x000328A8 File Offset: 0x00030AA8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value != null) ? new bool?(value.Equals(parameter)) : null;
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x000328D4 File Offset: 0x00030AD4
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || !value.Equals(true))
			{
				return Binding.DoNothing;
			}
			return parameter;
		}
	}
}
