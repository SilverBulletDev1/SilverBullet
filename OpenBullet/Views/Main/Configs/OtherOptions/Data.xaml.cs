using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using RuriLib;
using RuriLib.Models;

namespace OpenBullet.Views.Main.Configs.OtherOptions
{
	// Token: 0x02000112 RID: 274
	public partial class Data : Page
	{
		// Token: 0x06000754 RID: 1876 RVA: 0x0002A510 File Offset: 0x00028710
		public Data()
		{
			this.vm = SB.ConfigManager.CurrentConfig.Config.Settings;
			base.DataContext = this.vm;
			this.InitializeComponent();
			this.allowedWordlist1Combobox.Items.Add("");
			foreach (string i in SB.Settings.Environment.GetWordlistTypeNames())
			{
				this.allowedWordlist1Combobox.Items.Add(i);
			}
			try
			{
				this.allowedWordlist1Combobox.Text = this.vm.AllowedWordlist1;
			}
			catch
			{
				this.allowedWordlist1Combobox.SelectedIndex = 0;
			}
			this.allowedWordlist2Combobox.Items.Add("");
			foreach (string j in SB.Settings.Environment.GetWordlistTypeNames())
			{
				this.allowedWordlist2Combobox.Items.Add(j);
			}
			try
			{
				this.allowedWordlist2Combobox.Text = this.vm.AllowedWordlist2;
			}
			catch
			{
				this.allowedWordlist2Combobox.SelectedIndex = 0;
			}
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0002A6A0 File Offset: 0x000288A0
		private void clearRulesButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.DataRules.Clear();
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0002A6B2 File Offset: 0x000288B2
		private void addRuleButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.DataRules.Add(new DataRule(this.rand.Next()));
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0002A6D4 File Offset: 0x000288D4
		private void removeRuleButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.RemoveDataRuleById((int)(sender as Button).Tag);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0002A6F1 File Offset: 0x000288F1
		private void allowedWordlist1Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.AllowedWordlist1 = (string)this.allowedWordlist1Combobox.SelectedValue;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0002A70E File Offset: 0x0002890E
		private void allowedWordlist2Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.AllowedWordlist2 = (string)this.allowedWordlist2Combobox.SelectedValue;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0002A82E File Offset: 0x00028A2E
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 6)
			{
				((Button)target).Click += this.removeRuleButton_Click;
			}
		}

		// Token: 0x0400060F RID: 1551
		private ConfigSettings vm;

		// Token: 0x04000610 RID: 1552
		private Random rand = new Random();
	}
}
