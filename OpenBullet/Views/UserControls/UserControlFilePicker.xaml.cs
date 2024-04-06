using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Win32;
using OpenBullet.Plugins;

namespace OpenBullet.Views.UserControls
{
	// Token: 0x0200006F RID: 111
	public partial class UserControlFilePicker : UserControl, IControl
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000BDF3 File Offset: 0x00009FF3
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x0000BDFB File Offset: 0x00009FFB
		public string Filter { get; set; }

		// Token: 0x060002C3 RID: 707 RVA: 0x0000BE04 File Offset: 0x0000A004
		public UserControlFilePicker(string location, string filter)
		{
			this.InitializeComponent();
			base.DataContext = this;
			this.Filter = filter;
			this.SetValue(location);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000BE27 File Offset: 0x0000A027
		[return: Dynamic]
		public dynamic GetValue()
		{
			return this.Location.Text;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000BE34 File Offset: 0x0000A034
		public void SetValue(dynamic value)
		{
			TextBox location = this.Location;
			if (UserControlFilePicker.<>o__6.<>p__0 == null)
			{
				UserControlFilePicker.<>o__6.<>p__0 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(UserControlFilePicker)));
			}
			location.Text = UserControlFilePicker.<>o__6.<>p__0.Target(UserControlFilePicker.<>o__6.<>p__0, value);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000BE90 File Offset: 0x0000A090
		private void Choose_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = this.Filter;
			bool? result = ofd.ShowDialog();
			if (result != null && result.Value)
			{
				this.SetValue(ofd.FileName);
			}
		}
	}
}
