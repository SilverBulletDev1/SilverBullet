using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using AngleSharp.Text;

namespace OpenBullet
{
	// Token: 0x02000035 RID: 53
	public partial class LoadingWindow : Window
	{
		// Token: 0x0600010F RID: 271 RVA: 0x00004D5B File Offset: 0x00002F5B
		public LoadingWindow()
		{
			this.InitializeComponent();
			((Storyboard)base.FindResource("WaitStoryboard")).Begin();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00003B20 File Offset: 0x00001D20
		private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000305D File Offset: 0x0000125D
		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			base.DragMove();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00004D88 File Offset: 0x00002F88
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.CheckBox_Click(null, null);
			try
			{
				this.task = Task.Run(delegate
				{
					this.CheckForUpdate();
				}).ContinueWith(delegate(Task _)
				{
				}, (this.cancellationToken = new CancellationTokenSource()).Token);
			}
			catch
			{
				new MainWindow().Show();
				base.Close();
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00004E10 File Offset: 0x00003010
		private void CheckForUpdate()
		{
			Task.Delay(200);
			if (!this.checkUpdate.GetValueOrDefault())
			{
				Task.Delay(2000);
				this.showMainWindow = true;
				this.Window_Closing(null, null);
				return;
			}
			this.cancellationToken.Token.ThrowIfCancellationRequested();
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004E64 File Offset: 0x00003064
		private void CheckBox_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.checkUpdate == null)
				{
					if (File.Exists("Settings\\Update.txt"))
					{
						this.checkUpdate = new bool?(File.ReadAllText("Settings\\Update.txt").ToBoolean(false));
						this.checkBoxUpdate.IsChecked = this.checkUpdate;
						return;
					}
					this.checkUpdate = new bool?(true);
				}
				try
				{
					this.checkUpdate = new bool?(this.checkBoxUpdate.IsChecked.GetValueOrDefault());
				}
				catch (NullReferenceException)
				{
					this.checkUpdate = new bool?(true);
				}
				if (File.Exists("Settings\\Update.txt"))
				{
					using (File.CreateText("Settings\\Update.txt"))
					{
					}
				}
				File.WriteAllText("Settings\\Update.txt", this.checkUpdate.ToString());
			}
			catch
			{
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00004F64 File Offset: 0x00003164
		private void Window_Closing(object sender, CancelEventArgs e)
		{
			try
			{
				if (this.mainWindow == null && this.notesWindow == null && this.showMainWindow)
				{
					base.Hide();
					this.mainWindow = new MainWindow();
					this.mainWindow.Show();
					this.showMainWindow = false;
					if (e == null)
					{
						base.Close();
					}
				}
			}
			catch (InvalidOperationException)
			{
				try
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate
					{
						this.mainWindow = new MainWindow();
						this.mainWindow.Show();
						this.showMainWindow = false;
						if (e == null)
						{
							this.Close();
						}
					}));
				}
				catch
				{
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000304D File Offset: 0x0000124D
		private void dragPanel_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				base.DragMove();
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000305D File Offset: 0x0000125D
		private void titleLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			base.DragMove();
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005020 File Offset: 0x00003220
		private void quitPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (this._canClose)
			{
				Environment.Exit(0);
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005030 File Offset: 0x00003230
		private void quitPanel_MouseLeave(object sender, MouseEventArgs e)
		{
			this.quitPanel.Background = new SolidColorBrush(Colors.Transparent);
			this._canClose = false;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000504E File Offset: 0x0000324E
		private void quitPanel_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this._canClose = true;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005057 File Offset: 0x00003257
		private void quitPanel_MouseEnter(object sender, MouseEventArgs e)
		{
			this.quitPanel.Background = new SolidColorBrush(Colors.DarkRed);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005070 File Offset: 0x00003270
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				try
				{
					this.task.Dispose();
				}
				catch
				{
				}
				try
				{
					this.cancellationToken.Cancel(true);
				}
				catch
				{
				}
				try
				{
					this.cancellationToken.Dispose();
				}
				catch
				{
				}
				this.mainWindow = new MainWindow();
				if (this.notesWindow != null)
				{
					this.notesWindow.MainWindow = this.mainWindow;
				}
				this.mainWindow.Show();
				base.Close();
			}
			catch
			{
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005120 File Offset: 0x00003320
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.Start("https://discord.gg/8jFtRs");
			}
			catch
			{
			}
		}

		// Token: 0x0400010A RID: 266
		private bool? checkUpdate;

		// Token: 0x0400010B RID: 267
		private MainWindow mainWindow;

		// Token: 0x0400010C RID: 268
		private NotesWindow notesWindow;

		// Token: 0x0400010D RID: 269
		private bool _canClose;

		// Token: 0x0400010E RID: 270
		private bool showMainWindow = true;

		// Token: 0x0400010F RID: 271
		private const string discoard = "https://discord.gg/8jFtRs";

		// Token: 0x04000110 RID: 272
		private CancellationTokenSource cancellationToken;

		// Token: 0x04000111 RID: 273
		private Task task;
	}
}
