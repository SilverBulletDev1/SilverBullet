using System;
using System.Runtime.CompilerServices;
using OpenBullet.Views.StackerBlocks;
using RuriLib.Functions.Conditions;
using RuriLib.Models;
using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
	// Token: 0x02000140 RID: 320
	public class KeyViewModel : ViewModelBase
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x000309C5 File Offset: 0x0002EBC5
		// (set) Token: 0x06000925 RID: 2341 RVA: 0x000309D0 File Offset: 0x0002EBD0
		public KeyFullId Id
		{
			get
			{
				return this.id;
			}
			set
			{
				if (object.Equals(this.id, value))
				{
					return;
				}
				this.id = value;
				this.OnPropertyChanged("Id");
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x00030A00 File Offset: 0x0002EC00
		// (set) Token: 0x06000927 RID: 2343 RVA: 0x00030A08 File Offset: 0x0002EC08
		public Key Key
		{
			[CompilerGenerated]
			get
			{
				return this.<Key>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (object.Equals(this.<Key>k__BackingField, value))
				{
					return;
				}
				this.<Key>k__BackingField = value;
				this.OnPropertyChanged("LeftTerm");
				this.OnPropertyChanged("Comparer");
				this.OnPropertyChanged("RightTerm");
				this.OnPropertyChanged("Key");
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x00030A59 File Offset: 0x0002EC59
		// (set) Token: 0x06000929 RID: 2345 RVA: 0x00030A68 File Offset: 0x0002EC68
		public string LeftTerm
		{
			get
			{
				return this.Key.LeftTerm;
			}
			set
			{
				if (string.Equals(this.LeftTerm, value, StringComparison.Ordinal))
				{
					return;
				}
				this.Key.LeftTerm = value;
				this.OnPropertyChanged("LeftTerm");
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x00030A9E File Offset: 0x0002EC9E
		// (set) Token: 0x0600092B RID: 2347 RVA: 0x00030AAC File Offset: 0x0002ECAC
		public Comparer Comparer
		{
			get
			{
				return this.Key.Comparer;
			}
			set
			{
				if (this.Comparer == value)
				{
					return;
				}
				this.Key.Comparer = value;
				this.OnPropertyChanged("Comparer");
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x00030ADE File Offset: 0x0002ECDE
		// (set) Token: 0x0600092D RID: 2349 RVA: 0x00030AEC File Offset: 0x0002ECEC
		public string RightTerm
		{
			get
			{
				return this.Key.RightTerm;
			}
			set
			{
				if (string.Equals(this.RightTerm, value, StringComparison.Ordinal))
				{
					return;
				}
				this.Key.RightTerm = value;
				this.OnPropertyChanged("RightTerm");
			}
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x00030B22 File Offset: 0x0002ED22
		public KeyViewModel(Key key, int id, int parentId)
		{
			this.Key = key;
			this.Id = new KeyFullId
			{
				KeyId = id,
				ParentId = parentId
			};
			this.OnPropertyChanged("FullId");
			this.OnPropertyChanged("Id");
		}

		// Token: 0x04000727 RID: 1831
		private KeyFullId id;
	}
}
