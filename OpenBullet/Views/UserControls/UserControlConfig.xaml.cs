using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.CSharp.RuntimeBinder;
using OpenBullet.Plugins;
using RuriLib.ViewModels;

namespace OpenBullet.Views.UserControls
{
	// Token: 0x02000067 RID: 103
	public partial class UserControlConfig : UserControl, IControl
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000B372 File Offset: 0x00009572
		// (set) Token: 0x06000287 RID: 647 RVA: 0x0000B37A File Offset: 0x0000957A
		public ConfigViewModel Config { get; set; }

		// Token: 0x06000288 RID: 648 RVA: 0x0000B383 File Offset: 0x00009583
		public UserControlConfig()
		{
			this.InitializeComponent();
			base.DataContext = this;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000B398 File Offset: 0x00009598
		[return: Dynamic]
		public dynamic GetValue()
		{
			return this.Config;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000B3A0 File Offset: 0x000095A0
		public void SetValue(dynamic value)
		{
			if (UserControlConfig.<>o__6.<>p__0 == null)
			{
				UserControlConfig.<>o__6.<>p__0 = CallSite<Func<CallSite, object, ConfigViewModel>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(ConfigViewModel), typeof(UserControlConfig)));
			}
			this.Config = UserControlConfig.<>o__6.<>p__0.Target(UserControlConfig.<>o__6.<>p__0, value);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000B3F4 File Offset: 0x000095F4
		private void Choose_Click(object sender, RoutedEventArgs e)
		{
			new MainDialog(new DialogSelectConfig(this), "Select a Config").ShowDialog();
			if (this.Config != null)
			{
				this.ConfigName.Text = this.Config.Name;
			}
		}
	}
}
