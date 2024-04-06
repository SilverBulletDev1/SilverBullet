using System;
using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
	// Token: 0x0200011B RID: 283
	public class SilverZoneViewModel : ViewModelBase
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x0002C567 File Offset: 0x0002A767
		// (set) Token: 0x060007B6 RID: 1974 RVA: 0x0002C570 File Offset: 0x0002A770
		public string SupportersBadge
		{
			get
			{
				return this.supportersBadge;
			}
			set
			{
				if (string.Equals(this.supportersBadge, value, StringComparison.Ordinal))
				{
					return;
				}
				this.supportersBadge = value;
				this.OnPropertyChanged("SupportersBadge");
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x0002C5A1 File Offset: 0x0002A7A1
		// (set) Token: 0x060007B8 RID: 1976 RVA: 0x0002C5AC File Offset: 0x0002A7AC
		public string VerifiedMarketBadge
		{
			get
			{
				return this.verifiedMarketBadge;
			}
			set
			{
				if (string.Equals(this.verifiedMarketBadge, value, StringComparison.Ordinal))
				{
					return;
				}
				this.verifiedMarketBadge = value;
				this.OnPropertyChanged("VerifiedMarketBadge");
			}
		}

		// Token: 0x04000658 RID: 1624
		private string supportersBadge;

		// Token: 0x04000659 RID: 1625
		private string verifiedMarketBadge;
	}
}
