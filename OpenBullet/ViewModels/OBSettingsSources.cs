using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using OpenBullet.Models;
using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
	// Token: 0x02000129 RID: 297
	public class OBSettingsSources : ViewModelBase
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x0002E06A File Offset: 0x0002C26A
		// (set) Token: 0x06000869 RID: 2153 RVA: 0x0002E074 File Offset: 0x0002C274
		public ObservableCollection<Source> Sources
		{
			[CompilerGenerated]
			get
			{
				return this.<Sources>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (object.Equals(this.<Sources>k__BackingField, value))
				{
					return;
				}
				this.<Sources>k__BackingField = value;
				this.OnPropertyChanged("Sources");
			}
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0002E0A4 File Offset: 0x0002C2A4
		public void RemoveSourceById(int id)
		{
			this.Sources.Remove(this.GetSourceById(id));
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0002E0BC File Offset: 0x0002C2BC
		public Source GetSourceById(int id)
		{
			return this.Sources.FirstOrDefault((Source s) => s.Id == id);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0002E0F0 File Offset: 0x0002C2F0
		public void Reset()
		{
			OBSettingsSources def = new OBSettingsSources();
			foreach (PropertyInfo prop in ((IEnumerable<PropertyInfo>)new List<PropertyInfo>(typeof(OBSettingsSources).GetProperties())))
			{
				prop.SetValue(this, prop.GetValue(def, null));
			}
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0002E15C File Offset: 0x0002C35C
		public OBSettingsSources()
		{
			this.<Sources>k__BackingField = new ObservableCollection<Source>();
			base..ctor();
		}
	}
}
