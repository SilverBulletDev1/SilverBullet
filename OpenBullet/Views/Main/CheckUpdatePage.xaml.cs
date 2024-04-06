using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using OpenBullet.Views.CustomMessageBox;

namespace OpenBullet.Views.Main
{
	// Token: 0x020000A6 RID: 166
	public partial class CheckUpdatePage : Page
	{
		// Token: 0x06000447 RID: 1095 RVA: 0x000154E2 File Offset: 0x000136E2
		public CheckUpdatePage()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000448 RID: 1096
		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		// Token: 0x06000449 RID: 1097 RVA: 0x000154F0 File Offset: 0x000136F0
		private void CheckForUpdate_Click(object sender, RoutedEventArgs e)
		{
			Task.Run(delegate
			{
				this.CheckForUpdate();
			});
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00015504 File Offset: 0x00013704
		public void CheckForUpdate()
		{
			try
			{
				base.Dispatcher.Invoke(delegate
				{
					this.richTextBox.Document.Blocks.Clear();
				});
				base.Dispatcher.Invoke(delegate
				{
					this.richTextBoxDonate.Document.Blocks.Clear();
				});
				SBUpdate result = CheckUpdate.Run<SBUpdate>("https://raw.githubusercontent.com/mohamm4dx/SilverBullet/master/SilverBulletUpdater/SBUpdate.json");
				LatestRelease releasesLatest = CheckUpdate.Run<LatestRelease>("https://api.github.com/repos/mohamm4dx/SilverBullet/releases/latest");
				if (releasesLatest.Assets != null && releasesLatest.Assets.Length != 0)
				{
					result.DownloadUrl = releasesLatest.Assets.First<Assets>().Browser_Download_Url;
				}
				result.Available = releasesLatest.Available;
				base.Dispatcher.Invoke<bool>(() => this.runUpdaterButton.IsEnabled = result.Available);
				releasesLatest.Body.Split(new char[] { '\n' }).ToList<string>().ForEach(delegate(string re)
				{
					re = re.Replace("•", string.Empty).Replace("â€¢", string.Empty).Trim();
					if (result.ReleaseNotes.Any((ReleaseNotes r) => r.Note.Trim().Replace("â€¢", string.Empty).Replace("•", string.Empty) != re))
					{
						result.ReleaseNotes.Add(new ReleaseNotes
						{
							Note = re
						});
					}
				});
				base.Dispatcher.Invoke<object>(() => this.DataContext = result);
				try
				{
					int num;
					int j;
					for (j = 0; j < result.Donate.Length; j = num + 1)
					{
						base.Dispatcher.Invoke(delegate
						{
							this.AppendParagraph(this.richTextBoxDonate, new string[] { result.Donate[j].Address });
						});
						num = j;
					}
				}
				catch
				{
				}
				if (result.Available)
				{
					try
					{
						int num;
						int i;
						for (i = 0; i < result.ReleaseNotes.Count; i = num + 1)
						{
							base.Dispatcher.Invoke(delegate
							{
								this.AppendParagraph(this.richTextBox, new string[] { result.ReleaseNotes[i].Note });
							});
							num = i;
						}
					}
					catch
					{
					}
					if (!string.IsNullOrWhiteSpace(result.Message))
					{
						base.Dispatcher.Invoke(delegate
						{
							CustomMsgBox.Show(result.Message, false);
						});
					}
				}
				else
				{
					base.Dispatcher.Invoke(delegate
					{
						CustomMsgBox.Show("there are currently no updates available", false);
					});
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0001577C File Offset: 0x0001397C
		private void AppendParagraph(RichTextBox richTextBox, string[] paragraphs)
		{
			foreach (string par in paragraphs)
			{
				Bold bold = new Bold(new Run("• "));
				bold.SetResourceReference(TextElement.ForegroundProperty, "ForegroundMain");
				Paragraph paragraph = new Paragraph(bold);
				paragraph.SetResourceReference(TextElement.ForegroundProperty, "ForegroundMain");
				paragraph.Inlines.Add(new Run(par));
				richTextBox.Document.Blocks.Add(paragraph);
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x000157F4 File Offset: 0x000139F4
		private void RunUpdater_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Process updater;
				if ((updater = Process.GetProcessesByName("SilverBulletUpdater").FirstOrDefault<Process>()) == null)
				{
					Process.Start("Updater\\SilverBulletUpdater.exe");
				}
				else if (!updater.HasExited)
				{
					CheckUpdatePage.SetForegroundWindow(updater.MainWindowHandle);
				}
			}
			catch (Exception ex)
			{
				CustomMsgBox.ShowError(ex.Message, false);
			}
		}
	}
}
