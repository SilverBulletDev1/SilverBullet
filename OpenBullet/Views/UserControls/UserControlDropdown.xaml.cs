using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using Microsoft.CSharp.RuntimeBinder;
using OpenBullet.Plugins;
using RuriLib.ViewModels;

namespace OpenBullet.Views.UserControls
{
	// Token: 0x02000065 RID: 101
	public partial class UserControlDropdown : UserControl, IControl
	{
		// Token: 0x06000280 RID: 640 RVA: 0x0000B22C File Offset: 0x0000942C
		public UserControlDropdown(string value, string[] options, ViewModelBase viewModel = null)
		{
			this.InitializeComponent();
			base.DataContext = this;
			foreach (string option in options)
			{
				this.valueDropdown.Items.Add(option);
			}
			this.SetValue(value);
			this.viewModel = viewModel;
			if (viewModel != null)
			{
				viewModel.PropertyChanged += this.ViewModel_PropertyChanged;
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000B295 File Offset: 0x00009495
		private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.SetValue(this.viewModel.GetType().GetProperty(e.PropertyName).GetValue(this.viewModel));
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000B2BE File Offset: 0x000094BE
		[return: Dynamic]
		public dynamic GetValue()
		{
			return this.valueDropdown.SelectedValue;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000B2CC File Offset: 0x000094CC
		public void SetValue(dynamic value)
		{
			Selector selector = this.valueDropdown;
			if (UserControlDropdown.<>o__4.<>p__0 == null)
			{
				UserControlDropdown.<>o__4.<>p__0 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(UserControlDropdown)));
			}
			selector.SelectedValue = UserControlDropdown.<>o__4.<>p__0.Target(UserControlDropdown.<>o__4.<>p__0, value);
		}

		// Token: 0x040001F9 RID: 505
		private ViewModelBase viewModel;
	}
}
