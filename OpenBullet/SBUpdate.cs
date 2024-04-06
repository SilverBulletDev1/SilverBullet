using System;
using System.Collections.Generic;

namespace OpenBullet
{
	// Token: 0x02000015 RID: 21
	public class SBUpdate
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002AB8 File Offset: 0x00000CB8
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002AC0 File Offset: 0x00000CC0
		public string Version { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002AC9 File Offset: 0x00000CC9
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00002AD1 File Offset: 0x00000CD1
		public bool Available { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002ADA File Offset: 0x00000CDA
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00002AE2 File Offset: 0x00000CE2
		public string Message { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002AEB File Offset: 0x00000CEB
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00002AF3 File Offset: 0x00000CF3
		public string DownloadUrl { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002AFC File Offset: 0x00000CFC
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00002B04 File Offset: 0x00000D04
		public List<ReleaseNotes> ReleaseNotes { get; set; } = new List<ReleaseNotes>();

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002B0D File Offset: 0x00000D0D
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00002B15 File Offset: 0x00000D15
		public Donate[] Donate { get; set; }
	}
}
