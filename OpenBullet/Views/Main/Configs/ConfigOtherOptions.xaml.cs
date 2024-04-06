using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using OpenBullet.Views.Main.Configs.OtherOptions;

namespace OpenBullet.Views.Main.Configs
{
	// Token: 0x020000F9 RID: 249
	public partial class ConfigOtherOptions : Page
	{
		// Token: 0x06000693 RID: 1683 RVA: 0x000246E0 File Offset: 0x000228E0
		public ConfigOtherOptions()
		{
			this.InitializeComponent();
			this.menuOptionGeneral_MouseDown(this, null);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0002474E File Offset: 0x0002294E
		private void menuOptionGeneral_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.GeneralPage;
			this.menuOptionSelected(this.menuOptionGeneral);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0002476D File Offset: 0x0002296D
		private void menuOptionRequests_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.RequestsPage;
			this.menuOptionSelected(this.menuOptionRequests);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0002478C File Offset: 0x0002298C
		private void menuOptionProxies_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.ProxiesPage;
			this.menuOptionSelected(this.menuOptionProxies);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x000247AB File Offset: 0x000229AB
		private void menuOptionInput_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.InputsPage;
			this.menuOptionSelected(this.menuOptionInput);
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x000247CA File Offset: 0x000229CA
		private void menuOptionData_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.DataPage;
			this.menuOptionSelected(this.menuOptionData);
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x000247E9 File Offset: 0x000229E9
		private void menuOptionSelenium_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.SeleniumPage;
			this.menuOptionSelected(this.menuOptionSelenium);
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00024808 File Offset: 0x00022A08
		private void menuOptionCompile_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.CompilePage;
			this.menuOptionSelected(sender);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00024824 File Offset: 0x00022A24
		private void menuOptionSelected(object sender)
		{
			foreach (object obj in this.topMenu.Children)
			{
				Label child = (Label)obj;
				try
				{
					child.Foreground = Utils.GetBrush("ForegroundMain");
				}
				catch
				{
				}
			}
			((Label)sender).Foreground = Utils.GetBrush("ForegroundGood");
		}

		// Token: 0x04000576 RID: 1398
		public General GeneralPage = new General();

		// Token: 0x04000577 RID: 1399
		public Requests RequestsPage = new Requests();

		// Token: 0x04000578 RID: 1400
		public Proxies ProxiesPage = new Proxies();

		// Token: 0x04000579 RID: 1401
		public Inputs InputsPage = new Inputs();

		// Token: 0x0400057A RID: 1402
		public Data DataPage = new Data();

		// Token: 0x0400057B RID: 1403
		public Selenium SeleniumPage = new Selenium();

		// Token: 0x0400057C RID: 1404
		public Compile CompilePage = new Compile();
	}
}
