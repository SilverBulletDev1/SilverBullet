using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using OpenBullet.Views.Main.Configs;
using RuriLib.ViewModels;

namespace OpenBullet.Views.Main
{
	// Token: 0x020000BF RID: 191
	public partial class ConfigsSection : Page
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00017763 File Offset: 0x00015963
		public ConfigViewModel CurrentConfig
		{
			get
			{
				return SB.ConfigManager.CurrentConfig;
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0001776F File Offset: 0x0001596F
		public ConfigsSection()
		{
			this.InitializeComponent();
			this.ConfigManagerPage = new ConfigManager();
			SB.Logger.LogInfo(Components.ConfigManager, "Initialized Manager Page", false, 0);
			this.menuOptionManager_MouseDown(this, null);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x000177A2 File Offset: 0x000159A2
		private void menuOptionManager_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.ConfigManagerPage;
			this.menuOptionSelected(this.menuOptionManager);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x000177C4 File Offset: 0x000159C4
		public void menuOptionStacker_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (this.CurrentConfig != null && this.StackerPage != null)
			{
				this.Main.Content = this.StackerPage;
				this.menuOptionSelected(this.menuOptionStacker);
				return;
			}
			SB.Logger.LogError(Components.ConfigManager, "Cannot switch to stacker since no config is loaded or the loaded config isn't public", false, 0);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00017814 File Offset: 0x00015A14
		private void menuOptionOtherOptions_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (this.CurrentConfig != null)
			{
				if (this.OtherOptionsPage == null)
				{
					this.OtherOptionsPage = new ConfigOtherOptions();
				}
				this.Main.Content = this.OtherOptionsPage;
				this.menuOptionSelected(this.menuOptionOtherOptions);
				return;
			}
			SB.Logger.LogError(Components.ConfigManager, "Cannot switch to other options since no config is loaded", false, 0);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001786C File Offset: 0x00015A6C
		private void menuOptionOCRSettings_MouseDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				if (this.CurrentConfig != null)
				{
					if (this.ConfigOcrSettings == null)
					{
						this.ConfigOcrSettings = new ConfigOcrSettings(false);
					}
					this.Main.Content = this.ConfigOcrSettings;
					this.menuOptionSelected(this.menuOptionOCRSettings);
				}
				else
				{
					SB.Logger.LogError(Components.ConfigManager, "Cannot switch to other options since no config is loaded", false, 0);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x000178DC File Offset: 0x00015ADC
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

		// Token: 0x040003DF RID: 991
		public ConfigManager ConfigManagerPage;

		// Token: 0x040003E0 RID: 992
		public Stacker StackerPage;

		// Token: 0x040003E1 RID: 993
		public ConfigOtherOptions OtherOptionsPage;

		// Token: 0x040003E2 RID: 994
		public ConfigOcrSettings ConfigOcrSettings;
	}
}
