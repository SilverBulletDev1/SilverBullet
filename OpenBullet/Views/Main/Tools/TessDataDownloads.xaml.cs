using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace OpenBullet.Views.Main.Tools
{
	// Token: 0x020000D9 RID: 217
	public partial class TessDataDownloads : Page
	{
		// Token: 0x06000598 RID: 1432 RVA: 0x0001D1D0 File Offset: 0x0001B3D0
		public TessDataDownloads()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001D204 File Offset: 0x0001B404
		private void LoadSite_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			try
			{
				this.DispatcherInvoke(delegate
				{
					this.progressBar.Value = (double)e.ProgressPercentage;
				});
			}
			catch
			{
			}
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001D24C File Offset: 0x0001B44C
		private void DownloadBtn_Click(object sender, RoutedEventArgs e)
		{
			TessDataDownloads.<>c__DisplayClass8_0 CS$<>8__locals1 = new TessDataDownloads.<>c__DisplayClass8_0();
			CS$<>8__locals1.<>4__this = this;
			this.LogsText.Clear();
			this.progressBar.Value = 0.0;
			CS$<>8__locals1.i = 0;
			global::System.Windows.Controls.TextBox logsText = this.LogsText;
			logsText.Text = logsText.Text + "Downloading tessdata files..." + Environment.NewLine;
			CS$<>8__locals1.items = this.DownloadList.Items;
			try
			{
				Task task = this.taskDl;
				if (task != null)
				{
					task.Dispose();
				}
			}
			catch
			{
			}
			this.taskDl = Task.Run(delegate
			{
				TessDataDownloads.<>c__DisplayClass8_0.<<DownloadBtn_Click>b__0>d <<DownloadBtn_Click>b__0>d;
				<<DownloadBtn_Click>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<DownloadBtn_Click>b__0>d.<>4__this = CS$<>8__locals1;
				<<DownloadBtn_Click>b__0>d.<>1__state = -1;
				<<DownloadBtn_Click>b__0>d.<>t__builder.Start<TessDataDownloads.<>c__DisplayClass8_0.<<DownloadBtn_Click>b__0>d>(ref <<DownloadBtn_Click>b__0>d);
				return <<DownloadBtn_Click>b__0>d.<>t__builder.Task;
			}).ContinueWith(delegate(Task _)
			{
				TessDataDownloads <>4__this = CS$<>8__locals1.<>4__this;
				Action action;
				if ((action = CS$<>8__locals1.<>9__5) == null)
				{
					action = (CS$<>8__locals1.<>9__5 = delegate
					{
						global::System.Windows.Controls.TextBox logsText2 = CS$<>8__locals1.<>4__this.LogsText;
						logsText2.Text += "Your chosen languages have been downloaded ";
					});
				}
				<>4__this.DispatcherInvoke(action);
			});
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001D30C File Offset: 0x0001B50C
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			this.LogsText.Clear();
			this.LanguageList.Items.Clear();
			this.LogsText.Text = "Downloading language list...\n";
			Task.Run<string>(() => this.siteResponse = this.loadSite.DownloadString(this.url)).ContinueWith(delegate(Task<string> c)
			{
				foreach (object obj in this.lang.Matches(this.siteResponse))
				{
					Match line = (Match)obj;
					string val = line.Groups[1].Value.Split(new char[] { '"' }).First<string>();
					base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate
					{
						this.LanguageList.Items.Add(Path.GetFileNameWithoutExtension(val) + " (" + Path.GetExtension(val).Split(new char[] { '.' })[1] + ")");
					}));
				}
				this.DispatcherInvoke(delegate
				{
					global::System.Windows.Controls.TextBox logsText = this.LogsText;
					logsText.Text = logsText.Text + "Downloading language list Finished!" + Environment.NewLine;
				});
			});
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001D368 File Offset: 0x0001B568
		private void DispatcherInvoke(Action action)
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate
			{
				Action action2 = action;
				if (action2 == null)
				{
					return;
				}
				action2();
			}));
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001D39C File Offset: 0x0001B59C
		public async Task DownloadLanguage(int i, string language)
		{
			language = language.Split(new char[] { '(' }).First<string>().Trim();
			language += ".traineddata";
			this.loadSite.DownloadProgressChanged += this.LoadSite_DownloadProgressChanged;
			await this.loadSite.DownloadFileTaskAsync(new Uri("https://github.com/tesseract-ocr/tessdata/raw/3.04.00/" + language), AppDomain.CurrentDomain.BaseDirectory + "/tessdata/" + language);
			this.DispatcherInvoke(delegate
			{
				global::System.Windows.Controls.TextBox logsText = this.LogsText;
				logsText.Text = logsText.Text + "\t| Finished!" + Environment.NewLine;
			});
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001D3E7 File Offset: 0x0001B5E7
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.DownloadList.Items.Add(this.LanguageList.SelectedItem);
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001D405 File Offset: 0x0001B605
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			this.DownloadList.Items.Remove(this.DownloadList.SelectedItem);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001D424 File Offset: 0x0001B624
		private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (global::System.Windows.Forms.MessageBox.Show("This button will add ALL languages to the download list", "ALERT", MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				foreach (object obj in ((IEnumerable)this.LanguageList.Items))
				{
					string item = (string)obj;
					this.DownloadList.Items.Add(item);
				}
			}
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001D4A0 File Offset: 0x0001B6A0
		private void Button_MouseRightButtonDown_1(object sender, MouseButtonEventArgs e)
		{
			if (global::System.Windows.Forms.MessageBox.Show("This button will remove ALL languages to the download list", "ALERT", MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				this.DownloadList.Items.Clear();
			}
		}

		// Token: 0x0400048A RID: 1162
		private WebClient loadSite = new WebClient();

		// Token: 0x0400048B RID: 1163
		public string url = "https://github.com/tesseract-ocr/tessdata/tree/3.04.00";

		// Token: 0x0400048C RID: 1164
		public string siteResponse;

		// Token: 0x0400048D RID: 1165
		public string language;

		// Token: 0x0400048E RID: 1166
		private Regex lang = new Regex("title=\"(.*)\" href=\"/tesseract-ocr/tessdata/blob/");

		// Token: 0x0400048F RID: 1167
		private Task taskDl;
	}
}
