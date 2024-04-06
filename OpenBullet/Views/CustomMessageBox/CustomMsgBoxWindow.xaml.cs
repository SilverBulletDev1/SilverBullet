using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using MahApps.Metro.IconPacks;

namespace OpenBullet.Views.CustomMessageBox
{
	// Token: 0x0200011A RID: 282
	public partial class CustomMsgBoxWindow : Window, IDisposable
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x0002C2E8 File Offset: 0x0002A4E8
		// (set) Token: 0x060007A8 RID: 1960 RVA: 0x0002C2F0 File Offset: 0x0002A4F0
		public MessageBoxResult Result { get; set; }

		// Token: 0x060007A9 RID: 1961 RVA: 0x0002C2F9 File Offset: 0x0002A4F9
		public CustomMsgBoxWindow()
		{
			this.InitializeComponent();
			this.Result = MessageBoxResult.Cancel;
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0002C30E File Offset: 0x0002A50E
		private void BtnOk_Click(object sender, RoutedEventArgs e)
		{
			this.Result = MessageBoxResult.OK;
			base.Close();
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0002C31D File Offset: 0x0002A51D
		private void BtnCancel_Click(object sender, RoutedEventArgs e)
		{
			this.Result = MessageBoxResult.Cancel;
			base.Close();
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00007F62 File Offset: 0x00006162
		public void Dispose()
		{
			base.Close();
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0002C32C File Offset: 0x0002A52C
		private void BtnCopyMessage_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Clipboard.SetText(this.Message.Text);
			}
			catch
			{
			}
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x00003B20 File Offset: 0x00001D20
		private void titleBar_MouseDown(object sender, MouseButtonEventArgs e)
		{
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0000305D File Offset: 0x0000125D
		private void dragPanel_MouseDown(object sender, MouseButtonEventArgs e)
		{
			base.DragMove();
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0002C360 File Offset: 0x0002A560
		private void btnQuit_Click(object sender, RoutedEventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0002C368 File Offset: 0x0002A568
		private void btnQuit_MouseEnter(object sender, MouseEventArgs e)
		{
			this.btnQuit.BorderBrush = (this.btnQuit.Background = Brushes.DarkRed);
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0002C394 File Offset: 0x0002A594
		private void btnQuit_MouseLeave(object sender, MouseEventArgs e)
		{
			this.btnQuit.BorderBrush = (this.btnQuit.Background = Brushes.Transparent);
		}
	}
}
