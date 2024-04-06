using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.Main.Settings.RL
{
	// Token: 0x020000EE RID: 238
	public class CefSharp : Page, IComponentConnector
	{
		// Token: 0x0600061C RID: 1564 RVA: 0x0001FDBD File Offset: 0x0001DFBD
		public CefSharp()
		{
			this.InitializeComponent();
			base.DataContext = SB.Settings.RLSettings.CefSharp;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0001FDE0 File Offset: 0x0001DFE0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/main/settings/rurilib/cefsharp.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0001FE10 File Offset: 0x0001E010
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x04000510 RID: 1296
		private bool _contentLoaded;
	}
}
