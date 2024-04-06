using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.UserControls.Filters
{
	// Token: 0x02000085 RID: 133
	public partial class UserControlResize : UserControl
	{
		// Token: 0x06000352 RID: 850 RVA: 0x0000E1FE File Offset: 0x0000C3FE
		public UserControlResize()
		{
			this.InitializeComponent();
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000353 RID: 851 RVA: 0x0000E20C File Offset: 0x0000C40C
		// (remove) Token: 0x06000354 RID: 852 RVA: 0x0000E244 File Offset: 0x0000C444
		public event EventHandler SetFilter;

		// Token: 0x06000355 RID: 853 RVA: 0x0000E279 File Offset: 0x0000C479
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			e.Source = "Resize";
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.WidthTextBox.Text,
				this.HeightTextBox.Text
			}, e);
		}

		// Token: 0x0400028A RID: 650
		public const string ControlName = "Resize";
	}
}
