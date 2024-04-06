using System;
using System.Windows;
using System.Windows.Media;

namespace OpenBullet
{
	// Token: 0x0200002E RID: 46
	public static class Utils
	{
		// Token: 0x060000E0 RID: 224 RVA: 0x00003D50 File Offset: 0x00001F50
		public static Color GetColor(string propertyName)
		{
			Color color;
			try
			{
				color = ((SolidColorBrush)Application.Current.Resources[propertyName]).Color;
			}
			catch
			{
				color = ((SolidColorBrush)Application.Current.Resources["ForegroundMain"]).Color;
			}
			return color;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00003DB0 File Offset: 0x00001FB0
		public static SolidColorBrush GetBrush(string propertyName)
		{
			SolidColorBrush solidColorBrush;
			try
			{
				solidColorBrush = (SolidColorBrush)Application.Current.Resources[propertyName];
			}
			catch
			{
				solidColorBrush = (SolidColorBrush)Application.Current.Resources["ForegroundMain"];
			}
			return solidColorBrush;
		}
	}
}
