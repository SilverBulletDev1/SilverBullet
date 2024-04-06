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
	// Token: 0x0200007A RID: 122
	public partial class UserControlCvtColor : UserControl
	{
		// Token: 0x060002FA RID: 762 RVA: 0x0000CBF5 File Offset: 0x0000ADF5
		public UserControlCvtColor()
		{
			this.InitializeComponent();
			this.Init();
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000CC0C File Offset: 0x0000AE0C
		private void Init()
		{
			try
			{
				Enum.GetNames(typeof(ColorConversionCodes)).ToList<string>().ForEach(delegate(string c)
				{
					this.CodeComboBox.Items.Add(c);
				});
			}
			catch
			{
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060002FC RID: 764 RVA: 0x0000CC54 File Offset: 0x0000AE54
		// (remove) Token: 0x060002FD RID: 765 RVA: 0x0000CC8C File Offset: 0x0000AE8C
		public event EventHandler SetFilter;

		// Token: 0x060002FE RID: 766 RVA: 0x0000CCC4 File Offset: 0x0000AEC4
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			e.Source = "CvtColor";
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.CodeComboBox.SelectedItem.ToString(),
				this.dstCnTextBox.Text
			}, e);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000CD14 File Offset: 0x0000AF14
		private void CodeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.CodeComboBox.Items.Count == 0)
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
				this.CodeComboBox.SelectedItem.ToString(),
				this.dstCnTextBox.Text
			}, new TextChangedEventArgs(e.RoutedEvent, UndoAction.None)
			{
				Source = "CvtColor"
			});
		}

		// Token: 0x04000248 RID: 584
		public const string ControlName = "CvtColor";
	}
}
