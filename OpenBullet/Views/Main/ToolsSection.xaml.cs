using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using OpenBullet.Views.Main.Tools;

namespace OpenBullet.Views.Main
{
	// Token: 0x020000D1 RID: 209
	public partial class ToolsSection : Page
	{
		// Token: 0x06000541 RID: 1345 RVA: 0x0001AFCC File Offset: 0x000191CC
		public ToolsSection()
		{
			this.InitializeComponent();
			this.ListGenerator = new ListGenerator();
			this.SeleniumTools = new SeleniumTools();
			this.wordlistTools = new WordlistTools();
			this.tessDataDL = new TessDataDownloads();
			this.menuOptionListGenerator_MouseDown(this, null);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001B019 File Offset: 0x00019219
		private void menuOptionListGenerator_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.ListGenerator;
			this.menuOptionSelected(this.menuOptionListGenerator);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001B038 File Offset: 0x00019238
		private void menuOptionSeleniumUtilities_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.SeleniumTools;
			this.menuOptionSelected(this.menuOptionSeleniumTools);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001B057 File Offset: 0x00019257
		private void menuTessData_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.tessDataDL;
			this.menuOptionSelected(this.menuTessData);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001B076 File Offset: 0x00019276
		private void menuOptionWordlist_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.wordlistTools;
			this.menuOptionSelected(sender);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001B090 File Offset: 0x00019290
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

		// Token: 0x04000444 RID: 1092
		private ListGenerator ListGenerator;

		// Token: 0x04000445 RID: 1093
		private SeleniumTools SeleniumTools;

		// Token: 0x04000446 RID: 1094
		private TessDataDownloads tessDataDL;

		// Token: 0x04000447 RID: 1095
		private WordlistTools wordlistTools;
	}
}
