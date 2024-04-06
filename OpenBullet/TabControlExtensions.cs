using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace OpenBullet
{
	// Token: 0x02000021 RID: 33
	public static class TabControlExtensions
	{
		// Token: 0x06000075 RID: 117 RVA: 0x00002E30 File Offset: 0x00001030
		public static TabItem GetItemByItemName(this IEnumerable<TabItem> tabItems, string name)
		{
			return tabItems.FirstOrDefault(delegate(TabItem i)
			{
				object header = i.Header;
				return ((header != null) ? header.ToString() : null) == name;
			});
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002E5C File Offset: 0x0000105C
		public static int GetIndexByItemName(this TabControl tabControl, string name)
		{
			return tabControl.Items.IndexOf(tabControl.Items.OfType<TabItem>().GetItemByItemName(name));
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002E7C File Offset: 0x0000107C
		public static int SelectIndexByHeaderName(this TabControl tabControl, string headerName)
		{
			return tabControl.SelectedIndex = tabControl.GetIndexByItemName(headerName);
		}
	}
}
