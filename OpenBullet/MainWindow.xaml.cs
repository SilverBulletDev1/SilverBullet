using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using LiteDB;
using MahApps.Metro.IconPacks;
using MaterialDesignThemes.Wpf;
using OpenBullet.Plugins;
using OpenBullet.ViewModels;
using OpenBullet.Views.Main;
using OpenBullet.Views.Main.Runner;
using OpenBullet.Views.Main.Settings;
using OpenBullet.Views.StackerBlocks;
using OpenBullet.Views.UserControls;
using PluginFramework;
using RuriLib;
using RuriLib.LS;
using RuriLib.Models;
using RuriLib.ViewModels;

namespace OpenBullet
{
	// Token: 0x0200004F RID: 79
	public partial class MainWindow : Window
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x000081AE File Offset: 0x000063AE
		// (set) Token: 0x060001CA RID: 458 RVA: 0x000081B6 File Offset: 0x000063B6
		public RunnerManager RunnerManagerPage { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001CB RID: 459 RVA: 0x000081BF File Offset: 0x000063BF
		// (set) Token: 0x060001CC RID: 460 RVA: 0x000081C7 File Offset: 0x000063C7
		public Runner CurrentRunnerPage { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001CD RID: 461 RVA: 0x000081D0 File Offset: 0x000063D0
		// (set) Token: 0x060001CE RID: 462 RVA: 0x000081D8 File Offset: 0x000063D8
		public ProxyManager ProxyManagerPage { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001CF RID: 463 RVA: 0x000081E1 File Offset: 0x000063E1
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x000081E9 File Offset: 0x000063E9
		public WordlistManager WordlistManagerPage { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x000081F2 File Offset: 0x000063F2
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x000081FA File Offset: 0x000063FA
		public ConfigsSection ConfigsPage { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00008203 File Offset: 0x00006403
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x0000820B File Offset: 0x0000640B
		public HitsDB HitsDBPage { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00008214 File Offset: 0x00006414
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x0000821C File Offset: 0x0000641C
		public Settings OBSettingsPage { get; set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00008225 File Offset: 0x00006425
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x0000822D File Offset: 0x0000642D
		public ToolsSection ToolsPage { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00008236 File Offset: 0x00006436
		// (set) Token: 0x060001DA RID: 474 RVA: 0x0000823E File Offset: 0x0000643E
		public PluginsSection PluginsPage { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00008247 File Offset: 0x00006447
		// (set) Token: 0x060001DC RID: 476 RVA: 0x0000824F File Offset: 0x0000644F
		public Help AboutPage { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00008258 File Offset: 0x00006458
		// (set) Token: 0x060001DE RID: 478 RVA: 0x00008260 File Offset: 0x00006460
		public Rectangle Bounds { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001DF RID: 479 RVA: 0x00008269 File Offset: 0x00006469
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x00008271 File Offset: 0x00006471
		public SilverZone SilverZonePage { get; private set; }

		// Token: 0x060001E1 RID: 481 RVA: 0x0000827C File Offset: 0x0000647C
		public MainWindow()
		{
			SB.MainWindow = this;
			File.WriteAllText(SB.logFile, "");
			this.InitializeComponent();
			string title = "SilverBullet 1.1.4";
			base.Title = title;
			this.titleLabel.Content = title;
			try
			{
				Task.Run(delegate
				{
					LatestRelease update = CheckUpdate.Run<LatestRelease>("https://api.github.com/repos/mohamm4dx/SilverBullet/releases/latest");
					base.Dispatcher.Invoke<Visibility>(() => this.updateButton.Visibility = (update.Available ? Visibility.Visible : Visibility.Collapsed));
				});
			}
			catch
			{
			}
			foreach (string folder in new string[]
			{
				"Captchas", "ChromeExtensions", "Configs", "DB", "Hits", "Plugins", "Screenshots", "Settings", "Sounds", "Wordlists",
				"Js", "Compiled"
			}.Select((string f) => Path.Combine(Directory.GetCurrentDirectory(), f)))
			{
				if (!Directory.Exists(folder))
				{
					Directory.CreateDirectory(folder);
				}
			}
			try
			{
				SB.Settings.Environment = IOManager.ParseEnvironmentSettings(SB.envFile);
			}
			catch
			{
				SB.Logger.LogError(Components.Main, "Could not find / parse the Environment Settings file. Please fix the issue and try again.", true, 0);
				Environment.Exit(0);
			}
			if (SB.Settings.Environment.WordlistTypes.Count == 0 || SB.Settings.Environment.CustomKeychains.Count == 0)
			{
				SB.Logger.LogError(Components.Main, "At least one WordlistType and one CustomKeychain must be defined in the Environment Settings file.", true, 0);
				Environment.Exit(0);
			}
			SB.Settings.RLSettings = new RLSettingsViewModel();
			SB.Settings.ProxyManagerSettings = new ProxyManagerSettings();
			SB.SBSettings = new SBSettingsViewModel();
			if (!File.Exists(SB.rlSettingsFile))
			{
				MessageBox.Show("RuriLib Settings file not found, generating a default one");
				SB.Logger.LogWarning(Components.Main, "RuriLib Settings file not found, generating a default one", false, 0);
				IOManager.SaveSettings<RLSettingsViewModel>(SB.rlSettingsFile, SB.Settings.RLSettings);
				SB.Logger.LogInfo(Components.Main, "Created the default RuriLib Settings file " + SB.rlSettingsFile, false, 0);
			}
			else
			{
				SB.Settings.RLSettings = IOManager.LoadSettings<RLSettingsViewModel>(SB.rlSettingsFile);
				SB.Logger.LogInfo(Components.Main, "Loaded the existing RuriLib Settings file", false, 0);
			}
			if (!File.Exists(SB.proxyManagerSettingsFile))
			{
				SB.Logger.LogWarning(Components.Main, "Proxy manager Settings file not found, generating a default one", false, 0);
				SB.Settings.ProxyManagerSettings.ProxySiteUrls.Add(SB.defaultProxySiteUrl);
				SB.Settings.ProxyManagerSettings.ActiveProxySiteUrl = SB.defaultProxySiteUrl;
				SB.Settings.ProxyManagerSettings.ProxyKeys.Add(SB.defaultProxyKey);
				SB.Settings.ProxyManagerSettings.ActiveProxyKey = SB.defaultProxyKey;
				IOManager.SaveSettings<ProxyManagerSettings>(SB.proxyManagerSettingsFile, SB.Settings.ProxyManagerSettings);
				SB.Logger.LogInfo(Components.Main, "Created the default proxy manager Settings file " + SB.proxyManagerSettingsFile, false, 0);
			}
			else
			{
				SB.Settings.ProxyManagerSettings = IOManager.LoadSettings<ProxyManagerSettings>(SB.proxyManagerSettingsFile);
				SB.Logger.LogInfo(Components.Main, "Loaded the existing proxy manager Settings file", false, 0);
			}
			if (!File.Exists(SB.obSettingsFile))
			{
				MessageBox.Show("SilverBullet Settings file not found, generating a default one");
				SB.Logger.LogWarning(Components.Main, "SilverBullet Settings file not found, generating a default one", false, 0);
				SBIOManager.SaveSettings(SB.obSettingsFile, SB.SBSettings);
				SB.Logger.LogInfo(Components.Main, "Created the default SilverBullet Settings file " + SB.obSettingsFile, false, 0);
			}
			else
			{
				SB.SBSettings = SBIOManager.LoadSettings(SB.obSettingsFile);
				SB.Logger.LogInfo(Components.Main, "Loaded the existing SilverBullet Settings file", false, 0);
			}
			try
			{
				if (SB.SBSettings.General.BackupDB && (!File.Exists(SB.dataBaseBackupFile) || (File.Exists(SB.dataBaseBackupFile) && (DateTime.Now - File.GetCreationTime(SB.dataBaseBackupFile)).TotalDays > 1.0)))
				{
					using (LiteDatabase db = new LiteDatabase(SB.dataBaseFile, null))
					{
						db.GetCollection<CProxy>("proxies", BsonAutoId.ObjectId);
					}
					File.Delete(SB.dataBaseBackupFile);
					File.Copy(SB.dataBaseFile, SB.dataBaseBackupFile);
					SB.Logger.LogInfo(Components.Main, "Backed up the DB", false, 0);
				}
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.Main, "Could not backup the DB: " + ex.Message, false, 0);
			}
			base.Topmost = SB.SBSettings.General.AlwaysOnTop;
			ValueTuple<IEnumerable<PluginControl>, IEnumerable<IBlockPlugin>, IEnumerable<string>> plugins;
			try
			{
				plugins = Loader.LoadPlugins(SB.pluginsFolder);
			}
			catch (Exception ex2)
			{
				MessageBox.Show("Error in load plugins\n" + ex2.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
				Environment.Exit(0);
				throw;
			}
			SB.BlockPlugins = plugins.Item2.ToList<IBlockPlugin>();
			SB.PluginNames = plugins.Item3;
			SB.BlockMappings = new List<ValueTuple<Type, Type, LinearGradientBrush>>
			{
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(BlockBypassCF), typeof(PageBlockBypassCF), Colors.DarkSalmon.GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(BlockImageCaptcha), typeof(PageBlockCaptcha), Colors.DarkOrange.GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(BlockReportCaptcha), typeof(PageBlockReportCaptcha), Colors.DarkOrange.GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(BlockFunction), typeof(PageBlockFunction), Colors.YellowGreen.GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(BlockKeycheck), typeof(PageBlockKeycheck), Colors.DodgerBlue.GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(BlockLSCode), typeof(PageBlockLSCode), Colors.White.GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(BlockParse), typeof(PageBlockParse), Colors.Gold.GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(BlockRecaptcha), typeof(PageBlockRecaptcha), Colors.Turquoise.GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(BlockSolveCaptcha), typeof(PageBlockSolveCaptcha), Colors.Turquoise.GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(BlockRequest), typeof(PageBlockRequest), Colors.LimeGreen.GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(BlockTCP), typeof(PageBlockTCP), Colors.MediumPurple.GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(BlockOcr), typeof(PageBlockOcr), global::System.Windows.Media.Color.FromRgb(230, 230, 230).GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(BlockWebSocket), typeof(PageBlockWebSocket), global::System.Windows.Media.Color.FromRgb(245, 180, 0).GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(BlockSpeechToText), typeof(PageBlockSpeechToText), global::System.Windows.Media.Color.FromRgb(164, 198, 197).GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(BlockUtility), typeof(PageBlockUtility), Colors.Wheat.GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(SBlockBrowserAction), typeof(PageSBlockBrowserAction), Colors.Green.GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(SBlockElementAction), typeof(PageSBlockElementAction), Colors.Firebrick.GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(SBlockExecuteJS), typeof(PageSBlockExecuteJS), global::System.Windows.Media.Color.FromRgb(60, 193, 226).GetLinearGradientBrush()),
				new ValueTuple<Type, Type, LinearGradientBrush>(typeof(SBlockNavigate), typeof(PageSBlockNavigate), Colors.RoyalBlue.GetLinearGradientBrush())
			};
			foreach (IBlockPlugin plugin in plugins.Item2)
			{
				try
				{
					SB.BlockMappings.Add(new ValueTuple<Type, Type, LinearGradientBrush>(plugin.GetType(), typeof(BlockPluginPage), plugin.LinearGradientBrush));
					BlockParser.BlockMappings.Add(plugin.Name, plugin.GetType());
					SB.Logger.LogInfo(Components.Main, "Initialized " + plugin.Name + " block plugin", false, 0);
				}
				catch (Exception ex3)
				{
					SB.Logger.LogError(Components.Main, ex3.Message + "\n" + plugin.Name + ".dll", true, 0);
					Environment.Exit(0);
				}
			}
			SB.RunnerManager = new RunnerManagerViewModel();
			SB.ProxyManager = new ProxyManagerViewModel();
			SB.WordlistManager = new WordlistManagerViewModel();
			SB.ConfigManager = new ConfigManagerViewModel();
			SB.HitsDB = new HitsDBViewModel();
			this.RunnerManagerPage = new RunnerManager();
			if (SB.SBSettings.General.AutoCreateRunner & !SB.RunnerManager.RestoreSession())
			{
				SB.RunnerManager.Create();
				this.CurrentRunnerPage = SB.RunnerManager.RunnersCollection.FirstOrDefault<RunnerInstance>().View;
			}
			SB.Logger.LogInfo(Components.Main, "Initialized RunnerManager", false, 0);
			this.ProxyManagerPage = new ProxyManager();
			SB.Logger.LogInfo(Components.Main, "Initialized ProxyManager", false, 0);
			this.WordlistManagerPage = new WordlistManager();
			SB.Logger.LogInfo(Components.Main, "Initialized WordlistManager", false, 0);
			this.ConfigsPage = new ConfigsSection();
			SB.Logger.LogInfo(Components.Main, "Initialized ConfigManager", false, 0);
			this.HitsDBPage = new HitsDB();
			SB.Logger.LogInfo(Components.Main, "Initialized HitsDB", false, 0);
			this.OBSettingsPage = new Settings();
			SB.Logger.LogInfo(Components.Main, "Initialized Settings", false, 0);
			this.ToolsPage = new ToolsSection();
			SB.Logger.LogInfo(Components.Main, "Initialized Tools", false, 0);
			this.PluginsPage = new PluginsSection(plugins.Item1);
			SB.Logger.LogInfo(Components.Main, "Initialized Plugins", false, 0);
			this.AboutPage = new Help();
			this.SilverZonePage = new SilverZone(null);
			this.menuOptionRunner_Click(this, null);
			int width = SB.SBSettings.General.StartingWidth;
			int height = SB.SBSettings.General.StartingHeight;
			if (width > 800)
			{
				base.Width = (double)width;
			}
			if (height > 600)
			{
				base.Height = (double)height;
			}
			base.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			if (SB.SBSettings.Themes.EnableSnow)
			{
				base.Loaded += this.MainWindow_Loaded;
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00008DB4 File Offset: 0x00006FB4
		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			DispatcherTimer dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Interval = TimeSpan.FromMilliseconds((double)(10000 / SB.SBSettings.Themes.SnowAmount));
			dispatcherTimer.Tick += delegate(object s, EventArgs ea)
			{
				this.Snow();
			};
			dispatcherTimer.Start();
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00008DF4 File Offset: 0x00006FF4
		private void Snow()
		{
			if (this.snowBuffer >= 100)
			{
				for (int i = 0; i < this.Root.Children.Count; i++)
				{
					if (this.Root.Children[i].GetType() == typeof(Snowflake))
					{
						this.Root.Children.RemoveAt(i);
						break;
					}
				}
			}
			int x = this.rand.Next(-500, (int)this.Root.ActualWidth - 100);
			int y = -100;
			int s = this.rand.Next(5, 15);
			Snowflake flake = new Snowflake
			{
				Width = (double)s,
				Height = (double)s,
				RenderTransform = new TranslateTransform
				{
					X = (double)x,
					Y = (double)y
				},
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				IsHitTestVisible = false
			};
			Grid.SetColumn(flake, 1);
			Grid.SetRow(flake, 2);
			this.Root.Children.Add(flake);
			TimeSpan d = TimeSpan.FromSeconds((double)this.rand.Next(1, 4));
			x += this.rand.Next(100, 500);
			DoubleAnimation ax = new DoubleAnimation
			{
				To = new double?((double)x),
				Duration = d
			};
			flake.RenderTransform.BeginAnimation(TranslateTransform.XProperty, ax);
			y = (int)this.Root.ActualHeight + 200;
			DoubleAnimation ay = new DoubleAnimation
			{
				To = new double?((double)y),
				Duration = d
			};
			flake.RenderTransform.BeginAnimation(TranslateTransform.YProperty, ay);
			this.snowBuffer++;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00008FAF File Offset: 0x000071AF
		public void ShowRunnerManager()
		{
			this.CurrentRunnerPage = null;
			this.Main.Content = this.RunnerManagerPage;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00008FC9 File Offset: 0x000071C9
		public void ShowRunner(Runner page)
		{
			this.CurrentRunnerPage = page;
			this.Main.Content = page;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00008FDE File Offset: 0x000071DE
		public void menuOptionRunner_Click(object sender, RoutedEventArgs e)
		{
			if (this.CurrentRunnerPage == null)
			{
				this.Main.Content = this.RunnerManagerPage;
			}
			else
			{
				this.Main.Content = this.CurrentRunnerPage;
			}
			this.menuOptionSelected(this.menuOptionRunner);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00009018 File Offset: 0x00007218
		private void menuOptionProxyManager_Click(object sender, RoutedEventArgs e)
		{
			this.Main.Content = this.ProxyManagerPage;
			this.menuOptionSelected(this.menuOptionProxyManager);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00009037 File Offset: 0x00007237
		private void menuOptionWordlistManager_Click(object sender, RoutedEventArgs e)
		{
			this.Main.Content = this.WordlistManagerPage;
			this.menuOptionSelected(this.menuOptionWordlistManager);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00009056 File Offset: 0x00007256
		private void menuOptionConfigCreator_Click(object sender, RoutedEventArgs e)
		{
			this.Main.Content = this.ConfigsPage;
			this.menuOptionSelected(this.menuOptionConfigCreator);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00009075 File Offset: 0x00007275
		private void menuOptionHitsDatabase_Click(object sender, RoutedEventArgs e)
		{
			this.Main.Content = this.HitsDBPage;
			this.menuOptionSelected(this.menuOptionHitsDatabase);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00009094 File Offset: 0x00007294
		private void menuOptionTools_Click(object sender, RoutedEventArgs e)
		{
			this.Main.Content = this.ToolsPage;
			this.menuOptionSelected(this.menuOptionTools);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000090B3 File Offset: 0x000072B3
		private void menuOptionPlugins_Click(object sender, RoutedEventArgs e)
		{
			this.Main.Content = this.PluginsPage;
			this.menuOptionSelected(this.menuOptionPlugins);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000090D2 File Offset: 0x000072D2
		private void menuOptionSettings_Click(object sender, RoutedEventArgs e)
		{
			this.Main.Content = this.OBSettingsPage;
			this.menuOptionSelected(this.menuOptionSettings);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000090F1 File Offset: 0x000072F1
		private void menuOptionAbout_Click(object sender, RoutedEventArgs e)
		{
			this.Main.Content = this.AboutPage;
			this.menuOptionSelected(this.menuOptionAbout);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00009110 File Offset: 0x00007310
		private void menuOptioSilverZone_Click(object sender, RoutedEventArgs e)
		{
			this.Main.Content = this.SilverZonePage;
			this.menuOptionSelected(this.menuOptionSilverZone);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000912F File Offset: 0x0000732F
		private void UpdateButton_Click(object sender, RoutedEventArgs e)
		{
			Action <>9__1;
			Action <>9__2;
			Action <>9__3;
			Task.Run(delegate
			{
				Dispatcher dispatcher = this.Dispatcher;
				Action action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate
					{
						this.menuOptionAbout_Click(sender, e);
					});
				}
				dispatcher.Invoke(action, DispatcherPriority.Background);
				Dispatcher dispatcher2 = this.Dispatcher;
				Action action2;
				if ((action2 = <>9__2) == null)
				{
					action2 = (<>9__2 = delegate
					{
						this.AboutPage.checkForUpdateLabel_MouseLeftButtonUp(sender, null);
					});
				}
				dispatcher2.Invoke(action2, DispatcherPriority.Background);
				Dispatcher dispatcher3 = this.Dispatcher;
				Action action3;
				if ((action3 = <>9__3) == null)
				{
					action3 = (<>9__3 = delegate
					{
						this.AboutPage.CheckUpdatePage.CheckForUpdate();
					});
				}
				dispatcher3.Invoke(action3, DispatcherPriority.Background);
			});
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000915C File Offset: 0x0000735C
		private void menuOptionSelected(object sender)
		{
			foreach (object child in this.topMenu.Children)
			{
				try
				{
					Badged badged = child as Badged;
					Button option;
					if (badged != null)
					{
						option = badged.Content as Button;
					}
					else
					{
						option = (Button)child;
					}
					option.Foreground = Utils.GetBrush("ForegroundMain");
				}
				catch
				{
				}
			}
			((Button)sender).Foreground = Utils.GetBrush("ForegroundMenuSelected");
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00009208 File Offset: 0x00007408
		private void IconDiscord_MouseDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				string url = string.Empty;
				using (Task.Run(delegate
				{
					using (WebClient wc = new WebClient())
					{
						wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:87.0) Gecko/20100101 Firefox/87.0");
						url = wc.DownloadString("https://raw.githubusercontent.com/mohamm4dx/SilverBullet/master/OpenBullet/Discoard.txt");
					}
				}).ContinueWith(delegate(Task _)
				{
					if (string.IsNullOrWhiteSpace(url))
					{
						MessageBox.Show("not found!", "ERROR");
						return;
					}
					using (Process.Start(url))
					{
					}
				}))
				{
				}
			}
			catch (InvalidOperationException)
			{
			}
			catch (NullReferenceException)
			{
			}
			catch (Exception)
			{
				MessageBox.Show("An error occurred", "ERROR");
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x000092A4 File Offset: 0x000074A4
		private void IconTelegram_MouseDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				Process.Start("tg://resolve?domain=SilverBullet_Soft");
			}
			catch (Exception)
			{
				Process.Start("https://t.me/mohamm4dx");
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000092DC File Offset: 0x000074DC
		private void quitButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.CheckOnQuit())
			{
				try
				{
					NotepadPlusExtensions.Close();
				}
				catch
				{
				}
				Environment.Exit(0);
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00009314 File Offset: 0x00007514
		private void quitButton_MouseEnter(object sender, MouseEventArgs e)
		{
			this.quitButton.Background = new SolidColorBrush(Colors.DarkRed);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000932B File Offset: 0x0000752B
		private void quitButton_MouseLeave(object sender, MouseEventArgs e)
		{
			this.quitButton.Background = new SolidColorBrush(Colors.Transparent);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00009344 File Offset: 0x00007544
		private bool CheckOnQuit()
		{
			int active = SB.RunnerManager.RunnersCollection.Count((RunnerInstance r) => r.ViewModel.Busy);
			if (!SB.SBSettings.General.DisableQuitWarning || active > 0)
			{
				SB.Logger.LogWarning(Components.Main, "Prompting quit confirmation", false, 0);
				if (active == 0)
				{
					if (MessageBox.Show("Are you sure you want to quit?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
					{
						return false;
					}
				}
				else if (MessageBox.Show(string.Format("There are {0} active runners. Are you sure you want to quit?", active), "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
				{
					return false;
				}
			}
			if (!SB.SBSettings.General.DisableNotSavedWarning && !SB.MainWindow.ConfigsPage.ConfigManagerPage.CheckSaved())
			{
				SB.Logger.LogWarning(Components.Main, "Config not saved, prompting quit confirmation", false, 0);
				if (MessageBox.Show("The Config in Stacker wasn't saved.\nAre you sure you want to quit?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
				{
					return false;
				}
			}
			SB.Logger.LogInfo(Components.Main, "Saving RunnerManager session to the database", false, 0);
			SB.RunnerManager.SaveSession();
			SB.Logger.LogInfo(Components.Main, "Quit sequence initiated", false, 0);
			return true;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00009464 File Offset: 0x00007664
		private void maximizeButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (base.WindowState == WindowState.Normal)
				{
					WpfScreen screen = WpfScreen.GetScreenFrom(this);
					base.MaxHeight = screen.WorkingArea.Height;
					base.MaxWidth = screen.WorkingArea.Width;
					base.WindowState = WindowState.Maximized;
				}
				else
				{
					base.WindowState = WindowState.Normal;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x000094D0 File Offset: 0x000076D0
		private void maximizeButton_MouseEnter(object sender, MouseEventArgs e)
		{
			this.maximizeButton.Background = new SolidColorBrush(Colors.DimGray);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x000094E7 File Offset: 0x000076E7
		private void maximizeButton_MouseLeave(object sender, MouseEventArgs e)
		{
			this.maximizeButton.Background = new SolidColorBrush(Colors.Transparent);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x000094FE File Offset: 0x000076FE
		private void minimizeButton_Click(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00009507 File Offset: 0x00007707
		private void minimizeButton_MouseEnter(object sender, MouseEventArgs e)
		{
			this.minimizeButton.Background = new SolidColorBrush(Colors.DimGray);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000951E File Offset: 0x0000771E
		private void minimizeButton_MouseLeave(object sender, MouseEventArgs e)
		{
			this.minimizeButton.Background = new SolidColorBrush(Colors.Transparent);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00009535 File Offset: 0x00007735
		private void titleBar_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
			{
				this.maximizeButton_Click(sender, e);
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00009550 File Offset: 0x00007750
		private void dragPanel_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				this.WindowDrag(sender, e);
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00009562 File Offset: 0x00007762
		private void gripImage_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (Mouse.Capture(this.gripImage))
			{
				this._isResizing = true;
				this._startPosition = Mouse.GetPosition(this);
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00009584 File Offset: 0x00007784
		private void gripImage_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (this._isResizing)
			{
				global::System.Windows.Point currentPosition = Mouse.GetPosition(this);
				double diffX = currentPosition.X - this._startPosition.X;
				double diffY = currentPosition.Y - this._startPosition.Y;
				base.Width += diffX;
				base.Height += diffY;
				this._startPosition = currentPosition;
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x000095EB File Offset: 0x000077EB
		private void gripImage_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (this._isResizing)
			{
				this._isResizing = false;
				Mouse.Capture(null);
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00009604 File Offset: 0x00007804
		private void screenshotImage_MouseDown(object sender, MouseButtonEventArgs e)
		{
			BitmapSource bitmapSource = MainWindow.CopyScreen((int)base.Width, (int)base.Height, (int)base.Top, (int)base.Left);
			Clipboard.SetImage(bitmapSource);
			MainWindow.GetBitmap(bitmapSource).Save("screenshot.jpg", ImageFormat.Jpeg);
			SB.Logger.LogInfo(Components.Main, "Acquired screenshot", false, 0);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00009660 File Offset: 0x00007860
		private static BitmapSource CopyScreen(int width, int height, int top, int left)
		{
			BitmapSource bitmapSource;
			using (Bitmap screenBmp = new Bitmap(width, height, global::System.Drawing.Imaging.PixelFormat.Format32bppArgb))
			{
				using (Graphics bmpGraphics = Graphics.FromImage(screenBmp))
				{
					bmpGraphics.CopyFromScreen(left, top, 0, 0, screenBmp.Size);
					bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(screenBmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
				}
			}
			return bitmapSource;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x000096E0 File Offset: 0x000078E0
		private static Bitmap GetBitmap(BitmapSource source)
		{
			Bitmap bmp = new Bitmap(source.PixelWidth, source.PixelHeight, global::System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			BitmapData data = bmp.LockBits(new Rectangle(global::System.Drawing.Point.Empty, bmp.Size), ImageLockMode.WriteOnly, global::System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			source.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
			bmp.UnlockBits(data);
			return bmp;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000974D File Offset: 0x0000794D
		private void logImage_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (SB.LogWindow == null)
			{
				SB.LogWindow = new LogWindow();
				SB.LogWindow.Show();
				return;
			}
			SB.LogWindow.Show();
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00009778 File Offset: 0x00007978
		public void SetStyle()
		{
			try
			{
				SolidColorBrush brush = Utils.GetBrush("BackgroundMain");
				if (!SB.SBSettings.Themes.UseImage)
				{
					base.Background = brush;
					this.Main.Background = brush;
				}
				else
				{
					if (File.Exists(SB.SBSettings.Themes.BackgroundImage))
					{
						base.Background = new ImageBrush(new BitmapImage(new Uri(SB.SBSettings.Themes.BackgroundImage)))
						{
							Opacity = (double)SB.SBSettings.Themes.BackgroundImageOpacity / 100.0
						};
					}
					else
					{
						base.Background = brush;
					}
					if (File.Exists(SB.SBSettings.Themes.BackgroundLogo))
					{
						ImageBrush lbrush = new ImageBrush(new BitmapImage(new Uri(SB.SBSettings.Themes.BackgroundLogo)));
						lbrush.AlignmentX = AlignmentX.Center;
						lbrush.AlignmentY = AlignmentY.Center;
						lbrush.Stretch = Stretch.None;
						lbrush.Opacity = (double)SB.SBSettings.Themes.BackgroundImageOpacity / 100.0;
						this.Main.Background = lbrush;
					}
					else
					{
						this.Main.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/Images/Themes/empty.png", UriKind.Absolute)));
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x000098F4 File Offset: 0x00007AF4
		private void Window_SourceInitialized(object sender, EventArgs e)
		{
			try
			{
				this.silverZoneTimer = new Timer(delegate(object _)
				{
					try
					{
						int badge = this.SilverZonePage.GetBadge();
						base.Dispatcher.Invoke<object>(() => this.silverZoneBadged.Badge = ((badge > 99) ? "99+" : badge.ToString()));
					}
					catch
					{
					}
				}, null, TimeSpan.Zero, TimeSpan.FromMinutes(30.0));
			}
			catch
			{
			}
		}

		// Token: 0x06000209 RID: 521
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0600020A RID: 522
		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		// Token: 0x0600020B RID: 523 RVA: 0x00009944 File Offset: 0x00007B44
		private void WindowDrag(object sender, MouseButtonEventArgs e)
		{
			MainWindow.ReleaseCapture();
			MainWindow.SendMessage(new WindowInteropHelper(this).Handle, 161U, (IntPtr)2, (IntPtr)0);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000996E File Offset: 0x00007B6E
		private void ResizeWindow(MainWindow.ResizeDirection direction)
		{
			MainWindow.SendMessage(this._hwndSource.Handle, 274U, (IntPtr)((int)(61440 + direction)), IntPtr.Zero);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00009998 File Offset: 0x00007B98
		private void BuySbPro_Click(object sender, RoutedEventArgs e)
		{
			new MainDialog(new DialogSilverBulletPro(), "Buy SilverBullet Pro")
			{
				ResizeMode = ResizeMode.CanResizeWithGrip,
				SizeToContent = SizeToContent.Manual,
				Width = 700.0,
				Height = 450.0
			}.ShowDialog();
		}

		// Token: 0x0400018B RID: 395
		private Random rand = new Random();

		// Token: 0x0400018C RID: 396
		private int snowBuffer;

		// Token: 0x04000199 RID: 409
		private global::System.Windows.Point _startPosition;

		// Token: 0x0400019A RID: 410
		private bool _isResizing;

		// Token: 0x0400019B RID: 411
		private bool _canQuit;

		// Token: 0x0400019C RID: 412
		private Timer silverZoneTimer;

		// Token: 0x0400019D RID: 413
		private HwndSource _hwndSource;

		// Token: 0x02000050 RID: 80
		private enum ResizeDirection
		{
			// Token: 0x040001B9 RID: 441
			Left = 1,
			// Token: 0x040001BA RID: 442
			Right,
			// Token: 0x040001BB RID: 443
			Top,
			// Token: 0x040001BC RID: 444
			TopLeft,
			// Token: 0x040001BD RID: 445
			TopRight,
			// Token: 0x040001BE RID: 446
			Bottom,
			// Token: 0x040001BF RID: 447
			BottomLeft,
			// Token: 0x040001C0 RID: 448
			BottomRight
		}
	}
}
