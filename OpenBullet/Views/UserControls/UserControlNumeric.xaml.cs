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
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Primitives;

namespace OpenBullet.Views.UserControls
{
	// Token: 0x02000071 RID: 113
	public partial class UserControlNumeric : UserControl, IControl
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000BF53 File Offset: 0x0000A153
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0000BF5B File Offset: 0x0000A15B
		public int Minimum { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000BF64 File Offset: 0x0000A164
		// (set) Token: 0x060002CC RID: 716 RVA: 0x0000BF6C File Offset: 0x0000A16C
		public int Maximum { get; set; }

		// Token: 0x060002CD RID: 717 RVA: 0x0000BF78 File Offset: 0x0000A178
		public UserControlNumeric(int defaultValue, int minimum, int maximum, ViewModelBase viewModel = null)
		{
			this.InitializeComponent();
			base.DataContext = this;
			this.Minimum = minimum;
			this.Maximum = maximum;
			this.SetValue(defaultValue);
			this.viewModel = viewModel;
			if (viewModel != null)
			{
				viewModel.PropertyChanged += this.ViewModel_PropertyChanged;
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000BFD1 File Offset: 0x0000A1D1
		private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.SetValue(this.viewModel.GetType().GetProperty(e.PropertyName).GetValue(this.viewModel));
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000BFFA File Offset: 0x0000A1FA
		[return: Dynamic]
		public dynamic GetValue()
		{
			return this.valueNumeric.Value;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000C00C File Offset: 0x0000A20C
		public void SetValue(dynamic value)
		{
			UpDownBase<int?> upDownBase = this.valueNumeric;
			if (UserControlNumeric.<>o__12.<>p__0 == null)
			{
				UserControlNumeric.<>o__12.<>p__0 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(int), typeof(UserControlNumeric)));
			}
			upDownBase.Value = new int?(UserControlNumeric.<>o__12.<>p__0.Target(UserControlNumeric.<>o__12.<>p__0, value));
		}

		// Token: 0x04000224 RID: 548
		private ViewModelBase viewModel;
	}
}
