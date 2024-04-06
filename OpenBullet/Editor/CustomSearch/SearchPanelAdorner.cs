using System;
using System.Windows.Documents;
using ICSharpCode.AvalonEdit.Editing;

namespace OpenBullet.Editor.CustomSearch
{
	// Token: 0x02000160 RID: 352
	internal class SearchPanelAdorner : Adorner
	{
		// Token: 0x060009FE RID: 2558 RVA: 0x00032E0E File Offset: 0x0003100E
		public SearchPanelAdorner(TextArea textArea, SearchTextEditor panel)
			: base(textArea)
		{
			this.panel = panel;
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060009FF RID: 2559 RVA: 0x000027E2 File Offset: 0x000009E2
		protected override int VisualChildrenCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x04000784 RID: 1924
		private SearchTextEditor panel;
	}
}
