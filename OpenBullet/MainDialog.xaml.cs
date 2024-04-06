using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet
{
	// Token: 0x0200004A RID: 74
	public partial class MainDialog : Window
	{
		// Token: 0x060001AE RID: 430 RVA: 0x00007DC1 File Offset: 0x00005FC1
		public MainDialog(Page content, string title)
		{
			this.InitializeComponent();
			base.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			base.Content = content;
			base.Title = title;
		}
	}
}
