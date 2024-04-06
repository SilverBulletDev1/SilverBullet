using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RuriLib;
using RuriLib.Models;
using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
	// Token: 0x0200011D RID: 285
	public class BlockKeycheckViewModel : ViewModelBase
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x0002C68B File Offset: 0x0002A88B
		// (set) Token: 0x060007C5 RID: 1989 RVA: 0x0002C694 File Offset: 0x0002A894
		public BlockKeycheck Block
		{
			[CompilerGenerated]
			get
			{
				return this.<Block>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (object.Equals(this.<Block>k__BackingField, value))
				{
					return;
				}
				this.<Block>k__BackingField = value;
				this.OnPropertyChanged("Block");
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x0002C6C4 File Offset: 0x0002A8C4
		// (set) Token: 0x060007C7 RID: 1991 RVA: 0x0002C6CC File Offset: 0x0002A8CC
		public ObservableCollection<KeychainViewModel> KeychainList
		{
			[CompilerGenerated]
			get
			{
				return this.<KeychainList>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (object.Equals(this.<KeychainList>k__BackingField, value))
				{
					return;
				}
				this.<KeychainList>k__BackingField = value;
				this.OnPropertyChanged("KeychainList");
			}
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0002C6FC File Offset: 0x0002A8FC
		public KeychainViewModel GetKeychainById(int id)
		{
			return this.KeychainList.Where((KeychainViewModel x) => x.Id == id).First<KeychainViewModel>();
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0002C732 File Offset: 0x0002A932
		public void RemoveKeychainById(int id)
		{
			this.KeychainList.Remove(this.GetKeychainById(id));
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0002C747 File Offset: 0x0002A947
		public void AddKeychain()
		{
			this.KeychainList.Add(new KeychainViewModel(new KeyChain(), this.rand.Next()));
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0002C76C File Offset: 0x0002A96C
		public BlockKeycheckViewModel(BlockKeycheck block)
		{
			this.Block = block;
			this.KeychainList = new ObservableCollection<KeychainViewModel>();
			foreach (KeyChain keychain in block.KeyChains)
			{
				this.KeychainList.Add(new KeychainViewModel(keychain, this.rand.Next()));
			}
		}

		// Token: 0x0400065E RID: 1630
		private Random rand = new Random(2);
	}
}
