using System;

namespace OpenBullet.Editor.CustomSearch
{
	// Token: 0x0200015F RID: 351
	public class SearchOptionsChangedEventArgs : EventArgs
	{
		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060009F5 RID: 2549 RVA: 0x00032DA5 File Offset: 0x00030FA5
		// (set) Token: 0x060009F6 RID: 2550 RVA: 0x00032DAD File Offset: 0x00030FAD
		public string SearchPattern { get; private set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060009F7 RID: 2551 RVA: 0x00032DB6 File Offset: 0x00030FB6
		// (set) Token: 0x060009F8 RID: 2552 RVA: 0x00032DBE File Offset: 0x00030FBE
		public bool MatchCase { get; private set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060009F9 RID: 2553 RVA: 0x00032DC7 File Offset: 0x00030FC7
		// (set) Token: 0x060009FA RID: 2554 RVA: 0x00032DCF File Offset: 0x00030FCF
		public bool UseRegex { get; private set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060009FB RID: 2555 RVA: 0x00032DD8 File Offset: 0x00030FD8
		// (set) Token: 0x060009FC RID: 2556 RVA: 0x00032DE0 File Offset: 0x00030FE0
		public bool WholeWords { get; private set; }

		// Token: 0x060009FD RID: 2557 RVA: 0x00032DE9 File Offset: 0x00030FE9
		public SearchOptionsChangedEventArgs(string searchPattern, bool matchCase, bool useRegex, bool wholeWords)
		{
			this.SearchPattern = searchPattern;
			this.MatchCase = matchCase;
			this.UseRegex = useRegex;
			this.WholeWords = wholeWords;
		}
	}
}
