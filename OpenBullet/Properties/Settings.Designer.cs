using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace OpenBullet.Properties
{
	// Token: 0x0200005B RID: 91
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.8.1.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000AA72 File Offset: 0x00008C72
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x040001DC RID: 476
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
