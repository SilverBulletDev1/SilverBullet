using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.UserControls.Filters
{
	// Token: 0x0200007C RID: 124
	public partial class UserControlFastNlMeansDenoisingColored : UserControl
	{
		// Token: 0x0600030E RID: 782 RVA: 0x0000D023 File Offset: 0x0000B223
		public UserControlFastNlMeansDenoisingColored()
		{
			this.InitializeComponent();
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600030F RID: 783 RVA: 0x0000D034 File Offset: 0x0000B234
		// (remove) Token: 0x06000310 RID: 784 RVA: 0x0000D06C File Offset: 0x0000B26C
		public event EventHandler SetFilter;

		// Token: 0x06000311 RID: 785 RVA: 0x0000D0A1 File Offset: 0x0000B2A1
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			e.Source = "FastNlMeansDenoisingColored";
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.StrengthTextBox.Text,
				this.ColorStrengthTextBox.Text
			}, e);
		}

		// Token: 0x04000253 RID: 595
		public const string ControlName = "FastNlMeansDenoisingColored";
	}
}
