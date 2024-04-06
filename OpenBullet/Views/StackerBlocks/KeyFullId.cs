using System;

namespace OpenBullet.Views.StackerBlocks
{
	// Token: 0x02000097 RID: 151
	public class KeyFullId
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x00013349 File Offset: 0x00011549
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x00013351 File Offset: 0x00011551
		public int KeyId { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0001335A File Offset: 0x0001155A
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x00013362 File Offset: 0x00011562
		public int ParentId { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0001336B File Offset: 0x0001156B
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x00013373 File Offset: 0x00011573
		public bool LeftTermInitialized { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0001337C File Offset: 0x0001157C
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x00013384 File Offset: 0x00011584
		public bool ConditionInitialized { get; set; }

		// Token: 0x060003FA RID: 1018 RVA: 0x0001338D File Offset: 0x0001158D
		public KeyFullId()
		{
			this.KeyId = 0;
			this.ParentId = 0;
			this.LeftTermInitialized = false;
			this.ConditionInitialized = false;
		}
	}
}
