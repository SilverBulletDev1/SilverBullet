using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using RuriLib.Runner;

namespace OpenBullet
{
	// Token: 0x02000031 RID: 49
	public class DialogSetProxies : Page, IComponentConnector
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00004485 File Offset: 0x00002685
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x0000448D File Offset: 0x0000268D
		public object Caller { get; set; }

		// Token: 0x060000F4 RID: 244 RVA: 0x00004496 File Offset: 0x00002696
		public DialogSetProxies(object caller)
		{
			this.InitializeComponent();
			this.Caller = caller;
			this.proxiesDefaultRadio.IsChecked = new bool?(true);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000044BC File Offset: 0x000026BC
		private void selectButton_Click(object sender, RoutedEventArgs e)
		{
			ProxyMode mode = ProxyMode.Default;
			if (this.proxiesDefaultRadio.IsChecked.Value)
			{
				mode = ProxyMode.Default;
			}
			else if (this.proxiesOnRadio.IsChecked.Value)
			{
				mode = ProxyMode.On;
			}
			else if (this.proxiesOffRadio.IsChecked.Value)
			{
				mode = ProxyMode.Off;
			}
			if (this.Caller.GetType() == typeof(RunnerViewModel))
			{
				(this.Caller as RunnerViewModel).ProxyMode = mode;
			}
			((MainDialog)base.Parent).Close();
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004554 File Offset: 0x00002754
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/dialogs/dialogsetproxies.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004584 File Offset: 0x00002784
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.proxiesDefaultRadio = (RadioButton)target;
				return;
			case 2:
				this.proxiesOnRadio = (RadioButton)target;
				return;
			case 3:
				this.proxiesOffRadio = (RadioButton)target;
				return;
			case 4:
				this.selectButton = (Button)target;
				this.selectButton.Click += this.selectButton_Click;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x040000FB RID: 251
		internal RadioButton proxiesDefaultRadio;

		// Token: 0x040000FC RID: 252
		internal RadioButton proxiesOnRadio;

		// Token: 0x040000FD RID: 253
		internal RadioButton proxiesOffRadio;

		// Token: 0x040000FE RID: 254
		internal Button selectButton;

		// Token: 0x040000FF RID: 255
		private bool _contentLoaded;
	}
}
