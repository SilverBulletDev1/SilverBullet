using System;
using System.Windows.Controls;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CustomSearch;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Search;

namespace OpenBullet.Editor.CustomSearch
{
	// Token: 0x0200015E RID: 350
	public class SearchTextEditor
	{
		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x00032997 File Offset: 0x00030B97
		public int Count
		{
			get
			{
				if (this.renderer != null)
				{
					return this.renderer.CurrentResults.Count;
				}
				return 0;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060009E4 RID: 2532 RVA: 0x000329B3 File Offset: 0x00030BB3
		// (set) Token: 0x060009E5 RID: 2533 RVA: 0x000329BB File Offset: 0x00030BBB
		public string SearchPattern { get; set; }

		// Token: 0x060009E6 RID: 2534 RVA: 0x000329C4 File Offset: 0x00030BC4
		public void UpdateSearch()
		{
			this.strategy = ICSharpCode.AvalonEdit.CustomSearch.SearchStrategyFactory.Create(this.SearchPattern ?? "", true, false, ICSharpCode.AvalonEdit.CustomSearch.SearchMode.Normal);
			this.OnSearchOptionsChanged(new SearchOptionsChangedEventArgs(this.SearchPattern, true, false, false));
			this.DoSearch(true);
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00002A58 File Offset: 0x00000C58
		private SearchTextEditor()
		{
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x000329FE File Offset: 0x00030BFE
		public static SearchTextEditor Install(TextEditor editor)
		{
			if (editor == null)
			{
				throw new ArgumentNullException("editor");
			}
			return SearchTextEditor.Install(editor.TextArea);
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00032A19 File Offset: 0x00030C19
		public static SearchTextEditor Install(TextArea textArea)
		{
			if (textArea == null)
			{
				throw new ArgumentNullException("textArea");
			}
			SearchTextEditor searchTextEditor = new SearchTextEditor();
			searchTextEditor.AttachInternal(textArea);
			return searchTextEditor;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x00032A38 File Offset: 0x00030C38
		private void AttachInternal(TextArea textArea)
		{
			this.textArea = textArea;
			this.renderer = new SearchResultBackgroundRenderer();
			this.currentDocument = textArea.Document;
			if (this.currentDocument != null)
			{
				this.currentDocument.TextChanged += this.textArea_Document_TextChanged;
			}
			textArea.DocumentChanged += this.textArea_DocumentChanged;
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x00032A94 File Offset: 0x00030C94
		private void textArea_DocumentChanged(object sender, EventArgs e)
		{
			if (this.currentDocument != null)
			{
				this.currentDocument.TextChanged -= this.textArea_Document_TextChanged;
			}
			this.currentDocument = this.textArea.Document;
			if (this.currentDocument != null)
			{
				this.currentDocument.TextChanged += this.textArea_Document_TextChanged;
				this.DoSearch(false);
			}
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00032AF7 File Offset: 0x00030CF7
		private void textArea_Document_TextChanged(object sender, EventArgs e)
		{
			this.DoSearch(false);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00032B00 File Offset: 0x00030D00
		public void Reactivate()
		{
			if (this.searchTextBox == null)
			{
				return;
			}
			this.searchTextBox.Focus();
			this.searchTextBox.SelectAll();
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00032B24 File Offset: 0x00030D24
		public void FindNext()
		{
			SearchResult result = this.renderer.CurrentResults.FindFirstSegmentWithStartAfter(this.textArea.Caret.Offset + 1);
			if (result == null)
			{
				result = this.renderer.CurrentResults.FirstSegment;
			}
			if (result != null)
			{
				this.SelectResult(result);
			}
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00032B74 File Offset: 0x00030D74
		public void FindPrevious()
		{
			SearchResult result = this.renderer.CurrentResults.FindFirstSegmentWithStartAfter(this.textArea.Caret.Offset);
			if (result != null)
			{
				result = this.renderer.CurrentResults.GetPreviousSegment(result);
			}
			if (result == null)
			{
				result = this.renderer.CurrentResults.LastSegment;
			}
			if (result != null)
			{
				this.SelectResult(result);
			}
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00032BD8 File Offset: 0x00030DD8
		public void DoSearch(bool changeSelection)
		{
			this.renderer.CurrentResults.Clear();
			if (!string.IsNullOrEmpty(this.SearchPattern))
			{
				int offset = this.textArea.Caret.Offset;
				if (changeSelection)
				{
					this.textArea.ClearSelection();
				}
				foreach (ICSharpCode.AvalonEdit.CustomSearch.ISearchResult searchResult in this.strategy.FindAll(this.textArea.Document, 0, this.textArea.Document.TextLength))
				{
					SearchResult result = (SearchResult)searchResult;
					if (changeSelection && result.StartOffset >= offset)
					{
						this.SelectResult(result);
						changeSelection = false;
					}
					this.renderer.CurrentResults.Add(result);
				}
			}
			this.textArea.TextView.InvalidateLayer(KnownLayer.Selection);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x00032CBC File Offset: 0x00030EBC
		private void SelectResult(SearchResult result)
		{
			this.textArea.Caret.Offset = result.StartOffset;
			this.textArea.Selection = Selection.Create(this.textArea, result.StartOffset, result.EndOffset);
			this.textArea.Caret.BringCaretToView();
			this.textArea.Caret.Show();
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060009F2 RID: 2546 RVA: 0x00032D24 File Offset: 0x00030F24
		// (remove) Token: 0x060009F3 RID: 2547 RVA: 0x00032D5C File Offset: 0x00030F5C
		public event EventHandler<SearchOptionsChangedEventArgs> SearchOptionsChanged;

		// Token: 0x060009F4 RID: 2548 RVA: 0x00032D91 File Offset: 0x00030F91
		protected virtual void OnSearchOptionsChanged(SearchOptionsChangedEventArgs e)
		{
			EventHandler<SearchOptionsChangedEventArgs> searchOptionsChanged = this.SearchOptionsChanged;
			if (searchOptionsChanged == null)
			{
				return;
			}
			searchOptionsChanged(this, e);
		}

		// Token: 0x04000778 RID: 1912
		private TextArea textArea;

		// Token: 0x04000779 RID: 1913
		private SearchInputHandler handler;

		// Token: 0x0400077A RID: 1914
		private TextDocument currentDocument;

		// Token: 0x0400077B RID: 1915
		private SearchResultBackgroundRenderer renderer;

		// Token: 0x0400077C RID: 1916
		private TextBox searchTextBox;

		// Token: 0x0400077D RID: 1917
		private ICSharpCode.AvalonEdit.CustomSearch.ISearchStrategy strategy;
	}
}
