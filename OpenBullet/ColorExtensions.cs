using System;
using System.Windows.Media;

namespace OpenBullet
{
	// Token: 0x02000023 RID: 35
	public static class ColorExtensions
	{
		// Token: 0x0600007A RID: 122 RVA: 0x00002EB8 File Offset: 0x000010B8
		public static uint ColorToUInt(this Color color)
		{
			return (uint)(((int)color.A << 24) | ((int)color.R << 16) | ((int)color.G << 8) | (int)color.B);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002EE4 File Offset: 0x000010E4
		public static string ConvertToString(this Color c)
		{
			return string.Concat(new string[]
			{
				c.R.ToString(),
				",",
				c.G.ToString(),
				",",
				c.B.ToString()
			});
		}
	}
}
