using System;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

// Token: 0x02000008 RID: 8
internal class LineColorizer : DocumentColorizingTransformer
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000011 RID: 17 RVA: 0x00002374 File Offset: 0x00000574
	// (set) Token: 0x06000012 RID: 18 RVA: 0x0000237C File Offset: 0x0000057C
	public int LineNumber { get; set; }

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000013 RID: 19 RVA: 0x00002385 File Offset: 0x00000585
	// (set) Token: 0x06000014 RID: 20 RVA: 0x0000238D File Offset: 0x0000058D
	public Brush Brush { get; set; }

	// Token: 0x06000015 RID: 21 RVA: 0x00002396 File Offset: 0x00000596
	public LineColorizer(int lineNumber, Brush brush)
	{
		this.LineNumber = lineNumber;
		this.Brush = brush;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x000023AC File Offset: 0x000005AC
	protected override void ColorizeLine(DocumentLine line)
	{
		if (!line.IsDeleted && line.LineNumber == this.LineNumber)
		{
			base.ChangeLinePart(line.Offset, line.EndOffset, new Action<VisualLineElement>(this.ApplyChanges));
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000023E2 File Offset: 0x000005E2
	private void ApplyChanges(VisualLineElement element)
	{
		element.TextRunProperties.SetForegroundBrush(this.Brush);
	}
}
