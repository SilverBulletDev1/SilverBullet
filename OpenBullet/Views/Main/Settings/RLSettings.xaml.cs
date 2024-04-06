using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using OpenBullet.Views.Main.Settings.RL;
using RuriLib;
using RuriLib.ViewModels;

namespace OpenBullet.Views.Main.Settings
{
	// Token: 0x020000E8 RID: 232
	public partial class RLSettings : Page
	{
		// Token: 0x060005E9 RID: 1513 RVA: 0x0001ECA4 File Offset: 0x0001CEA4
		public RLSettings()
		{
			this.InitializeComponent();
			this.menuOptionGeneral_MouseDown(this, null);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0001ED07 File Offset: 0x0001CF07
		private void menuOptionGeneral_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.GeneralPage;
			this.menuOptionSelected(this.menuOptionGeneral);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0001ED26 File Offset: 0x0001CF26
		private void menuOptionProxies_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.ProxiesPage;
			this.menuOptionSelected(this.menuOptionProxies);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001ED45 File Offset: 0x0001CF45
		private void menuOptionCaptchas_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.CaptchasPage;
			this.menuOptionSelected(this.menuOptionCaptchas);
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001ED64 File Offset: 0x0001CF64
		private void menuOptionSelenium_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.SeleniumPage;
			this.menuOptionSelected(this.menuOptionSelenium);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0001ED83 File Offset: 0x0001CF83
		private void menuOptionOcr_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.OcrPage;
			this.menuOptionSelected(this.menuOptionOcr);
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0001EDA2 File Offset: 0x0001CFA2
		private void menuOptionCefSharpBrw_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.cefSharpPage;
			this.menuOptionSelected(sender);
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0001EDBC File Offset: 0x0001CFBC
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

		// Token: 0x060005F1 RID: 1521 RVA: 0x0001EE4C File Offset: 0x0001D04C
		private void saveButton_Click(object sender, RoutedEventArgs e)
		{
			IOManager.SaveSettings<RLSettingsViewModel>(SB.rlSettingsFile, SB.Settings.RLSettings);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0001EE62 File Offset: 0x0001D062
		private void resetButton_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to reset all your RuriLib settings?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{
				SB.Settings.RLSettings.Reset();
			}
		}

		// Token: 0x040004DE RID: 1246
		private OpenBullet.Views.Main.Settings.RL.General GeneralPage = new OpenBullet.Views.Main.Settings.RL.General();

		// Token: 0x040004DF RID: 1247
		private Proxies ProxiesPage = new Proxies();

		// Token: 0x040004E0 RID: 1248
		private Captchas CaptchasPage = new Captchas();

		// Token: 0x040004E1 RID: 1249
		private Selenium SeleniumPage = new Selenium();

		// Token: 0x040004E2 RID: 1250
		private Ocr OcrPage = new Ocr();

		// Token: 0x040004E3 RID: 1251
		private CefSharp cefSharpPage = new CefSharp();
	}
}
