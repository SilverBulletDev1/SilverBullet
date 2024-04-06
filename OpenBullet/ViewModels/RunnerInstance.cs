using System;
using OpenBullet.Views.Main.Runner;
using RuriLib.Runner;

namespace OpenBullet.ViewModels
{
	// Token: 0x0200013D RID: 317
	public class RunnerInstance
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x000304C9 File Offset: 0x0002E6C9
		// (set) Token: 0x06000904 RID: 2308 RVA: 0x000304D1 File Offset: 0x0002E6D1
		public Runner View { get; private set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x000304DA File Offset: 0x0002E6DA
		// (set) Token: 0x06000906 RID: 2310 RVA: 0x000304E2 File Offset: 0x0002E6E2
		public RunnerViewModel ViewModel { get; private set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000907 RID: 2311 RVA: 0x000304EB File Offset: 0x0002E6EB
		// (set) Token: 0x06000908 RID: 2312 RVA: 0x000304F3 File Offset: 0x0002E6F3
		public int Id { get; set; }

		// Token: 0x06000909 RID: 2313 RVA: 0x000304FC File Offset: 0x0002E6FC
		public RunnerInstance(int id)
		{
			this.Id = id;
			this.ViewModel = new RunnerViewModel(SB.Settings.Environment, SB.Settings.RLSettings, null);
			this.View = new Runner(this.ViewModel);
		}
	}
}
