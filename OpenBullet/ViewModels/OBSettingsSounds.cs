using System;
using System.Collections.Generic;
using System.Reflection;
using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
	// Token: 0x02000128 RID: 296
	public class OBSettingsSounds : ViewModelBase
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x0002DF30 File Offset: 0x0002C130
		// (set) Token: 0x06000861 RID: 2145 RVA: 0x0002DF38 File Offset: 0x0002C138
		public bool EnableSounds
		{
			get
			{
				return this.enableSounds;
			}
			set
			{
				if (this.enableSounds == value)
				{
					return;
				}
				this.enableSounds = value;
				this.OnPropertyChanged("EnableSounds");
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x0002DF65 File Offset: 0x0002C165
		// (set) Token: 0x06000863 RID: 2147 RVA: 0x0002DF70 File Offset: 0x0002C170
		public string OnHitSound
		{
			get
			{
				return this.onHitSound;
			}
			set
			{
				if (string.Equals(this.onHitSound, value, StringComparison.Ordinal))
				{
					return;
				}
				this.onHitSound = value;
				this.OnPropertyChanged("OnHitSound");
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x0002DFA1 File Offset: 0x0002C1A1
		// (set) Token: 0x06000865 RID: 2149 RVA: 0x0002DFAC File Offset: 0x0002C1AC
		public string OnReloadSound
		{
			get
			{
				return this.onReloadSound;
			}
			set
			{
				if (string.Equals(this.onReloadSound, value, StringComparison.Ordinal))
				{
					return;
				}
				this.onReloadSound = value;
				this.OnPropertyChanged("OnReloadSound");
			}
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0002DFE0 File Offset: 0x0002C1E0
		public void Reset()
		{
			OBSettingsSounds def = new OBSettingsSounds();
			foreach (PropertyInfo prop in ((IEnumerable<PropertyInfo>)new List<PropertyInfo>(typeof(OBSettingsSounds).GetProperties())))
			{
				prop.SetValue(this, prop.GetValue(def, null));
			}
		}

		// Token: 0x0400069E RID: 1694
		private bool enableSounds;

		// Token: 0x0400069F RID: 1695
		private string onHitSound = "rifle_hit.wav";

		// Token: 0x040006A0 RID: 1696
		private string onReloadSound = "rifle_reload.wav";
	}
}
