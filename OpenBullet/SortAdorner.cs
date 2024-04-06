using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace OpenBullet
{
	// Token: 0x0200004C RID: 76
	public class SortAdorner : Adorner
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00007E65 File Offset: 0x00006065
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x00007E6D File Offset: 0x0000606D
		public ListSortDirection Direction { get; private set; }

		// Token: 0x060001B6 RID: 438 RVA: 0x00007E76 File Offset: 0x00006076
		public SortAdorner(UIElement element, ListSortDirection dir)
			: base(element)
		{
			this.Direction = dir;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00007E88 File Offset: 0x00006088
		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			if (base.AdornedElement.RenderSize.Width < 20.0)
			{
				return;
			}
			TranslateTransform transform = new TranslateTransform(base.AdornedElement.RenderSize.Width - 15.0, (base.AdornedElement.RenderSize.Height - 5.0) / 2.0);
			drawingContext.PushTransform(transform);
			Geometry geometry = SortAdorner.ascGeometry;
			if (this.Direction == ListSortDirection.Descending)
			{
				geometry = SortAdorner.descGeometry;
			}
			drawingContext.DrawGeometry(Brushes.Black, null, geometry);
			drawingContext.Pop();
		}

		// Token: 0x04000184 RID: 388
		private static Geometry ascGeometry = Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

		// Token: 0x04000185 RID: 389
		private static Geometry descGeometry = Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");
	}
}
