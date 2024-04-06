using System;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace OpenBullet.AvalonEdit
{
	// Token: 0x0200015D RID: 349
	public class TruncateLongLines : VisualLineElementGenerator
	{
		// Token: 0x060009E0 RID: 2528 RVA: 0x0003291C File Offset: 0x00030B1C
		public override int GetFirstInterestedOffset(int startOffset)
		{
			DocumentLine line = base.CurrentContext.VisualLine.LastDocumentLine;
			if (line.Length > 2000)
			{
				int ellipsisOffset = line.Offset + 2000 - 100 - "...".Length;
				if (startOffset <= ellipsisOffset)
				{
					return ellipsisOffset;
				}
			}
			return -1;
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00032969 File Offset: 0x00030B69
		public override VisualLineElement ConstructElement(int offset)
		{
			return new FormattedTextElement("...", base.CurrentContext.VisualLine.LastDocumentLine.EndOffset - offset - 100);
		}

		// Token: 0x04000775 RID: 1909
		private const int maxLength = 2000;

		// Token: 0x04000776 RID: 1910
		private const string ellipsis = "...";

		// Token: 0x04000777 RID: 1911
		private const int charactersAfterEllipsis = 100;
	}
}
