using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using OpenBullet.Models;
using OpenBullet.ViewModels;
using RuriLib;
using RuriLib.Runner;

namespace OpenBullet.Views.Main
{
	// Token: 0x020000C6 RID: 198
	public partial class RunnerManager : Page
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x00018DAD File Offset: 0x00016FAD
		// (set) Token: 0x060004EB RID: 1259 RVA: 0x00018DB5 File Offset: 0x00016FB5
		private bool DelegateCalled { get; set; }

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060004EC RID: 1260 RVA: 0x00018DC0 File Offset: 0x00016FC0
		// (remove) Token: 0x060004ED RID: 1261 RVA: 0x00018DF8 File Offset: 0x00016FF8
		public event RunnerManager.StartRunnerEventHandler StartRunner;

		// Token: 0x060004EE RID: 1262 RVA: 0x00018E2D File Offset: 0x0001702D
		protected virtual void OnStartRunner()
		{
			RunnerManager.StartRunnerEventHandler startRunner = this.StartRunner;
			if (startRunner == null)
			{
				return;
			}
			startRunner(this, EventArgs.Empty);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00018E45 File Offset: 0x00017045
		public RunnerManager()
		{
			this.vm = SB.RunnerManager;
			base.DataContext = this.vm;
			this.InitializeComponent();
			base.Loaded += delegate
			{
				if (this.vm.RunnersCollection.Count > 0)
				{
					this.helpMessageLabel.Visibility = Visibility.Collapsed;
					this.dlCount.Visibility = Visibility.Collapsed;
					this.mostDownloads.Visibility = Visibility.Collapsed;
					return;
				}
				this.dlCount.Visibility = Visibility.Collapsed;
				this.mostDownloads.Visibility = Visibility.Collapsed;
				string json = string.Empty;
				try
				{
					Task.Run(delegate
					{
						using (WebClient client = new WebClient())
						{
							client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:87.0) Gecko/20100101 Firefox/87.0");
							json = client.DownloadString("https://api.github.com/repos/mohamm4dx/SilverBullet/releases");
							SBRelease[] sbReleases = IOManager.DeserializeObject<SBRelease[]>(json);
							if (sbReleases != null && sbReleases.Length != 0)
							{
								SBRelease currentRelease = sbReleases.FirstOrDefault((SBRelease r) => r.Ver.ToString() == "1.1.4");
								SBRelease mostDlRelease = sbReleases.OrderByDescending((SBRelease r) => r.Assets[0].download_count).FirstOrDefault<SBRelease>();
								if (currentRelease != null)
								{
									this.Dispatcher.Invoke<Visibility>(() => this.dlCount.Visibility = Visibility.Visible);
									this.Dispatcher.Invoke<string>(() => this.dlCount.Text = string.Format("Download Count From Github: {0}", currentRelease.Assets[0].download_count));
									if (mostDlRelease.Assets[0].download_count == currentRelease.Assets[0].download_count)
									{
										this.Dispatcher.Invoke<string>(() => this.mostDownloads.Text = "Most Downloads For This Version");
									}
									else
									{
										this.Dispatcher.Invoke<string>(() => this.mostDownloads.Text = string.Format("Most Downloads For {0} Version is {1} Downloads", mostDlRelease.Ver, mostDlRelease.Assets[0].download_count));
									}
									this.Dispatcher.Invoke<Visibility>(() => this.mostDownloads.Visibility = Visibility.Visible);
								}
							}
						}
					});
				}
				catch
				{
				}
			};
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00018E7C File Offset: 0x0001707C
		private void addRunnerButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.Create();
			this.helpMessageLabel.Visibility = Visibility.Collapsed;
			this.dlCount.Visibility = Visibility.Collapsed;
			this.mostDownloads.Visibility = Visibility.Collapsed;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00018EB0 File Offset: 0x000170B0
		private void removeRunnerButton_Click(object sender, RoutedEventArgs e)
		{
			int id = (int)((Button)e.OriginalSource).Tag;
			if (this.vm.Get(id).ViewModel.Master.Status != WorkerStatus.Idle)
			{
				MessageBox.Show("The Runner is active! Please stop it before removing it.");
				return;
			}
			this.vm.Remove(id);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00018F08 File Offset: 0x00017108
		private void startRunnerButton_Click(object sender, RoutedEventArgs e)
		{
			int id = (int)((Button)e.OriginalSource).Tag;
			RunnerInstance runner = this.vm.Get(id);
			this.StartRunner += runner.View.OnStartRunner;
			this.OnStartRunner();
			this.StartRunner -= runner.View.OnStartRunner;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00018F6C File Offset: 0x0001716C
		private void runnerInstanceGrid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (this.DelegateCalled)
			{
				this.DelegateCalled = false;
				return;
			}
			if (sender.GetType() == typeof(Grid))
			{
				int id = (int)(sender as Grid).Tag;
				SB.MainWindow.ShowRunner(this.vm.Get(id).View);
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00018FCC File Offset: 0x000171CC
		private static T FindParent<T>(DependencyObject child) where T : DependencyObject
		{
			DependencyObject parentObject = VisualTreeHelper.GetParent(child);
			if (parentObject == null)
			{
				return default(T);
			}
			T parent = parentObject as T;
			if (parent != null)
			{
				return parent;
			}
			return RunnerManager.FindParent<T>(parentObject);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001900C File Offset: 0x0001720C
		private void selectConfig_MouseDown(object sender, MouseButtonEventArgs e)
		{
			int id = (int)RunnerManager.FindParent<Grid>(sender as DependencyObject).Tag;
			RunnerInstance runner = SB.MainWindow.RunnerManagerPage.vm.Get(id);
			if (!runner.ViewModel.Busy)
			{
				this.DelegateCalled = true;
				new MainDialog(new DialogSelectConfig(runner.View), "Select Config").ShowDialog();
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00019074 File Offset: 0x00017274
		private void selectWordlist_MouseDown(object sender, MouseButtonEventArgs e)
		{
			int id = (int)RunnerManager.FindParent<Grid>(sender as DependencyObject).Tag;
			RunnerInstance runner = SB.MainWindow.RunnerManagerPage.vm.Get(id);
			if (!runner.ViewModel.Busy)
			{
				this.DelegateCalled = true;
				new MainDialog(new DialogSelectWordlist(runner.View), "Select Wordlist").ShowDialog();
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x000190DC File Offset: 0x000172DC
		private void selectProxies_MouseDown(object sender, MouseButtonEventArgs e)
		{
			int id = (int)RunnerManager.FindParent<Grid>(sender as DependencyObject).Tag;
			RunnerInstance runner = SB.MainWindow.RunnerManagerPage.vm.Get(id);
			if (!runner.ViewModel.Busy)
			{
				this.DelegateCalled = true;
				new MainDialog(new DialogSetProxies(runner.ViewModel), "Set Proxies").ShowDialog();
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00019144 File Offset: 0x00017344
		private void selectBots_MouseDown(object sender, MouseButtonEventArgs e)
		{
			int id = (int)RunnerManager.FindParent<Grid>(sender as DependencyObject).Tag;
			RunnerInstance runner = SB.MainWindow.RunnerManagerPage.vm.Get(id);
			if (!runner.ViewModel.Busy)
			{
				this.DelegateCalled = true;
				new MainDialog(new DialogSelectBots(runner.ViewModel, runner.ViewModel.BotsAmount), "Select Bots Number").ShowDialog();
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x000191B8 File Offset: 0x000173B8
		private void stopAllRunnersButton_Click(object sender, RoutedEventArgs e)
		{
			foreach (RunnerInstance runner in this.vm.RunnersCollection.Where((RunnerInstance r) => r.ViewModel.Busy))
			{
				this.StartRunner += runner.View.OnStartRunner;
				this.OnStartRunner();
				this.StartRunner -= runner.View.OnStartRunner;
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001925C File Offset: 0x0001745C
		private void removeAllRunnersButton_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to remove all Runners?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
			{
				List<RunnerInstance> list = this.vm.RunnersCollection.Where((RunnerInstance r) => !r.ViewModel.Busy).ToList<RunnerInstance>();
				for (int i = list.Count - 1; i >= 0; i--)
				{
					this.vm.RunnersCollection.Remove(list[i]);
				}
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x000192E0 File Offset: 0x000174E0
		private void startAllRunnersButton_Click(object sender, RoutedEventArgs e)
		{
			foreach (RunnerInstance runner in this.vm.RunnersCollection.Where((RunnerInstance r) => !r.ViewModel.Busy))
			{
				this.StartRunner += runner.View.OnStartRunner;
				this.OnStartRunner();
				this.StartRunner -= runner.View.OnStartRunner;
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00019384 File Offset: 0x00017584
		private void LabelCustom_MouseEnter(object sender, MouseEventArgs e)
		{
			try
			{
				int id = (int)RunnerManager.FindParent<Grid>(sender as DependencyObject).Tag;
				SB.MainWindow.RunnerManagerPage.vm.Get(id).View.LabelCustom_MouseEnter(sender, e);
			}
			catch
			{
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x000193E0 File Offset: 0x000175E0
		private void LabelCustom_MouseLeave(object sender, MouseEventArgs e)
		{
			try
			{
				(e.OriginalSource as Label).ToolTip = null;
			}
			catch
			{
			}
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001954C File Offset: 0x0001774C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 9:
				((Grid)target).MouseDown += this.runnerInstanceGrid_MouseDown;
				return;
			case 10:
				((Label)target).MouseDown += this.selectConfig_MouseDown;
				return;
			case 11:
				((Label)target).MouseDown += this.selectConfig_MouseDown;
				return;
			case 12:
				((Label)target).MouseDown += this.selectWordlist_MouseDown;
				return;
			case 13:
				((Label)target).MouseDown += this.selectWordlist_MouseDown;
				return;
			case 14:
				((Label)target).MouseDown += this.selectBots_MouseDown;
				return;
			case 15:
				((Label)target).MouseDown += this.selectBots_MouseDown;
				return;
			case 16:
				((Label)target).MouseDown += this.selectProxies_MouseDown;
				return;
			case 17:
				((Label)target).MouseDown += this.selectProxies_MouseDown;
				return;
			case 18:
				((Label)target).MouseEnter += this.LabelCustom_MouseEnter;
				((Label)target).MouseLeave += this.LabelCustom_MouseLeave;
				return;
			case 19:
				((Button)target).Click += this.startRunnerButton_Click;
				return;
			case 20:
				((Button)target).Click += this.removeRunnerButton_Click;
				return;
			default:
				return;
			}
		}

		// Token: 0x04000406 RID: 1030
		private RunnerManagerViewModel vm;

		// Token: 0x020000C7 RID: 199
		// (Invoke) Token: 0x06000506 RID: 1286
		public delegate void StartRunnerEventHandler(object sender, EventArgs e);
	}
}
