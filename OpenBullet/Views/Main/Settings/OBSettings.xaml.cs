using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace OpenBullet.Views.Main.Settings
{
	// Token: 0x020000E7 RID: 231
	public partial class OBSettings : Page
	{
		// Token: 0x060005DF RID: 1503 RVA: 0x0001E9B4 File Offset: 0x0001CBB4
		public OBSettings()
		{
			this.InitializeComponent();
			this.menuOptionGeneral_MouseDown(this, null);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001EA01 File Offset: 0x0001CC01
		private void menuOptionGeneral_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.GeneralPage;
			this.menuOptionSelected(this.menuOptionGeneral);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001EA20 File Offset: 0x0001CC20
		private void menuOptionSounds_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.SoundsPage;
			this.menuOptionSelected(this.menuOptionSounds);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0001EA3F File Offset: 0x0001CC3F
		private void menuOptionSources_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.SourcesPage;
			this.menuOptionSelected(this.menuOptionSources);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0001EA5E File Offset: 0x0001CC5E
		private void menuOptionThemes_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.ThemesPage;
			this.menuOptionSelected(this.menuOptionThemes);
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001EA80 File Offset: 0x0001CC80
		private void menuOptionSelected(object sender)
		{
			foreach (object child in this.topMenu.Children)
			{
				try
				{
					((Label)child).Foreground = Utils.GetBrush("ForegroundMain");
				}
				catch
				{
				}
			}
			((Label)sender).Foreground = Utils.GetBrush("ForegroundGood");
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001EB10 File Offset: 0x0001CD10
		private void saveButton_Click(object sender, RoutedEventArgs e)
		{
			SBIOManager.SaveSettings(SB.obSettingsFile, SB.SBSettings);
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001EB21 File Offset: 0x0001CD21
		private void resetButton_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to reset all your OpenBullet settings?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{
				SB.SBSettings.Reset();
			}
		}

		// Token: 0x040004D1 RID: 1233
		private General GeneralPage = new General();

		// Token: 0x040004D2 RID: 1234
		private Sounds SoundsPage = new Sounds();

		// Token: 0x040004D3 RID: 1235
		private Sources SourcesPage = new Sources();

		// Token: 0x040004D4 RID: 1236
		private Themes ThemesPage = new Themes();
	}
}
