using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using OpenBullet.ViewModels;
using OpenBullet.Views.Dialogs;
using RuriLib;
using RuriLib.Models;

namespace OpenBullet.Views.Main
{
	// Token: 0x020000D2 RID: 210
	public partial class WordlistManager : Page
	{
		// Token: 0x06000549 RID: 1353 RVA: 0x0001B233 File Offset: 0x00019433
		public WordlistManager()
		{
			this.vm = SB.WordlistManager;
			base.DataContext = this.vm;
			this.InitializeComponent();
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001B258 File Offset: 0x00019458
		public void AddWordlist(Wordlist wordlist)
		{
			try
			{
				this.vm.Add(wordlist);
			}
			catch (Exception e)
			{
				SB.Logger.LogError(Components.WordlistManager, e.Message, false, 0);
			}
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001B29C File Offset: 0x0001949C
		private void addButton_Click(object sender, RoutedEventArgs e)
		{
			new MainDialog(new DialogAddWordlist(this), "Add Wordlist").ShowDialog();
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001B2B4 File Offset: 0x000194B4
		private void deleteButton_Click(object sender, RoutedEventArgs e)
		{
			SB.Logger.LogInfo(Components.WordlistManager, string.Format("Deleting {0} references from the DB", this.wordlistListView.SelectedItems.Count), false, 0);
			foreach (Wordlist wordlist in this.wordlistListView.SelectedItems.Cast<Wordlist>().ToList<Wordlist>())
			{
				this.vm.Remove(wordlist);
			}
			SB.Logger.LogInfo(Components.WordlistManager, "Successfully deleted the wordlist references from the DB", false, 0);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001B35C File Offset: 0x0001955C
		private void deleteAllButton_Click(object sender, RoutedEventArgs e)
		{
			SB.Logger.LogWarning(Components.WordlistManager, "Purge selected, prompting warning", false, 0);
			if (global::System.Windows.MessageBox.Show("This will purge the WHOLE Wordlists DB, are you sure you want to continue?", "WARNING", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
			{
				SB.Logger.LogInfo(Components.WordlistManager, "Purge initiated", false, 0);
				this.vm.RemoveAll();
				SB.Logger.LogInfo(Components.WordlistManager, "Purge finished", false, 0);
				return;
			}
			SB.Logger.LogInfo(Components.WordlistManager, "Purge dismissed", false, 0);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001B3D2 File Offset: 0x000195D2
		private void deleteNotFoundWordlistsButton_Click(object sender, RoutedEventArgs e)
		{
			SB.Logger.LogWarning(Components.WordlistManager, "Deleting wordlists with missing files.", false, 0);
			this.vm.DeleteNotFound();
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0001B3F1 File Offset: 0x000195F1
		private void searchButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.SearchString = this.filterTextbox.Text;
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0001B409 File Offset: 0x00019609
		private void filterTextbox_KeyDown(object sender, global::System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == global::System.Windows.Input.Key.Return)
			{
				this.searchButton_Click(this, null);
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001B41C File Offset: 0x0001961C
		private void listViewColumnHeader_Click(object sender, RoutedEventArgs e)
		{
			GridViewColumnHeader column = sender as GridViewColumnHeader;
			string sortBy = column.Tag.ToString();
			if (this.listViewSortCol != null)
			{
				AdornerLayer.GetAdornerLayer(this.listViewSortCol).Remove(this.listViewSortAdorner);
				this.wordlistListView.Items.SortDescriptions.Clear();
			}
			ListSortDirection newDir = ListSortDirection.Ascending;
			if (this.listViewSortCol == column && this.listViewSortAdorner.Direction == newDir)
			{
				newDir = ListSortDirection.Descending;
			}
			this.listViewSortCol = column;
			this.listViewSortAdorner = new SortAdorner(this.listViewSortCol, newDir);
			AdornerLayer.GetAdornerLayer(this.listViewSortCol).Add(this.listViewSortAdorner);
			this.wordlistListView.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001B4D8 File Offset: 0x000196D8
		private void wordlistListViewDrop(object sender, global::System.Windows.DragEventArgs e)
		{
			if (e.Data.GetDataPresent(global::System.Windows.DataFormats.FileDrop))
			{
				foreach (string file in ((string[])e.Data.GetData(global::System.Windows.DataFormats.FileDrop)).Where((string x) => x.EndsWith(".txt")).ToArray<string>())
				{
					try
					{
						string path = file;
						string cwd = Directory.GetCurrentDirectory();
						if (path.StartsWith(cwd))
						{
							path = path.Substring(cwd.Length + 1);
						}
						Wordlist wordlist = new Wordlist(Path.GetFileNameWithoutExtension(file), path, SB.Settings.Environment.WordlistTypes.First<WordlistType>().Name, "", true, false, null);
						string first = File.ReadLines(wordlist.Path).First((string l) => !string.IsNullOrWhiteSpace(l));
						wordlist.Type = SB.Settings.Environment.RecognizeWordlistType(first);
						this.AddWordlist(wordlist);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001B60C File Offset: 0x0001980C
		private void editButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.wordlistListView.SelectedIndex != -1 && this.wordlistListView.SelectedItem != null)
				{
					DialogEditWordlist dialogEditWordlist = new DialogEditWordlist((Wordlist)this.wordlistListView.SelectedItem);
					new MainDialog(dialogEditWordlist, "Edit Wordlist").ShowDialog();
					if (dialogEditWordlist.DialogResult == DialogResult.OK)
					{
						this.vm.Update(dialogEditWordlist.WordList);
						this.vm.RefreshList();
					}
				}
			}
			catch (Exception ex)
			{
				SB.Logger.Log(ex.Message, LogLevel.Error, true, 0);
			}
		}

		// Token: 0x0400044F RID: 1103
		private WordlistManagerViewModel vm;

		// Token: 0x04000450 RID: 1104
		private GridViewColumnHeader listViewSortCol;

		// Token: 0x04000451 RID: 1105
		private SortAdorner listViewSortAdorner;
	}
}
