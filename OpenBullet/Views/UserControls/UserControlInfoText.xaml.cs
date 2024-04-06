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
	// Token: 0x02000069 RID: 105
	public partial class UserControlInfoText : UserControl, IControl
	{
		// Token: 0x0600028E RID: 654 RVA: 0x0000B4AB File Offset: 0x000096AB
		public UserControlInfoText(string defaultValue, ViewModelBase viewModel = null)
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

		// Token: 0x0600028F RID: 655 RVA: 0x0000B4E3 File Offset: 0x000096E3
		private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.SetValue(this.viewModel.GetType().GetProperty(e.PropertyName).GetValue(this.viewModel));
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000B50C File Offset: 0x0000970C
		[return: Dynamic]
		public dynamic GetValue()
		{
			return this.valueLabel.Content;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000B51C File Offset: 0x0000971C
		public void SetValue(dynamic value)
		{
			ContentControl contentControl = this.valueLabel;
			if (UserControlInfoText.<>o__4.<>p__0 == null)
			{
				UserControlInfoText.<>o__4.<>p__0 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(UserControlInfoText)));
			}
			contentControl.Content = UserControlInfoText.<>o__4.<>p__0.Target(UserControlInfoText.<>o__4.<>p__0, value);
		}

		// Token: 0x04000202 RID: 514
		private ViewModelBase viewModel;
	}
}
