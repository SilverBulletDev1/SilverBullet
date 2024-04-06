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
	// Token: 0x02000087 RID: 135
	public partial class UserControlThreshold : UserControl
	{
		// Token: 0x06000361 RID: 865 RVA: 0x0000E627 File Offset: 0x0000C827
		public UserControlThreshold()
		{
			this.InitializeComponent();
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000362 RID: 866 RVA: 0x0000E638 File Offset: 0x0000C838
		// (remove) Token: 0x06000363 RID: 867 RVA: 0x0000E670 File Offset: 0x0000C870
		public event EventHandler SetFilter;

		// Token: 0x06000364 RID: 868 RVA: 0x0000E6A8 File Offset: 0x0000C8A8
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			e.Source = "Threshold";
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.ThreshTextBox.Text,
				this.MaxValueTextBox.Text,
				this.ThresholdTypeComboBox.SelectedItem.ToString()
			}, e);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000E708 File Offset: 0x0000C908
		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.ThreshTextBox.Text,
				this.MaxValueTextBox.Text,
				this.ThresholdTypeComboBox.SelectedItem.ToString()
			}, new TextChangedEventArgs(e.RoutedEvent, UndoAction.None)
			{
				Source = "Threshold"
			});
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000E774 File Offset: 0x0000C974
		private void UserControl_Initialized(object sender, EventArgs e)
		{
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

		// Token: 0x04000295 RID: 661
		public const string ControlName = "Threshold";
	}
}
