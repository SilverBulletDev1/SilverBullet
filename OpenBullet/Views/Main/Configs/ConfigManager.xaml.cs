using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using OpenBullet.ViewModels;
using PluginFramework;
using RuriLib;
using RuriLib.ViewModels;

namespace OpenBullet.Views.Main.Configs
{
	// Token: 0x020000FA RID: 250
	public partial class ConfigManager : Page
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x00024A3F File Offset: 0x00022C3F
		private IEnumerable<ConfigViewModel> Selected
		{
			get
			{
				return this.configsListView.SelectedItems.Cast<ConfigViewModel>();
			}
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00024A51 File Offset: 0x00022C51
		public void OnSaveConfig(object sender, EventArgs e)
		{
			this.saveConfigButton_Click(this, new RoutedEventArgs());
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00024A5F File Offset: 0x00022C5F
		public ConfigManager()
		{
			this.vm = SB.ConfigManager;
			base.DataContext = this.vm;
			this.InitializeComponent();
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00024A84 File Offset: 0x00022C84
		public bool CheckSaved()
		{
			Stacker stacker = SB.MainWindow.ConfigsPage.StackerPage;
			if (this.vm.CurrentConfig == null || SB.SBSettings.General.DisableNotSavedWarning || stacker == null)
			{
				return true;
			}
			stacker.SetScript();
			ConfigViewModel cvm = SB.Stacker.Config;
			return cvm == null || this.vm.SavedHash == cvm.Config.Script.GetHashCode();
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00024AF8 File Offset: 0x00022CF8
		public void SaveState()
		{
			ConfigViewModel cvm = SB.Stacker.Config;
			if (cvm != null)
			{
				this.vm.SavedHash = cvm.Config.Script.GetHashCode();
			}
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00024B30 File Offset: 0x00022D30
		private void loadConfigButton_Click(object sender, RoutedEventArgs e)
		{
			if (!this.CheckSaved())
			{
				SB.Logger.LogWarning(Components.Stacker, "Config not saved, prompting quit confirmation", false, 0);
				if (MessageBox.Show("The Config in Stacker wasn't saved.\nAre you sure you want to load another config?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
				{
					return;
				}
			}
			this.LoadConfig(this.configsListView.SelectedItem as ConfigViewModel);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00024B83 File Offset: 0x00022D83
		private void saveConfigButton_Click(object sender, RoutedEventArgs e)
		{
			this.SaveConfig();
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00024B8C File Offset: 0x00022D8C
		private void deleteConfigsButton_Click(object sender, RoutedEventArgs e)
		{
			SB.Logger.LogWarning(Components.ConfigManager, "Deletion initiated, prompting warning", false, 0);
			if (MessageBox.Show("This will delete the physical files from your disk! Are you sure you want to continue?", "WARNING", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
			{
				this.vm.Remove(this.Selected);
				SB.Logger.LogInfo(Components.ConfigManager, "Deletion completed", false, 0);
				return;
			}
			SB.Logger.LogInfo(Components.ConfigManager, "Deletion cancelled", false, 0);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00024BF6 File Offset: 0x00022DF6
		private void rescanConfigsButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.Rescan();
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00024C04 File Offset: 0x00022E04
		private void newConfigButton_Click(object sender, RoutedEventArgs e)
		{
			if (!this.CheckSaved())
			{
				SB.Logger.LogWarning(Components.Stacker, "Config not saved, prompting quit confirmation", false, 0);
				if (MessageBox.Show("The Config in Stacker wasn't saved.\r\nAre you sure you want to create a new config?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
				{
					return;
				}
			}
			new MainDialog(new DialogNewConfig(this), "New Config").ShowDialog();
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00024C57 File Offset: 0x00022E57
		private void searchButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.SearchString = this.filterTextbox.Text;
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00024C6F File Offset: 0x00022E6F
		private void filterTextbox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				this.searchButton_Click(this, null);
			}
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00024C84 File Offset: 0x00022E84
		private void openConfigFolderButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.Start(Path.Combine(Directory.GetCurrentDirectory(), SB.configFolder));
			}
			catch
			{
				SB.Logger.LogError(Components.ConfigManager, "No config folder found!", true, 0);
			}
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00024CD0 File Offset: 0x00022ED0
		public void SaveConfig()
		{
			if (this.vm.CurrentConfig == null || SB.MainWindow.ConfigsPage.StackerPage == null || SB.MainWindow.ConfigsPage.OtherOptionsPage == null)
			{
				SB.Logger.LogError(Components.ConfigManager, "No config eligible for saving!", true, 0);
				return;
			}
			if (this.vm.CurrentConfig.Remote)
			{
				SB.Logger.LogError(Components.ConfigManager, "The config was pulled from a remote source and cannot be saved!", true, 0);
				return;
			}
			if (this.vm.CurrentConfigName == string.Empty)
			{
				SB.Logger.LogError(Components.ConfigManager, "Empty config name, cannot save", true, 0);
				return;
			}
			StackerViewModel stacker = SB.Stacker;
			stacker.ConvertKeychains();
			stacker.ConvertPlugins();
			if (stacker.View == StackerView.Blocks)
			{
				stacker.LS.FromBlocks(stacker.GetList());
			}
			this.vm.CurrentConfig.Config.Script = stacker.LS.Script;
			SB.Logger.LogInfo(Components.ConfigManager, "Saving config " + this.vm.CurrentConfigName, false, 0);
			this.vm.CurrentConfig.Config.Settings.LastModified = DateTime.Now;
			this.vm.CurrentConfig.Config.Settings.Version = "1.1.4 [SB]";
			this.vm.CurrentConfig.Config.Settings.RequiredPlugins = this.vm.GetRequiredPlugins(this.vm.CurrentConfig).ToArray<string>();
			SB.Logger.LogInfo(Components.ConfigManager, "Converted the unbinded observables and set the Last Modified date", false, 0);
			try
			{
				this.vm.SaveCurrent();
				this.SaveState();
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.ConfigManager, "Failed to save the config. Reason: " + ex.Message, true, 0);
			}
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00024EA4 File Offset: 0x000230A4
		public void LoadConfig(ConfigViewModel config)
		{
			if (config == null)
			{
				SB.Logger.LogError(Components.ConfigManager, "The config to load cannot be null", true, 0);
				return;
			}
			try
			{
				SBIOManager.CheckRequiredPlugins(SB.BlockPlugins.Select((IBlockPlugin b) => b.Name), config.Config);
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.ConfigManager, ex.Message, true, 0);
				return;
			}
			this.vm.CurrentConfig = config;
			if (this.vm.CurrentConfig.Remote)
			{
				SB.Logger.LogError(Components.ConfigManager, "The config was pulled from a remote source and cannot be edited!", true, 0);
				this.vm.CurrentConfig = null;
				return;
			}
			SB.Logger.LogInfo(Components.ConfigManager, "Loading config: " + this.vm.CurrentConfig.Name, false, 0);
			SB.MainWindow.ConfigsPage.menuOptionStacker.IsEnabled = true;
			SB.MainWindow.ConfigsPage.menuOptionOtherOptions.IsEnabled = true;
			StackerViewModel newStackerVM = new StackerViewModel(this.vm.CurrentConfig);
			if (SB.MainWindow.ConfigsPage.StackerPage != null)
			{
				newStackerVM.TestData = SB.Stacker.TestData;
				newStackerVM.TestProxy = SB.Stacker.TestProxy;
				newStackerVM.ProxyType = SB.Stacker.ProxyType;
			}
			SB.Stacker = newStackerVM;
			SB.MainWindow.ConfigsPage.StackerPage = new Stacker();
			SB.Logger.LogInfo(Components.ConfigManager, "Created and assigned a new Stacker instance", false, 0);
			SB.MainWindow.ConfigsPage.OtherOptionsPage = new ConfigOtherOptions();
			SB.Logger.LogInfo(Components.ConfigManager, "Created and assigned a new Other Options instance", false, 0);
			SB.MainWindow.ConfigsPage.ConfigOcrSettings = new ConfigOcrSettings(false);
			SB.Logger.LogInfo(Components.ConfigManager, "Created and assigned a new Ocr Testing instance", false, 0);
			SB.MainWindow.ConfigsPage.menuOptionStacker_MouseDown(this, null);
			SB.MainWindow.ConfigsPage.StackerPage.SetScript();
			this.SaveState();
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x000250AC File Offset: 0x000232AC
		public void CreateConfig(string name, string category, string author)
		{
			ConfigSettings settings = new ConfigSettings();
			settings.Name = name;
			settings.Author = author;
			ConfigViewModel newConfig = new ConfigViewModel(string.Empty, name, category, new Config(settings, string.Empty), false);
			this.vm.Add(newConfig);
			this.vm.CurrentConfig = newConfig;
			StackerViewModel newStackerVM = new StackerViewModel(this.vm.CurrentConfig);
			if (SB.MainWindow.ConfigsPage.StackerPage != null)
			{
				newStackerVM.TestData = SB.Stacker.TestData;
				newStackerVM.TestProxy = SB.Stacker.TestProxy;
				newStackerVM.ProxyType = SB.Stacker.ProxyType;
			}
			SB.Stacker = newStackerVM;
			SB.MainWindow.ConfigsPage.StackerPage = new Stacker();
			SB.Logger.LogInfo(Components.ConfigManager, "Created and assigned a new Stacker instance", false, 0);
			SB.MainWindow.ConfigsPage.OtherOptionsPage = new ConfigOtherOptions();
			SB.Logger.LogInfo(Components.ConfigManager, "Created and assigned a new Other Options instance", false, 0);
			SB.MainWindow.ConfigsPage.menuOptionStacker_MouseDown(this, null);
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x000251B4 File Offset: 0x000233B4
		private void configsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				this.vm.HoveredConfig = ((ConfigViewModel)this.configsListView.SelectedItem).Config;
			}
			catch
			{
			}
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x000251F8 File Offset: 0x000233F8
		private void listViewColumnHeader_Click(object sender, RoutedEventArgs e)
		{
			GridViewColumnHeader column = sender as GridViewColumnHeader;
			string sortBy = column.Tag.ToString();
			if (this.listViewSortCol != null)
			{
				AdornerLayer.GetAdornerLayer(this.listViewSortCol).Remove(this.listViewSortAdorner);
				this.configsListView.Items.SortDescriptions.Clear();
			}
			ListSortDirection newDir = ListSortDirection.Ascending;
			if (this.listViewSortCol == column && this.listViewSortAdorner.Direction == newDir)
			{
				newDir = ListSortDirection.Descending;
			}
			this.listViewSortCol = column;
			this.listViewSortAdorner = new SortAdorner(this.listViewSortCol, newDir);
			AdornerLayer.GetAdornerLayer(this.listViewSortCol).Add(this.listViewSortAdorner);
			this.configsListView.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x000252B1 File Offset: 0x000234B1
		private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			this.loadConfigButton_Click(this, new RoutedEventArgs());
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x000252C0 File Offset: 0x000234C0
		private void pasteConfig_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				IDataObject dataObject = Clipboard.GetDataObject();
				if (dataObject.GetDataPresent(DataFormats.FileDrop))
				{
					string pathConfig = (dataObject.GetData(DataFormats.FileDrop) as string[]).FirstOrDefault<string>();
					this.PasteConfig(pathConfig);
				}
			}
			catch (Exception ex)
			{
				SB.Logger.Log(ex.Message, LogLevel.Error, true, 0);
			}
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00025328 File Offset: 0x00023528
		private void PasteConfig(string path)
		{
			string confExtension = Path.GetExtension(path);
			if ((path != null && confExtension == ".svb") || confExtension == ".loli" || confExtension == ".loliX" || confExtension == ".anom")
			{
				if (!File.Exists(path))
				{
					return;
				}
				string configName = Path.GetFileName(path);
				string dst = Environment.CurrentDirectory + "\\Configs\\" + configName;
				if (File.Exists("Configs\\" + configName))
				{
					if (MessageBox.Show("The File you want to copy already exists. Do you want to replace it?", "File exists [" + Path.GetFileNameWithoutExtension(path) + "]", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
					{
						bool b = path == dst;
						if (!b)
						{
							File.Delete(dst);
						}
						File.Copy(path, "Configs\\" + configName, b);
						this.rescanConfigsButton_Click(null, null);
						return;
					}
					string fullPath = ("Configs\\" + Path.GetFileName(path)).Rename();
					File.Copy(path, fullPath, false);
					this.rescanConfigsButton_Click(null, null);
					return;
				}
				else
				{
					File.Copy(path, "Configs\\" + Path.GetFileName(path), false);
					if (confExtension != ".loliX")
					{
						FileInfo info = new FileInfo("Configs\\" + Path.GetFileName(path));
						if (info.Exists)
						{
							info.MoveTo(Path.Combine(info.Directory.FullName, Path.GetFileNameWithoutExtension(info.Name) + ".svb"));
						}
					}
					this.rescanConfigsButton_Click(null, null);
				}
			}
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x000254A0 File Offset: 0x000236A0
		private void ListViewItem_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.V)
				{
					this.pasteConfig_Click(sender, e);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x000254E4 File Offset: 0x000236E4
		private void filterTextbox_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(this.filterTextbox.Text))
				{
					this.searchButton_Click(sender, null);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00025520 File Offset: 0x00023720
		private void configsListView_DragEnter(object sender, DragEventArgs e)
		{
			try
			{
				if (e.Data.GetDataPresent(DataFormats.FileDrop))
				{
					e.Effects = DragDropEffects.Copy;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0002555C File Offset: 0x0002375C
		private void configsListView_Drop(object sender, DragEventArgs e)
		{
			try
			{
				string[] locations = (string[])e.Data.GetData(DataFormats.FileDrop);
				Task.Run(delegate
				{
					for (int i = 0; i < locations.Length; i++)
					{
						try
						{
							string loc = locations[i];
							if ((loc.EndsWith(".svb") || loc.EndsWith(".loli") || loc.EndsWith(".anom") || loc.EndsWith(".loliX")) && File.Exists(loc))
							{
								this.Dispatcher.Invoke(delegate
								{
									this.PasteConfig(loc);
								});
							}
						}
						catch
						{
						}
					}
				});
			}
			catch
			{
			}
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x000258E0 File Offset: 0x00023AE0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 11)
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = Control.MouseDoubleClickEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.ListViewItem_MouseDoubleClick);
				((Style)target).Setters.Add(eventSetter);
				eventSetter = new EventSetter();
				eventSetter.Event = UIElement.KeyDownEvent;
				eventSetter.Handler = new KeyEventHandler(this.ListViewItem_KeyDown);
				((Style)target).Setters.Add(eventSetter);
			}
		}

		// Token: 0x04000587 RID: 1415
		private ConfigManagerViewModel vm;

		// Token: 0x04000588 RID: 1416
		private GridViewColumnHeader listViewSortCol;

		// Token: 0x04000589 RID: 1417
		private SortAdorner listViewSortAdorner;
	}
}
