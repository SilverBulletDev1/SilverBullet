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
	// Token: 0x02000075 RID: 117
	public partial class UserControlTextMulti : UserControl, IControl
	{
		// Token: 0x060002D9 RID: 729 RVA: 0x0000C1E8 File Offset: 0x0000A3E8
		public UserControlTextMulti(string[] defaultValue, bool readOnly = false, ViewModelBase viewModel = null)
		{
			this.InitializeComponent();
			base.DataContext = this;
			this.valueTextbox.IsReadOnly = readOnly;
			this.SetValue(defaultValue);
			this.viewModel = viewModel;
			if (viewModel != null)
			{
				viewModel.PropertyChanged += this.ViewModel_PropertyChanged;
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000C237 File Offset: 0x0000A437
		private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.SetValue(this.viewModel.GetType().GetProperty(e.PropertyName).GetValue(this.viewModel));
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000C260 File Offset: 0x0000A460
		[return: Dynamic]
		public dynamic GetValue()
		{
			return this.valueTextbox.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000C28C File Offset: 0x0000A48C
		public void SetValue(dynamic value)
		{
			if (UserControlTextMulti.<>o__4.<>p__0 == null)
			{
				UserControlTextMulti.<>o__4.<>p__0 = CallSite<Func<CallSite, object, string[]>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string[]), typeof(UserControlTextMulti)));
			}
			string[] val = UserControlTextMulti.<>o__4.<>p__0.Target(UserControlTextMulti.<>o__4.<>p__0, value);
			this.valueTextbox.Text = string.Join(Environment.NewLine, val);
		}

		// Token: 0x0400022E RID: 558
		private ViewModelBase viewModel;
	}
}
