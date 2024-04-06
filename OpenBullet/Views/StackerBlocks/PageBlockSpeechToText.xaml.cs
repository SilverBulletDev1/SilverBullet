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
	// Token: 0x0200008D RID: 141
	public partial class PageBlockSpeechToText : Page
	{
		// Token: 0x060003AA RID: 938 RVA: 0x0001158E File Offset: 0x0000F78E
		public PageBlockSpeechToText(BlockSpeechToText block)
		{
			this.InitializeComponent();
			this.vm = block;
			base.DataContext = this.vm;
		}

		// Token: 0x040002CC RID: 716
		private BlockSpeechToText vm;
	}
}
