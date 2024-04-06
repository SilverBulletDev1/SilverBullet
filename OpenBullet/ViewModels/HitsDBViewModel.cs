using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using OpenBullet.Repositories;
using RuriLib.Interfaces;
using RuriLib.Models;
using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
	// Token: 0x02000123 RID: 291
	public class HitsDBViewModel : ViewModelBase, IHitsDB
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0002CF4E File Offset: 0x0002B14E
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x0002CF58 File Offset: 0x0002B158
		public ObservableCollection<Hit> HitsCollection
		{
			get
			{
				return this.hitsCollection;
			}
			private set
			{
				if (object.Equals(this.hitsCollection, value))
				{
					return;
				}
				this.hitsCollection = value;
				this.OnPropertyChanged("Total");
				this.OnPropertyChanged("Hits");
				this.OnPropertyChanged("ConfigsList");
				this.OnPropertyChanged("Filtered");
				this.OnPropertyChanged("HitsCollection");
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x0002CFB4 File Offset: 0x0002B1B4
		public int Total
		{
			get
			{
				return this.HitsCollection.Count;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0002CFC1 File Offset: 0x0002B1C1
		public IEnumerable<Hit> Hits
		{
			get
			{
				return this.HitsCollection;
			}
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0002CFCC File Offset: 0x0002B1CC
		public HitsDBViewModel()
		{
			this._repo = new LiteDBRepository<Hit>(SB.dataBaseFile, "hits");
			this.HitsCollection = new ObservableCollection<Hit>();
			this.HookFilters();
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x0002D026 File Offset: 0x0002B226
		public List<string> ConfigsList
		{
			get
			{
				return this.HitsCollection.Select((Hit x) => x.ConfigName).Distinct<string>().ToList<string>();
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x0002D05C File Offset: 0x0002B25C
		// (set) Token: 0x060007FA RID: 2042 RVA: 0x0002D064 File Offset: 0x0002B264
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
				CollectionViewSource.GetDefaultView(this.HitsCollection).Refresh();
				this.OnPropertyChanged("Filtered");
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0002D0B0 File Offset: 0x0002B2B0
		// (set) Token: 0x060007FC RID: 2044 RVA: 0x0002D0B8 File Offset: 0x0002B2B8
		public string TypeFilter
		{
			get
			{
				return this.typeFilter;
			}
			set
			{
				if (string.Equals(this.typeFilter, value, StringComparison.Ordinal))
				{
					return;
				}
				this.typeFilter = value;
				this.OnPropertyChanged("TypeFilter");
				CollectionViewSource.GetDefaultView(this.HitsCollection).Refresh();
				this.OnPropertyChanged("Filtered");
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x0002D104 File Offset: 0x0002B304
		// (set) Token: 0x060007FE RID: 2046 RVA: 0x0002D10C File Offset: 0x0002B30C
		public string ConfigFilter
		{
			get
			{
				return this.configFilter;
			}
			set
			{
				if (string.Equals(this.configFilter, value, StringComparison.Ordinal))
				{
					return;
				}
				this.configFilter = value;
				this.OnPropertyChanged("ConfigFilter");
				CollectionViewSource.GetDefaultView(this.HitsCollection).Refresh();
				this.OnPropertyChanged("Filtered");
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x0002D158 File Offset: 0x0002B358
		public int Filtered
		{
			get
			{
				return this.HitsCollection.Count((Hit h) => this.HitsFilter(h));
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0002D171 File Offset: 0x0002B371
		public void HookFilters()
		{
			((CollectionView)CollectionViewSource.GetDefaultView(this.HitsCollection)).Filter = new Predicate<object>(this.HitsFilter);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0002D194 File Offset: 0x0002B394
		private bool HitsFilter(object item)
		{
			return !((item as Hit).Type != this.TypeFilter) && (!(this.ConfigFilter != HitsDBViewModel.defaultFilter) || !((item as Hit).ConfigName != this.ConfigFilter)) && (string.IsNullOrEmpty(this.SearchString) || (item as Hit).CapturedData.ToCaptureString().ToLower().Contains(this.SearchString.ToLower()));
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0002D21B File Offset: 0x0002B41B
		public void Add(Hit hit)
		{
			this.HitsCollection.Add(hit);
			this._repo.Add(hit);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0002D235 File Offset: 0x0002B435
		public void RefreshList()
		{
			this.HitsCollection = new ObservableCollection<Hit>(this._repo.Get());
			this.HookFilters();
			this.OnPropertyChanged("Total");
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0002D25E File Offset: 0x0002B45E
		public void Update(Hit hit)
		{
			this._repo.Update(hit);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0002D26C File Offset: 0x0002B46C
		public void Remove(Hit hit)
		{
			this.HitsCollection.Remove(hit);
			this._repo.Remove(hit);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0002D288 File Offset: 0x0002B488
		public void Remove(IEnumerable<Hit> hits)
		{
			Hit[] toRemove = hits.ToArray<Hit>();
			foreach (Hit hit in toRemove)
			{
				this.HitsCollection.Remove(hit);
			}
			this._repo.Remove(toRemove);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0002D2C9 File Offset: 0x0002B4C9
		public void RemoveAll()
		{
			this.HitsCollection.Clear();
			this._repo.RemoveAll();
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0002D2E4 File Offset: 0x0002B4E4
		public void DeleteDuplicates()
		{
			List<Hit> duplicates = (from h in this.HitsCollection
				group h by h.GetHashCode(SB.SBSettings.General.IgnoreWordlistOnHitDedupe) into g
				where g.Count<Hit>() > 1
				select g).SelectMany((IGrouping<int, Hit> g) => g.OrderBy((Hit h) => h.Date).Reverse<Hit>().Skip(1)).ToList<Hit>();
			this.Remove(duplicates);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0002D370 File Offset: 0x0002B570
		public void DeleteFiltered()
		{
			List<Hit> filtered = this.HitsCollection.Where((Hit h) => (string.IsNullOrEmpty(this.SearchString) || h.CapturedString.ToLower().Contains(this.SearchString.ToLower())) && (this.ConfigFilter == "All" || h.ConfigName == this.ConfigFilter) && h.Type == this.TypeFilter).ToList<Hit>();
			this.Remove(filtered);
		}

		// Token: 0x0400066D RID: 1645
		public LiteDBRepository<Hit> _repo;

		// Token: 0x0400066E RID: 1646
		private ObservableCollection<Hit> hitsCollection;

		// Token: 0x0400066F RID: 1647
		public static readonly string defaultFilter = "All";

		// Token: 0x04000670 RID: 1648
		private string searchString = "";

		// Token: 0x04000671 RID: 1649
		private string typeFilter = "SUCCESS";

		// Token: 0x04000672 RID: 1650
		private string configFilter = HitsDBViewModel.defaultFilter;
	}
}
