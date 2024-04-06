using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.Main.Configs.OtherOptions
{
	// Token: 0x02000113 RID: 275
	public partial class General : Page
	{
		// Token: 0x0600075D RID: 1885 RVA: 0x0002A84B File Offset: 0x00028A4B
		public General()
		{
			this.InitializeComponent();
			base.DataContext = SB.ConfigManager.CurrentConfig.Config.Settings;
		}
	}
}
