using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace OpenBullet
{
	// Token: 0x02000025 RID: 37
	public partial class NotesWindow : Window
	{
		// Token: 0x0600007E RID: 126 RVA: 0x0000301D File Offset: 0x0000121D
		public NotesWindow()
		{
			this.InitializeComponent();
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600007F RID: 127 RVA: 0x0000302B File Offset: 0x0000122B
		// (set) Token: 0x06000080 RID: 128 RVA: 0x00003033 File Offset: 0x00001233
		public string SBUrl { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000081 RID: 129 RVA: 0x0000303C File Offset: 0x0000123C
		// (set) Token: 0x06000082 RID: 130 RVA: 0x00003044 File Offset: 0x00001244
		public bool DontShowMainWindow { get; set; }

		// Token: 0x06000083 RID: 131 RVA: 0x0000304D File Offset: 0x0000124D
		private void dragPanel_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				base.DragMove();
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000305D File Offset: 0x0000125D
		private void titleLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			base.DragMove();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003065 File Offset: 0x00001265
		private void quitPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (this._canClose)
			{
				this._canClose = false;
				if (this.MainWindow == null)
				{
					new MainWindow().Show();
				}
				base.Close();
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000308E File Offset: 0x0000128E
		private void quitPanel_MouseLeave(object sender, MouseEventArgs e)
		{
			this.quitPanel.Background = new SolidColorBrush(Colors.Transparent);
			this._canClose = false;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000030AC File Offset: 0x000012AC
		private void quitPanel_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this._canClose = true;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000030B5 File Offset: 0x000012B5
		private void quitPanel_MouseEnter(object sender, MouseEventArgs e)
		{
			this.quitPanel.Background = new SolidColorBrush(Colors.DarkRed);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000030CC File Offset: 0x000012CC
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Clipboard.SetText(this.SBUrl);
			}
			catch
			{
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000030FC File Offset: 0x000012FC
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.MainWindow == null && !this.DontShowMainWindow)
				{
					this.MainWindow = new MainWindow();
				}
				this.MainWindow.Show();
				base.Close();
			}
			catch
			{
			}
		}

		// Token: 0x04000098 RID: 152
		private bool _canClose;

		// Token: 0x0400009B RID: 155
		public MainWindow MainWindow;
	}
}
