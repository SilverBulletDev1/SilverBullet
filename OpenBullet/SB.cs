using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using OpenBullet.ViewModels;
using PluginFramework;
using RuriLib;
using RuriLib.Interfaces;

namespace OpenBullet
{
	// Token: 0x02000026 RID: 38
	public static class SB
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000032D8 File Offset: 0x000014D8
		public static IApplication App
		{
			get
			{
				return new SilverBulletApp
				{
					RunnerManager = SB.RunnerManager,
					ProxyManager = SB.ProxyManager,
					ProxyChecker = SB.ProxyManager,
					WordlistManager = SB.WordlistManager,
					ConfigManager = SB.ConfigManager,
					HitsDB = SB.HitsDB,
					Settings = SB.Settings,
					Logger = SB.Logger,
					Alerter = SB.Alerter
				};
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600008E RID: 142 RVA: 0x0000335F File Offset: 0x0000155F
		public static IEnumerable<BlockBase> BlockPluginsAsBase
		{
			get
			{
				return SB.BlockPlugins.Cast<BlockBase>();
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600008F RID: 143 RVA: 0x0000336B File Offset: 0x0000156B
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00003372 File Offset: 0x00001572
		public static MainWindow MainWindow { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000091 RID: 145 RVA: 0x0000337A File Offset: 0x0000157A
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00003381 File Offset: 0x00001581
		public static LogWindow LogWindow { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00003389 File Offset: 0x00001589
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00003390 File Offset: 0x00001590
		public static RunnerManagerViewModel RunnerManager { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003398 File Offset: 0x00001598
		// (set) Token: 0x06000096 RID: 150 RVA: 0x0000339F File Offset: 0x0000159F
		public static ProxyManagerViewModel ProxyManager { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000033A7 File Offset: 0x000015A7
		// (set) Token: 0x06000098 RID: 152 RVA: 0x000033AE File Offset: 0x000015AE
		public static WordlistManagerViewModel WordlistManager { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000033B6 File Offset: 0x000015B6
		// (set) Token: 0x0600009A RID: 154 RVA: 0x000033BD File Offset: 0x000015BD
		public static ConfigManagerViewModel ConfigManager { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600009B RID: 155 RVA: 0x000033C5 File Offset: 0x000015C5
		// (set) Token: 0x0600009C RID: 156 RVA: 0x000033CC File Offset: 0x000015CC
		public static StackerViewModel Stacker { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600009D RID: 157 RVA: 0x000033D4 File Offset: 0x000015D4
		// (set) Token: 0x0600009E RID: 158 RVA: 0x000033DB File Offset: 0x000015DB
		public static HitsDBViewModel HitsDB { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600009F RID: 159 RVA: 0x000033E3 File Offset: 0x000015E3
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x000033EA File Offset: 0x000015EA
		public static Alerter Alerter { get; set; } = new Alerter();

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000033F2 File Offset: 0x000015F2
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x000033F9 File Offset: 0x000015F9
		public static LoggerViewModel Logger { get; set; } = new LoggerViewModel();

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003401 File Offset: 0x00001601
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003408 File Offset: 0x00001608
		public static GlobalSettings Settings { get; set; } = new GlobalSettings();

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003410 File Offset: 0x00001610
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00003417 File Offset: 0x00001617
		public static SBSettingsViewModel SBSettings { get; set; }

		// Token: 0x040000A4 RID: 164
		public const string Version = "1.1.4";

		// Token: 0x040000A5 RID: 165
		public const string CompilerVersion = "1.1";

		// Token: 0x040000A6 RID: 166
		public static List<ValueTuple<Type, Type, LinearGradientBrush>> BlockMappings = new List<ValueTuple<Type, Type, LinearGradientBrush>>();

		// Token: 0x040000A7 RID: 167
		public static List<IBlockPlugin> BlockPlugins;

		// Token: 0x040000A8 RID: 168
		public static IEnumerable<string> PluginNames;

		// Token: 0x040000B5 RID: 181
		public static readonly string dataBaseFile = "DB/OpenBullet.db";

		// Token: 0x040000B6 RID: 182
		public static readonly string dataBaseBackupFile = "DB/OpenBullet-BackupCopy.db";

		// Token: 0x040000B7 RID: 183
		public static readonly string obSettingsFile = "Settings/OBSettings.json";

		// Token: 0x040000B8 RID: 184
		public static readonly string rlSettingsFile = "Settings/RLSettings.json";

		// Token: 0x040000B9 RID: 185
		public static readonly string proxyManagerSettingsFile = "Settings/ProxyManagerSettings.json";

		// Token: 0x040000BA RID: 186
		public static readonly string envFile = "Settings/Environment.ini";

		// Token: 0x040000BB RID: 187
		public static readonly string licenseFile = "Settings/License.txt";

		// Token: 0x040000BC RID: 188
		public static readonly string logFile = "Log.txt";

		// Token: 0x040000BD RID: 189
		public static readonly string configFolder = "Configs";

		// Token: 0x040000BE RID: 190
		public static readonly string pluginsFolder = "Plugins";

		// Token: 0x040000BF RID: 191
		public static readonly string defaultProxySiteUrl = "https://google.com";

		// Token: 0x040000C0 RID: 192
		public static readonly string defaultProxyKey = "title>Google";
	}
}
