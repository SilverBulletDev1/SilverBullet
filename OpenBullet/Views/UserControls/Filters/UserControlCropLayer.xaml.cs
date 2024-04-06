using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ImageProcessor.Imaging;

namespace OpenBullet.Views.UserControls.Filters
{
	// Token: 0x02000079 RID: 121
	public partial class UserControlCropLayer : UserControl
	{
		// Token: 0x060002F1 RID: 753 RVA: 0x0000C8E1 File Offset: 0x0000AAE1
		public UserControlCropLayer()
		{
			this.InitializeComponent();
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060002F2 RID: 754 RVA: 0x0000C8F0 File Offset: 0x0000AAF0
		// (remove) Token: 0x060002F3 RID: 755 RVA: 0x0000C928 File Offset: 0x0000AB28
		public event EventHandler SetFilter;

		// Token: 0x060002F4 RID: 756 RVA: 0x0000C960 File Offset: 0x0000AB60
		private void TextBoxCL_TextChanged(object sender, TextChangedEventArgs e)
		{
			e.Source = "CropLayer";
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.LeftTextBox.Text,
				this.TopTextBox.Text,
				this.RightTextBox.Text,
				this.BottomTextBox.Text,
				this.CropModeBox.SelectedItem.ToString()
			}, e);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000C9DC File Offset: 0x0000ABDC
		private void CropModeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.LeftTextBox.Text,
				this.TopTextBox.Text,
				this.RightTextBox.Text,
				this.BottomTextBox.Text,
				this.CropModeBox.SelectedItem.ToString()
			}, new TextChangedEventArgs(e.RoutedEvent, UndoAction.None)
			{
				Source = "CropLayer"
			});
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000CA64 File Offset: 0x0000AC64
		private void UserControl_Initialized(object sender, EventArgs e)
		{
			try
			{
				Enum.GetNames(typeof(CropMode)).ToList<string>().ForEach(delegate(string c)
				{
					this.CropModeBox.Items.Add(c);
				});
			}
			catch
			{
			}
		}

		// Token: 0x04000240 RID: 576
		public const string ControlName = "CropLayer";
	}
}
