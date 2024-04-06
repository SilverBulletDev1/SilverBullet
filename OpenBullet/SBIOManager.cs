using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using OpenBullet.ViewModels;
using RuriLib;

namespace OpenBullet
{
	// Token: 0x02000027 RID: 39
	public class SBIOManager
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x000034CD File Offset: 0x000016CD
		public static void SaveSettings(string settingsFile, SBSettingsViewModel settings)
		{
			File.WriteAllText(settingsFile, JsonConvert.SerializeObject(settings, Formatting.Indented));
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000034DC File Offset: 0x000016DC
		public static SBSettingsViewModel LoadSettings(string settingsFile)
		{
			return JsonConvert.DeserializeObject<SBSettingsViewModel>(File.ReadAllText(settingsFile));
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000034EC File Offset: 0x000016EC
		public static void CheckRequiredPlugins(IEnumerable<string> available, Config config)
		{
			foreach (string required in config.Settings.RequiredPlugins)
			{
				if (!available.Contains(required))
				{
					throw new Exception("This config requires the plugin " + required + " which is missing from the Plugins folder and hence cannot be opened!");
				}
			}
		}

		// Token: 0x040000C1 RID: 193
		private static JsonSerializerSettings settings = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.All
		};
	}
}
