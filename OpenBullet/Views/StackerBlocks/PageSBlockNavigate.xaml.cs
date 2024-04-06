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
	// Token: 0x020000A4 RID: 164
	public partial class PageSBlockNavigate : Page
	{
		// Token: 0x06000441 RID: 1089 RVA: 0x0001542D File Offset: 0x0001362D
		public PageSBlockNavigate(SBlockNavigate block)
		{
			this.InitializeComponent();
			this.vm = block;
			base.DataContext = this.vm;
		}

		// Token: 0x04000382 RID: 898
		private SBlockNavigate vm;
	}
}
