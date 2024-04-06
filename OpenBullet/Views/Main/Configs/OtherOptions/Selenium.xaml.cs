using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.Main.Configs.OtherOptions
{
	// Token: 0x02000111 RID: 273
	public partial class Selenium : Page
	{
		// Token: 0x06000751 RID: 1873 RVA: 0x0002A4A9 File Offset: 0x000286A9
		public Selenium()
		{
			this.InitializeComponent();
			base.DataContext = SB.ConfigManager.CurrentConfig.Config.Settings;
		}
	}
}
