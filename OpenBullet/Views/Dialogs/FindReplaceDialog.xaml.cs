using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;

namespace OpenBullet.Views.Dialogs
{
	// Token: 0x02000117 RID: 279
	public partial class FindReplaceDialog : Window
	{
		// Token: 0x06000775 RID: 1909 RVA: 0x0002AD90 File Offset: 0x00028F90
		public FindReplaceDialog(TextEditor editor)
		{
			this.InitializeComponent();
			this.editor = editor;
			this.txtFind.Text = (this.txtFind2.Text = FindReplaceDialog.textToFind);
			this.cbCaseSensitive.IsChecked = new bool?(FindReplaceDialog.caseSensitive);
			this.cbWholeWord.IsChecked = new bool?(FindReplaceDialog.wholeWord);
			this.cbRegex.IsChecked = new bool?(FindReplaceDialog.useRegex);
			this.cbWildcards.IsChecked = new bool?(FindReplaceDialog.useWildcards);
			this.cbSearchUp.IsChecked = new bool?(FindReplaceDialog.searchUp);
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0002AE38 File Offset: 0x00029038
		private void Window_Closed(object sender, EventArgs e)
		{
			FindReplaceDialog.textToFind = this.txtFind2.Text;
			bool? flag = this.cbCaseSensitive.IsChecked;
			bool flag2 = true;
			FindReplaceDialog.caseSensitive = (flag.GetValueOrDefault() == flag2) & (flag != null);
			flag = this.cbWholeWord.IsChecked;
			flag2 = true;
			FindReplaceDialog.wholeWord = (flag.GetValueOrDefault() == flag2) & (flag != null);
			flag = this.cbRegex.IsChecked;
			flag2 = true;
			FindReplaceDialog.useRegex = (flag.GetValueOrDefault() == flag2) & (flag != null);
			flag = this.cbWildcards.IsChecked;
			flag2 = true;
			FindReplaceDialog.useWildcards = (flag.GetValueOrDefault() == flag2) & (flag != null);
			flag = this.cbSearchUp.IsChecked;
			flag2 = true;
			FindReplaceDialog.searchUp = (flag.GetValueOrDefault() == flag2) & (flag != null);
			FindReplaceDialog.theDialog = null;
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0002AF14 File Offset: 0x00029114
		private void FindNextClick(object sender, RoutedEventArgs e)
		{
			if (!this.FindNext(this.txtFind.Text))
			{
				SystemSounds.Beep.Play();
			}
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0002AF33 File Offset: 0x00029133
		private void FindNext2Click(object sender, RoutedEventArgs e)
		{
			if (!this.FindNext(this.txtFind2.Text))
			{
				SystemSounds.Beep.Play();
			}
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0002AF54 File Offset: 0x00029154
		private void ReplaceClick(object sender, RoutedEventArgs e)
		{
			Regex regEx = this.GetRegEx(this.txtFind2.Text, false);
			string input = this.editor.Text.Substring(this.editor.SelectionStart, this.editor.SelectionLength);
			Match match = regEx.Match(input);
			bool replaced = false;
			if (match.Success && match.Index == 0 && match.Length == input.Length)
			{
				this.editor.Document.Replace(this.editor.SelectionStart, this.editor.SelectionLength, this.txtReplace.Text);
				replaced = true;
			}
			if (!this.FindNext(this.txtFind2.Text) && !replaced)
			{
				SystemSounds.Beep.Play();
			}
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0002B014 File Offset: 0x00029214
		private void ReplaceAllClick(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show(string.Concat(new string[]
			{
				"Are you sure you want to Replace All occurences of \"",
				this.txtFind2.Text,
				"\" with \"",
				this.txtReplace.Text,
				"\"?"
			}), "Replace All", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
			{
				Regex regEx = this.GetRegEx(this.txtFind2.Text, true);
				int offset = 0;
				this.editor.BeginChange();
				foreach (object obj in regEx.Matches(this.editor.Text))
				{
					Match match = (Match)obj;
					this.editor.Document.Replace(offset + match.Index, match.Length, this.txtReplace.Text);
					offset += this.txtReplace.Text.Length - match.Length;
				}
				this.editor.EndChange();
			}
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0002B130 File Offset: 0x00029330
		private bool FindNext(string textToFind)
		{
			Regex regex = this.GetRegEx(textToFind, false);
			int start = (regex.Options.HasFlag(RegexOptions.RightToLeft) ? this.editor.SelectionStart : (this.editor.SelectionStart + this.editor.SelectionLength));
			Match match = regex.Match(this.editor.Text, start);
			if (!match.Success)
			{
				if (regex.Options.HasFlag(RegexOptions.RightToLeft))
				{
					match = regex.Match(this.editor.Text, this.editor.Text.Length);
				}
				else
				{
					match = regex.Match(this.editor.Text, 0);
				}
			}
			if (match.Success)
			{
				this.editor.Select(match.Index, match.Length);
				TextLocation loc = this.editor.Document.GetLocation(match.Index);
				this.editor.ScrollTo(loc.Line, loc.Column);
			}
			return match.Success;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0002B244 File Offset: 0x00029444
		private Regex GetRegEx(string textToFind, bool leftToRight = false)
		{
			RegexOptions options = RegexOptions.None;
			bool? flag = this.cbSearchUp.IsChecked;
			bool flag2 = true;
			if (((flag.GetValueOrDefault() == flag2) & (flag != null)) && !leftToRight)
			{
				options |= RegexOptions.RightToLeft;
			}
			flag = this.cbCaseSensitive.IsChecked;
			flag2 = false;
			if ((flag.GetValueOrDefault() == flag2) & (flag != null))
			{
				options |= RegexOptions.IgnoreCase;
			}
			flag = this.cbRegex.IsChecked;
			flag2 = true;
			if ((flag.GetValueOrDefault() == flag2) & (flag != null))
			{
				return new Regex(textToFind, options);
			}
			string pattern = Regex.Escape(textToFind);
			flag = this.cbWildcards.IsChecked;
			flag2 = true;
			if ((flag.GetValueOrDefault() == flag2) & (flag != null))
			{
				pattern = pattern.Replace("\\*", ".*").Replace("\\?", ".");
			}
			flag = this.cbWholeWord.IsChecked;
			flag2 = true;
			if ((flag.GetValueOrDefault() == flag2) & (flag != null))
			{
				pattern = "\\b" + pattern + "\\b";
			}
			return new Regex(pattern, options);
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0002B350 File Offset: 0x00029550
		public static void ShowForReplace(TextEditor editor)
		{
			if (FindReplaceDialog.theDialog == null)
			{
				FindReplaceDialog.theDialog = new FindReplaceDialog(editor);
				FindReplaceDialog.theDialog.tabMain.SelectedIndex = 1;
				FindReplaceDialog.theDialog.Show();
				FindReplaceDialog.theDialog.Activate();
			}
			else
			{
				FindReplaceDialog.theDialog.tabMain.SelectedIndex = 1;
				FindReplaceDialog.theDialog.Activate();
			}
			if (!editor.TextArea.Selection.IsMultiline)
			{
				FindReplaceDialog.theDialog.txtFind.Text = (FindReplaceDialog.theDialog.txtFind2.Text = editor.TextArea.Selection.GetText());
				FindReplaceDialog.theDialog.txtFind.SelectAll();
				FindReplaceDialog.theDialog.txtFind2.SelectAll();
				FindReplaceDialog.theDialog.txtFind2.Focus();
			}
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0002B424 File Offset: 0x00029624
		public static void ShowForFind(TextEditor editor)
		{
			if (FindReplaceDialog.theDialog == null)
			{
				FindReplaceDialog.theDialog = new FindReplaceDialog(editor);
				FindReplaceDialog.theDialog.tabMain.SelectedIndex = 0;
				FindReplaceDialog.theDialog.Show();
				FindReplaceDialog.theDialog.Activate();
				return;
			}
			FindReplaceDialog.theDialog.tabMain.SelectedIndex = 0;
			FindReplaceDialog.theDialog.Activate();
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0002B484 File Offset: 0x00029684
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				this.FindNextClick(null, null);
				return;
			}
			if (e.Key == Key.Escape)
			{
				base.Close();
			}
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0002B4A8 File Offset: 0x000296A8
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				this.txtFind.Focus();
			}
			catch
			{
			}
		}

		// Token: 0x04000626 RID: 1574
		private static string textToFind = "";

		// Token: 0x04000627 RID: 1575
		private static bool caseSensitive = true;

		// Token: 0x04000628 RID: 1576
		private static bool wholeWord = false;

		// Token: 0x04000629 RID: 1577
		private static bool useRegex = false;

		// Token: 0x0400062A RID: 1578
		private static bool useWildcards = false;

		// Token: 0x0400062B RID: 1579
		private static bool searchUp = false;

		// Token: 0x0400062C RID: 1580
		private TextEditor editor;

		// Token: 0x0400062D RID: 1581
		private static FindReplaceDialog theDialog = null;
	}
}
