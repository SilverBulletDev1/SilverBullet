using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.UserControls.Filters
{
	// Token: 0x02000083 RID: 131
	public partial class UserControlNegate : UserControl
	{
		// Token: 0x06000344 RID: 836 RVA: 0x0000DE39 File Offset: 0x0000C039
		public UserControlNegate()
		{
			this.InitializeComponent();
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000345 RID: 837 RVA: 0x0000DE48 File Offset: 0x0000C048
		// (remove) Token: 0x06000346 RID: 838 RVA: 0x0000DE80 File Offset: 0x0000C080
		public event EventHandler SetFilter;

		// Token: 0x06000347 RID: 839 RVA: 0x00003B20 File Offset: 0x00001D20
		private void UserControl_Initialized(object sender, EventArgs e)
		{
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000DEB8 File Offset: 0x0000C0B8
		private void ChannelsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.ChannelsComboBox.Items.Count == 0)
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
				this.CheckBoxOnlyGrayscale.IsChecked.GetValueOrDefault().ToString(),
				this.ChannelsComboBox.SelectedItem.ToString()
			}, new TextChangedEventArgs(e.RoutedEvent, UndoAction.None)
			{
				Source = "Negate"
			});
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000DF38 File Offset: 0x0000C138
		private void CheckBoxOnlyGrayscale_Click(object sender, RoutedEventArgs e)
		{
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.CheckBoxOnlyGrayscale.IsChecked.GetValueOrDefault().ToString(),
				this.ChannelsComboBox.SelectedItem.ToString()
			}, new TextChangedEventArgs(e.RoutedEvent, UndoAction.None)
			{
				Source = "Negate"
			});
		}

		// Token: 0x0400027F RID: 639
		public const string ControlName = "Negate";
	}
}
