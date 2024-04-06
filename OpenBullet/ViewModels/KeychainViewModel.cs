using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using RuriLib.Models;
using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
	// Token: 0x0200013E RID: 318
	public class KeychainViewModel : ViewModelBase
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x0003053C File Offset: 0x0002E73C
		// (set) Token: 0x0600090B RID: 2315 RVA: 0x00030544 File Offset: 0x0002E744
		public int Id
		{
			get
			{
				return this.id;
			}
			set
			{
				if (this.id == value)
				{
					return;
				}
				this.id = value;
				this.OnPropertyChanged("Id");
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x00030571 File Offset: 0x0002E771
		// (set) Token: 0x0600090D RID: 2317 RVA: 0x0003057C File Offset: 0x0002E77C
		public KeyChain Keychain
		{
			[CompilerGenerated]
			get
			{
				return this.<Keychain>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (object.Equals(this.<Keychain>k__BackingField, value))
				{
					return;
				}
				this.<Keychain>k__BackingField = value;
				this.OnPropertyChanged("Type");
				this.OnPropertyChanged("CustomVisibility");
				this.OnPropertyChanged("KeychainColor");
				this.OnPropertyChanged("Mode");
				this.OnPropertyChanged("CustomType");
				this.OnPropertyChanged("Keychain");
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x000305E3 File Offset: 0x0002E7E3
		// (set) Token: 0x0600090F RID: 2319 RVA: 0x000305EC File Offset: 0x0002E7EC
		public ObservableCollection<KeyViewModel> KeyList
		{
			[CompilerGenerated]
			get
			{
				return this.<KeyList>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (object.Equals(this.<KeyList>k__BackingField, value))
				{
					return;
				}
				this.<KeyList>k__BackingField = value;
				this.OnPropertyChanged("KeyList");
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x0003061C File Offset: 0x0002E81C
		// (set) Token: 0x06000911 RID: 2321 RVA: 0x0003062C File Offset: 0x0002E82C
		public KeyChain.KeychainType Type
		{
			get
			{
				return this.Keychain.Type;
			}
			set
			{
				if (this.Type == value)
				{
					return;
				}
				this.Keychain.Type = value;
				this.OnPropertyChanged("Type");
				this.OnPropertyChanged("KeychainColor");
				this.OnPropertyChanged("CustomVisibility");
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x00030674 File Offset: 0x0002E874
		// (set) Token: 0x06000913 RID: 2323 RVA: 0x0003067C File Offset: 0x0002E87C
		public bool TypeInitialized
		{
			[CompilerGenerated]
			get
			{
				return this.<TypeInitialized>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (this.<TypeInitialized>k__BackingField == value)
				{
					return;
				}
				this.<TypeInitialized>k__BackingField = value;
				this.OnPropertyChanged("TypeInitialized");
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x000306A9 File Offset: 0x0002E8A9
		// (set) Token: 0x06000915 RID: 2325 RVA: 0x000306B8 File Offset: 0x0002E8B8
		public KeyChain.KeychainMode Mode
		{
			get
			{
				return this.Keychain.Mode;
			}
			set
			{
				if (this.Mode == value)
				{
					return;
				}
				this.Keychain.Mode = value;
				this.OnPropertyChanged("Mode");
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x000306EA File Offset: 0x0002E8EA
		// (set) Token: 0x06000917 RID: 2327 RVA: 0x000306F4 File Offset: 0x0002E8F4
		public bool ModeInitialized
		{
			[CompilerGenerated]
			get
			{
				return this.<ModeInitialized>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (this.<ModeInitialized>k__BackingField == value)
				{
					return;
				}
				this.<ModeInitialized>k__BackingField = value;
				this.OnPropertyChanged("ModeInitialized");
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x00030721 File Offset: 0x0002E921
		// (set) Token: 0x06000919 RID: 2329 RVA: 0x00030730 File Offset: 0x0002E930
		public string CustomType
		{
			get
			{
				return this.Keychain.CustomType;
			}
			set
			{
				if (string.Equals(this.CustomType, value, StringComparison.Ordinal))
				{
					return;
				}
				this.Keychain.CustomType = value;
				this.OnPropertyChanged("CustomType");
				this.OnPropertyChanged("KeychainColor");
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x00030771 File Offset: 0x0002E971
		public Visibility CustomVisibility
		{
			get
			{
				if (this.Type != KeyChain.KeychainType.Custom)
				{
					return Visibility.Collapsed;
				}
				return Visibility.Visible;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x0003077F File Offset: 0x0002E97F
		// (set) Token: 0x0600091C RID: 2332 RVA: 0x00030788 File Offset: 0x0002E988
		public bool CustomTypeInitialized
		{
			[CompilerGenerated]
			get
			{
				return this.<CustomTypeInitialized>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (this.<CustomTypeInitialized>k__BackingField == value)
				{
					return;
				}
				this.<CustomTypeInitialized>k__BackingField = value;
				this.OnPropertyChanged("CustomTypeInitialized");
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x000307B8 File Offset: 0x0002E9B8
		public SolidColorBrush KeychainColor
		{
			get
			{
				Color color = Colors.Black;
				switch (this.Type)
				{
				case KeyChain.KeychainType.Success:
					color = (Color)ColorConverter.ConvertFromString("#006600");
					break;
				case KeyChain.KeychainType.Failure:
					color = (Color)ColorConverter.ConvertFromString("#cc0000");
					break;
				case KeyChain.KeychainType.Ban:
					color = (Color)ColorConverter.ConvertFromString("#660066");
					break;
				case KeyChain.KeychainType.Retry:
					color = (Color)ColorConverter.ConvertFromString("#cc9900");
					break;
				case KeyChain.KeychainType.Custom:
					color = SB.Settings.Environment.GetCustomKeychain(this.CustomType).Color;
					break;
				}
				return new SolidColorBrush(color);
			}
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00030858 File Offset: 0x0002EA58
		public KeyViewModel GetKeyById(int id)
		{
			return this.KeyList.Where((KeyViewModel x) => x.Id.KeyId == id).First<KeyViewModel>();
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0003088E File Offset: 0x0002EA8E
		public void RemoveKeyById(int id)
		{
			this.Keychain.Keys.Remove(this.GetKeyById(id).Key);
			this.KeyList.Remove(this.GetKeyById(id));
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x000308C0 File Offset: 0x0002EAC0
		public void AddKey()
		{
			Key key = new Key();
			this.Keychain.Keys.Add(key);
			this.KeyList.Add(new KeyViewModel(key, this.rand.Next(), this.Id));
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00030908 File Offset: 0x0002EB08
		public KeychainViewModel(KeyChain keychain, int id)
		{
			this.<KeyList>k__BackingField = new ObservableCollection<KeyViewModel>();
			base..ctor();
			this.Keychain = keychain;
			this.Id = id;
			this.TypeInitialized = false;
			this.ModeInitialized = false;
			this.CustomTypeInitialized = false;
			foreach (Key key in keychain.Keys)
			{
				this.KeyList.Add(new KeyViewModel(key, this.rand.Next(), this.Id));
			}
		}

		// Token: 0x0400071F RID: 1823
		private Random rand = new Random(3);

		// Token: 0x04000720 RID: 1824
		private int id;
	}
}
