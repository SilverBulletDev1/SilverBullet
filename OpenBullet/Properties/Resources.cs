using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace OpenBullet.Properties
{
	// Token: 0x0200005A RID: 90
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x06000247 RID: 583 RVA: 0x00002A58 File Offset: 0x00000C58
		internal Resources()
		{
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000AA1C File Offset: 0x00008C1C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					Resources.resourceMan = new ResourceManager("OpenBullet.Properties.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000AA48 File Offset: 0x00008C48
		// (set) Token: 0x0600024A RID: 586 RVA: 0x0000AA4F File Offset: 0x00008C4F
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000AA57 File Offset: 0x00008C57
		internal static Bitmap SB_PRO
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("SB_PRO", Resources.resourceCulture);
			}
		}

		// Token: 0x040001DA RID: 474
		private static ResourceManager resourceMan;

		// Token: 0x040001DB RID: 475
		private static CultureInfo resourceCulture;
	}
}
