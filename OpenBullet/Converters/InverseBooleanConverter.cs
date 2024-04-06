using System;
using System.Globalization;
using System.Windows.Data;

namespace OpenBullet.Converters
{
	// Token: 0x0200015B RID: 347
	[ValueConversion(typeof(bool), typeof(bool))]
	public class InverseBooleanConverter : IValueConverter
	{
		// Token: 0x060009DC RID: 2524 RVA: 0x000328EE File Offset: 0x00030AEE
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType != typeof(bool))
			{
				throw new InvalidOperationException("The target must be a boolean");
			}
			return !(bool)value;
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0000273D File Offset: 0x0000093D
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
