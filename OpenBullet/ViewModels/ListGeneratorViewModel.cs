using System;
using System.Linq;
using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
	// Token: 0x02000125 RID: 293
	internal class ListGeneratorViewModel : ViewModelBase
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x0002D49D File Offset: 0x0002B69D
		// (set) Token: 0x06000815 RID: 2069 RVA: 0x0002D4A8 File Offset: 0x0002B6A8
		public bool OnlyLuhn
		{
			get
			{
				return this.onlyLuhn;
			}
			set
			{
				if (this.onlyLuhn == value)
				{
					return;
				}
				this.onlyLuhn = value;
				this.OnPropertyChanged("OnlyLuhn");
				this.OnPropertyChanged("OutputLines");
				this.OnPropertyChanged("OutputSize");
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x0002D4EB File Offset: 0x0002B6EB
		// (set) Token: 0x06000817 RID: 2071 RVA: 0x0002D4F4 File Offset: 0x0002B6F4
		public bool AutoImport
		{
			get
			{
				return this.autoImport;
			}
			set
			{
				if (this.autoImport == value)
				{
					return;
				}
				this.autoImport = value;
				this.OnPropertyChanged("AutoImport");
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x0002D521 File Offset: 0x0002B721
		// (set) Token: 0x06000819 RID: 2073 RVA: 0x0002D52C File Offset: 0x0002B72C
		public string Mask
		{
			get
			{
				return this.mask;
			}
			set
			{
				if (string.Equals(this.mask, value, StringComparison.Ordinal))
				{
					return;
				}
				this.mask = value;
				this.OnPropertyChanged("Mask");
				this.OnPropertyChanged("OutputLines");
				this.OnPropertyChanged("OutputSize");
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x0002D573 File Offset: 0x0002B773
		// (set) Token: 0x0600081B RID: 2075 RVA: 0x0002D57C File Offset: 0x0002B77C
		public string AllowedCharacters
		{
			get
			{
				return this.allowedCharacters;
			}
			set
			{
				if (string.Equals(this.allowedCharacters, value, StringComparison.Ordinal))
				{
					return;
				}
				this.allowedCharacters = value;
				this.OnPropertyChanged("AllowedCharacters");
				this.OnPropertyChanged("OutputLines");
				this.OnPropertyChanged("OutputSize");
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x0002D5C4 File Offset: 0x0002B7C4
		public int OutputLines
		{
			get
			{
				string text = this.Mask.Split(new char[] { ':' })[0].Replace("*", "");
				int varCount = (from c in this.Mask.ToCharArray()
					where c == '*'
					select c).Count<char>();
				int lines = (int)Math.Pow((double)this.AllowedCharacters.Length, (double)varCount);
				bool flag;
				if (!text.ToCharArray().Any((char c) => !char.IsDigit(c)))
				{
					flag = !this.AllowedCharacters.ToCharArray().Any((char c) => !char.IsDigit(c));
				}
				else
				{
					flag = false;
				}
				if (!flag || !this.OnlyLuhn)
				{
					return lines;
				}
				return lines / 10;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x0002D6B2 File Offset: 0x0002B8B2
		public string OutputSize
		{
			get
			{
				return ListGeneratorViewModel.SizeSuffix((long)(2 * this.Mask.Length * this.OutputLines), 0);
			}
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0002D6D0 File Offset: 0x0002B8D0
		private static string SizeSuffix(long value, int decimalPlaces = 1)
		{
			if (decimalPlaces < 0)
			{
				throw new ArgumentOutOfRangeException("decimalPlaces");
			}
			if (value < 0L)
			{
				return "-" + ListGeneratorViewModel.SizeSuffix(-value, 1);
			}
			if (value == 0L)
			{
				return string.Format("{0:n" + decimalPlaces.ToString() + "} bytes", 0);
			}
			int mag = (int)Math.Log((double)value, 1024.0);
			decimal adjustedSize = value / (1L << mag * 10);
			if (Math.Round(adjustedSize, decimalPlaces) >= 1000m)
			{
				mag++;
				adjustedSize /= 1024m;
			}
			return string.Format("{0:n" + decimalPlaces.ToString() + "} {1}", adjustedSize, ListGeneratorViewModel.SizeSuffixes[mag]);
		}

		// Token: 0x04000679 RID: 1657
		private bool onlyLuhn;

		// Token: 0x0400067A RID: 1658
		private bool autoImport;

		// Token: 0x0400067B RID: 1659
		private string mask = "657438923467423847****:**";

		// Token: 0x0400067C RID: 1660
		private string allowedCharacters = "0123456789";

		// Token: 0x0400067D RID: 1661
		private static readonly string[] SizeSuffixes = new string[] { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
	}
}
