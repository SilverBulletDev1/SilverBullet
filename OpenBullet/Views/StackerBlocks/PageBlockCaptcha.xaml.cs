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
	// Token: 0x02000094 RID: 148
	public partial class PageBlockCaptcha : Page
	{
		// Token: 0x060003CC RID: 972 RVA: 0x00011C15 File Offset: 0x0000FE15
		public PageBlockCaptcha(BlockImageCaptcha block)
		{
			this.InitializeComponent();
			this.vm = block;
			base.DataContext = this.vm;
		}

		// Token: 0x040002E6 RID: 742
		private BlockImageCaptcha vm;
	}
}
