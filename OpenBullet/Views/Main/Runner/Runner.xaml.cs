using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Extreme.Net;
using MahApps.Metro.IconPacks;
using OpenBullet.ViewModels;
using PluginFramework;
using RuriLib;
using RuriLib.Functions.Files;
using RuriLib.Models;
using RuriLib.Runner;
using RuriLib.ViewModels;

namespace OpenBullet.Views.Main.Runner
{
	// Token: 0x020000D4 RID: 212
	public partial class Runner : Page
	{
		// Token: 0x0600055A RID: 1370 RVA: 0x0001B8FC File Offset: 0x00019AFC
		public Runner(RunnerViewModel vm)
		{
			this.vm = vm;
			base.DataContext = vm;
			this.InitializeComponent();
			vm.MessageArrived += this.LogRunnerData;
			vm.WorkerStatusChanged += this.LogWorkerStatus;
			vm.WorkerStatusChanged += this.ProcessStatusChange;
			vm.FoundHit += this.PlayHitSound;
			vm.FoundHit += this.RegisterHit;
			vm.ReloadProxies += this.PlayReloadSound;
			vm.ReloadProxies += this.LoadProxiesFromManager;
			vm.DispatchAction += this.ExecuteAction;
			vm.SaveProgress += this.SaveProgressToDB;
			vm.AskCustomInputs += this.InitCustomInputs;
			if (SB.SBSettings.General.ChangeRunnerInterface)
			{
				SB.Logger.LogInfo(Components.About, "Changed the Runner interface", false, 0);
				Grid.SetColumn(this.rightGrid, 0);
				Grid.SetRow(this.rightGrid, 2);
				Grid.SetColumn(this.bottomLeftGrid, 2);
				Grid.SetRow(this.bottomLeftGrid, 0);
			}
			this.logBox.AppendText("", Colors.White);
			this.logBox.AppendText("Runner initialized succesfully!" + Environment.NewLine, Utils.GetColor("ForegroundMain"));
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001BA64 File Offset: 0x00019C64
		private void LogRunnerData(IRunnerMessaging sender, LogLevel level, string message, bool prompt, int timeout)
		{
			SB.Logger.Log(Components.Runner, level, message, prompt, timeout, false);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001BA7C File Offset: 0x00019C7C
		private void LogWorkerStatus(IRunnerMessaging sender)
		{
			RunnerViewModel runner = sender as RunnerViewModel;
			switch (runner.Master.Status)
			{
			case WorkerStatus.Idle:
				base.Dispatcher.Invoke(delegate
				{
					this.logBox.AppendText(string.Format("Aborted Runner at {0}.{1}", DateTime.Now, Environment.NewLine), Utils.GetColor("ForegroundBad"));
				});
				return;
			case WorkerStatus.Running:
				base.Dispatcher.Invoke(delegate
				{
					this.logBox.AppendText(string.Format("Started Running Config {0} with Wordlist {1} at {2}.{3}", new object[]
					{
						runner.ConfigName,
						runner.WordlistName,
						DateTime.Now,
						Environment.NewLine
					}), Utils.GetColor("ForegroundGood"));
				});
				return;
			case WorkerStatus.Stopping:
				base.Dispatcher.Invoke(delegate
				{
					this.logBox.AppendText(string.Format("Sent Abort Request at {0}.{1}", DateTime.Now, Environment.NewLine), Utils.GetColor("ForegroundCustom"));
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001BB0D File Offset: 0x00019D0D
		private void ExecuteAction(IRunnerMessaging sender, Action action)
		{
			Application.Current.Dispatcher.Invoke(action);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001BB20 File Offset: 0x00019D20
		private void RegisterHit(IRunnerMessaging sender, Hit hit)
		{
			Application.Current.Dispatcher.Invoke(delegate
			{
				if (this.vm.Config != null && this.vm.Config.Settings.SaveHitsToTextFile)
				{
					try
					{
						SB.Logger.LogInfo(Components.Runner, string.Concat(new string[] { "Adding ", hit.Type, " hit ", hit.Data, " to the text file" }), false, 0);
						string folderName = Path.Combine("Hits", Files.MakeValidFileName(this.vm.Config.Settings.Name, true));
						if (!Directory.Exists(folderName))
						{
							Directory.CreateDirectory(folderName);
						}
						string fileName = Path.Combine(folderName, hit.Type + ".txt");
						object @lock = FileLocker.GetLock(fileName);
						lock (@lock)
						{
							File.AppendAllText(fileName, hit.Data + " | " + hit.CapturedString + Environment.NewLine);
						}
						return;
					}
					catch (Exception ex)
					{
						SB.Logger.LogError(Components.Runner, string.Concat(new string[] { "Failed to add ", hit.Type, " hit ", hit.Data, " to the text file - ", ex.Message }), false, 0);
						return;
					}
				}
				try
				{
					SB.Logger.LogInfo(Components.Runner, string.Concat(new string[] { "Adding ", hit.Type, " hit ", hit.Data, " to the DB" }), false, 0);
					SB.HitsDB.Add(hit);
					SB.MainWindow.HitsDBPage.AddConfigToFilter(this.vm.ConfigName);
				}
				catch (Exception ex2)
				{
					SB.Logger.LogError(Components.Runner, string.Concat(new string[] { "Failed to add ", hit.Type, " hit ", hit.Data, " to the DB - ", ex2.Message }), false, 0);
				}
			});
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0001BB5C File Offset: 0x00019D5C
		private void PlayHitSound(IRunnerMessaging sender, Hit hit)
		{
			if (SB.SBSettings.Sounds.EnableSounds && hit.Type == "SUCCESS")
			{
				try
				{
					while (this.soundLock)
					{
						Thread.Sleep(10);
					}
					this.soundLock = true;
					this.hitPlayer.Play();
					this.soundLock = false;
				}
				catch
				{
				}
				finally
				{
					this.soundLock = false;
				}
			}
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001BBE0 File Offset: 0x00019DE0
		private void PlayReloadSound(IRunnerMessaging sender)
		{
			if (SB.SBSettings.Sounds.EnableSounds)
			{
				try
				{
					while (this.soundLock)
					{
						Thread.Sleep(10);
					}
					this.soundLock = true;
					this.reloadPlayer.Play();
					this.soundLock = false;
				}
				catch
				{
				}
				finally
				{
					this.soundLock = false;
				}
			}
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001BC54 File Offset: 0x00019E54
		private void LoadProxiesFromManager(IRunnerMessaging sender)
		{
			List<CProxy> proxies = SB.ProxyManager.ProxiesCollection.ToList<CProxy>();
			List<CProxy> toAdd;
			if (this.vm.Config.Settings.OnlySocks)
			{
				toAdd = proxies.Where((CProxy x) => x.Type > ProxyType.Http).ToList<CProxy>();
			}
			else if (this.vm.Config.Settings.OnlySsl)
			{
				toAdd = proxies.Where((CProxy x) => x.Type == ProxyType.Http).ToList<CProxy>();
			}
			else
			{
				toAdd = proxies;
			}
			this.vm.ProxyPool = new ProxyPool(toAdd, SB.Settings.RLSettings.Proxies.ShuffleOnStart);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0001BD20 File Offset: 0x00019F20
		private void ProcessStatusChange(IRunnerMessaging sender)
		{
			if (this.vm.Master.Status == WorkerStatus.Idle)
			{
				this.SaveRecord();
				base.Dispatcher.Invoke(delegate
				{
					this.startButtonLabel.Text = "START";
					this.startButtonIcon.Kind = PackIconMaterialKind.PlayCircleOutline;
				});
			}
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0001BD51 File Offset: 0x00019F51
		private void SaveProgressToDB(IRunnerMessaging sender)
		{
			this.SaveRecord();
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001BD59 File Offset: 0x00019F59
		private void InitCustomInputs(IRunnerMessaging sender)
		{
			Application.Current.Dispatcher.Invoke(delegate
			{
				this.vm.CustomInputs = new List<KeyValuePair<string, string>>();
				foreach (CustomInput input in this.vm.Config.Settings.CustomInputs)
				{
					SB.Logger.LogInfo(Components.Runner, "Asking for input " + input.Description, false, 0);
					new MainDialog(new DialogCustomInput(this.vm, input.VariableName, input.Description), "Custom Input").ShowDialog();
				}
				this.vm.CustomInputsInitialized = true;
			});
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001BD76 File Offset: 0x00019F76
		public void OnStartRunner(object sender, EventArgs e)
		{
			this.startButton_Click(this, new RoutedEventArgs());
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001BD84 File Offset: 0x00019F84
		public void startButton_Click(object sender, RoutedEventArgs e)
		{
			switch (this.vm.Master.Status)
			{
			case WorkerStatus.Idle:
				try
				{
					SBIOManager.CheckRequiredPlugins(SB.BlockPlugins.Select((IBlockPlugin b) => b.Name), this.vm.Config);
				}
				catch (Exception ex)
				{
					SB.Logger.LogError(Components.Runner, ex.Message, true, 0);
					break;
				}
				this.SetupSoundPlayers();
				ThreadPool.SetMinThreads(this.vm.BotsAmount * 2 + 1, this.vm.BotsAmount * 2 + 1);
				ServicePointManager.DefaultConnectionLimit = 10000;
				this.startButtonLabel.Text = "STOP";
				this.startButtonIcon.Kind = PackIconMaterialKind.StopCircleOutline;
				if (SB.Settings.RLSettings.General.DisableBotsListView)
				{
					this.botsListView.ItemsSource = null;
				}
				else
				{
					this.botsListView.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
					{
						Source = this.vm.Bots
					});
				}
				this.vm.Start();
				return;
			case WorkerStatus.Running:
				this.vm.Stop();
				this.startButtonLabel.Text = "HARD ABORT";
				this.startButtonIcon.Kind = PackIconMaterialKind.StopCircleOutline;
				return;
			case WorkerStatus.Stopping:
				this.vm.ForceStop();
				this.startButtonLabel.Text = "START";
				this.startButtonIcon.Kind = PackIconMaterialKind.PlayCircleOutline;
				this.SaveRecord();
				break;
			default:
				return;
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001BF24 File Offset: 0x0001A124
		private void SetupSoundPlayers()
		{
			string hitSound = "Sounds/" + SB.SBSettings.Sounds.OnHitSound;
			string reloadSound = "Sounds/" + SB.SBSettings.Sounds.OnReloadSound;
			if (File.Exists(hitSound))
			{
				this.hitPlayer = new SoundPlayer(hitSound);
			}
			if (File.Exists(reloadSound))
			{
				this.reloadPlayer = new SoundPlayer(reloadSound);
			}
			SB.Logger.LogInfo(Components.Runner, "Set up sound players", false, 0);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001BF9F File Offset: 0x0001A19F
		public void SetConfig(Config config)
		{
			this.vm.SetConfig(config, SB.SBSettings.General.RecommendedBots);
			this.RetrieveRecord();
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001BFC2 File Offset: 0x0001A1C2
		public void SetWordlist(Wordlist wordlist)
		{
			this.vm.SetWordlist(wordlist);
			this.RetrieveRecord();
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001BFD6 File Offset: 0x0001A1D6
		private void selectConfigButton_Click(object sender, RoutedEventArgs e)
		{
			new MainDialog(new DialogSelectConfig(this), "Select Config").ShowDialog();
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001BFEE File Offset: 0x0001A1EE
		private void selectWordlistButton_Click(object sender, RoutedEventArgs e)
		{
			new MainDialog(new DialogSelectWordlist(this), "Select Wordlist").ShowDialog();
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00003B20 File Offset: 0x00001D20
		private void selectpProxylistButton_Click(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001C006 File Offset: 0x0001A206
		private void hitsFilterButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.ResultsFilter = BotStatus.SUCCESS;
			SB.Logger.LogInfo(Components.Runner, string.Format("Changed valid filter to {0}", this.vm.ResultsFilter), false, 0);
			this.RefreshListView();
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001C041 File Offset: 0x0001A241
		private void customFilterButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.ResultsFilter = BotStatus.CUSTOM;
			SB.Logger.LogInfo(Components.Runner, string.Format("Changed valid filter to {0}", this.vm.ResultsFilter), false, 0);
			this.RefreshListView();
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001C07C File Offset: 0x0001A27C
		private void toCheckFilterButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.ResultsFilter = BotStatus.NONE;
			SB.Logger.LogInfo(Components.Runner, string.Format("Changed valid filter to {0}", this.vm.ResultsFilter), false, 0);
			this.RefreshListView();
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001C0B8 File Offset: 0x0001A2B8
		private void RefreshListView()
		{
			this.validListView.ItemsSource = this.vm.EmptyList;
			BotStatus resultsFilter = this.vm.ResultsFilter;
			if (resultsFilter == BotStatus.NONE)
			{
				this.validListView.ItemsSource = this.vm.ToCheckList;
				return;
			}
			if (resultsFilter == BotStatus.SUCCESS)
			{
				this.validListView.ItemsSource = this.vm.HitsList;
				return;
			}
			if (resultsFilter != BotStatus.CUSTOM)
			{
				return;
			}
			this.validListView.ItemsSource = this.vm.CustomList;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001C137 File Offset: 0x0001A337
		private ListView GetCurrentListView()
		{
			return this.validListView;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001C13F File Offset: 0x0001A33F
		private void showManagerButton_Click(object sender, RoutedEventArgs e)
		{
			SB.MainWindow.ShowRunnerManager();
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00003B20 File Offset: 0x00001D20
		private void ListViewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001C14C File Offset: 0x0001A34C
		private void showHTML_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				File.WriteAllText("source.html", ((ValidData)this.GetCurrentListView().SelectedItem).Source);
				Process.Start("source.html");
				SB.Logger.LogInfo(Components.Runner, "Saved the html to source.html and opened it with the default viewer", false, 0);
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.Runner, "Couldn't show the HTML - " + ex.Message, true, 0);
			}
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0001C1C8 File Offset: 0x0001A3C8
		private void showLog_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				new MainDialog(new DialogShowLog(((ValidData)this.GetCurrentListView().SelectedItem).Log), "Complete Log").Show();
				SB.Logger.LogInfo(Components.Runner, "Opened the log for the hit " + ((ValidData)this.GetCurrentListView().SelectedItem).Data, false, 0);
			}
			catch
			{
				MessageBox.Show("FAILED");
			}
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001C24C File Offset: 0x0001A44C
		private void copySelectedData_Click(object sender, RoutedEventArgs e)
		{
			string clipboardText = "";
			try
			{
				foreach (object obj in this.GetCurrentListView().SelectedItems)
				{
					ValidData selected = (ValidData)obj;
					clipboardText = clipboardText + selected.Data + Environment.NewLine;
				}
				SB.Logger.LogInfo(Components.Runner, string.Format("Copied {0} data", this.GetCurrentListView().SelectedItems.Count), false, 0);
				Clipboard.SetText(clipboardText);
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.Runner, "Exception while copying data - " + ex.Message, false, 0);
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001C320 File Offset: 0x0001A520
		private void copySelectedCaptureOnly_Click(object sender, RoutedEventArgs e)
		{
			string clipboardText = "";
			try
			{
				foreach (object obj in this.GetCurrentListView().SelectedItems)
				{
					ValidData selected = (ValidData)obj;
					clipboardText = clipboardText + selected.CapturedData + Environment.NewLine;
				}
				SB.Logger.LogInfo(Components.Runner, string.Format("Copied {0} data", this.GetCurrentListView().SelectedItems.Count), false, 0);
				Clipboard.SetText(clipboardText);
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.Runner, "Exception while copying data - " + ex.Message, false, 0);
			}
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001C3F4 File Offset: 0x0001A5F4
		private void copySelectedCapture_Click(object sender, RoutedEventArgs e)
		{
			string clipboardText = "";
			try
			{
				foreach (object obj in this.GetCurrentListView().SelectedItems)
				{
					ValidData selected = (ValidData)obj;
					clipboardText = string.Concat(new string[]
					{
						clipboardText,
						selected.Data,
						" | ",
						selected.CapturedData,
						Environment.NewLine
					});
				}
				SB.Logger.LogInfo(Components.Runner, string.Format("Copied {0} data", this.GetCurrentListView().SelectedItems.Count), false, 0);
				Clipboard.SetText(clipboardText);
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.Runner, "Exception while copying data - " + ex.Message, false, 0);
			}
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001C4E8 File Offset: 0x0001A6E8
		private void selectAll_Click(object sender, RoutedEventArgs e)
		{
			this.GetCurrentListView().SelectAll();
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001C4F8 File Offset: 0x0001A6F8
		private void copySelectedProxy_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Clipboard.SetText(((ValidData)this.GetCurrentListView().SelectedItem).Proxy);
				SB.Logger.LogInfo(Components.Runner, "Copied the proxy " + ((ValidData)this.GetCurrentListView().SelectedItem).Proxy, false, 0);
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.Runner, "Couldn't copy the proxy for the selected hit - " + ex.Message, false, 0);
			}
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001C580 File Offset: 0x0001A780
		private void sendToDebugger_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				StackerViewModel stacker = SB.Stacker;
				ValidData current = this.GetCurrentListView().SelectedItem as ValidData;
				stacker.TestData = current.Data;
				stacker.TestProxy = current.Proxy;
				stacker.ProxyType = current.ProxyType;
				SB.Logger.LogInfo(Components.Runner, "Sent to the debugger", false, 0);
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.Runner, "Could not send data and proxy to the debugger - " + ex.Message, false, 0);
			}
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001C60C File Offset: 0x0001A80C
		private void SaveRecord()
		{
			SB.RunnerManager.SaveRecord(this.vm.Config, this.vm.Wordlist, this.vm.TestedCount + this.vm.StartingPoint);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001C645 File Offset: 0x0001A845
		private void RetrieveRecord()
		{
			this.vm.StartingPoint = SB.RunnerManager.RetrieveRecord(this.vm.Config, this.vm.Wordlist);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0001C674 File Offset: 0x0001A874
		public void LabelCustom_MouseEnter(object sender, MouseEventArgs e)
		{
			try
			{
				if (this.vm.CustomCount != 0)
				{
					Label label = e.OriginalSource as Label;
					string toolTip = string.Empty;
					var customItems = from v in this.vm.CustomList
						select v.Type into cType
						group cType by cType into g
						let customCount = g.Count<string>()
						orderby customCount descending
						select new
						{
							Count = customCount,
							Name = g.Key
						};
					if (customItems != null && customItems.Count() > 0)
					{
						customItems.ToList().ForEach(delegate(ct)
						{
							toolTip += string.Format("{0}:{1}\n", ct.Name, ct.Count);
						});
						toolTip = toolTip.TrimEnd(new char[] { '\n' });
						label.ToolTip = new ToolTip
						{
							Content = toolTip
						};
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001C7E8 File Offset: 0x0001A9E8
		public void LabelCustom_MouseLeave(object sender, MouseEventArgs e)
		{
			try
			{
				(e.OriginalSource as Label).ToolTip = null;
			}
			catch
			{
			}
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0001C81C File Offset: 0x0001AA1C
		private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			try
			{
				Regex regex = new Regex("[^0-9]+");
				e.Handled = regex.IsMatch(e.Text);
				if (!e.Handled)
				{
					TextBox textBox = (TextBox)sender;
					string value = textBox.Text;
					if (textBox.SelectedText != string.Empty)
					{
						value = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectedText.Length);
					}
					int botsAmount = int.Parse(value + e.Text);
					e.Handled = (double)botsAmount > this.botsSlider.Maximum || (double)botsAmount <= this.botsSlider.Minimum - 1.0;
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001CCBC File Offset: 0x0001AEBC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 21)
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = UIElement.MouseRightButtonDownEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.ListViewItem_MouseRightButtonDown);
				((Style)target).Setters.Add(eventSetter);
			}
		}

		// Token: 0x0400045E RID: 1118
		private RunnerViewModel vm;

		// Token: 0x0400045F RID: 1119
		private SoundPlayer hitPlayer;

		// Token: 0x04000460 RID: 1120
		private SoundPlayer reloadPlayer;

		// Token: 0x04000461 RID: 1121
		private bool soundLock;
	}
}
