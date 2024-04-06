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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using OpenBullet.ViewModels;
using OpenBullet.Views.Main.Runner;
using OpenBullet.Views.UserControls;
using RuriLib;
using RuriLib.Models;
using RuriLib.Runner;

namespace OpenBullet
{
	// Token: 0x0200003A RID: 58
	public class DialogSelectWordlist : Page, IComponentConnector, IStyleConnector
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000139 RID: 313 RVA: 0x000055BB File Offset: 0x000037BB
		// (set) Token: 0x0600013A RID: 314 RVA: 0x000055C3 File Offset: 0x000037C3
		private object Caller { get; set; }

		// Token: 0x0600013B RID: 315 RVA: 0x000055CC File Offset: 0x000037CC
		public DialogSelectWordlist(object caller)
		{
			this.Caller = caller;
			this.vm = SB.WordlistManager;
			base.DataContext = this.vm;
			this.InitializeComponent();
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000055F8 File Offset: 0x000037F8
		private void selectButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.Caller.GetType() == typeof(Runner) && this.wordlistsListView.SelectedItems.Count > 1)
			{
				Runner runner = (Runner)this.Caller;
				RunnerViewModel runnerVm = runner.DataContext as RunnerViewModel;
				List<string> wordlists = new List<string>();
				string wordlistNames = string.Empty;
				foreach (object obj in this.wordlistsListView.SelectedItems)
				{
					Wordlist item = (Wordlist)obj;
					if (runnerVm.DataPool == null)
					{
						runnerVm.DataPool = new DataPool(item.Path);
					}
					wordlistNames = wordlistNames + item.Name + " & ";
					string[] list = File.ReadAllLines(item.Path);
					if (item.RemoveDup)
					{
						list = list.Distinct<string>().ToArray<string>();
					}
					wordlists.AddRange(list);
				}
				if (wordlists.Count > 0)
				{
					runnerVm.DataPool.List = wordlists;
					runnerVm.DataPool.Size = (long)wordlists.Count;
				}
				Wordlist w = (Wordlist)this.wordlistsListView.SelectedItem;
				runner.SetWordlist(new Wordlist(w.Name, w.Path, w.Type, w.Purpose, true, true, w.SubWordlists)
				{
					Total = (long)wordlists.Count,
					Name = wordlistNames.Trim().TrimEnd(new char[] { '&' }) + " [Multiple]",
					RemoveDup = false
				});
			}
			else if (this.Caller.GetType() == typeof(Runner))
			{
				((Runner)this.Caller).SetWordlist((Wordlist)this.wordlistsListView.SelectedItem);
			}
			else if (this.Caller.GetType() == typeof(UserControlWordlist))
			{
				((UserControlWordlist)this.Caller).Wordlist = (Wordlist)this.wordlistsListView.SelectedItem;
			}
			((MainDialog)base.Parent).Close();
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00005840 File Offset: 0x00003A40
		private void listViewColumnHeader_Click(object sender, RoutedEventArgs e)
		{
			GridViewColumnHeader column = sender as GridViewColumnHeader;
			string sortBy = column.Tag.ToString();
			if (this.listViewSortCol != null)
			{
				AdornerLayer.GetAdornerLayer(this.listViewSortCol).Remove(this.listViewSortAdorner);
				this.wordlistsListView.Items.SortDescriptions.Clear();
			}
			ListSortDirection newDir = ListSortDirection.Ascending;
			if (this.listViewSortCol == column && this.listViewSortAdorner.Direction == newDir)
			{
				newDir = ListSortDirection.Descending;
			}
			this.listViewSortCol = column;
			this.listViewSortAdorner = new SortAdorner(this.listViewSortCol, newDir);
			AdornerLayer.GetAdornerLayer(this.listViewSortCol).Add(this.listViewSortAdorner);
			this.wordlistsListView.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000058F9 File Offset: 0x00003AF9
		private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			this.selectButton_Click(this, null);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00005903 File Offset: 0x00003B03
		private void ListViewItem_KeyDown(object sender, global::System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == global::System.Windows.Input.Key.Return)
			{
				this.selectButton_Click(this, null);
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00005918 File Offset: 0x00003B18
		private void importWordlistButton_MouseDown(object sender, MouseButtonEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Wordlist file | *.txt";
			ofd.FilterIndex = 1;
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				this.pathTextBox.Text = ofd.FileName;
				if (this.addToWordlistsCheckBox.IsChecked.GetValueOrDefault())
				{
					try
					{
						Wordlist wordlist = WordlistManagerViewModel.FileToWordlist(ofd.FileName);
						wordlist.RemoveDup = this.removeDupCheckBox.IsChecked.GetValueOrDefault();
						this.vm.Add(wordlist);
						return;
					}
					catch (Exception ex)
					{
						SB.Logger.Log(ex.Message, LogLevel.Error, true, 0);
						return;
					}
				}
				try
				{
					Wordlist wordlist2 = WordlistManagerViewModel.FileToWordlist(ofd.FileName);
					wordlist2.RemoveDup = this.removeDupCheckBox.IsChecked.GetValueOrDefault();
					((Runner)this.Caller).SetWordlist(wordlist2);
					((MainDialog)base.Parent).Close();
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00005A24 File Offset: 0x00003C24
		private void searchButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.SearchString = this.filterTextbox.Text;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00005A3C File Offset: 0x00003C3C
		private void filterTextbox_KeyDown(object sender, global::System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == global::System.Windows.Input.Key.Return)
			{
				this.searchButton_Click(this, null);
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005A4F File Offset: 0x00003C4F
		private void wordlistsListView_DragEnter(object sender, global::System.Windows.DragEventArgs e)
		{
			if (e.Data.GetDataPresent(global::System.Windows.DataFormats.FileDrop))
			{
				e.Effects = global::System.Windows.DragDropEffects.Copy;
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00005A6C File Offset: 0x00003C6C
		private void wordlistsListView_Drop(object sender, global::System.Windows.DragEventArgs e)
		{
			try
			{
				try
				{
					Task task = this.myTask;
					if (task != null)
					{
						task.Dispose();
					}
				}
				catch
				{
				}
				if (this.addToWordlistsCheckBox.IsChecked.GetValueOrDefault())
				{
					Wordlist wordlist;
					this.myTask = Task.Run(delegate
					{
						foreach (string path in ((string[])e.Data.GetData(global::System.Windows.DataFormats.FileDrop)).Where((string w) => w.EndsWith(".txt")))
						{
							try
							{
								Wordlist wordlist = WordlistManagerViewModel.FileToWordlist(path);
								this.Dispatcher.Invoke<bool>(() => wordlist.RemoveDup = this.removeDupCheckBox.IsChecked.GetValueOrDefault());
								this.Dispatcher.Invoke(delegate
								{
									this.vm.Add(wordlist);
								});
							}
							catch
							{
							}
						}
					});
				}
				else
				{
					string wordlistPath = ((string[])e.Data.GetData(global::System.Windows.DataFormats.FileDrop))[0];
					if (wordlistPath.EndsWith(".txt") && File.Exists(wordlistPath))
					{
						Wordlist wordlist = WordlistManagerViewModel.FileToWordlist(wordlistPath);
						wordlist.RemoveDup = this.removeDupCheckBox.IsChecked.GetValueOrDefault();
						((Runner)this.Caller).SetWordlist(wordlist);
						((MainDialog)base.Parent).Close();
					}
				}
			}
			catch (Exception ex)
			{
				SB.Logger.Log(ex.Message, LogLevel.Error, true, 0);
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00005B7C File Offset: 0x00003D7C
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			((MainDialog)base.Parent).Closing += delegate
			{
				try
				{
					Task task = this.myTask;
					if (task != null)
					{
						task.Dispose();
					}
				}
				catch
				{
				}
			};
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005B9C File Offset: 0x00003D9C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/dialogs/dialogselectwordlist.xaml", UriKind.Relative);
			global::System.Windows.Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005BCC File Offset: 0x00003DCC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((DialogSelectWordlist)target).Loaded += this.Page_Loaded;
				return;
			case 2:
				this.filterTextbox = (global::System.Windows.Controls.TextBox)target;
				this.filterTextbox.KeyDown += this.filterTextbox_KeyDown;
				return;
			case 3:
				this.searchButton = (global::System.Windows.Controls.Button)target;
				this.searchButton.Click += this.searchButton_Click;
				return;
			case 4:
				this.wordlistsListView = (global::System.Windows.Controls.ListView)target;
				this.wordlistsListView.DragEnter += this.wordlistsListView_DragEnter;
				this.wordlistsListView.Drop += this.wordlistsListView_Drop;
				return;
			case 6:
				((GridViewColumnHeader)target).Click += this.listViewColumnHeader_Click;
				return;
			case 7:
				((GridViewColumnHeader)target).Click += this.listViewColumnHeader_Click;
				return;
			case 8:
				((GridViewColumnHeader)target).Click += this.listViewColumnHeader_Click;
				return;
			case 9:
				((GridViewColumnHeader)target).Click += this.listViewColumnHeader_Click;
				return;
			case 10:
				((GridViewColumnHeader)target).Click += this.listViewColumnHeader_Click;
				return;
			case 11:
				this.pathTextBox = (global::System.Windows.Controls.TextBox)target;
				return;
			case 12:
				((Image)target).MouseDown += this.importWordlistButton_MouseDown;
				return;
			case 13:
				this.addToWordlistsCheckBox = (global::System.Windows.Controls.CheckBox)target;
				return;
			case 14:
				this.removeDupCheckBox = (global::System.Windows.Controls.CheckBox)target;
				return;
			case 15:
				this.selectButton = (global::System.Windows.Controls.Button)target;
				this.selectButton.Click += this.selectButton_Click;
				return;
			}
			this._contentLoaded = true;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005DA0 File Offset: 0x00003FA0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 5)
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = global::System.Windows.Controls.Control.MouseDoubleClickEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.ListViewItem_MouseDoubleClick);
				((Style)target).Setters.Add(eventSetter);
				eventSetter = new EventSetter();
				eventSetter.Event = UIElement.KeyDownEvent;
				eventSetter.Handler = new global::System.Windows.Input.KeyEventHandler(this.ListViewItem_KeyDown);
				((Style)target).Setters.Add(eventSetter);
			}
		}

		// Token: 0x04000125 RID: 293
		private GridViewColumnHeader listViewSortCol;

		// Token: 0x04000126 RID: 294
		private SortAdorner listViewSortAdorner;

		// Token: 0x04000128 RID: 296
		private WordlistManagerViewModel vm;

		// Token: 0x04000129 RID: 297
		private Task myTask;

		// Token: 0x0400012A RID: 298
		internal global::System.Windows.Controls.TextBox filterTextbox;

		// Token: 0x0400012B RID: 299
		internal global::System.Windows.Controls.Button searchButton;

		// Token: 0x0400012C RID: 300
		internal global::System.Windows.Controls.ListView wordlistsListView;

		// Token: 0x0400012D RID: 301
		internal global::System.Windows.Controls.TextBox pathTextBox;

		// Token: 0x0400012E RID: 302
		internal global::System.Windows.Controls.CheckBox addToWordlistsCheckBox;

		// Token: 0x0400012F RID: 303
		internal global::System.Windows.Controls.CheckBox removeDupCheckBox;

		// Token: 0x04000130 RID: 304
		internal global::System.Windows.Controls.Button selectButton;

		// Token: 0x04000131 RID: 305
		private bool _contentLoaded;
	}
}
