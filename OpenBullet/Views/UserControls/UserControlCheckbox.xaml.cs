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
	// Token: 0x02000061 RID: 97
	public partial class UserControlCheckbox : UserControl, IControl
	{
		// Token: 0x0600026D RID: 621 RVA: 0x0000AF6D File Offset: 0x0000916D
		public UserControlCheckbox(bool defaultValue, ViewModelBase viewModel = null)
		{
			this.InitializeComponent();
			base.DataContext = this;
			this.SetValue(defaultValue);
			this.viewModel = viewModel;
			if (viewModel != null)
			{
				viewModel.PropertyChanged += this.ViewModel_PropertyChanged;
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000AFAA File Offset: 0x000091AA
		private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.SetValue(this.viewModel.GetType().GetProperty(e.PropertyName).GetValue(this.viewModel));
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000AFD4 File Offset: 0x000091D4
		[return: Dynamic]
		public dynamic GetValue()
		{
			return this.valueCheckbox.IsChecked.Value;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000AFFC File Offset: 0x000091FC
		public void SetValue(dynamic value)
		{
			ToggleButton toggleButton = this.valueCheckbox;
			if (UserControlCheckbox.<>o__4.<>p__0 == null)
			{
				UserControlCheckbox.<>o__4.<>p__0 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(bool), typeof(UserControlCheckbox)));
			}
			toggleButton.IsChecked = new bool?(UserControlCheckbox.<>o__4.<>p__0.Target(UserControlCheckbox.<>o__4.<>p__0, value));
		}

		// Token: 0x040001EE RID: 494
		private ViewModelBase viewModel;
	}
}
