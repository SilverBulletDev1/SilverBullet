using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using OpenBullet.Models;
using OpenBullet.ViewModels;

namespace OpenBullet.Views.Main.Settings.OpenBullet
{
	// Token: 0x020000EB RID: 235
	public partial class Sources : Page
	{
		// Token: 0x060005FF RID: 1535 RVA: 0x0001F251 File Offset: 0x0001D451
		public Sources()
		{
			this.vm = SB.SBSettings.Sources;
			base.DataContext = this.vm;
			this.InitializeComponent();
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001F288 File Offset: 0x0001D488
		private void authTypeCombobox_Loaded(object sender, RoutedEventArgs e)
		{
			Source s = this.vm.GetSourceById((int)(sender as ComboBox).Tag);
			if (s.AuthInitialized)
			{
				return;
			}
			s.AuthInitialized = true;
			foreach (string t in Enum.GetNames(typeof(Source.AuthMode)))
			{
				(sender as ComboBox).Items.Add(t);
			}
			(sender as ComboBox).SelectedIndex = (int)s.Auth;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001F306 File Offset: 0x0001D506
		private void authTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.GetSourceById((int)(sender as ComboBox).Tag).Auth = (Source.AuthMode)(sender as ComboBox).SelectedIndex;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001F333 File Offset: 0x0001D533
		private void removeSourceButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.RemoveSourceById((int)(sender as Button).Tag);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001F350 File Offset: 0x0001D550
		private void clearSourcesButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.Sources.Clear();
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0001F362 File Offset: 0x0001D562
		private void addSourceButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.Sources.Add(new Source(this.rand.Next()));
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0001F434 File Offset: 0x0001D634
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 4)
			{
				((ComboBox)target).Loaded += this.authTypeCombobox_Loaded;
				((ComboBox)target).SelectionChanged += this.authTypeCombobox_SelectionChanged;
				return;
			}
			if (connectionId != 5)
			{
				return;
			}
			((Button)target).Click += this.removeSourceButton_Click;
		}

		// Token: 0x040004F7 RID: 1271
		private OBSettingsSources vm;

		// Token: 0x040004F8 RID: 1272
		private Random rand = new Random();
	}
}
