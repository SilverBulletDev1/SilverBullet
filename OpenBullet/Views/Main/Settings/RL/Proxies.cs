using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Extreme.Net;
using RuriLib.Models;
using RuriLib.Runner;
using RuriLib.ViewModels;

namespace OpenBullet.Views.Main.Settings.RL
{
	// Token: 0x020000F3 RID: 243
	public class Proxies : Page, IComponentConnector, IStyleConnector
	{
		// Token: 0x06000635 RID: 1589 RVA: 0x0002080C File Offset: 0x0001EA0C
		public Proxies()
		{
			this.vm = SB.Settings.RLSettings.Proxies;
			base.DataContext = this.vm;
			this.InitializeComponent();
			foreach (string i in Enum.GetNames(typeof(ProxyType)))
			{
				if (i != "Chain")
				{
					this.reloadTypeCombobox.Items.Add(i);
				}
			}
			this.reloadTypeCombobox.SelectedIndex = (int)SB.Settings.RLSettings.Proxies.ReloadType;
			foreach (string s in Enum.GetNames(typeof(ProxyReloadSource)))
			{
				this.reloadSourceCombobox.Items.Add(s);
			}
			this.reloadSourceCombobox.SelectedIndex = (int)SB.Settings.RLSettings.Proxies.ReloadSource;
			this.globalBanKeysTextbox.AppendText(string.Join(Environment.NewLine, SB.Settings.RLSettings.Proxies.GlobalBanKeys), Colors.White);
			this.globalRetryKeysTextbox.AppendText(string.Join(Environment.NewLine, SB.Settings.RLSettings.Proxies.GlobalRetryKeys), Colors.White);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00020960 File Offset: 0x0001EB60
		private void globalBanKeysTextbox_TextChanged(object sender, TextChangedEventArgs e)
		{
			SB.Settings.RLSettings.Proxies.GlobalBanKeys = this.globalBanKeysTextbox.Lines();
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00020981 File Offset: 0x0001EB81
		private void globalRetryKeysTextbox_TextChanged(object sender, TextChangedEventArgs e)
		{
			SB.Settings.RLSettings.Proxies.GlobalRetryKeys = this.globalRetryKeysTextbox.Lines();
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x000209A4 File Offset: 0x0001EBA4
		private void reloadSourceCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SB.Settings.RLSettings.Proxies.ReloadSource = (ProxyReloadSource)this.reloadSourceCombobox.SelectedIndex;
			switch (SB.Settings.RLSettings.Proxies.ReloadSource)
			{
			case ProxyReloadSource.Manager:
				this.reloadTabControl.SelectedIndex = 0;
				this.addRemoteProxySourceButton.Visibility = Visibility.Collapsed;
				this.clearRemoteProxySourcesButton.Visibility = Visibility.Collapsed;
				this.testRemoteProxySourcesButton.Visibility = Visibility.Collapsed;
				return;
			case ProxyReloadSource.File:
				this.reloadTabControl.SelectedIndex = 1;
				this.addRemoteProxySourceButton.Visibility = Visibility.Collapsed;
				this.clearRemoteProxySourcesButton.Visibility = Visibility.Collapsed;
				this.testRemoteProxySourcesButton.Visibility = Visibility.Collapsed;
				return;
			case ProxyReloadSource.Remote:
				this.reloadTabControl.SelectedIndex = 2;
				this.addRemoteProxySourceButton.Visibility = Visibility.Visible;
				this.clearRemoteProxySourcesButton.Visibility = Visibility.Visible;
				this.testRemoteProxySourcesButton.Visibility = Visibility.Visible;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00020A8A File Offset: 0x0001EC8A
		private void reloadTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SB.Settings.RLSettings.Proxies.ReloadType = (ProxyType)this.reloadTypeCombobox.SelectedIndex;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00020AAB File Offset: 0x0001ECAB
		private void remoteProxyTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.GetRemoteProxySourceById((int)(sender as ComboBox).Tag).Type = (ProxyType)(sender as ComboBox).SelectedIndex;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00020AD8 File Offset: 0x0001ECD8
		private void remoteProxyTypeCombobox_Loaded(object sender, RoutedEventArgs e)
		{
			RemoteProxySource s = this.vm.GetRemoteProxySourceById((int)(sender as ComboBox).Tag);
			if (s.TypeInitialized)
			{
				return;
			}
			s.TypeInitialized = true;
			foreach (string t in Enum.GetNames(typeof(ProxyType)))
			{
				if (t != "Chain")
				{
					(sender as ComboBox).Items.Add(t);
				}
			}
			(sender as ComboBox).SelectedIndex = (int)s.Type;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00020B63 File Offset: 0x0001ED63
		private void removeRemoteProxySourceButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.RemoveRemoteProxySourceById((int)(sender as Button).Tag);
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00020B80 File Offset: 0x0001ED80
		private void addRemoteProxySourceButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.RemoteProxySources.Add(new RemoteProxySource(this.rand.Next()));
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00020BA2 File Offset: 0x0001EDA2
		private void clearRemoteProxySourcesButton_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Are you sure?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
			{
				return;
			}
			this.vm.RemoteProxySources.Clear();
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00020BC8 File Offset: 0x0001EDC8
		private async void TestRemoteProxySourcesButton_Click(object sender, RoutedEventArgs e)
		{
			List<string> prompt = new List<string> { "Results:" };
			foreach (RemoteProxySourceResult result in await Task.WhenAll<RemoteProxySourceResult>((from s in this.vm.RemoteProxySources
				where s.Active
				select RunnerViewModel.GetProxiesFromRemoteSourceAsync(s.Url, s.Type, s.Pattern, s.Output)).ToList<Task<RemoteProxySourceResult>>()))
			{
				if (result.Successful)
				{
					string first = "NONE";
					if (result.Proxies.Count > 0)
					{
						first = result.Proxies.First<CProxy>().Proxy;
					}
					prompt.Add(string.Format("[SUCCESS] {0} - Got {1} proxies (first: {2})", result.Url, result.Proxies.Count, first));
				}
				else
				{
					prompt.Add("[FAILURE] " + result.Url + " - " + result.Error);
				}
			}
			MessageBox.Show(string.Join(Environment.NewLine, prompt));
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00020C00 File Offset: 0x0001EE00
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/main/settings/rurilib/proxies.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00020C30 File Offset: 0x0001EE30
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.globalBanKeysTextbox = (RichTextBox)target;
				this.globalBanKeysTextbox.TextChanged += this.globalBanKeysTextbox_TextChanged;
				return;
			case 2:
				this.globalRetryKeysTextbox = (RichTextBox)target;
				this.globalRetryKeysTextbox.TextChanged += this.globalRetryKeysTextbox_TextChanged;
				return;
			case 3:
				this.reloadSourceCombobox = (ComboBox)target;
				this.reloadSourceCombobox.SelectionChanged += this.reloadSourceCombobox_SelectionChanged;
				return;
			case 4:
				this.addRemoteProxySourceButton = (Button)target;
				this.addRemoteProxySourceButton.Click += this.addRemoteProxySourceButton_Click;
				return;
			case 5:
				this.clearRemoteProxySourcesButton = (Button)target;
				this.clearRemoteProxySourcesButton.Click += this.clearRemoteProxySourcesButton_Click;
				return;
			case 6:
				this.testRemoteProxySourcesButton = (Button)target;
				this.testRemoteProxySourcesButton.Click += this.TestRemoteProxySourcesButton_Click;
				return;
			case 7:
				this.reloadTabControl = (TabControl)target;
				return;
			case 8:
				this.emptyTab = (TabItem)target;
				return;
			case 9:
				this.fileTab = (TabItem)target;
				return;
			case 10:
				this.reloadTypeCombobox = (ComboBox)target;
				this.reloadTypeCombobox.SelectionChanged += this.reloadTypeCombobox_SelectionChanged;
				return;
			case 11:
				this.remoteTab = (TabItem)target;
				return;
			case 12:
				this.remoteProxySourcesControl = (ItemsControl)target;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00020DC0 File Offset: 0x0001EFC0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 13)
			{
				((ComboBox)target).Loaded += this.remoteProxyTypeCombobox_Loaded;
				((ComboBox)target).SelectionChanged += this.remoteProxyTypeCombobox_SelectionChanged;
				return;
			}
			if (connectionId != 14)
			{
				return;
			}
			((Button)target).Click += this.removeRemoteProxySourceButton_Click;
		}

		// Token: 0x04000528 RID: 1320
		private SettingsProxies vm;

		// Token: 0x04000529 RID: 1321
		private Random rand = new Random();

		// Token: 0x0400052A RID: 1322
		internal RichTextBox globalBanKeysTextbox;

		// Token: 0x0400052B RID: 1323
		internal RichTextBox globalRetryKeysTextbox;

		// Token: 0x0400052C RID: 1324
		internal ComboBox reloadSourceCombobox;

		// Token: 0x0400052D RID: 1325
		internal Button addRemoteProxySourceButton;

		// Token: 0x0400052E RID: 1326
		internal Button clearRemoteProxySourcesButton;

		// Token: 0x0400052F RID: 1327
		internal Button testRemoteProxySourcesButton;

		// Token: 0x04000530 RID: 1328
		internal TabControl reloadTabControl;

		// Token: 0x04000531 RID: 1329
		internal TabItem emptyTab;

		// Token: 0x04000532 RID: 1330
		internal TabItem fileTab;

		// Token: 0x04000533 RID: 1331
		internal ComboBox reloadTypeCombobox;

		// Token: 0x04000534 RID: 1332
		internal TabItem remoteTab;

		// Token: 0x04000535 RID: 1333
		internal ItemsControl remoteProxySourcesControl;

		// Token: 0x04000536 RID: 1334
		private bool _contentLoaded;
	}
}
