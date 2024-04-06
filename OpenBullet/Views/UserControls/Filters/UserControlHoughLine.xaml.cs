using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.UserControls.Filters
{
	// Token: 0x0200007D RID: 125
	public partial class UserControlHoughLine : UserControl
	{
		// Token: 0x06000314 RID: 788 RVA: 0x0000D17A File Offset: 0x0000B37A
		public UserControlHoughLine()
		{
			this.InitializeComponent();
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000315 RID: 789 RVA: 0x0000D188 File Offset: 0x0000B388
		// (remove) Token: 0x06000316 RID: 790 RVA: 0x0000D1C0 File Offset: 0x0000B3C0
		public event EventHandler SetFilter;

		// Token: 0x06000317 RID: 791 RVA: 0x0000D1F8 File Offset: 0x0000B3F8
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			e.Source = "HoughLine";
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.WidthTextBox.Text,
				this.HeightTextBox.Text,
				this.ThresholdTextBox.Text
			}, e);
		}

		// Token: 0x04000258 RID: 600
		public const string ControlName = "HoughLine";
	}
}
