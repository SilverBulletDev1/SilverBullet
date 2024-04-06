using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.Main.Settings.OpenBullet
{
	// Token: 0x020000EA RID: 234
	public partial class General : Page
	{
		// Token: 0x060005FC RID: 1532 RVA: 0x0001F1F8 File Offset: 0x0001D3F8
		public General()
		{
			this.InitializeComponent();
			base.DataContext = SB.SBSettings.General;
		}
	}
}
