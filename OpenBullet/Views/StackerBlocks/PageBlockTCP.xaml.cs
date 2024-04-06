using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using RuriLib;

namespace OpenBullet.Views.StackerBlocks
{
	// Token: 0x0200009F RID: 159
	public partial class PageBlockTCP : Page
	{
		// Token: 0x06000423 RID: 1059 RVA: 0x000144A0 File Offset: 0x000126A0
		public PageBlockTCP(BlockTCP block)
		{
			this.InitializeComponent();
			this.block = block;
			base.DataContext = this.block;
			foreach (string c in Enum.GetNames(typeof(TCPCommand)))
			{
				this.tcpCommandCombobox.Items.Add(c);
			}
			this.tcpCommandCombobox.SelectedIndex = (int)block.TCPCommand;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00014514 File Offset: 0x00012714
		private void tcpCommandCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.block.TCPCommand = (TCPCommand)((ComboBox)e.OriginalSource).SelectedIndex;
			TCPCommand tcpcommand = this.block.TCPCommand;
			if (tcpcommand == TCPCommand.Connect)
			{
				this.commandTabControl.SelectedIndex = 1;
				return;
			}
			if (tcpcommand != TCPCommand.Send)
			{
				this.commandTabControl.SelectedIndex = 0;
				return;
			}
			this.commandTabControl.SelectedIndex = 2;
		}

		// Token: 0x0400034D RID: 845
		private BlockTCP block;
	}
}
