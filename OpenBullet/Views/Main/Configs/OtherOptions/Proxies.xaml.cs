using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.Main.Configs.OtherOptions
{
	// Token: 0x02000115 RID: 277
	public partial class Proxies : Page
	{
		// Token: 0x06000767 RID: 1895 RVA: 0x0002AA0C File Offset: 0x00028C0C
		public Proxies()
		{
			this.InitializeComponent();
			base.DataContext = SB.ConfigManager.CurrentConfig.Config.Settings;
		}
	}
}
