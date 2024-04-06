using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.UserControls.Filters
{
	// Token: 0x02000080 RID: 128
	public partial class UserControlInputTextAndBoolean : UserControl
	{
		// Token: 0x06000323 RID: 803 RVA: 0x0000D55F File Offset: 0x0000B75F
		public UserControlInputTextAndBoolean()
		{
			this.InitializeComponent();
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000324 RID: 804 RVA: 0x0000D570 File Offset: 0x0000B770
		// (remove) Token: 0x06000325 RID: 805 RVA: 0x0000D5A8 File Offset: 0x0000B7A8
		public event EventHandler SetFilter;

		// Token: 0x06000326 RID: 806 RVA: 0x0000D5E0 File Offset: 0x0000B7E0
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			e.Source = "InputTextAndBoolean";
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.InputTextBox.Text,
				this.CheckBox.IsChecked.GetValueOrDefault().ToString()
			}, e);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000D63C File Offset: 0x0000B83C
		private void CheckBox_Click(object sender, RoutedEventArgs e)
		{
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.InputTextBox.Text,
				this.CheckBox.IsChecked.GetValueOrDefault().ToString()
			}, new TextChangedEventArgs(e.RoutedEvent, UndoAction.None)
			{
				Source = "InputTextAndBoolean"
			});
		}

		// Token: 0x04000267 RID: 615
		public const string ControlName = "InputTextAndBoolean";
	}
}
