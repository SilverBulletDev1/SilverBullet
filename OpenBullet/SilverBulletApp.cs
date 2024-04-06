using System;
using RuriLib.Interfaces;

namespace OpenBullet
{
	// Token: 0x0200002D RID: 45
	public struct SilverBulletApp : IApplication
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00003CB5 File Offset: 0x00001EB5
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00003CBD File Offset: 0x00001EBD
		public IRunnerManager RunnerManager { readonly get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00003CC6 File Offset: 0x00001EC6
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00003CCE File Offset: 0x00001ECE
		public IProxyManager ProxyManager { readonly get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00003CD7 File Offset: 0x00001ED7
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00003CDF File Offset: 0x00001EDF
		public IProxyChecker ProxyChecker { readonly get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00003CE8 File Offset: 0x00001EE8
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00003CF0 File Offset: 0x00001EF0
		public IWordlistManager WordlistManager { readonly get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00003CF9 File Offset: 0x00001EF9
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00003D01 File Offset: 0x00001F01
		public IConfigManager ConfigManager { readonly get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00003D0A File Offset: 0x00001F0A
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00003D12 File Offset: 0x00001F12
		public IHitsDB HitsDB { readonly get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00003D1B File Offset: 0x00001F1B
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00003D23 File Offset: 0x00001F23
		public IAlerter Alerter { readonly get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00003D2C File Offset: 0x00001F2C
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00003D34 File Offset: 0x00001F34
		public ILogger Logger { readonly get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00003D3D File Offset: 0x00001F3D
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00003D45 File Offset: 0x00001F45
		public ISettings Settings { readonly get; set; }
	}
}
