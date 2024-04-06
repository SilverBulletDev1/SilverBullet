using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace OpenBullet
{
	// Token: 0x0200004D RID: 77
	public partial class SplashWindow : Window
	{
		// Token: 0x060001B9 RID: 441 RVA: 0x00007F54 File Offset: 0x00006154
		public SplashWindow()
		{
			this.InitializeComponent();
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00007F62 File Offset: 0x00006162
		private void agreeButton_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000304D File Offset: 0x0000124D
		private void WindowMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				base.DragMove();
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00007F6A File Offset: 0x0000616A
		private void quitImage_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Environment.Exit(0);
		}
	}
}
