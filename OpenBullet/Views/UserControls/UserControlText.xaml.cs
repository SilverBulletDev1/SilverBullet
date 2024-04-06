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
	// Token: 0x02000073 RID: 115
	public partial class UserControlText : UserControl, IControl
	{
		// Token: 0x060002D3 RID: 723 RVA: 0x0000C0B8 File Offset: 0x0000A2B8
		public UserControlText(string defaultValue, bool readOnly = false, ViewModelBase viewModel = null)
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

		// Token: 0x060002D4 RID: 724 RVA: 0x0000C107 File Offset: 0x0000A307
		private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.SetValue(this.viewModel.GetType().GetProperty(e.PropertyName).GetValue(this.viewModel));
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000C130 File Offset: 0x0000A330
		[return: Dynamic]
		public dynamic GetValue()
		{
			return this.valueTextbox.Text;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000C140 File Offset: 0x0000A340
		public void SetValue(dynamic value)
		{
			TextBox textBox = this.valueTextbox;
			if (UserControlText.<>o__4.<>p__0 == null)
			{
				UserControlText.<>o__4.<>p__0 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(UserControlText)));
			}
			textBox.Text = UserControlText.<>o__4.<>p__0.Target(UserControlText.<>o__4.<>p__0, value);
		}

		// Token: 0x0400022A RID: 554
		private ViewModelBase viewModel;
	}
}
