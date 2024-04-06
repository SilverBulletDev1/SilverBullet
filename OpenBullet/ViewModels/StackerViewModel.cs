using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using Extreme.Net;
using OpenBullet.Views.StackerBlocks;
using RuriLib;
using RuriLib.LS;
using RuriLib.Models;
using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
	// Token: 0x02000144 RID: 324
	public class StackerViewModel : ViewModelBase
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x00030E8A File Offset: 0x0002F08A
		// (set) Token: 0x06000946 RID: 2374 RVA: 0x00030E94 File Offset: 0x0002F094
		public StackerView View
		{
			[CompilerGenerated]
			get
			{
				return this.<View>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (this.<View>k__BackingField == value)
				{
					return;
				}
				this.<View>k__BackingField = value;
				this.OnPropertyChanged("View");
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x00030EC1 File Offset: 0x0002F0C1
		// (set) Token: 0x06000948 RID: 2376 RVA: 0x00030ECC File Offset: 0x0002F0CC
		public BotData BotData
		{
			[CompilerGenerated]
			get
			{
				return this.<BotData>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (object.Equals(this.<BotData>k__BackingField, value))
				{
					return;
				}
				this.<BotData>k__BackingField = value;
				this.OnPropertyChanged("BotData");
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x00030EFC File Offset: 0x0002F0FC
		// (set) Token: 0x0600094A RID: 2378 RVA: 0x00030F04 File Offset: 0x0002F104
		public LoliScript LS
		{
			[CompilerGenerated]
			get
			{
				return this.<LS>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (object.Equals(this.<LS>k__BackingField, value))
				{
					return;
				}
				this.<LS>k__BackingField = value;
				this.OnPropertyChanged("LS");
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x00030F34 File Offset: 0x0002F134
		// (set) Token: 0x0600094C RID: 2380 RVA: 0x00030F3C File Offset: 0x0002F13C
		public bool ControlsEnabled
		{
			get
			{
				return this.controlsEnabled;
			}
			set
			{
				if (this.controlsEnabled == value)
				{
					return;
				}
				this.controlsEnabled = value;
				this.OnPropertyChanged("ControlsEnabled");
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x00030F69 File Offset: 0x0002F169
		// (set) Token: 0x0600094E RID: 2382 RVA: 0x00030F74 File Offset: 0x0002F174
		public ConfigViewModel Config
		{
			[CompilerGenerated]
			get
			{
				return this.<Config>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (object.Equals(this.<Config>k__BackingField, value))
				{
					return;
				}
				this.<Config>k__BackingField = value;
				this.OnPropertyChanged("Config");
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x00030FA4 File Offset: 0x0002F1A4
		// (set) Token: 0x06000950 RID: 2384 RVA: 0x00030FAC File Offset: 0x0002F1AC
		public ObservableCollection<StackerBlockViewModel> Stack
		{
			[CompilerGenerated]
			get
			{
				return this.<Stack>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (object.Equals(this.<Stack>k__BackingField, value))
				{
					return;
				}
				this.<Stack>k__BackingField = value;
				this.OnPropertyChanged("SelectedBlocks");
				this.OnPropertyChanged("CurrentBlockIndex");
				this.OnPropertyChanged("Stack");
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x00030FF2 File Offset: 0x0002F1F2
		public List<StackerBlockViewModel> SelectedBlocks
		{
			get
			{
				return this.Stack.Where((StackerBlockViewModel b) => b.Selected).ToList<StackerBlockViewModel>();
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x00031023 File Offset: 0x0002F223
		// (set) Token: 0x06000953 RID: 2387 RVA: 0x0003102C File Offset: 0x0002F22C
		public StackerBlockViewModel CurrentBlock
		{
			[CompilerGenerated]
			get
			{
				return this.<CurrentBlock>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (object.Equals(this.<CurrentBlock>k__BackingField, value))
				{
					return;
				}
				this.<CurrentBlock>k__BackingField = value;
				this.OnPropertyChanged("CurrentBlockIndex");
				this.OnPropertyChanged("CurrentBlock");
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x00031067 File Offset: 0x0002F267
		public int CurrentBlockIndex
		{
			get
			{
				return this.Stack.IndexOf(this.CurrentBlock);
			}
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0003107C File Offset: 0x0002F27C
		public void DeselectAll()
		{
			foreach (StackerBlockViewModel stackerBlockViewModel in this.Stack)
			{
				stackerBlockViewModel.Selected = false;
			}
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x000310C8 File Offset: 0x0002F2C8
		public void SelectAll()
		{
			foreach (StackerBlockViewModel stackerBlockViewModel in this.Stack)
			{
				stackerBlockViewModel.Selected = true;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000957 RID: 2391 RVA: 0x00031114 File Offset: 0x0002F314
		// (set) Token: 0x06000958 RID: 2392 RVA: 0x0003111C File Offset: 0x0002F31C
		public BlockBase LastDeletedBlock
		{
			[CompilerGenerated]
			get
			{
				return this.<LastDeletedBlock>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (object.Equals(this.<LastDeletedBlock>k__BackingField, value))
				{
					return;
				}
				this.<LastDeletedBlock>k__BackingField = value;
				this.OnPropertyChanged("LastDeletedBlock");
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x0003114C File Offset: 0x0002F34C
		// (set) Token: 0x0600095A RID: 2394 RVA: 0x00031154 File Offset: 0x0002F354
		public int LastDeletedIndex
		{
			[CompilerGenerated]
			get
			{
				return this.<LastDeletedIndex>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (this.<LastDeletedIndex>k__BackingField == value)
				{
					return;
				}
				this.<LastDeletedIndex>k__BackingField = value;
				this.OnPropertyChanged("LastDeletedIndex");
			}
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x00031181 File Offset: 0x0002F381
		public List<BlockBase> GetList()
		{
			this.ConvertKeychains();
			this.ConvertPlugins();
			return this.Stack.Select((StackerBlockViewModel x) => x.Block).ToList<BlockBase>();
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x000311C0 File Offset: 0x0002F3C0
		public void ConvertKeychains()
		{
			foreach (StackerBlockViewModel stackerBlockViewModel in this.Stack)
			{
				BlockBase block = stackerBlockViewModel.Block;
				Page page = stackerBlockViewModel.Page;
				if (page.GetType() == typeof(PageBlockKeycheck))
				{
					PageBlockKeycheck kcpage = (PageBlockKeycheck)page;
					((BlockKeycheck)block).KeyChains = kcpage.vm.KeychainList.Select((KeychainViewModel k) => k.Keychain).ToList<KeyChain>();
				}
			}
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x00031270 File Offset: 0x0002F470
		public void ConvertPlugins()
		{
			foreach (StackerBlockViewModel stackerBlockViewModel in this.Stack.Where((StackerBlockViewModel sb) => sb.Block.IsPlugin()))
			{
				(stackerBlockViewModel.Page as BlockPluginPage).SetPropertyValues();
			}
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x000312E8 File Offset: 0x0002F4E8
		public StackerBlockViewModel GetBlockById(int id)
		{
			return this.Stack.Where((StackerBlockViewModel x) => x.Id == id).First<StackerBlockViewModel>();
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0003131E File Offset: 0x0002F51E
		public void AddBlock(BlockBase block, int index = -1)
		{
			if (index < 0 || index > this.Stack.Count)
			{
				index = this.Stack.Count;
			}
			this.Stack.Insert(index, new StackerBlockViewModel(block, this.rand));
			this.UpdateHeights();
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0003135D File Offset: 0x0002F55D
		public void ClearBlocks()
		{
			this.Stack.Clear();
			this.UpdateHeights();
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x00031370 File Offset: 0x0002F570
		public void UpdateHeights()
		{
			foreach (StackerBlockViewModel stackerBlockViewModel in this.Stack)
			{
				stackerBlockViewModel.UpdateHeight(this.Clamp(600 / this.Stack.Count, 30, 60));
			}
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x000313D8 File Offset: 0x0002F5D8
		public void MoveBlockUp(StackerBlockViewModel block)
		{
			int oldIndex = this.Stack.IndexOf(block);
			if (oldIndex != 0)
			{
				this.Stack.Move(oldIndex, oldIndex - 1);
			}
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x00031404 File Offset: 0x0002F604
		public void MoveBlockDown(StackerBlockViewModel block)
		{
			int oldIndex = this.Stack.IndexOf(block);
			if (oldIndex != this.Stack.Count - 1)
			{
				this.Stack.Move(oldIndex, oldIndex + 1);
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0003143D File Offset: 0x0002F63D
		public int Clamp(int a, int min, int max)
		{
			if (a < min)
			{
				return min;
			}
			if (a > max)
			{
				return max;
			}
			return a;
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000965 RID: 2405 RVA: 0x0003144C File Offset: 0x0002F64C
		// (set) Token: 0x06000966 RID: 2406 RVA: 0x00031454 File Offset: 0x0002F654
		public ProxyType ProxyType
		{
			get
			{
				return this.proxyType;
			}
			set
			{
				if (this.proxyType == value)
				{
					return;
				}
				this.proxyType = value;
				this.OnPropertyChanged("ProxyType");
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x00031481 File Offset: 0x0002F681
		// (set) Token: 0x06000968 RID: 2408 RVA: 0x0003148C File Offset: 0x0002F68C
		public string TestData
		{
			get
			{
				return this.testData;
			}
			set
			{
				if (string.Equals(this.testData, value, StringComparison.Ordinal))
				{
					return;
				}
				this.testData = value;
				this.OnPropertyChanged("TestData");
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x000314BD File Offset: 0x0002F6BD
		// (set) Token: 0x0600096A RID: 2410 RVA: 0x000314C8 File Offset: 0x0002F6C8
		public string TestDataType
		{
			get
			{
				return this.testDataType;
			}
			set
			{
				if (string.Equals(this.testDataType, value, StringComparison.Ordinal))
				{
					return;
				}
				this.testDataType = value;
				this.OnPropertyChanged("TestDataType");
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x000314F9 File Offset: 0x0002F6F9
		// (set) Token: 0x0600096C RID: 2412 RVA: 0x00031504 File Offset: 0x0002F704
		public string TestProxy
		{
			get
			{
				return this.testProxy;
			}
			set
			{
				if (string.Equals(this.testProxy, value, StringComparison.Ordinal))
				{
					return;
				}
				this.testProxy = value;
				this.OnPropertyChanged("TestProxy");
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x00031535 File Offset: 0x0002F735
		// (set) Token: 0x0600096E RID: 2414 RVA: 0x00031540 File Offset: 0x0002F740
		public bool UseProxy
		{
			get
			{
				return this.useProxy;
			}
			set
			{
				if (this.useProxy == value)
				{
					return;
				}
				this.useProxy = value;
				this.OnPropertyChanged("UseProxy");
				this.OnPropertyChanged("UseProxyString");
				this.OnPropertyChanged("UseProxyColor");
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x00031583 File Offset: 0x0002F783
		public string UseProxyString
		{
			get
			{
				if (!this.UseProxy)
				{
					return "OFF";
				}
				return "ON";
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x00031598 File Offset: 0x0002F798
		public SolidColorBrush UseProxyColor
		{
			get
			{
				if (!this.UseProxy)
				{
					return Utils.GetBrush("ForegroundBad");
				}
				return Utils.GetBrush("ForegroundGood");
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x000315B7 File Offset: 0x0002F7B7
		// (set) Token: 0x06000972 RID: 2418 RVA: 0x000315C0 File Offset: 0x0002F7C0
		public bool SBS
		{
			get
			{
				return this.sbs;
			}
			set
			{
				if (this.sbs == value)
				{
					return;
				}
				this.sbs = value;
				this.OnPropertyChanged("SBS");
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x000315ED File Offset: 0x0002F7ED
		// (set) Token: 0x06000974 RID: 2420 RVA: 0x000315F8 File Offset: 0x0002F7F8
		public bool SBSClear
		{
			get
			{
				return this.sbsClear;
			}
			set
			{
				if (this.sbsClear == value)
				{
					return;
				}
				this.sbsClear = value;
				this.OnPropertyChanged("SBSClear");
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x00031625 File Offset: 0x0002F825
		// (set) Token: 0x06000976 RID: 2422 RVA: 0x00031630 File Offset: 0x0002F830
		public bool SBSEnabled
		{
			get
			{
				return this.sbsEnabled;
			}
			set
			{
				if (this.sbsEnabled == value)
				{
					return;
				}
				this.sbsEnabled = value;
				this.OnPropertyChanged("SBSEnabled");
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x0003165D File Offset: 0x0002F85D
		// (set) Token: 0x06000978 RID: 2424 RVA: 0x00031668 File Offset: 0x0002F868
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
				this.OnPropertyChanged("SearchProgress");
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x000316A4 File Offset: 0x0002F8A4
		// (set) Token: 0x0600097A RID: 2426 RVA: 0x000316AC File Offset: 0x0002F8AC
		public int TotalSearchMatches
		{
			get
			{
				return this.totalSearchMatches;
			}
			set
			{
				if (this.totalSearchMatches == value)
				{
					return;
				}
				this.totalSearchMatches = value;
				this.OnPropertyChanged("TotalSearchMatches");
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x000316D9 File Offset: 0x0002F8D9
		// (set) Token: 0x0600097C RID: 2428 RVA: 0x000316E4 File Offset: 0x0002F8E4
		public bool DisableToolTip
		{
			get
			{
				return this.disableToolTip;
			}
			set
			{
				if (this.disableToolTip == value)
				{
					return;
				}
				this.disableToolTip = value;
				this.OnPropertyChanged("DisableToolTip");
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x00031711 File Offset: 0x0002F911
		// (set) Token: 0x0600097E RID: 2430 RVA: 0x0003171C File Offset: 0x0002F91C
		public bool ScriptCompletion
		{
			get
			{
				return this.scriptCompletion;
			}
			set
			{
				if (this.scriptCompletion == value)
				{
					return;
				}
				this.scriptCompletion = value;
				this.OnPropertyChanged("ScriptCompletion");
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x00031749 File Offset: 0x0002F949
		// (set) Token: 0x06000980 RID: 2432 RVA: 0x00031754 File Offset: 0x0002F954
		public int CurrentSearchMatch
		{
			get
			{
				return this.currentSearchMatch;
			}
			set
			{
				if (this.currentSearchMatch == value)
				{
					return;
				}
				this.currentSearchMatch = value;
				this.OnPropertyChanged("CurrentSearchMatch");
			}
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x00031784 File Offset: 0x0002F984
		public StackerViewModel(ConfigViewModel config)
		{
			this.<Stack>k__BackingField = new ObservableCollection<StackerBlockViewModel>();
			this.testData = "";
			this.testDataType = "";
			this.testProxy = "";
			this.searchString = "";
			base..ctor();
			this.Config = config;
		}

		// Token: 0x04000732 RID: 1842
		private Random rand = new Random();

		// Token: 0x04000736 RID: 1846
		private bool controlsEnabled = true;

		// Token: 0x0400073C RID: 1852
		private ProxyType proxyType;

		// Token: 0x0400073D RID: 1853
		private string testData;

		// Token: 0x0400073E RID: 1854
		private string testDataType;

		// Token: 0x0400073F RID: 1855
		private string testProxy;

		// Token: 0x04000740 RID: 1856
		private bool useProxy;

		// Token: 0x04000741 RID: 1857
		private bool sbs;

		// Token: 0x04000742 RID: 1858
		private bool sbsClear;

		// Token: 0x04000743 RID: 1859
		private bool sbsEnabled;

		// Token: 0x04000744 RID: 1860
		private string searchString;

		// Token: 0x04000745 RID: 1861
		private int totalSearchMatches;

		// Token: 0x04000746 RID: 1862
		private bool disableToolTip;

		// Token: 0x04000747 RID: 1863
		private bool scriptCompletion;

		// Token: 0x04000748 RID: 1864
		private int currentSearchMatch;
	}
}
