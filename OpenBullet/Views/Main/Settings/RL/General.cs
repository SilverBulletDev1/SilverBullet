using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using RuriLib.ViewModels;

namespace OpenBullet.Views.Main.Settings.RL
{
	// Token: 0x020000F6 RID: 246
	public class General : Page, IComponentConnector
	{
		// Token: 0x06000649 RID: 1609 RVA: 0x00021064 File Offset: 0x0001F264
		public General()
		{
			this.InitializeComponent();
			base.DataContext = SB.Settings.RLSettings.General;
			foreach (string i in Enum.GetNames(typeof(BotsDisplayMode)))
			{
				this.botsDisplayModeCombobox.Items.Add(i);
			}
			this.botsDisplayModeCombobox.SelectedIndex = (int)SB.Settings.RLSettings.General.BotsDisplayMode;
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x000210E5 File Offset: 0x0001F2E5
		private void botsDisplayModeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SB.Settings.RLSettings.General.BotsDisplayMode = (BotsDisplayMode)this.botsDisplayModeCombobox.SelectedIndex;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00021108 File Offset: 0x0001F308
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/main/settings/rurilib/general.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00021138 File Offset: 0x0001F338
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				this.botsDisplayModeCombobox = (ComboBox)target;
				this.botsDisplayModeCombobox.SelectionChanged += this.botsDisplayModeCombobox_SelectionChanged;
				return;
			}
			this._contentLoaded = true;
		}

		// Token: 0x0400053F RID: 1343
		internal ComboBox botsDisplayModeCombobox;

		// Token: 0x04000540 RID: 1344
		private bool _contentLoaded;
	}
}
