using System;

namespace OpenBullet.ViewModels
{
	// Token: 0x0200011C RID: 284
	public class SBSettingsViewModel
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x0002C5E5 File Offset: 0x0002A7E5
		// (set) Token: 0x060007BB RID: 1979 RVA: 0x0002C5ED File Offset: 0x0002A7ED
		public SBSettingsGeneral General { get; set; } = new SBSettingsGeneral();

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x0002C5F6 File Offset: 0x0002A7F6
		// (set) Token: 0x060007BD RID: 1981 RVA: 0x0002C5FE File Offset: 0x0002A7FE
		public OBSettingsSounds Sounds { get; set; } = new OBSettingsSounds();

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x0002C607 File Offset: 0x0002A807
		// (set) Token: 0x060007BF RID: 1983 RVA: 0x0002C60F File Offset: 0x0002A80F
		public OBSettingsSources Sources { get; set; } = new OBSettingsSources();

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x0002C618 File Offset: 0x0002A818
		// (set) Token: 0x060007C1 RID: 1985 RVA: 0x0002C620 File Offset: 0x0002A820
		public OBSettingsThemes Themes { get; set; } = new OBSettingsThemes();

		// Token: 0x060007C2 RID: 1986 RVA: 0x0002C629 File Offset: 0x0002A829
		public void Reset()
		{
			this.General.Reset();
			this.Sounds.Reset();
			this.Sources.Reset();
			this.Themes.Reset();
		}
	}
}
