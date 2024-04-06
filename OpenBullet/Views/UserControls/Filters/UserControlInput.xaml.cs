using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.UserControls.Filters
{
	// Token: 0x0200007E RID: 126
	public partial class UserControlInput : UserControl
	{
		// Token: 0x0600031A RID: 794 RVA: 0x0000D31A File Offset: 0x0000B51A
		public UserControlInput()
		{
			this.InitializeComponent();
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600031B RID: 795 RVA: 0x0000D328 File Offset: 0x0000B528
		// (remove) Token: 0x0600031C RID: 796 RVA: 0x0000D360 File Offset: 0x0000B560
		public event EventHandler SetFilter;

		// Token: 0x0600031D RID: 797 RVA: 0x0000D395 File Offset: 0x0000B595
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			e.Source = "Input";
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[] { (sender as TextBox).Text }, e);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000D3C7 File Offset: 0x0000B5C7
		private void UserControl_Initialized(object sender, EventArgs e)
		{
			this.SetInputType(UserControlInput.InputType.Text);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000D3D0 File Offset: 0x0000B5D0
		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox inputBox = sender as ComboBox;
			if (inputBox.Visibility != Visibility.Visible)
			{
				return;
			}
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[] { (inputBox.SelectedItem as ComboBoxItem).Content.ToString() }, new TextChangedEventArgs(e.RoutedEvent, UndoAction.None)
			{
				Source = "Input"
			});
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000D434 File Offset: 0x0000B634
		internal void SetInputType(UserControlInput.InputType inputType)
		{
			if (inputType == UserControlInput.InputType.Text)
			{
				this.InputComboBox.Visibility = Visibility.Collapsed;
				this.InputTextBox.Visibility = Visibility.Visible;
				return;
			}
			if (inputType != UserControlInput.InputType.Boolean)
			{
				this.InputTextBox.Visibility = Visibility.Visible;
				this.InputComboBox.Visibility = Visibility.Collapsed;
				return;
			}
			this.InputTextBox.Visibility = Visibility.Collapsed;
			this.InputComboBox.Visibility = Visibility.Visible;
		}

		// Token: 0x0400025E RID: 606
		public const string ControlName = "Input";

		// Token: 0x0200007F RID: 127
		public enum InputType
		{
			// Token: 0x04000265 RID: 613
			Text,
			// Token: 0x04000266 RID: 614
			Boolean
		}
	}
}
