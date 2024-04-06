using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace OpenBullet.Views.Main
{
	// Token: 0x020000AD RID: 173
	public partial class ReleaseNotesPage : Page
	{
		// Token: 0x06000465 RID: 1125 RVA: 0x00015CA2 File Offset: 0x00013EA2
		public ReleaseNotesPage()
		{
			this.InitializeComponent();
			base.DataContext = this;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00015CB8 File Offset: 0x00013EB8
		private void AppendNote(string[] notes)
		{
			foreach (string note in notes)
			{
				Bold bold = new Bold(new Run("• "));
				bold.SetResourceReference(TextElement.ForegroundProperty, "ForegroundMain");
				Paragraph paragraph = new Paragraph(bold);
				paragraph.SetResourceReference(TextElement.ForegroundProperty, "ForegroundMain");
				paragraph.Inlines.Add(new Run(note));
				this.richTextBox.Document.Blocks.Add(paragraph);
			}
			Paragraph endPar = new Paragraph(new Bold(new Run("========================")));
			endPar.SetResourceReference(TextElement.ForegroundProperty, "ForegroundMain");
			this.richTextBox.Document.Blocks.Add(endPar);
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x00015D74 File Offset: 0x00013F74
		public string App
		{
			get
			{
				return "Silver Bullet 1.1.4";
			}
		}
	}
}
