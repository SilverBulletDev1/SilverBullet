using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using RuriLib;
using RuriLib.ViewModels;

namespace OpenBullet.Views.Main.Settings
{
	// Token: 0x020000E9 RID: 233
	public class Settings : Page, IComponentConnector
	{
		// Token: 0x060005F5 RID: 1525 RVA: 0x0001F03B File Offset: 0x0001D23B
		public Settings()
		{
			this.InitializeComponent();
			this.menuOptionRuriLib_MouseDown(this, null);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0001F067 File Offset: 0x0001D267
		private void menuOptionRuriLib_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.RLSettingsPage;
			this.menuOptionSelected(this.menuOptionRuriLib);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001F086 File Offset: 0x0001D286
		private void menuOptionOpenBullet_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.OBSettingsPage;
			this.menuOptionSelected(this.menuOptionOpenBullet);
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001F0A8 File Offset: 0x0001D2A8
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

		// Token: 0x060005F9 RID: 1529 RVA: 0x0001EE4C File Offset: 0x0001D04C
		private void saveButton_Click(object sender, RoutedEventArgs e)
		{
			IOManager.SaveSettings<RLSettingsViewModel>(SB.rlSettingsFile, SB.Settings.RLSettings);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0001F138 File Offset: 0x0001D338
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/main/settings.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0001F168 File Offset: 0x0001D368
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.topMenu = (StackPanel)target;
				return;
			case 2:
				this.menuOptionRuriLib = (Label)target;
				this.menuOptionRuriLib.MouseDown += this.menuOptionRuriLib_MouseDown;
				return;
			case 3:
				this.menuOptionOpenBullet = (Label)target;
				this.menuOptionOpenBullet.MouseDown += this.menuOptionOpenBullet_MouseDown;
				return;
			case 4:
				this.Main = (Frame)target;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x040004EF RID: 1263
		private OBSettings OBSettingsPage = new OBSettings();

		// Token: 0x040004F0 RID: 1264
		private RLSettings RLSettingsPage = new RLSettings();

		// Token: 0x040004F1 RID: 1265
		internal StackPanel topMenu;

		// Token: 0x040004F2 RID: 1266
		internal Label menuOptionRuriLib;

		// Token: 0x040004F3 RID: 1267
		internal Label menuOptionOpenBullet;

		// Token: 0x040004F4 RID: 1268
		internal Frame Main;

		// Token: 0x040004F5 RID: 1269
		private bool _contentLoaded;
	}
}
