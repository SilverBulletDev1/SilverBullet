using System;
using System.Collections.Generic;
using System.Linq;
using PluginFramework;
using RuriLib;

namespace OpenBullet
{
	// Token: 0x0200001A RID: 26
	public static class BlocksExtensions
	{
		// Token: 0x06000061 RID: 97 RVA: 0x00002BCA File Offset: 0x00000DCA
		public static IEnumerable<BlockBase> OnlyPlugins(this IEnumerable<BlockBase> blocks)
		{
			return blocks.Where((BlockBase b) => b.IsPlugin());
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002BF1 File Offset: 0x00000DF1
		public static bool IsPlugin(this BlockBase block)
		{
			return block.GetType().GetInterface("IBlockPlugin") == typeof(IBlockPlugin);
		}
	}
}
