using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using Microsoft.Win32;
using OpenBullet.ViewModels;
using OpenBullet.Views.Main.Runner;
using RuriLib;
using RuriLib.Models;
using RuriLib.Runner;
using RuriLib.ViewModels;

namespace OpenBullet.Views.Main
{
	// Token: 0x020000C0 RID: 192
	public partial class HitsDB : Page
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x00017A7F File Offset: 0x00015C7F
		private IEnumerable<Hit> Selected
		{
			get
			{
				return this.hitsListView.SelectedItems.Cast<Hit>();
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00017A94 File Offset: 0x00015C94
		public HitsDB()
		{
			this.vm = SB.HitsDB;
			base.DataContext = this.vm;
			this.InitializeComponent();
			this.vm.RefreshList();
			foreach (string i in new List<string> { "SUCCESS", "NONE" }.Concat(SB.Settings.Environment.GetCustomKeychainNames()))
			{
				if (!this.typeFilterCombobox.Items.Contains(i))
				{
					this.typeFilterCombobox.Items.Add(i);
				}
			}
			this.typeFilterCombobox.SelectedIndex = 0;
			this.configFilterCombobox.Items.Add(HitsDBViewModel.defaultFilter);
			foreach (string c2 in this.vm.ConfigsList.OrderBy((string c) => c))
			{
				this.configFilterCombobox.Items.Add(c2);
			}
			this.configFilterCombobox.SelectedIndex = 0;
			ContextMenu contextMenu = (ContextMenu)base.Resources["ItemContextMenu"];
			MenuItem copyMenu = (MenuItem)contextMenu.Items[0];
			MenuItem saveMenu = (MenuItem)contextMenu.Items[1];
			foreach (ExportFormat f in SB.Settings.Environment.ExportFormats)
			{
				MenuItem j = new MenuItem();
				j.Header = f.Format;
				j.Click += this.copySelectedCustom_Click;
				((MenuItem)copyMenu.Items[4]).Items.Add(j);
			}
			foreach (ExportFormat f2 in SB.Settings.Environment.ExportFormats)
			{
				MenuItem k = new MenuItem();
				k.Header = f2.Format;
				k.Click += this.saveSelectedCustom_Click;
				((MenuItem)saveMenu.Items[3]).Items.Add(k);
			}
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00017D90 File Offset: 0x00015F90
		public void AddConfigToFilter(string name)
		{
			if (!this.configFilterCombobox.Items.Cast<string>().Any((string i) => i == name))
			{
				this.configFilterCombobox.Items.Add(name);
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00017DE4 File Offset: 0x00015FE4
		private void configFilterCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.ConfigFilter = (string)this.configFilterCombobox.SelectedValue;
			SB.Logger.LogInfo(Components.HitsDB, string.Format("Changed config filter to {0}, found {1} hits", this.vm.ConfigFilter, this.vm.Total), false, 0);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00017E40 File Offset: 0x00016040
		private void typeFilterCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.TypeFilter = this.typeFilterCombobox.SelectedItem.ToString();
			SB.Logger.LogInfo(Components.HitsDB, string.Format("Changed type filter to {0}, found {1} hits", this.vm.TypeFilter, this.vm.Total), false, 0);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00017E9C File Offset: 0x0001609C
		private void typeFilterCombobox_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				this.vm.TypeFilter = this.typeFilterCombobox.Text;
				SB.Logger.LogInfo(Components.HitsDB, string.Format("Changed type filter to {0}, found {1} hits", this.vm.TypeFilter, this.vm.Total), false, 0);
			}
			catch
			{
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00017F08 File Offset: 0x00016108
		private void purgeButton_Click(object sender, RoutedEventArgs e)
		{
			SB.Logger.LogWarning(Components.HitsDB, "Purge selected, prompting warning", false, 0);
			if (MessageBox.Show("This will purge the WHOLE Hits DB, are you sure you want to continue?", "WARNING", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
			{
				SB.Logger.LogInfo(Components.HitsDB, "Purge initiated", false, 0);
				this.vm.RemoveAll();
				SB.Logger.LogInfo(Components.HitsDB, "Purge finished", false, 0);
			}
			else
			{
				SB.Logger.LogInfo(Components.HitsDB, "Purge dismissed", false, 0);
			}
			try
			{
				List<string> configsList = this.vm.ConfigsList;
				if (configsList != null && configsList.Count >= 2 && this.vm.Hits.Count<Hit>() == 0)
				{
					this.configFilterCombobox.Items.Clear();
					this.configFilterCombobox.Items.Add("All");
				}
			}
			catch
			{
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00017FEC File Offset: 0x000161EC
		private void listViewColumnHeader_Click(object sender, RoutedEventArgs e)
		{
			GridViewColumnHeader column = sender as GridViewColumnHeader;
			string sortBy = column.Tag.ToString();
			if (this.listViewSortCol != null)
			{
				AdornerLayer.GetAdornerLayer(this.listViewSortCol).Remove(this.listViewSortAdorner);
				this.hitsListView.Items.SortDescriptions.Clear();
			}
			ListSortDirection newDir = ListSortDirection.Ascending;
			if (this.listViewSortCol == column && this.listViewSortAdorner.Direction == newDir)
			{
				newDir = ListSortDirection.Descending;
			}
			this.listViewSortCol = column;
			this.listViewSortAdorner = new SortAdorner(this.listViewSortCol, newDir);
			AdornerLayer.GetAdornerLayer(this.listViewSortCol).Add(this.listViewSortAdorner);
			this.hitsListView.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00003B20 File Offset: 0x00001D20
		private void ListViewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x000180A5 File Offset: 0x000162A5
		private string GetSaveFile()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "TXT files | *.txt";
			saveFileDialog.FilterIndex = 1;
			saveFileDialog.ShowDialog();
			return saveFileDialog.FileName;
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x000180CC File Offset: 0x000162CC
		private void copySelectedData_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.Selected.CopyToClipboard((Hit hit) => hit.Data);
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.HitsDB, "Exception while copying hits - " + ex.Message, false, 0);
			}
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00018138 File Offset: 0x00016338
		private void copySelectedCapture_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.Selected.CopyToClipboard(this.mappingCapture);
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.HitsDB, "Exception while copying hits - " + ex.Message, false, 0);
			}
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00018188 File Offset: 0x00016388
		private void copySelectedFull_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.Selected.CopyToClipboard(this.mappingFull);
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.HitsDB, "Exception while copying hits - " + ex.Message, false, 0);
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x000181D8 File Offset: 0x000163D8
		private void copySelectedCustom_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.Selected.CopyToClipboard((Hit hit) => hit.ToFormattedString((sender as MenuItem).Header.ToString().Unescape(false)));
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.HitsDB, "Exception while copying hits - " + ex.Message, false, 0);
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001823C File Offset: 0x0001643C
		private void saveSelectedData_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.Selected.SaveToFile(this.GetSaveFile(), (Hit hit) => hit.Data);
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.HitsDB, "Exception while saving hits - " + ex.Message, false, 0);
			}
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x000182AC File Offset: 0x000164AC
		private void saveSelectedCapture_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.Selected.SaveToFile(this.GetSaveFile(), this.mappingCapture);
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.HitsDB, "Exception while saving hits - " + ex.Message, false, 0);
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00018304 File Offset: 0x00016504
		private void saveSelectedFull_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.Selected.SaveToFile(this.GetSaveFile(), this.mappingFull);
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.HitsDB, "Exception while saving hits - " + ex.Message, false, 0);
			}
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001835C File Offset: 0x0001655C
		private void saveSelectedCustom_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.Selected.SaveToFile(this.GetSaveFile(), (Hit hit) => hit.ToFormattedString((sender as MenuItem).Header.ToString().Unescape(false)));
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.HitsDB, "Exception while copying hits - " + ex.Message, false, 0);
			}
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x000183C8 File Offset: 0x000165C8
		private void selectAll_Click(object sender, RoutedEventArgs e)
		{
			this.hitsListView.SelectAll();
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x000183D8 File Offset: 0x000165D8
		private void copySelectedProxy_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Clipboard.SetText(((Hit)this.hitsListView.SelectedItem).Proxy);
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.HitsDB, "Failed to copy selected proxy - " + ex.Message, false, 0);
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00018434 File Offset: 0x00016634
		private void searchButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.SearchString = this.searchBar.Text;
			SB.Logger.LogInfo(Components.HitsDB, "Changed capture filter to '" + this.vm.SearchString + string.Format("', found {0} hits", this.vm.Total), false, 0);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00018494 File Offset: 0x00016694
		private void sendToRecheck_Click(object sender, RoutedEventArgs e)
		{
			if (this.hitsListView.SelectedItems.Count == 0)
			{
				SB.Logger.LogError(Components.HitsDB, "No hits selected!", true, 0);
				return;
			}
			Hit first = (Hit)this.hitsListView.SelectedItem;
			Wordlist wordlist = new Wordlist("Recheck-" + first.ConfigName, "NULL", SB.Settings.Environment.RecognizeWordlistType(first.Data), "", true, true, null);
			RunnerManagerViewModel runnerManager = SB.RunnerManager;
			runnerManager.Create();
			Runner page = runnerManager.RunnersCollection.Last<RunnerInstance>().View;
			RunnerViewModel runner = runnerManager.RunnersCollection.Last<RunnerInstance>().ViewModel;
			SB.MainWindow.ShowRunner(page);
			runner.SetWordlist(wordlist);
			runner.DataPool = new DataPool((from Hit h in this.hitsListView.SelectedItems
				select h.Data).ToList<string>(), null, false, false);
			try
			{
				Config cfg = SB.ConfigManager.ConfigsCollection.First((ConfigViewModel c) => c.Name == first.ConfigName).Config;
				runner.SetConfig(cfg, false);
				runner.BotsAmount = Math.Min(cfg.Settings.SuggestedBots, this.hitsListView.SelectedItems.Count);
			}
			catch
			{
			}
			SB.MainWindow.menuOptionRunner_Click(this, null);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00018620 File Offset: 0x00016820
		private void deleteSelected_Click(object sender, RoutedEventArgs e)
		{
			SB.Logger.LogInfo(Components.HitsDB, string.Format("Deleting {0} hits", this.hitsListView.SelectedItems.Count), false, 0);
			this.vm.Remove(this.Selected);
			SB.Logger.LogInfo(Components.HitsDB, "Succesfully sent the delete query and refreshed the list", false, 0);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001867C File Offset: 0x0001687C
		private void removeDuplicatesButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.DeleteDuplicates();
			SB.Logger.LogInfo(Components.HitsDB, "Deleted duplicate hits", false, 0);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001869C File Offset: 0x0001689C
		private void deleteFilteredButton_Click(object sender, RoutedEventArgs e)
		{
			SB.Logger.LogWarning(Components.HitsDB, "Delete filtered selected, prompting warning", false, 0);
			if (MessageBox.Show("This will delete all the hits that are currently being displayed, are you sure you want to continue?", "WARNING", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes)
			{
				return;
			}
			this.vm.DeleteFiltered();
			SB.Logger.LogInfo(Components.HitsDB, "Deleted filtered hits", false, 0);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x000186F0 File Offset: 0x000168F0
		private void searchBar_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Key == global::System.Windows.Input.Key.Return)
				{
					this.searchButton_Click(sender, e);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00018724 File Offset: 0x00016924
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				List<string> list = new List<string>();
				list.AddRange(from v in this.vm.Hits
					select v.Type into cType
					group cType by cType into g
					select g.Key);
				foreach (string i in list.Distinct<string>().ToList<string>().Concat(SB.Settings.Environment.GetCustomKeychainNames()))
				{
					if (!this.typeFilterCombobox.Items.Contains(i))
					{
						this.typeFilterCombobox.Items.Add(i);
					}
				}
			}
			catch
			{
			}
			try
			{
				List<string> configsList = this.vm.ConfigsList;
				if (configsList != null && configsList.Count >= 2 && this.vm.Hits.Count<Hit>() == 0)
				{
					this.configFilterCombobox.Items.Clear();
					this.configFilterCombobox.Items.Add("All");
				}
			}
			catch
			{
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00018C1C File Offset: 0x00016E1C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 18)
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = UIElement.MouseRightButtonDownEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.ListViewItem_MouseRightButtonDown);
				((Style)target).Setters.Add(eventSetter);
			}
		}

		// Token: 0x040003EA RID: 1002
		private HitsDBViewModel vm;

		// Token: 0x040003EB RID: 1003
		private GridViewColumnHeader listViewSortCol;

		// Token: 0x040003EC RID: 1004
		private SortAdorner listViewSortAdorner;

		// Token: 0x040003ED RID: 1005
		private Func<Hit, string> mappingCapture = (Hit hit) => hit.Data + " | " + hit.CapturedString;

		// Token: 0x040003EE RID: 1006
		private Func<Hit, string> mappingFull = (Hit hit) => string.Concat(new string[]
		{
			"Data = ",
			hit.Data,
			" | Type = ",
			hit.Type,
			" | Config = ",
			hit.ConfigName,
			" | Wordlist = ",
			hit.WordlistName,
			" | Proxy = ",
			hit.Proxy,
			" | Date = ",
			hit.Date.ToLongDateString(),
			" | CapturedData = ",
			hit.CapturedString
		});
	}
}
