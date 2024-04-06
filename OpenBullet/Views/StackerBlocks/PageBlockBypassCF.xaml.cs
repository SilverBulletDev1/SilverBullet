using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using RuriLib;
using RuriLib.Functions.Requests;

namespace OpenBullet.Views.StackerBlocks
{
	// Token: 0x02000093 RID: 147
	public partial class PageBlockBypassCF : Page
	{
		// Token: 0x060003C8 RID: 968 RVA: 0x00011B20 File Offset: 0x0000FD20
		public PageBlockBypassCF(BlockBypassCF block)
		{
			this.InitializeComponent();
			this.vm = block;
			base.DataContext = this.vm;
			foreach (string s in Enum.GetNames(typeof(SecurityProtocol)))
			{
				this.securityProtocolCombobox.Items.Add(s);
			}
			this.securityProtocolCombobox.SelectedIndex = (int)this.vm.SecurityProtocol;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00011B96 File Offset: 0x0000FD96
		private void securityProtocolCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.SecurityProtocol = (SecurityProtocol)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x040002E3 RID: 739
		private BlockBypassCF vm;
	}
}
