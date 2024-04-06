using System;
using System.Collections.Generic;
using System.Reflection;
using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
	// Token: 0x0200012B RID: 299
	public class OBSettingsThemes : ViewModelBase
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x0002E17F File Offset: 0x0002C37F
		// (set) Token: 0x06000871 RID: 2161 RVA: 0x0002E188 File Offset: 0x0002C388
		public string BackgroundMain
		{
			get
			{
				return this.backgroundMain;
			}
			set
			{
				if (string.Equals(this.backgroundMain, value, StringComparison.Ordinal))
				{
					return;
				}
				this.backgroundMain = value;
				this.OnPropertyChanged("BackgroundMain");
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x0002E1B9 File Offset: 0x0002C3B9
		// (set) Token: 0x06000873 RID: 2163 RVA: 0x0002E1C4 File Offset: 0x0002C3C4
		public string BackgroundSecondary
		{
			get
			{
				return this.backgroundSecondary;
			}
			set
			{
				if (string.Equals(this.backgroundSecondary, value, StringComparison.Ordinal))
				{
					return;
				}
				this.backgroundSecondary = value;
				this.OnPropertyChanged("BackgroundSecondary");
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x0002E1F5 File Offset: 0x0002C3F5
		// (set) Token: 0x06000875 RID: 2165 RVA: 0x0002E200 File Offset: 0x0002C400
		public string ForegroundMain
		{
			get
			{
				return this.foregroundMain;
			}
			set
			{
				if (string.Equals(this.foregroundMain, value, StringComparison.Ordinal))
				{
					return;
				}
				this.foregroundMain = value;
				this.OnPropertyChanged("ForegroundMain");
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x0002E231 File Offset: 0x0002C431
		// (set) Token: 0x06000877 RID: 2167 RVA: 0x0002E23C File Offset: 0x0002C43C
		public string ForegroundGood
		{
			get
			{
				return this.foregroundGood;
			}
			set
			{
				if (string.Equals(this.foregroundGood, value, StringComparison.Ordinal))
				{
					return;
				}
				this.foregroundGood = value;
				this.OnPropertyChanged("ForegroundGood");
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x0002E26D File Offset: 0x0002C46D
		// (set) Token: 0x06000879 RID: 2169 RVA: 0x0002E278 File Offset: 0x0002C478
		public string ForegroundBad
		{
			get
			{
				return this.foregroundBad;
			}
			set
			{
				if (string.Equals(this.foregroundBad, value, StringComparison.Ordinal))
				{
					return;
				}
				this.foregroundBad = value;
				this.OnPropertyChanged("ForegroundBad");
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x0002E2A9 File Offset: 0x0002C4A9
		// (set) Token: 0x0600087B RID: 2171 RVA: 0x0002E2B4 File Offset: 0x0002C4B4
		public string ForegroundCustom
		{
			get
			{
				return this.foregroundFree;
			}
			set
			{
				if (string.Equals(this.foregroundFree, value, StringComparison.Ordinal))
				{
					return;
				}
				this.foregroundFree = value;
				this.OnPropertyChanged("ForegroundCustom");
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x0002E2E5 File Offset: 0x0002C4E5
		// (set) Token: 0x0600087D RID: 2173 RVA: 0x0002E2F0 File Offset: 0x0002C4F0
		public string ForegroundRetry
		{
			get
			{
				return this.foregroundRetry;
			}
			set
			{
				if (string.Equals(this.foregroundRetry, value, StringComparison.Ordinal))
				{
					return;
				}
				this.foregroundRetry = value;
				this.OnPropertyChanged("ForegroundRetry");
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x0002E321 File Offset: 0x0002C521
		// (set) Token: 0x0600087F RID: 2175 RVA: 0x0002E32C File Offset: 0x0002C52C
		public string ForegroundToCheck
		{
			get
			{
				return this.foregroundToCheck;
			}
			set
			{
				if (string.Equals(this.foregroundToCheck, value, StringComparison.Ordinal))
				{
					return;
				}
				this.foregroundToCheck = value;
				this.OnPropertyChanged("ForegroundToCheck");
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x0002E35D File Offset: 0x0002C55D
		// (set) Token: 0x06000881 RID: 2177 RVA: 0x0002E368 File Offset: 0x0002C568
		public string ForegroundOcrRate
		{
			get
			{
				return this.foregroundOcrRate;
			}
			set
			{
				if (string.Equals(this.foregroundOcrRate, value, StringComparison.Ordinal))
				{
					return;
				}
				this.foregroundOcrRate = value;
				this.OnPropertyChanged("ForegroundOcrRate");
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x0002E399 File Offset: 0x0002C599
		// (set) Token: 0x06000883 RID: 2179 RVA: 0x0002E3A4 File Offset: 0x0002C5A4
		public string ForegroundMenuSelected
		{
			get
			{
				return this.foregroundMenuSelected;
			}
			set
			{
				if (string.Equals(this.foregroundMenuSelected, value, StringComparison.Ordinal))
				{
					return;
				}
				this.foregroundMenuSelected = value;
				this.OnPropertyChanged("ForegroundMenuSelected");
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x0002E3D5 File Offset: 0x0002C5D5
		// (set) Token: 0x06000885 RID: 2181 RVA: 0x0002E3E0 File Offset: 0x0002C5E0
		public bool UseImage
		{
			get
			{
				return this.useImage;
			}
			set
			{
				if (this.useImage == value)
				{
					return;
				}
				this.useImage = value;
				this.OnPropertyChanged("UseImage");
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x0002E40D File Offset: 0x0002C60D
		// (set) Token: 0x06000887 RID: 2183 RVA: 0x0002E418 File Offset: 0x0002C618
		public string BackgroundImage
		{
			get
			{
				return this.backgroundImage;
			}
			set
			{
				if (string.Equals(this.backgroundImage, value, StringComparison.Ordinal))
				{
					return;
				}
				this.backgroundImage = value;
				this.OnPropertyChanged("BackgroundImage");
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x0002E449 File Offset: 0x0002C649
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x0002E454 File Offset: 0x0002C654
		public int BackgroundImageOpacity
		{
			get
			{
				return this.backgroundImageOpacity;
			}
			set
			{
				if (this.backgroundImageOpacity == value)
				{
					return;
				}
				this.backgroundImageOpacity = value;
				this.OnPropertyChanged("BackgroundImageOpacity");
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x0002E481 File Offset: 0x0002C681
		// (set) Token: 0x0600088B RID: 2187 RVA: 0x0002E48C File Offset: 0x0002C68C
		public string BackgroundLogo
		{
			get
			{
				return this.backgroundLogo;
			}
			set
			{
				if (string.Equals(this.backgroundLogo, value, StringComparison.Ordinal))
				{
					return;
				}
				this.backgroundLogo = value;
				this.OnPropertyChanged("BackgroundLogo");
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x0002E4BD File Offset: 0x0002C6BD
		// (set) Token: 0x0600088D RID: 2189 RVA: 0x0002E4C8 File Offset: 0x0002C6C8
		public bool EnableSnow
		{
			get
			{
				return this.enableSnow;
			}
			set
			{
				if (this.enableSnow == value)
				{
					return;
				}
				this.enableSnow = value;
				this.OnPropertyChanged("EnableSnow");
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x0002E4F5 File Offset: 0x0002C6F5
		// (set) Token: 0x0600088F RID: 2191 RVA: 0x0002E500 File Offset: 0x0002C700
		public int SnowAmount
		{
			get
			{
				return this.snowAmount;
			}
			set
			{
				if (this.snowAmount == value)
				{
					return;
				}
				this.snowAmount = value;
				this.OnPropertyChanged("SnowAmount");
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x0002E52D File Offset: 0x0002C72D
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x0002E538 File Offset: 0x0002C738
		public bool AllowTransparency
		{
			get
			{
				return this.allowTransparency;
			}
			set
			{
				if (this.allowTransparency == value)
				{
					return;
				}
				this.allowTransparency = value;
				this.OnPropertyChanged("AllowTransparency");
			}
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0002E568 File Offset: 0x0002C768
		public void Reset()
		{
			OBSettingsThemes def = new OBSettingsThemes();
			foreach (PropertyInfo prop in ((IEnumerable<PropertyInfo>)new List<PropertyInfo>(typeof(OBSettingsThemes).GetProperties())))
			{
				prop.SetValue(this, prop.GetValue(def, null));
			}
		}

		// Token: 0x040006A3 RID: 1699
		private string backgroundMain = "#222";

		// Token: 0x040006A4 RID: 1700
		private string backgroundSecondary = "#111";

		// Token: 0x040006A5 RID: 1701
		private string foregroundMain = "#dcdcdc";

		// Token: 0x040006A6 RID: 1702
		private string foregroundGood = "#adff2f";

		// Token: 0x040006A7 RID: 1703
		private string foregroundBad = "#ff6347";

		// Token: 0x040006A8 RID: 1704
		private string foregroundFree = "#ff8c00";

		// Token: 0x040006A9 RID: 1705
		private string foregroundRetry = "#ffff00";

		// Token: 0x040006AA RID: 1706
		private string foregroundToCheck = "#7fffd4";

		// Token: 0x040006AB RID: 1707
		private string foregroundOcrRate = "#ff77bafd";

		// Token: 0x040006AC RID: 1708
		private string foregroundMenuSelected = "#1e90ff";

		// Token: 0x040006AD RID: 1709
		private bool useImage;

		// Token: 0x040006AE RID: 1710
		private string backgroundImage = "";

		// Token: 0x040006AF RID: 1711
		private int backgroundImageOpacity = 100;

		// Token: 0x040006B0 RID: 1712
		private string backgroundLogo = "";

		// Token: 0x040006B1 RID: 1713
		private bool enableSnow;

		// Token: 0x040006B2 RID: 1714
		private int snowAmount = 100;

		// Token: 0x040006B3 RID: 1715
		private bool allowTransparency;
	}
}
