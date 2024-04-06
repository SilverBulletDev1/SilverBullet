using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.Main.Settings.OpenBullet
{
	// Token: 0x020000ED RID: 237
	public partial class Sounds : Page
	{
		// Token: 0x06000619 RID: 1561 RVA: 0x0001FD66 File Offset: 0x0001DF66
		public Sounds()
		{
			this.InitializeComponent();
			base.DataContext = SB.SBSettings.Sounds;
		}
	}
}
