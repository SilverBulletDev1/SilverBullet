using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Data;
using OpenBullet.Models;
using OpenBullet.Repositories;
using PluginFramework;
using RuriLib;
using RuriLib.Functions.Formats;
using RuriLib.Interfaces;
using RuriLib.LS;
using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
	// Token: 0x0200011F RID: 287
	public class ConfigManagerViewModel : ViewModelBase, IConfigManager
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x0002C808 File Offset: 0x0002AA08
		// (set) Token: 0x060007CF RID: 1999 RVA: 0x0002C810 File Offset: 0x0002AA10
		public ObservableCollection<ConfigViewModel> ConfigsCollection
		{
			get
			{
				return this.configsCollection;
			}
			private set
			{
				if (object.Equals(this.configsCollection, value))
				{
					return;
				}
				this.configsCollection = value;
				this.OnPropertyChanged("Configs");
				this.OnPropertyChanged("Total");
				this.OnPropertyChanged("ConfigsCollection");
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0002C856 File Offset: 0x0002AA56
		public IEnumerable<ConfigViewModel> Configs
		{
			get
			{
				return this.ConfigsCollection;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0002C85E File Offset: 0x0002AA5E
		public int Total
		{
			get
			{
				return this.ConfigsCollection.Count;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x0002C86B File Offset: 0x0002AA6B
		// (set) Token: 0x060007D3 RID: 2003 RVA: 0x0002C874 File Offset: 0x0002AA74
		public int SavedHash
		{
			[CompilerGenerated]
			get
			{
				return this.<SavedHash>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (this.<SavedHash>k__BackingField == value)
				{
					return;
				}
				this.<SavedHash>k__BackingField = value;
				this.OnPropertyChanged("SavedHash");
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x0002C8A1 File Offset: 0x0002AAA1
		// (set) Token: 0x060007D5 RID: 2005 RVA: 0x0002C8AC File Offset: 0x0002AAAC
		public Config HoveredConfig
		{
			get
			{
				return this.hoveredConfig;
			}
			set
			{
				if (object.Equals(this.hoveredConfig, value))
				{
					return;
				}
				this.hoveredConfig = value;
				this.OnPropertyChanged("HoveredConfig");
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0002C8DC File Offset: 0x0002AADC
		// (set) Token: 0x060007D7 RID: 2007 RVA: 0x0002C8E4 File Offset: 0x0002AAE4
		public ConfigViewModel CurrentConfig
		{
			get
			{
				return this.currentConfig;
			}
			set
			{
				if (object.Equals(this.currentConfig, value))
				{
					return;
				}
				this.currentConfig = value;
				this.OnPropertyChanged("CurrentConfig");
				this.OnPropertyChanged("CurrentConfigName");
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x0002C91F File Offset: 0x0002AB1F
		public string CurrentConfigName
		{
			get
			{
				if (this.CurrentConfig != null)
				{
					return this.CurrentConfig.Name;
				}
				return "None";
			}
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0002C93A File Offset: 0x0002AB3A
		public ConfigManagerViewModel()
		{
			this._diskRepos = new ConfigRepository(SB.configFolder);
			this.Rescan();
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x0002C964 File Offset: 0x0002AB64
		public IEnumerable<string> GetRequiredPlugins(ConfigViewModel config)
		{
			return (from IBlockPlugin p in new LoliScript(config.Config.Script).ToBlocks().OnlyPlugins()
				select p.Name).Distinct<string>();
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x0002C9B9 File Offset: 0x0002ABB9
		// (set) Token: 0x060007DC RID: 2012 RVA: 0x0002C9C4 File Offset: 0x0002ABC4
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
				CollectionViewSource.GetDefaultView(this.ConfigsCollection).Refresh();
				this.OnPropertyChanged("Total");
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0002CA10 File Offset: 0x0002AC10
		public void HookFilters()
		{
			((CollectionView)CollectionViewSource.GetDefaultView(this.ConfigsCollection)).Filter = new Predicate<object>(this.ConfigsFilter);
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0002CA33 File Offset: 0x0002AC33
		private bool ConfigsFilter(object item)
		{
			return (item as ConfigViewModel).Name.ToLower().Contains(this.searchString.ToLower());
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0002CA58 File Offset: 0x0002AC58
		public List<ConfigViewModel> GetConfigsFromDisk(bool sort = false, bool reverse = false)
		{
			List<ConfigViewModel> configs = this._diskRepos.Get().ToList<ConfigViewModel>();
			if (sort)
			{
				configs.Sort((ConfigViewModel m1, ConfigViewModel m2) => m1.Config.Settings.LastModified.CompareTo(m2.Config.Settings.LastModified));
				if (reverse)
				{
					configs.Reverse();
				}
			}
			return configs;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0002CAA8 File Offset: 0x0002ACA8
		public IEnumerable<ConfigViewModel> GetConfigsFromSources()
		{
			List<ConfigViewModel> configs = new List<ConfigViewModel>();
			foreach (Source source in SB.SBSettings.Sources.Sources)
			{
				try
				{
					configs.AddRange(this.PullSource(source));
				}
				catch (Exception ex)
				{
					SB.Logger.LogError(Components.ConfigManager, "Error with API " + source.ApiUrl + "\r\nReason: " + ex.Message, true, 0);
				}
			}
			return configs;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0002CB44 File Offset: 0x0002AD44
		public IEnumerable<ConfigViewModel> PullSource(Source source)
		{
			IEnumerable<ConfigViewModel> enumerable;
			using (WebClient wc = new WebClient())
			{
				List<ConfigViewModel> configs = new List<ConfigViewModel>();
				Source.AuthMode auth = source.Auth;
				if (auth != Source.AuthMode.ApiKey)
				{
					if (auth == Source.AuthMode.UserPass)
					{
						string header = (source.Username + ":" + source.Password).ToBase64();
						wc.Headers.Add(HttpRequestHeader.Authorization, "Basic " + header);
					}
				}
				else
				{
					wc.Headers.Add(HttpRequestHeader.Authorization, source.ApiKey);
				}
				byte[] file = new byte[0];
				file = wc.DownloadData(source.ApiUrl);
				string status = wc.ResponseHeaders["Result"];
				if (status != null && status == "Error")
				{
					throw new Exception("The server says: " + Encoding.ASCII.GetString(file));
				}
				try
				{
					using (ZipArchive zip = new ZipArchive(new MemoryStream(file), ZipArchiveMode.Read))
					{
						foreach (ZipArchiveEntry zipArchiveEntry in zip.Entries)
						{
							string subCategory = Path.GetDirectoryName(zipArchiveEntry.FullName).Replace("\\", " - ");
							string category = ((subCategory == string.Empty) ? "Remote" : ("Remote - " + subCategory));
							using (Stream stream = zipArchiveEntry.Open())
							{
								using (TextReader tr = new StreamReader(stream))
								{
									Config cfg = IOManager.DeserializeConfig(tr.ReadToEnd());
									configs.Add(new ConfigViewModel(string.Empty, "", category, cfg, true));
								}
							}
						}
					}
				}
				catch
				{
				}
				enumerable = configs;
			}
			return enumerable;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0002CD9C File Offset: 0x0002AF9C
		public void Add(ConfigViewModel config)
		{
			this._diskRepos.Add(config);
			this.ConfigsCollection.Add(config);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0002CDB6 File Offset: 0x0002AFB6
		public void Rescan()
		{
			this.ConfigsCollection = new ObservableCollection<ConfigViewModel>(this.GetConfigsFromDisk(true, true).Concat(this.GetConfigsFromSources()));
			this.HookFilters();
			this.OnPropertyChanged("Total");
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0002CDE7 File Offset: 0x0002AFE7
		public void Update(ConfigViewModel config)
		{
			this._diskRepos.Update(config);
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0002CDF5 File Offset: 0x0002AFF5
		public void SaveCurrent()
		{
			this.Update(this.CurrentConfig);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0002CE03 File Offset: 0x0002B003
		public void Remove(ConfigViewModel config)
		{
			this._diskRepos.Remove(config);
			this.ConfigsCollection.Remove(config);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0002CE20 File Offset: 0x0002B020
		public void Remove(IEnumerable<ConfigViewModel> configs)
		{
			ConfigViewModel[] toRemove = configs.ToArray<ConfigViewModel>();
			foreach (ConfigViewModel config in toRemove)
			{
				this.ConfigsCollection.Remove(config);
			}
			this._diskRepos.Remove(toRemove);
		}

		// Token: 0x04000662 RID: 1634
		private ConfigRepository _diskRepos;

		// Token: 0x04000663 RID: 1635
		private ObservableCollection<ConfigViewModel> configsCollection;

		// Token: 0x04000665 RID: 1637
		private Config hoveredConfig;

		// Token: 0x04000666 RID: 1638
		private ConfigViewModel currentConfig;

		// Token: 0x04000667 RID: 1639
		private string searchString = "";
	}
}
