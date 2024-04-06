using System;
using System.Text.RegularExpressions;

namespace OpenBullet
{
	// Token: 0x02000018 RID: 24
	public class LatestRelease
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002B53 File Offset: 0x00000D53
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00002B5B File Offset: 0x00000D5B
		public string Name { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002B64 File Offset: 0x00000D64
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00002B6C File Offset: 0x00000D6C
		public Assets[] Assets { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002B75 File Offset: 0x00000D75
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00002B7D File Offset: 0x00000D7D
		public string Body { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002B86 File Offset: 0x00000D86
		public Version Ver
		{
			get
			{
				return Version.Parse(Regex.Match(this.Name, "\\d+(\\.\\d+)+").Value);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002BA2 File Offset: 0x00000DA2
		public bool Available
		{
			get
			{
				return this.Ver > Version.Parse("1.1.4");
			}
		}
	}
}
