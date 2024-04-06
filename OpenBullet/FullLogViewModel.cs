using System;
using System.Collections.Generic;
using RuriLib.ViewModels;

namespace OpenBullet
{
	// Token: 0x02000034 RID: 52
	public class FullLogViewModel : ViewModelBase
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00004C5B File Offset: 0x00002E5B
		// (set) Token: 0x06000107 RID: 263 RVA: 0x00004C64 File Offset: 0x00002E64
		public string SearchString
		{
			get
			{
				return this.searchString;
			}
			set
			{
				if (string.Equals(this.searchString, value, StringComparison.Ordinal))
				{
					return;
				}
				this.searchString = value;
				this.OnPropertyChanged("SearchString");
				this.OnPropertyChanged("SearchProgress");
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00004CA0 File Offset: 0x00002EA0
		// (set) Token: 0x06000109 RID: 265 RVA: 0x00004CA8 File Offset: 0x00002EA8
		public List<int> Indexes
		{
			get
			{
				return this.indexes;
			}
			set
			{
				if (object.Equals(this.indexes, value))
				{
					return;
				}
				this.indexes = value;
				this.OnPropertyChanged("Indexes");
				this.OnPropertyChanged("TotalSearchMatches");
				this.OnPropertyChanged("CurrentSearchMatch");
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00004CEE File Offset: 0x00002EEE
		public int TotalSearchMatches
		{
			get
			{
				return this.Indexes.Count;
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00004CFB File Offset: 0x00002EFB
		public void UpdateTotalSearchMatches()
		{
			this.OnPropertyChanged("TotalSearchMatches");
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00004D08 File Offset: 0x00002F08
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00004D10 File Offset: 0x00002F10
		public int CurrentSearchMatch
		{
			get
			{
				return this.currentSearchMatch;
			}
			set
			{
				if (this.currentSearchMatch == value)
				{
					return;
				}
				this.currentSearchMatch = value;
				this.OnPropertyChanged("CurrentSearchMatch");
			}
		}

		// Token: 0x04000107 RID: 263
		private string searchString = "";

		// Token: 0x04000108 RID: 264
		private List<int> indexes = new List<int>();

		// Token: 0x04000109 RID: 265
		private int currentSearchMatch;
	}
}
