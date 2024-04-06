using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using LiteDB;
using OpenBullet.Repositories;
using RuriLib;
using RuriLib.Interfaces;
using RuriLib.Models;
using RuriLib.Runner;
using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
	// Token: 0x02000136 RID: 310
	public class RunnerManagerViewModel : ViewModelBase, IRunnerManager
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x0002FD8E File Offset: 0x0002DF8E
		// (set) Token: 0x060008E8 RID: 2280 RVA: 0x0002FD98 File Offset: 0x0002DF98
		public ObservableCollection<RunnerInstance> RunnersCollection
		{
			[CompilerGenerated]
			get
			{
				return this.<RunnersCollection>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (object.Equals(this.<RunnersCollection>k__BackingField, value))
				{
					return;
				}
				this.<RunnersCollection>k__BackingField = value;
				this.OnPropertyChanged("Runners");
				this.OnPropertyChanged("RunnersCollection");
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x0002FDD3 File Offset: 0x0002DFD3
		public IEnumerable<IRunner> Runners
		{
			get
			{
				return this.RunnersCollection.Select((RunnerInstance i) => i.ViewModel);
			}
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0002FDFF File Offset: 0x0002DFFF
		public RunnerManagerViewModel()
		{
			this.<RunnersCollection>k__BackingField = new ObservableCollection<RunnerInstance>();
			this.rand = new Random();
			base..ctor();
			this._repo = new LiteDBRepository<RunnerSessionData>(SB.dataBaseFile, "runners");
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0002FE34 File Offset: 0x0002E034
		public RunnerInstance Get(int id)
		{
			return this.RunnersCollection.Where((RunnerInstance r) => r.Id == id).First<RunnerInstance>();
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0002FE6C File Offset: 0x0002E06C
		public IRunner Create()
		{
			RunnerInstance instance = new RunnerInstance(this.rand.Next());
			instance.ViewModel.ConfigChanged += this.OnRunnerSessionChanged;
			instance.ViewModel.WordlistChanged += this.OnRunnerSessionChanged;
			this.RunnersCollection.Add(instance);
			return instance.ViewModel;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0002FECC File Offset: 0x0002E0CC
		public void Remove(IRunner runner)
		{
			this.RunnersCollection.Remove(this.RunnersCollection.First((RunnerInstance r) => r.ViewModel == runner));
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0002FF09 File Offset: 0x0002E109
		public void Remove(int id)
		{
			this.RunnersCollection.Remove(this.Get(id));
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0002FF1E File Offset: 0x0002E11E
		public void RemoveAll()
		{
			this.RunnersCollection.Clear();
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0002FF2B File Offset: 0x0002E12B
		public void OnRunnerSessionChanged(IRunnerMessaging obj)
		{
			this.SaveSession();
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0002FF34 File Offset: 0x0002E134
		public void SaveSession()
		{
			this._repo.RemoveAll();
			this._repo.Add(from r in this.RunnersCollection
				select r.ViewModel into r
				select new RunnerSessionData
				{
					Bots = r.BotsAmount,
					Config = ((r.Config != null) ? r.ConfigName : ""),
					Wordlist = ((r.Wordlist != null) ? r.Wordlist.Path : ""),
					ProxyMode = r.ProxyMode
				});
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0002FFA8 File Offset: 0x0002E1A8
		public bool RestoreSession()
		{
			RunnerSessionData[] runners = this._repo.Get().ToArray<RunnerSessionData>();
			if (runners.Length == 0)
			{
				return false;
			}
			RunnerSessionData[] array = runners;
			for (int i = 0; i < array.Length; i++)
			{
				RunnerSessionData r = array[i];
				try
				{
					IRunner runner = this.Create();
					runner.BotsAmount = r.Bots;
					runner.ProxyMode = r.ProxyMode;
					ConfigViewModel configViewModel = SB.ConfigManager.Configs.FirstOrDefault((ConfigViewModel c) => c.Name == r.Config);
					if (configViewModel == null)
					{
						throw new Exception("The Config " + r.Config + " was not found in the ConfigManager");
					}
					ConfigViewModel configVM = configViewModel;
					runner.SetConfig(configVM.Config, false);
					Wordlist wordlist = SB.WordlistManager.Wordlists.FirstOrDefault((Wordlist w) => w.Path == r.Wordlist);
					if (wordlist == null)
					{
						if (!File.Exists(r.Wordlist))
						{
							throw new Exception("The Wordlist " + r.Wordlist + " was not found in the WordlistManager or on Disk");
						}
						wordlist = WordlistManagerViewModel.FileToWordlist(r.Wordlist);
					}
					runner.SetWordlist(wordlist);
					runner.StartingPoint = this.RetrieveRecord(configVM.Config, wordlist);
				}
				catch (Exception ex)
				{
					SB.Logger.LogError(Components.RunnerManager, ex.Message, false, 0);
				}
			}
			return true;
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00030114 File Offset: 0x0002E314
		public int RetrieveRecord(Config config, Wordlist wordlist)
		{
			if (wordlist == null || config == null)
			{
				return 1;
			}
			int num;
			using (LiteDatabase db = new LiteDatabase(SB.dataBaseFile, null))
			{
				Record record = db.GetCollection<Record>("records", BsonAutoId.ObjectId).FindOne((Record r) => r.ConfigName == config.Settings.Name && r.WordlistLocation == wordlist.Path);
				if (record != null)
				{
					num = record.Checkpoint;
				}
				else
				{
					num = 1;
				}
			}
			return num;
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0003026C File Offset: 0x0002E46C
		public void SaveRecord(Config config, Wordlist wordlist, int progress)
		{
			if (config == null || wordlist == null)
			{
				return;
			}
			using (LiteDatabase db = new LiteDatabase("filename=" + SB.dataBaseFile + "; Connection=Shared;", null))
			{
				ILiteCollection<Record> coll = db.GetCollection<Record>("records", BsonAutoId.ObjectId);
				Record record = new Record(config.Settings.Name, wordlist.Path, progress);
				coll.DeleteMany((Record r) => r.ConfigName == config.Settings.Name && r.WordlistLocation == wordlist.Path);
				coll.Insert(record);
			}
		}

		// Token: 0x0400070E RID: 1806
		private LiteDBRepository<RunnerSessionData> _repo;

		// Token: 0x04000710 RID: 1808
		private Random rand;
	}
}
