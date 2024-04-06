using System;
using System.Text.RegularExpressions;

namespace OpenBullet.Models
{
	// Token: 0x02000155 RID: 341
	public class SBRelease
	{
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x00032606 File Offset: 0x00030806
		// (set) Token: 0x060009BD RID: 2493 RVA: 0x0003260E File Offset: 0x0003080E
		public string Name { get; set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x00032617 File Offset: 0x00030817
		// (set) Token: 0x060009BF RID: 2495 RVA: 0x0003261F File Offset: 0x0003081F
		public Assets[] Assets { get; set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x00032628 File Offset: 0x00030828
		public Version Ver
		{
			get
			{
				return Version.Parse(Regex.Match(this.Name, "\\d+(\\.\\d+)+").Value);
			}
		}
	}
}
