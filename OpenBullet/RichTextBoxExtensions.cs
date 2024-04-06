using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace OpenBullet
{
	// Token: 0x0200001F RID: 31
	public static class RichTextBoxExtensions
	{
		// Token: 0x0600006C RID: 108 RVA: 0x00002CC7 File Offset: 0x00000EC7
		public static void AppendText(this RichTextBox box, string text, Color color)
		{
			new TextRange(box.Document.ContentEnd, box.Document.ContentEnd)
			{
				Text = text
			}.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002CFB File Offset: 0x00000EFB
		public static string[] Lines(this RichTextBox box)
		{
			return new TextRange(box.Document.ContentStart, box.Document.ContentEnd).Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002D31 File Offset: 0x00000F31
		public static string GetText(this RichTextBox box)
		{
			return new TextRange(box.Document.ContentStart, box.Document.ContentEnd).Text;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002D53 File Offset: 0x00000F53
		public static string GetTextFromLines(this RichTextBox box)
		{
			return box.Lines().Aggregate((string current, string next) => current + next);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002D80 File Offset: 0x00000F80
		public static string Select(this RichTextBox rtb, int offset, int length, Color color)
		{
			TextSelection selection = rtb.Selection;
			TextPointer contentStart = rtb.Document.ContentStart;
			TextPointer startPos = RichTextBoxExtensions.GetTextPointAt(contentStart, offset);
			TextPointer endPos = RichTextBoxExtensions.GetTextPointAt(contentStart, offset + length);
			selection.Select(startPos, endPos);
			selection.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(color));
			return rtb.Selection.Text;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002DD4 File Offset: 0x00000FD4
		public static TextPointer GetTextPointAt(TextPointer from, int pos)
		{
			TextPointer ret = from;
			int i = 0;
			while (i < pos && ret != null)
			{
				if (ret.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.Text || ret.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.None)
				{
					i++;
				}
				if (ret.GetPositionAtOffset(1, LogicalDirection.Forward) == null)
				{
					return ret;
				}
				ret = ret.GetPositionAtOffset(1, LogicalDirection.Forward);
			}
			return ret;
		}
	}
}
