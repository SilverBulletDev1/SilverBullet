using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RuriLib;
using RuriLib.Interfaces;
using RuriLib.ViewModels;

namespace OpenBullet.Repositories
{
	// Token: 0x02000056 RID: 86
	public class ConfigRepository : IRepository<ConfigViewModel, ValueTuple<string, string>>
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000A190 File Offset: 0x00008390
		// (set) Token: 0x06000224 RID: 548 RVA: 0x0000A198 File Offset: 0x00008398
		public string BaseFolder { get; set; }

		// Token: 0x06000225 RID: 549 RVA: 0x0000A1A1 File Offset: 0x000083A1
		public ConfigRepository(string baseFolder)
		{
			this.BaseFolder = baseFolder;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000A1B0 File Offset: 0x000083B0
		public void Add(ConfigViewModel entity)
		{
			string path = this.GetPath(entity);
			if (entity.Category != ConfigRepository.defaultCategory && !Directory.Exists(entity.Category))
			{
				Directory.CreateDirectory(Path.Combine(this.BaseFolder, entity.Category));
			}
			if (!File.Exists(path))
			{
				IOManager.SaveConfig(entity.Config, path);
				return;
			}
			throw new IOException("A config with the same name and category already exists");
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000A21C File Offset: 0x0000841C
		public IEnumerable<ConfigViewModel> Get()
		{
			List<ConfigViewModel> configs = new List<ConfigViewModel>();
			bool loliX = false;
			IEnumerable<string> enumerable = Directory.EnumerateFiles(SB.configFolder);
			Func<string, bool> <>9__0;
			Func<string, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (string file) => file.EndsWith(".svb") || file.EndsWith(".loli") || file.EndsWith(".anom") || (loliX = file.EndsWith(".loliX")));
			}
			foreach (string file3 in enumerable.Where(func))
			{
				try
				{
					configs.Add(new ConfigViewModel(file3, Path.GetFileNameWithoutExtension(file3), ConfigRepository.defaultCategory, IOManager.LoadConfig(file3, loliX), false));
					if (loliX)
					{
						FileSystemInfo fileSystemInfo = new FileInfo("Configs\\" + Path.GetFileName(file3));
						IOManager.SaveConfig(configs.Last<ConfigViewModel>().Config, "Configs\\" + Path.GetFileNameWithoutExtension(file3) + ".svb");
						fileSystemInfo.Delete();
					}
					loliX = false;
				}
				catch
				{
					loliX = false;
				}
			}
			foreach (string categoryFolder in Directory.EnumerateDirectories(SB.configFolder))
			{
				foreach (string file2 in from file in Directory.EnumerateFiles(categoryFolder)
					where file.EndsWith(".loli")
					select file)
				{
					try
					{
						configs.Add(new ConfigViewModel(file2, Path.GetFileNameWithoutExtension(file2), Path.GetFileName(categoryFolder), IOManager.LoadConfig(file2, false), false));
					}
					catch
					{
					}
				}
			}
			return configs;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000A40C File Offset: 0x0000860C
		public ConfigViewModel Get(ValueTuple<string, string> id)
		{
			string category = id.Item1;
			string fileName = id.Item2;
			string path = this.GetPath(category, fileName, string.Empty);
			return new ConfigViewModel(path, fileName, category, IOManager.LoadConfig(path, false), false);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000A448 File Offset: 0x00008648
		public void Remove(ConfigViewModel entity)
		{
			string path = this.GetPath(entity.Category, entity.FileName, Path.GetExtension(entity.Path));
			if (File.Exists(path))
			{
				File.Delete(path);
				return;
			}
			throw new IOException("File not found");
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000A48C File Offset: 0x0000868C
		public void RemoveAll()
		{
			Directory.Delete(this.BaseFolder, true);
			Directory.CreateDirectory(this.BaseFolder);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000A4A8 File Offset: 0x000086A8
		public void Update(ConfigViewModel entity)
		{
			string path = this.GetPath(entity);
			if (!File.Exists(path))
			{
				path = path.CreateFileName(entity.Name, true);
				using (File.Create(path))
				{
				}
			}
			IOManager.SaveConfig(entity.Config, path);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000A504 File Offset: 0x00008704
		public void Add(IEnumerable<ConfigViewModel> entities)
		{
			foreach (ConfigViewModel entity in entities)
			{
				this.Add(entity);
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000A54C File Offset: 0x0000874C
		public void Remove(IEnumerable<ConfigViewModel> entities)
		{
			foreach (ConfigViewModel entity in entities)
			{
				this.Remove(entity);
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000A594 File Offset: 0x00008794
		public void Update(IEnumerable<ConfigViewModel> entities)
		{
			foreach (ConfigViewModel entity in entities)
			{
				this.Update(entity);
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000A5DC File Offset: 0x000087DC
		private string GetPath(ConfigViewModel config)
		{
			return this.GetPath(config.Category, config.FileName, string.Empty);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000A5F8 File Offset: 0x000087F8
		private string GetPath(string category, string fileName, string suffix)
		{
			string file = fileName + "." + (string.IsNullOrEmpty(suffix) ? "svb" : suffix.Remove(0, 1));
			if (category != ConfigRepository.defaultCategory)
			{
				return Path.Combine(this.BaseFolder, category, file);
			}
			return Path.Combine(this.BaseFolder, file);
		}

		// Token: 0x040001CF RID: 463
		public static string defaultCategory = "Default";

		// Token: 0x040001D1 RID: 465
		private const string Suffix = "svb";
	}
}
