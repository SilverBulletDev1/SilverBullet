using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using Extreme.Net;
using MahApps.Metro.IconPacks;
using Microsoft.Win32;
using OpenBullet.ViewModels;
using RuriLib;
using RuriLib.Models;
using RuriLib.Runner;
using RuriLib.ViewModels;
using Xceed.Wpf.Toolkit;

namespace OpenBullet.Views.Main
{
	// Token: 0x020000CB RID: 203
	public partial class ProxyManager : Page
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x00019A51 File Offset: 0x00017C51
		private IEnumerable<CProxy> Selected
		{
			get
			{
				return this.proxiesListView.SelectedItems.Cast<CProxy>();
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00019A64 File Offset: 0x00017C64
		public ProxyManager()
		{
			this.vm = SB.ProxyManager;
			base.DataContext = this.vm;
			this.InitializeComponent();
			this.botsSlider.Maximum = (double)ProxyManagerViewModel.maximumBots;
			this.vm.RefreshList();
			this.vm.UpdateProperties();
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00019AC8 File Offset: 0x00017CC8
		private async void checkButton_Click(object sender, RoutedEventArgs e)
		{
			ProxyManager.<>c__DisplayClass8_0 CS$<>8__locals1 = new ProxyManager.<>c__DisplayClass8_0();
			CS$<>8__locals1.<>4__this = this;
			WorkerStatus status = this.Status;
			if (status != WorkerStatus.Idle)
			{
				if (status != WorkerStatus.Running)
				{
					return;
				}
			}
			else
			{
				SB.Logger.LogInfo(Components.ProxyManager, "Disabling the UI and starting the checker", false, 0);
				this.checkButtonLabel.Text = "ABORT";
				this.botsSlider.IsEnabled = false;
				this.Status = WorkerStatus.Running;
				if (!SB.Settings.ProxyManagerSettings.ProxySiteUrls.Contains(this.vm.TestSite))
				{
					SB.Settings.ProxyManagerSettings.ProxySiteUrls.Add(this.vm.TestSite);
				}
				if (!SB.Settings.ProxyManagerSettings.ProxyKeys.Contains(this.vm.SuccessKey))
				{
					SB.Settings.ProxyManagerSettings.ProxyKeys.Add(this.vm.SuccessKey);
				}
				IOManager.SaveSettings<ProxyManagerSettings>(SB.proxyManagerSettingsFile, SB.Settings.ProxyManagerSettings);
				ProxyManager.<>c__DisplayClass8_0 CS$<>8__locals2 = CS$<>8__locals1;
				IEnumerable<CProxy> enumerable;
				if (!this.vm.OnlyUntested)
				{
					enumerable = this.vm.Proxies;
				}
				else
				{
					enumerable = this.vm.Proxies.Where((CProxy p) => p.Working == ProxyWorking.UNTESTED);
				}
				CS$<>8__locals2.items = enumerable;
				this.progressBar.Value = 0.0;
				this.cts = new CancellationTokenSource();
				try
				{
					try
					{
						await Task.Run(delegate
						{
							ProxyManager.<>c__DisplayClass8_0.<<checkButton_Click>b__1>d <<checkButton_Click>b__1>d;
							<<checkButton_Click>b__1>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<checkButton_Click>b__1>d.<>4__this = CS$<>8__locals1;
							<<checkButton_Click>b__1>d.<>1__state = -1;
							<<checkButton_Click>b__1>d.<>t__builder.Start<ProxyManager.<>c__DisplayClass8_0.<<checkButton_Click>b__1>d>(ref <<checkButton_Click>b__1>d);
							return <<checkButton_Click>b__1>d.<>t__builder.Task;
						});
					}
					catch
					{
						SB.Logger.LogWarning(Components.ProxyManager, "Abort signal received", false, 0);
					}
					return;
				}
				finally
				{
					this.checkButtonLabel.Text = "CHECK";
					this.botsSlider.IsEnabled = true;
					this.Status = WorkerStatus.Idle;
				}
			}
			try
			{
				this.cts.Cancel();
			}
			catch
			{
			}
			try
			{
				this.cts.Dispose();
			}
			catch
			{
			}
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00019B00 File Offset: 0x00017D00
		public void AddProxies(IEnumerable<string> raw, ProxyType defaultType = ProxyType.Http, string defaultUsername = "", string defaultPassword = "")
		{
			SB.Logger.LogInfo(Components.ProxyManager, string.Format("Adding {0} {1} proxies to the database", raw.Count<string>(), defaultType), false, 0);
			List<CProxy> proxies = new List<CProxy>();
			foreach (string p2 in raw.Where((string p) => !string.IsNullOrEmpty(p)).Distinct<string>().ToList<string>())
			{
				try
				{
					CProxy proxy = new CProxy().Parse(p2, defaultType, defaultUsername, defaultPassword);
					if (!proxy.IsNumeric || proxy.IsValidNumeric)
					{
						proxies.Add(proxy);
					}
				}
				catch
				{
				}
			}
			this.vm.AddRange(proxies);
			this.vm.UpdateProperties();
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00019BF4 File Offset: 0x00017DF4
		private void botsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			this.vm.BotsAmount = (int)e.NewValue;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00019C08 File Offset: 0x00017E08
		private void exportButton_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Text File |*.txt";
			sfd.Title = "Export proxies";
			sfd.ShowDialog();
			if (sfd.FileName != string.Empty)
			{
				if (this.Selected.Count<CProxy>() > 0)
				{
					SB.Logger.LogInfo(Components.ProxyManager, string.Format("Exporting {0} proxies", this.proxiesListView.Items.Count), false, 0);
					this.Selected.SaveToFile(sfd.FileName, (CProxy p) => p.Proxy);
					return;
				}
				global::System.Windows.MessageBox.Show("No proxies selected!");
				SB.Logger.LogWarning(Components.ProxyManager, "No proxies selected", false, 0);
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00019CD8 File Offset: 0x00017ED8
		private void deleteButton_Click(object sender, RoutedEventArgs e)
		{
			SB.Logger.LogInfo(Components.ProxyManager, string.Format("Deleting {0} proxies", this.proxiesListView.SelectedItems.Count), false, 0);
			this.vm.Remove(this.Selected);
			this.vm.UpdateProperties();
			SB.Logger.LogInfo(Components.ProxyManager, "Proxies deleted successfully", false, 0);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00019D3F File Offset: 0x00017F3F
		private void deleteAllButton_Click(object sender, RoutedEventArgs e)
		{
			SB.Logger.LogWarning(Components.ProxyManager, "Purging all proxies", false, 0);
			this.vm.RemoveAll();
			this.vm.UpdateProperties();
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00019D69 File Offset: 0x00017F69
		private void deleteNotWorkingButton_Click(object sender, RoutedEventArgs e)
		{
			SB.Logger.LogInfo(Components.ProxyManager, "Deleting all non working proxies", false, 0);
			this.vm.RemoveNotWorking();
			this.vm.UpdateProperties();
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00019D93 File Offset: 0x00017F93
		private void importButton_Click(object sender, RoutedEventArgs e)
		{
			new MainDialog(new DialogAddProxies(this), "Import Proxies").ShowDialog();
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00019DAB File Offset: 0x00017FAB
		private void deleteDuplicatesButton_Click(object sender, RoutedEventArgs e)
		{
			SB.Logger.LogInfo(Components.ProxyManager, "Deleting duplicate proxies", false, 0);
			this.vm.RemoveDuplicates();
			this.vm.UpdateProperties();
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00019DD5 File Offset: 0x00017FD5
		private void DeleteUntestedButton_Click(object sender, RoutedEventArgs e)
		{
			SB.Logger.LogInfo(Components.ProxyManager, "Deleting all untested proxies", false, 0);
			this.vm.RemoveUntested();
			this.vm.UpdateProperties();
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00019E00 File Offset: 0x00018000
		private void ProxyListViewDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				foreach (string file in ((string[])e.Data.GetData(DataFormats.FileDrop)).Where((string x) => x.EndsWith(".txt")).ToArray<string>())
				{
					try
					{
						string[] lines = File.ReadAllLines(file);
						if (file.ToLower().Contains("http"))
						{
							this.AddProxies(lines, ProxyType.Http, "", "");
						}
						else if (file.ToLower().Contains("socks4a"))
						{
							this.AddProxies(lines, ProxyType.Socks4a, "", "");
						}
						else if (file.ToLower().Contains("socks4"))
						{
							this.AddProxies(lines, ProxyType.Socks4, "", "");
						}
						else if (file.ToLower().Contains("socks5"))
						{
							this.AddProxies(lines, ProxyType.Socks5, "", "");
						}
						else
						{
							SB.Logger.LogError(Components.ProxyManager, "Failed to parse proxies type from file name, defaulting to HTTP", false, 0);
							this.AddProxies(lines, ProxyType.Http, "", "");
						}
					}
					catch (Exception ex)
					{
						SB.Logger.LogError(Components.ProxyManager, "Failed to open file " + file + " - " + ex.Message, false, 0);
					}
				}
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00019F7C File Offset: 0x0001817C
		private void copySelectedProxies_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.Selected.CopyToClipboard((CProxy p) => p.Host + ":" + p.Port);
				SB.Logger.LogInfo(Components.ProxyManager, string.Format("Copied {0} proxies", this.Selected.Count<CProxy>()), false, 0);
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.ProxyManager, "Failed to copy proxies - " + ex.Message, false, 0);
			}
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001A00C File Offset: 0x0001820C
		private void copySelectedProxiesFull_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.Selected.CopyToClipboard((CProxy p) => string.Format("({0}){1}:{2}", p.Type, p.Host, p.Port) + (string.IsNullOrEmpty(p.Username) ? "" : (":" + p.Username + ":" + p.Password)));
				SB.Logger.LogInfo(Components.ProxyManager, string.Format("Copied {0} proxies", this.Selected.Count<CProxy>()), false, 0);
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.ProxyManager, "Failed to copy proxies - " + ex.Message, false, 0);
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001A09C File Offset: 0x0001829C
		private void listViewColumnHeader_Click(object sender, RoutedEventArgs e)
		{
			GridViewColumnHeader column = sender as GridViewColumnHeader;
			string sortBy = column.Tag.ToString();
			if (this.listViewSortCol != null)
			{
				AdornerLayer.GetAdornerLayer(this.listViewSortCol).Remove(this.listViewSortAdorner);
				this.proxiesListView.Items.SortDescriptions.Clear();
			}
			ListSortDirection newDir = ListSortDirection.Ascending;
			if (this.listViewSortCol == column && this.listViewSortAdorner.Direction == newDir)
			{
				newDir = ListSortDirection.Descending;
			}
			this.listViewSortCol = column;
			this.listViewSortAdorner = new SortAdorner(this.listViewSortCol, newDir);
			AdornerLayer.GetAdornerLayer(this.listViewSortCol).Add(this.listViewSortAdorner);
			this.proxiesListView.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00003B20 File Offset: 0x00001D20
		private void ListViewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001A158 File Offset: 0x00018358
		private void AddProxySiteUrl_OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (!SB.Settings.ProxyManagerSettings.ProxySiteUrls.Contains(this.vm.TestSite))
			{
				SB.Settings.ProxyManagerSettings.ProxySiteUrls.Add(this.vm.TestSite);
				IOManager.SaveSettings<ProxyManagerSettings>(SB.proxyManagerSettingsFile, SB.Settings.ProxyManagerSettings);
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001A1BC File Offset: 0x000183BC
		private void RemoveProxySiteUrl_OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			Image image = sender as Image;
			if (image != null)
			{
				Grid grid = image.Parent as Grid;
				if (grid != null && grid.Children.Count == 2)
				{
					TextBlock textBlock = grid.Children[0] as TextBlock;
					if (textBlock != null)
					{
						SB.Settings.ProxyManagerSettings.ProxySiteUrls.Remove(textBlock.Text);
						IOManager.SaveSettings<ProxyManagerSettings>(SB.proxyManagerSettingsFile, SB.Settings.ProxyManagerSettings);
					}
				}
			}
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0001A234 File Offset: 0x00018434
		private void AddProxyKey_OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (!SB.Settings.ProxyManagerSettings.ProxyKeys.Contains(this.vm.SuccessKey))
			{
				SB.Settings.ProxyManagerSettings.ProxyKeys.Add(this.vm.SuccessKey);
				IOManager.SaveSettings<ProxyManagerSettings>(SB.proxyManagerSettingsFile, SB.Settings.ProxyManagerSettings);
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001A298 File Offset: 0x00018498
		private void RemoveProxyKey_OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			Image image = sender as Image;
			if (image != null)
			{
				Grid grid = image.Parent as Grid;
				if (grid != null && grid.Children.Count == 2)
				{
					TextBlock textBlock = grid.Children[0] as TextBlock;
					if (textBlock != null)
					{
						SB.Settings.ProxyManagerSettings.ProxyKeys.Remove(textBlock.Text);
						IOManager.SaveSettings<ProxyManagerSettings>(SB.proxyManagerSettingsFile, SB.Settings.ProxyManagerSettings);
					}
				}
			}
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001A310 File Offset: 0x00018510
		private void Image_MouseEnter(object sender, MouseEventArgs e)
		{
			Image image = sender as Image;
			if (image != null)
			{
				image.Width = 25.0;
				image.Height = 25.0;
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001A348 File Offset: 0x00018548
		private void Image_MouseLeave(object sender, MouseEventArgs e)
		{
			Image image = sender as Image;
			if (image != null)
			{
				image.Width = 20.0;
				image.Height = 20.0;
			}
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001A798 File Offset: 0x00018998
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 4)
			{
				((Image)target).MouseDown += this.RemoveProxySiteUrl_OnMouseDown;
				((Image)target).MouseEnter += this.Image_MouseEnter;
				((Image)target).MouseLeave += this.Image_MouseLeave;
				return;
			}
			if (connectionId == 7)
			{
				((Image)target).MouseDown += this.RemoveProxyKey_OnMouseDown;
				((Image)target).MouseEnter += this.Image_MouseEnter;
				((Image)target).MouseLeave += this.Image_MouseLeave;
				return;
			}
			if (connectionId != 13)
			{
				return;
			}
			EventSetter eventSetter = new EventSetter();
			eventSetter.Event = UIElement.MouseRightButtonDownEvent;
			eventSetter.Handler = new MouseButtonEventHandler(this.ListViewItem_MouseRightButtonDown);
			((Style)target).Setters.Add(eventSetter);
		}

		// Token: 0x0400041D RID: 1053
		private ProxyManagerViewModel vm;

		// Token: 0x0400041E RID: 1054
		private GridViewColumnHeader listViewSortCol;

		// Token: 0x0400041F RID: 1055
		private SortAdorner listViewSortAdorner;

		// Token: 0x04000420 RID: 1056
		private WorkerStatus Status;

		// Token: 0x04000421 RID: 1057
		private CancellationTokenSource cts = new CancellationTokenSource();
	}
}
