using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.Main.Configs.OtherOptions
{
	// Token: 0x02000110 RID: 272
	public partial class Requests : Page
	{
		// Token: 0x0600074E RID: 1870 RVA: 0x0002A448 File Offset: 0x00028648
		public Requests()
		{
			this.InitializeComponent();
			base.DataContext = SB.ConfigManager.CurrentConfig.Config.Settings;
		}
	}
}
