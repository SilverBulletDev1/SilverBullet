using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace OpenBullet
{
	// Token: 0x02000038 RID: 56
	public class WpfScreen
	{
		// Token: 0x06000126 RID: 294 RVA: 0x0000537C File Offset: 0x0000357C
		public static IEnumerable<WpfScreen> AllScreens()
		{
			foreach (Screen screen in Screen.AllScreens)
			{
				yield return new WpfScreen(screen);
			}
			Screen[] array = null;
			yield break;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00005385 File Offset: 0x00003585
		public static WpfScreen GetScreenFrom(Window window)
		{
			return new WpfScreen(Screen.FromHandle(new WindowInteropHelper(window).Handle));
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000539C File Offset: 0x0000359C
		public static WpfScreen GetScreenFrom(global::System.Windows.Point point)
		{
			int x = (int)Math.Round(point.X);
			int y = (int)Math.Round(point.Y);
			return new WpfScreen(Screen.FromPoint(new global::System.Drawing.Point(x, y)));
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000129 RID: 297 RVA: 0x000053D6 File Offset: 0x000035D6
		public static WpfScreen Primary
		{
			get
			{
				return new WpfScreen(Screen.PrimaryScreen);
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000053E2 File Offset: 0x000035E2
		internal WpfScreen(Screen screen)
		{
			this.screen = screen;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600012B RID: 299 RVA: 0x000053F1 File Offset: 0x000035F1
		public Rect DeviceBounds
		{
			get
			{
				return this.GetRect(this.screen.Bounds);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00005404 File Offset: 0x00003604
		public Rect WorkingArea
		{
			get
			{
				return this.GetRect(this.screen.WorkingArea);
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005418 File Offset: 0x00003618
		private Rect GetRect(Rectangle value)
		{
			return new Rect
			{
				X = (double)value.X,
				Y = (double)value.Y,
				Width = (double)value.Width,
				Height = (double)value.Height
			};
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000546C File Offset: 0x0000366C
		public Tuple<double, double> CenterWindowOnScreen(Rect workArea, double width, double height)
		{
			double num = (workArea.Width - width) / 2.0 + workArea.Left;
			double top = (workArea.Height - height) / 2.0 + workArea.Top;
			return new Tuple<double, double>(num, top);
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600012F RID: 303 RVA: 0x000054B6 File Offset: 0x000036B6
		public bool IsPrimary
		{
			get
			{
				return this.screen.Primary;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000130 RID: 304 RVA: 0x000054C3 File Offset: 0x000036C3
		public string DeviceName
		{
			get
			{
				return this.screen.DeviceName;
			}
		}

		// Token: 0x0400011F RID: 287
		private readonly Screen screen;
	}
}
