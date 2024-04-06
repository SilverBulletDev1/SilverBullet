using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.UserControls.Filters
{
	// Token: 0x02000078 RID: 120
	public partial class UserControlBlur : UserControl
	{
		// Token: 0x060002E9 RID: 745 RVA: 0x0000C6A5 File Offset: 0x0000A8A5
		public UserControlBlur()
		{
			this.InitializeComponent();
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060002EA RID: 746 RVA: 0x0000C6B4 File Offset: 0x0000A8B4
		// (remove) Token: 0x060002EB RID: 747 RVA: 0x0000C6EC File Offset: 0x0000A8EC
		public event EventHandler SetFilter;

		// Token: 0x060002EC RID: 748 RVA: 0x0000C724 File Offset: 0x0000A924
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			e.Source = "Blur";
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.RadiusTextBox.Text,
				this.SigmaTextBox.Text,
				this.ChannelsComboBox.SelectedItem.ToString()
			}, e);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000C784 File Offset: 0x0000A984
		private void ChannelsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.ChannelsComboBox.Visibility != Visibility.Visible)
			{
				return;
			}
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.RadiusTextBox.Text,
				this.SigmaTextBox.Text,
				this.ChannelsComboBox.SelectedItem.ToString()
			}, new TextChangedEventArgs(e.RoutedEvent, UndoAction.None)
			{
				Source = "Blur"
			});
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00003B20 File Offset: 0x00001D20
		private void UserControl_Initialized(object sender, EventArgs e)
		{
		}

		// Token: 0x0400023A RID: 570
		public const string ControlName = "Blur";
	}
}
