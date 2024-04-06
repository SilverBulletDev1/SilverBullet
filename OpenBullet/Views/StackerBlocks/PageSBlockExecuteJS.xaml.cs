using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using RuriLib;

namespace OpenBullet.Views.StackerBlocks
{
	// Token: 0x020000A3 RID: 163
	public partial class PageSBlockExecuteJS : Page
	{
		// Token: 0x0600043D RID: 1085 RVA: 0x0001534C File Offset: 0x0001354C
		public PageSBlockExecuteJS(SBlockExecuteJS block)
		{
			this.InitializeComponent();
			this.vm = block;
			base.DataContext = this.vm;
			this.javascriptCodeEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("JavaScript");
			this.javascriptCodeEditor.ShowLineNumbers = true;
			this.javascriptCodeEditor.Text = this.vm.JavascriptCode;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x000153B4 File Offset: 0x000135B4
		private void javascriptCodeEditor_LostFocus(object sender, EventArgs e)
		{
			this.vm.JavascriptCode = this.javascriptCodeEditor.Text;
		}

		// Token: 0x0400037F RID: 895
		private SBlockExecuteJS vm;
	}
}
