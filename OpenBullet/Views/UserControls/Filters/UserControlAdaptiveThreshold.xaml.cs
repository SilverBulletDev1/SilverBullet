using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using OpenCvSharp;

namespace OpenBullet.Views.UserControls.Filters
{
	// Token: 0x02000077 RID: 119
	public partial class UserControlAdaptiveThreshold : UserControl
	{
		// Token: 0x060002DF RID: 735 RVA: 0x0000C33E File Offset: 0x0000A53E
		public UserControlAdaptiveThreshold()
		{
			this.InitializeComponent();
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060002E0 RID: 736 RVA: 0x0000C34C File Offset: 0x0000A54C
		// (remove) Token: 0x060002E1 RID: 737 RVA: 0x0000C384 File Offset: 0x0000A584
		public event EventHandler SetFilter;

		// Token: 0x060002E2 RID: 738 RVA: 0x0000C3BC File Offset: 0x0000A5BC
		private void UserControl_Initialized(object sender, EventArgs e)
		{
			try
			{
				Enum.GetNames(typeof(AdaptiveThresholdTypes)).ToList<string>().ForEach(delegate(string a)
				{
					this.AdaptiveMethodComboBox.Items.Add(a);
				});
			}
			catch
			{
			}
			try
			{
				Enum.GetNames(typeof(ThresholdTypes)).ToList<string>().ForEach(delegate(string t)
				{
					this.ThresholdTypeComboBox.Items.Add(t);
				});
			}
			catch
			{
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000C43C File Offset: 0x0000A63C
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			e.Source = "AdaptiveThreshold";
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.MaxValueTextBox.Text,
				this.AdaptiveMethodComboBox.SelectedItem.ToString(),
				this.ThresholdTypeComboBox.SelectedItem.ToString(),
				this.BlockSizeTextBox.Text,
				this.ConstantTextBox.Text
			}, e);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000C4BC File Offset: 0x0000A6BC
		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.MaxValueTextBox.Text,
				this.AdaptiveMethodComboBox.SelectedItem.ToString(),
				this.ThresholdTypeComboBox.SelectedItem.ToString(),
				this.BlockSizeTextBox.Text,
				this.ConstantTextBox.Text
			}, new TextChangedEventArgs(e.RoutedEvent, UndoAction.None)
			{
				Source = "AdaptiveThreshold"
			});
		}

		// Token: 0x04000232 RID: 562
		public const string ControlName = "AdaptiveThreshold";
	}
}
