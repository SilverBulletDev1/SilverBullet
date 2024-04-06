using System;

namespace OpenBullet.Models
{
	// Token: 0x02000154 RID: 340
	public class ControlText<T>
	{
		// Token: 0x060009B7 RID: 2487 RVA: 0x000325CE File Offset: 0x000307CE
		public ControlText(T cType, string text)
		{
			this.Control = cType;
			this.Text = text;
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x000325E4 File Offset: 0x000307E4
		// (set) Token: 0x060009B9 RID: 2489 RVA: 0x000325EC File Offset: 0x000307EC
		public T Control { get; private set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x000325F5 File Offset: 0x000307F5
		// (set) Token: 0x060009BB RID: 2491 RVA: 0x000325FD File Offset: 0x000307FD
		public string Text { get; private set; }
	}
}
