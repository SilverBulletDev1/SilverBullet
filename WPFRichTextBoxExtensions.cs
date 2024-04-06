using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;

// Token: 0x02000007 RID: 7
public static class WPFRichTextBoxExtensions
{
	// Token: 0x0600000F RID: 15 RVA: 0x0000229C File Offset: 0x0000049C
	public static void AppendText(this RichTextBox box, string text, global::System.Windows.Media.Color color)
	{
		box.SelectionStart = box.TextLength;
		box.SelectionLength = 0;
		box.SelectionColor = global::System.Drawing.Color.FromArgb((int)color.A, (int)color.R, (int)color.G, (int)color.B);
		box.AppendText(text);
		box.SelectionColor = box.ForeColor;
		box.AppendText(Environment.NewLine);
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002304 File Offset: 0x00000504
	public static void AppendTextToEditor(this TextEditor editor, string text, global::System.Windows.Media.Color color)
	{
		editor.TextArea.ClearSelection();
		editor.TextArea.TextView.LineTransformers.Add(new LineColorizer(editor.LineCount, new SolidColorBrush(global::System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B))));
		editor.AppendText(text);
		editor.AppendText(Environment.NewLine);
	}
}
