using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace OpenBullet.Views.Main
{
	// Token: 0x020000BE RID: 190
	public partial class Help : Page
	{
		// Token: 0x060004A6 RID: 1190 RVA: 0x000174E8 File Offset: 0x000156E8
		public Help()
		{
			this.InitializeComponent();
			this.aboutPage = new AboutPage();
			this.releaseNotesPage = new ReleaseNotesPage();
			this.CheckUpdatePage = new CheckUpdatePage();
			this.requiresPage = new RequiresPage();
			this.Main.Content = this.aboutPage;
			this.menuOptionSelected(this.aboutLabel);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00003B20 File Offset: 0x00001D20
		private void repoButton_Click(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00003B20 File Offset: 0x00001D20
		private void docuButton_Click(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0001754C File Offset: 0x0001574C
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
			((Label)sender).Foreground = Utils.GetBrush("ForegroundCustom");
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x000175DC File Offset: 0x000157DC
		private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.aboutPage;
			this.menuOptionSelected(sender);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x000175F6 File Offset: 0x000157F6
		private void Label_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.releaseNotesPage;
			this.menuOptionSelected(sender);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00017610 File Offset: 0x00015810
		public void checkForUpdateLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.CheckUpdatePage;
			this.menuOptionSelected(this.checkForUpdateLabel);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001762F File Offset: 0x0001582F
		private void requiresLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.requiresPage;
			this.menuOptionSelected(this.requiresLabel);
		}

		// Token: 0x040003D4 RID: 980
		private AboutPage aboutPage;

		// Token: 0x040003D5 RID: 981
		private ReleaseNotesPage releaseNotesPage;

		// Token: 0x040003D6 RID: 982
		public CheckUpdatePage CheckUpdatePage;

		// Token: 0x040003D7 RID: 983
		private RequiresPage requiresPage;
	}
}
