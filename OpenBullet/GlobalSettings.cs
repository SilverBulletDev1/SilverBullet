using System;
using RuriLib.Interfaces;
using RuriLib.Models;
using RuriLib.ViewModels;

namespace OpenBullet
{
	// Token: 0x02000049 RID: 73
	public class GlobalSettings : ISettings
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00007D8E File Offset: 0x00005F8E
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x00007D96 File Offset: 0x00005F96
		public EnvironmentSettings Environment { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00007D9F File Offset: 0x00005F9F
		// (set) Token: 0x060001AA RID: 426 RVA: 0x00007DA7 File Offset: 0x00005FA7
		public RLSettingsViewModel RLSettings { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00007DB0 File Offset: 0x00005FB0
		// (set) Token: 0x060001AC RID: 428 RVA: 0x00007DB8 File Offset: 0x00005FB8
		public ProxyManagerSettings ProxyManagerSettings { get; set; }
	}
}
