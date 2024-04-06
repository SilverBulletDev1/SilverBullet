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
	// Token: 0x0200008E RID: 142
	public partial class PageBlockWebSocket : Page
	{
		// Token: 0x060003AD RID: 941 RVA: 0x000115FC File Offset: 0x0000F7FC
		public PageBlockWebSocket(BlockWebSocket block)
		{
			this.InitializeComponent();
			this.vm = block;
			base.DataContext = block;
			this.customCookiesRTB.AppendText(this.vm.GetCustomHeaders());
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0001163C File Offset: 0x0000F83C
		private void wsCommandCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.vm == null)
			{
				return;
			}
			this.vm.Command = (WSCommand)((ComboBox)e.OriginalSource).SelectedIndex;
			WSCommand command = this.vm.Command;
			if (command == WSCommand.Connect)
			{
				this.wsCommandTabControl.SelectedIndex = 1;
				return;
			}
			if (command != WSCommand.Send)
			{
				this.wsCommandTabControl.SelectedIndex = 0;
				return;
			}
			this.wsCommandTabControl.SelectedIndex = 2;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x000116A8 File Offset: 0x0000F8A8
		private void customCookiesRTB_LostFocus(object sender, RoutedEventArgs e)
		{
			try
			{
				this.vm.SetCustomCookies(this.customCookiesRTB.Lines());
			}
			catch
			{
			}
		}

		// Token: 0x040002CF RID: 719
		private BlockWebSocket vm;
	}
}
