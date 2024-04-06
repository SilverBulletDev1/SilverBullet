using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using RuriLib;

namespace OpenBullet.Views.StackerBlocks
{
	// Token: 0x02000098 RID: 152
	public partial class PageBlockLSCode : Page
	{
		// Token: 0x060003FB RID: 1019 RVA: 0x000133B4 File Offset: 0x000115B4
		public PageBlockLSCode(BlockLSCode block)
		{
			this.InitializeComponent();
			this.block = block;
			base.DataContext = this.block;
			this.scriptEditor.Text = block.Script;
			this.scriptEditor.ShowLineNumbers = true;
			this.scriptEditor.TextArea.Foreground = new SolidColorBrush(Colors.Gainsboro);
			this.scriptEditor.TextArea.TextView.LinkTextForegroundBrush = new SolidColorBrush(Colors.DodgerBlue);
			using (XmlReader reader = XmlReader.Create("LSHighlighting.xshd"))
			{
				this.scriptEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00013474 File Offset: 0x00011674
		private void openDocButton_Click(object sender, RoutedEventArgs e)
		{
			new MainDialog(new DialogLSDoc(), "LoliScript Documentation").Show();
		}

		// Token: 0x0400031C RID: 796
		private BlockLSCode block;
	}
}
