using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using RuriLib;

namespace OpenBullet.Views.StackerBlocks
{
	// Token: 0x02000099 RID: 153
	public partial class PageBlockParse : Page
	{
		// Token: 0x060003FF RID: 1023 RVA: 0x000134E9 File Offset: 0x000116E9
		public PageBlockParse(BlockParse block)
		{
			this.InitializeComponent();
			this.vm = block;
			base.DataContext = this.vm;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001350A File Offset: 0x0001170A
		private void LRRadio_Checked(object sender, RoutedEventArgs e)
		{
			this.typeTabControl.SelectedIndex = 0;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00013518 File Offset: 0x00011718
		private void CSSRadio_Checked(object sender, RoutedEventArgs e)
		{
			this.typeTabControl.SelectedIndex = 1;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00013526 File Offset: 0x00011726
		private void JSONRadio_Checked(object sender, RoutedEventArgs e)
		{
			this.typeTabControl.SelectedIndex = 2;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00013534 File Offset: 0x00011734
		private void REGEXRadio_Checked(object sender, RoutedEventArgs e)
		{
			this.typeTabControl.SelectedIndex = 3;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00013544 File Offset: 0x00011744
		private void LRRTB_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Key == Key.LeftShift)
				{
					int length = new TextRange(this.LRRTB.Document.ContentStart, this.LRRTB.Selection.Start).Text.Length;
					int len = this.LRRTB.Selection.Text.Length;
					int end = length + len - 1;
					string left = "";
					string right = "";
					int index = length;
					while (index != 0)
					{
						left = this.LRRTB.GetText()[index - 1].ToString() + left;
						index--;
						if (BlockFunction.CountStringOccurrences(this.LRRTB.GetText(), left) <= 1)
						{
							break;
						}
					}
					index = end;
					while (index != this.LRRTB.GetText().Length - 1)
					{
						right += this.LRRTB.GetText()[index + 1].ToString();
						index++;
						if (BlockFunction.CountStringOccurrences(this.LRRTB.GetText(), right) <= 1)
						{
							break;
						}
					}
					this.vm.LeftString = left;
					this.vm.RightString = right;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00013694 File Offset: 0x00011894
		private void CaptureBox_Click(object sender, RoutedEventArgs e)
		{
			if (this.vm.IsCapture)
			{
				this.vm.CreateEmpty = false;
				return;
			}
			this.vm.CreateEmpty = true;
		}

		// Token: 0x04000320 RID: 800
		private BlockParse vm;
	}
}
