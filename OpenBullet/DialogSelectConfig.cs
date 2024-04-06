using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using OpenBullet.ViewModels;
using OpenBullet.Views.Main.Runner;
using OpenBullet.Views.UserControls;
using RuriLib;
using RuriLib.ViewModels;

namespace OpenBullet
{
	// Token: 0x02000042 RID: 66
	public class DialogSelectConfig : Page, IComponentConnector, IStyleConnector
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000172 RID: 370 RVA: 0x000066DF File Offset: 0x000048DF
		// (set) Token: 0x06000173 RID: 371 RVA: 0x000066E7 File Offset: 0x000048E7
		private object Caller { get; set; }

		// Token: 0x06000174 RID: 372 RVA: 0x000066F0 File Offset: 0x000048F0
		public DialogSelectConfig(object caller)
		{
			this.Caller = caller;
			this.vm = SB.ConfigManager;
			base.DataContext = this.vm;
			this.InitializeComponent();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000671C File Offset: 0x0000491C
		private void selectButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.configsListView.SelectedItems.Count == 0)
			{
				return;
			}
			if (this.Caller.GetType() == typeof(Runner))
			{
				Config config = ((ConfigViewModel)this.configsListView.SelectedItem).Config;
				Runner runner = this.Caller as Runner;
				if (SB.SBSettings.General.LiveConfigUpdates)
				{
					runner.SetConfig(config);
				}
				else
				{
					runner.SetConfig(IOManager.CloneConfig(config));
				}
			}
			else if (this.Caller.GetType() == typeof(UserControlConfig))
			{
				((UserControlConfig)this.Caller).Config = (ConfigViewModel)this.configsListView.SelectedItem;
			}
			((MainDialog)base.Parent).Close();
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000067F0 File Offset: 0x000049F0
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

		// Token: 0x06000177 RID: 375 RVA: 0x000068A9 File Offset: 0x00004AA9
		private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			this.selectButton_Click(this, null);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000068B3 File Offset: 0x00004AB3
		private void ListViewItem_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				this.selectButton_Click(this, null);
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000068C6 File Offset: 0x00004AC6
		private void searchButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.SearchString = this.filterTextbox.Text;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000068DE File Offset: 0x00004ADE
		private void filterTextbox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				this.searchButton_Click(this, null);
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000068F1 File Offset: 0x00004AF1
		private void refreshButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.Rescan();
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00006900 File Offset: 0x00004B00
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/dialogs/dialogselectconfig.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00006930 File Offset: 0x00004B30
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.filterTextbox = (TextBox)target;
				this.filterTextbox.KeyDown += this.filterTextbox_KeyDown;
				return;
			case 2:
				this.searchButton = (Button)target;
				this.searchButton.Click += this.searchButton_Click;
				return;
			case 3:
				this.configsListView = (ListView)target;
				return;
			case 5:
				((GridViewColumnHeader)target).Click += this.listViewColumnHeader_Click;
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
				((GridViewColumnHeader)target).Click += this.listViewColumnHeader_Click;
				return;
			case 12:
				((GridViewColumnHeader)target).Click += this.listViewColumnHeader_Click;
				return;
			case 13:
				((GridViewColumnHeader)target).Click += this.listViewColumnHeader_Click;
				return;
			case 14:
				this.refreshButton = (Button)target;
				this.refreshButton.Click += this.refreshButton_Click;
				return;
			case 15:
				this.selectButton = (Button)target;
				this.selectButton.Click += this.selectButton_Click;
				return;
			}
			this._contentLoaded = true;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00006B04 File Offset: 0x00004D04
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 4)
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

		// Token: 0x04000149 RID: 329
		private GridViewColumnHeader listViewSortCol;

		// Token: 0x0400014A RID: 330
		private SortAdorner listViewSortAdorner;

		// Token: 0x0400014C RID: 332
		private ConfigManagerViewModel vm;

		// Token: 0x0400014D RID: 333
		internal TextBox filterTextbox;

		// Token: 0x0400014E RID: 334
		internal Button searchButton;

		// Token: 0x0400014F RID: 335
		internal ListView configsListView;

		// Token: 0x04000150 RID: 336
		internal Button refreshButton;

		// Token: 0x04000151 RID: 337
		internal Button selectButton;

		// Token: 0x04000152 RID: 338
		private bool _contentLoaded;
	}
}
