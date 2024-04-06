using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using RuriLib.ViewModels;

namespace OpenBullet.Views.Main.Settings.RL
{
	// Token: 0x020000F0 RID: 240
	public class Selenium : Page, IComponentConnector
	{
		// Token: 0x06000629 RID: 1577 RVA: 0x00020220 File Offset: 0x0001E420
		public Selenium()
		{
			this.InitializeComponent();
			base.DataContext = SB.Settings.RLSettings.Selenium;
			foreach (string i in Enum.GetNames(typeof(BrowserType)))
			{
				this.browserTypeCombobox.Items.Add(i);
			}
			this.browserTypeCombobox.SelectedIndex = (int)SB.Settings.RLSettings.Selenium.Browser;
			foreach (string ext in SB.Settings.RLSettings.Selenium.ChromeExtensions)
			{
				this.extensionsBox.AppendText(ext + Environment.NewLine);
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00020308 File Offset: 0x0001E508
		private void browserTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SB.Settings.RLSettings.Selenium.Browser = (BrowserType)this.browserTypeCombobox.SelectedIndex;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00020329 File Offset: 0x0001E529
		private void extensionsBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			SB.Settings.RLSettings.Selenium.ChromeExtensions = this.extensionsBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList<string>();
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00020364 File Offset: 0x0001E564
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/main/settings/rurilib/selenium.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00020394 File Offset: 0x0001E594
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				this.browserTypeCombobox = (ComboBox)target;
				this.browserTypeCombobox.SelectionChanged += this.browserTypeCombobox_SelectionChanged;
				return;
			}
			if (connectionId != 2)
			{
				this._contentLoaded = true;
				return;
			}
			this.extensionsBox = (TextBox)target;
			this.extensionsBox.TextChanged += this.extensionsBox_TextChanged;
		}

		// Token: 0x0400051C RID: 1308
		internal ComboBox browserTypeCombobox;

		// Token: 0x0400051D RID: 1309
		internal TextBox extensionsBox;

		// Token: 0x0400051E RID: 1310
		private bool _contentLoaded;
	}
}
