using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Data;
using OpenBullet.Repositories;
using RuriLib.Interfaces;
using RuriLib.Models;
using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
	// Token: 0x02000147 RID: 327
	public class WordlistManagerViewModel : ViewModelBase, IWordlistManager
	{
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x00031820 File Offset: 0x0002FA20
		// (set) Token: 0x0600098B RID: 2443 RVA: 0x00031828 File Offset: 0x0002FA28
		public ObservableCollection<Wordlist> WordlistsCollection
		{
			get
			{
				return this.wordlistsCollection;
			}
			private set
			{
				if (object.Equals(this.wordlistsCollection, value))
				{
					return;
				}
				this.wordlistsCollection = value;
				this.OnPropertyChanged("Total");
				this.OnPropertyChanged("Wordlists");
				this.OnPropertyChanged("WordlistsCollection");
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600098C RID: 2444 RVA: 0x0003186E File Offset: 0x0002FA6E
		public int Total
		{
			get
			{
				return this.WordlistsCollection.Count;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x0003187B File Offset: 0x0002FA7B
		public IEnumerable<Wordlist> Wordlists
		{
			get
			{
				return this.WordlistsCollection;
			}
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x00031883 File Offset: 0x0002FA83
		public WordlistManagerViewModel()
		{
			this._repo = new LiteDBRepository<Wordlist>(SB.dataBaseFile, "wordlists");
			this.WordlistsCollection = new ObservableCollection<Wordlist>();
			this.RefreshList();
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x000318BC File Offset: 0x0002FABC
		// (set) Token: 0x06000990 RID: 2448 RVA: 0x000318C4 File Offset: 0x0002FAC4
		public string SearchString
		{
			get
			{
				return this.searchString;
			}
			set
			{
				if (string.Equals(this.searchString, value, StringComparison.Ordinal))
				{
					return;
				}
				this.searchString = value;
				this.OnPropertyChanged("SearchString");
				CollectionViewSource.GetDefaultView(this.WordlistsCollection).Refresh();
				this.OnPropertyChanged("Total");
			}
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00031910 File Offset: 0x0002FB10
		public void HookFilters()
		{
			((CollectionView)CollectionViewSource.GetDefaultView(this.WordlistsCollection)).Filter = new Predicate<object>(this.WordlistsFilter);
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00031933 File Offset: 0x0002FB33
		private bool WordlistsFilter(object item)
		{
			return (item as Wordlist).Name.ToLower().Contains(this.searchString.ToLower());
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00031958 File Offset: 0x0002FB58
		public Wordlist GetWordlistByName(string name)
		{
			return this.WordlistsCollection.Where((Wordlist x) => x.Name == name).First<Wordlist>();
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x00031990 File Offset: 0x0002FB90
		public static Wordlist FileToWordlist(string path)
		{
			Wordlist wordlist = new Wordlist(Path.GetFileNameWithoutExtension(path), path, SB.Settings.Environment.WordlistTypes.First<WordlistType>().Name, "", true, false, null);
			string first = File.ReadLines(wordlist.Path).First<string>();
			wordlist.Type = SB.Settings.Environment.RecognizeWordlistType(first);
			return wordlist;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x000319F4 File Offset: 0x0002FBF4
		public void Add(Wordlist wordlist)
		{
			if (this.WordlistsCollection.Any((Wordlist w) => w.Path == wordlist.Path))
			{
				throw new Exception("Wordlist already present: " + wordlist.Path);
			}
			this.WordlistsCollection.Add(wordlist);
			this._repo.Add(wordlist);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x00031A64 File Offset: 0x0002FC64
		public void RefreshList()
		{
			this.WordlistsCollection = new ObservableCollection<Wordlist>(this._repo.Get());
			this.HookFilters();
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x00031A82 File Offset: 0x0002FC82
		public void Update(Wordlist wordlist)
		{
			this._repo.Update(wordlist);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x00031A90 File Offset: 0x0002FC90
		public void Remove(Wordlist wordlist)
		{
			this.WordlistsCollection.Remove(wordlist);
			this._repo.Remove(wordlist);
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x00031AAB File Offset: 0x0002FCAB
		public void RemoveAll()
		{
			this.WordlistsCollection.Clear();
			this._repo.RemoveAll();
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x00031AC4 File Offset: 0x0002FCC4
		public void DeleteNotFound()
		{
			for (int i = 0; i < this.WordlistsCollection.Count; i++)
			{
				Wordlist wordlist = this.WordlistsCollection[i];
				if (!File.Exists(wordlist.Path))
				{
					this.Remove(wordlist);
					i--;
				}
			}
		}

		// Token: 0x0400074F RID: 1871
		private LiteDBRepository<Wordlist> _repo;

		// Token: 0x04000750 RID: 1872
		private ObservableCollection<Wordlist> wordlistsCollection;

		// Token: 0x04000751 RID: 1873
		private string searchString = "";
	}
}
