using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.UserControls.Filters
{
	// Token: 0x02000084 RID: 132
	public partial class UserControlReplaceColor : UserControl
	{
		// Token: 0x0600034C RID: 844 RVA: 0x0000E05E File Offset: 0x0000C25E
		public UserControlReplaceColor()
		{
			this.InitializeComponent();
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600034D RID: 845 RVA: 0x0000E06C File Offset: 0x0000C26C
		// (remove) Token: 0x0600034E RID: 846 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public event EventHandler SetFilter;

		// Token: 0x0600034F RID: 847 RVA: 0x0000E0DC File Offset: 0x0000C2DC
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			e.Source = "ReplaceColor";
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.TargetTextBox.Text,
				this.ReplacementTextBox.Text,
				this.FuzzinessTextBox.Text
			}, e);
		}

		// Token: 0x04000284 RID: 644
		public const string ControlName = "ReplaceColor";
	}
}
