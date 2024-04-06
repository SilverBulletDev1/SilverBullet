using System;
using System.Collections.Generic;
using System.Reflection;
using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
	// Token: 0x02000127 RID: 295
	public class SBSettingsGeneral : ViewModelBase
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x0002D84B File Offset: 0x0002BA4B
		// (set) Token: 0x06000827 RID: 2087 RVA: 0x0002D854 File Offset: 0x0002BA54
		public bool DisplayLoliScriptOnLoad
		{
			get
			{
				return this.displayLoliScriptOnLoad;
			}
			set
			{
				if (this.displayLoliScriptOnLoad == value)
				{
					return;
				}
				this.displayLoliScriptOnLoad = value;
				this.OnPropertyChanged("DisplayLoliScriptOnLoad");
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x0002D881 File Offset: 0x0002BA81
		// (set) Token: 0x06000829 RID: 2089 RVA: 0x0002D88C File Offset: 0x0002BA8C
		public bool RecommendedBots
		{
			get
			{
				return this.recommendedBots;
			}
			set
			{
				if (this.recommendedBots == value)
				{
					return;
				}
				this.recommendedBots = value;
				this.OnPropertyChanged("RecommendedBots");
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x0002D8B9 File Offset: 0x0002BAB9
		// (set) Token: 0x0600082B RID: 2091 RVA: 0x0002D8C4 File Offset: 0x0002BAC4
		public int StartingWidth
		{
			get
			{
				return this.startingWidth;
			}
			set
			{
				if (this.startingWidth == value)
				{
					return;
				}
				this.startingWidth = value;
				this.OnPropertyChanged("StartingWidth");
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600082C RID: 2092 RVA: 0x0002D8F1 File Offset: 0x0002BAF1
		// (set) Token: 0x0600082D RID: 2093 RVA: 0x0002D8FC File Offset: 0x0002BAFC
		public int StartingHeight
		{
			get
			{
				return this.startingHeight;
			}
			set
			{
				if (this.startingHeight == value)
				{
					return;
				}
				this.startingHeight = value;
				this.OnPropertyChanged("StartingHeight");
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x0002D929 File Offset: 0x0002BB29
		// (set) Token: 0x0600082F RID: 2095 RVA: 0x0002D934 File Offset: 0x0002BB34
		public bool ChangeRunnerInterface
		{
			get
			{
				return this.changeRunnerInterface;
			}
			set
			{
				if (this.changeRunnerInterface == value)
				{
					return;
				}
				this.changeRunnerInterface = value;
				this.OnPropertyChanged("ChangeRunnerInterface");
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x0002D961 File Offset: 0x0002BB61
		// (set) Token: 0x06000831 RID: 2097 RVA: 0x0002D96C File Offset: 0x0002BB6C
		public bool DisableQuitWarning
		{
			get
			{
				return this.disableQuitWarning;
			}
			set
			{
				if (this.disableQuitWarning == value)
				{
					return;
				}
				this.disableQuitWarning = value;
				this.OnPropertyChanged("DisableQuitWarning");
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x0002D999 File Offset: 0x0002BB99
		// (set) Token: 0x06000833 RID: 2099 RVA: 0x0002D9A4 File Offset: 0x0002BBA4
		public bool DisableNotSavedWarning
		{
			get
			{
				return this.disableNotSavedWarning;
			}
			set
			{
				if (this.disableNotSavedWarning == value)
				{
					return;
				}
				this.disableNotSavedWarning = value;
				this.OnPropertyChanged("DisableNotSavedWarning");
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000834 RID: 2100 RVA: 0x0002D9D1 File Offset: 0x0002BBD1
		// (set) Token: 0x06000835 RID: 2101 RVA: 0x0002D9DC File Offset: 0x0002BBDC
		public string DefaultAuthor
		{
			get
			{
				return this.defaultAuthor;
			}
			set
			{
				if (string.Equals(this.defaultAuthor, value, StringComparison.Ordinal))
				{
					return;
				}
				this.defaultAuthor = value;
				this.OnPropertyChanged("DefaultAuthor");
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x0002DA0D File Offset: 0x0002BC0D
		// (set) Token: 0x06000837 RID: 2103 RVA: 0x0002DA18 File Offset: 0x0002BC18
		public bool LiveConfigUpdates
		{
			get
			{
				return this.liveConfigUpdates;
			}
			set
			{
				if (this.liveConfigUpdates == value)
				{
					return;
				}
				this.liveConfigUpdates = value;
				this.OnPropertyChanged("LiveConfigUpdates");
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000838 RID: 2104 RVA: 0x0002DA45 File Offset: 0x0002BC45
		// (set) Token: 0x06000839 RID: 2105 RVA: 0x0002DA50 File Offset: 0x0002BC50
		public bool DisableHTMLView
		{
			get
			{
				return this.disableHTMLView;
			}
			set
			{
				if (this.disableHTMLView == value)
				{
					return;
				}
				this.disableHTMLView = value;
				this.OnPropertyChanged("DisableHTMLView");
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600083A RID: 2106 RVA: 0x0002DA7D File Offset: 0x0002BC7D
		// (set) Token: 0x0600083B RID: 2107 RVA: 0x0002DA88 File Offset: 0x0002BC88
		public bool AlwaysOnTop
		{
			get
			{
				return this.alwaysOnTop;
			}
			set
			{
				if (this.alwaysOnTop == value)
				{
					return;
				}
				this.alwaysOnTop = value;
				this.OnPropertyChanged("AlwaysOnTop");
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x0002DAB5 File Offset: 0x0002BCB5
		// (set) Token: 0x0600083D RID: 2109 RVA: 0x0002DAC0 File Offset: 0x0002BCC0
		public bool AutoCreateRunner
		{
			get
			{
				return this.autoCreateRunner;
			}
			set
			{
				if (this.autoCreateRunner == value)
				{
					return;
				}
				this.autoCreateRunner = value;
				this.OnPropertyChanged("AutoCreateRunner");
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x0002DAED File Offset: 0x0002BCED
		// (set) Token: 0x0600083F RID: 2111 RVA: 0x0002DAF8 File Offset: 0x0002BCF8
		public bool PersistDebuggerLog
		{
			get
			{
				return this.persistDebuggerLog;
			}
			set
			{
				if (this.persistDebuggerLog == value)
				{
					return;
				}
				this.persistDebuggerLog = value;
				this.OnPropertyChanged("PersistDebuggerLog");
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x0002DB25 File Offset: 0x0002BD25
		// (set) Token: 0x06000841 RID: 2113 RVA: 0x0002DB30 File Offset: 0x0002BD30
		public bool DisableDebuggerLog
		{
			get
			{
				return this.disableDebuggerLog;
			}
			set
			{
				if (this.disableDebuggerLog == value)
				{
					return;
				}
				this.disableDebuggerLog = value;
				this.OnPropertyChanged("DisableDebuggerLog");
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x0002DB5D File Offset: 0x0002BD5D
		// (set) Token: 0x06000843 RID: 2115 RVA: 0x0002DB68 File Offset: 0x0002BD68
		public bool SendDebuggerLogToNotepadPlus
		{
			get
			{
				return this.sendDebuggerLogToNotepadPlus;
			}
			set
			{
				if (this.sendDebuggerLogToNotepadPlus == value)
				{
					return;
				}
				this.sendDebuggerLogToNotepadPlus = value;
				this.OnPropertyChanged("SendDebuggerLogToNotepadPlus");
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x0002DB95 File Offset: 0x0002BD95
		// (set) Token: 0x06000845 RID: 2117 RVA: 0x0002DBA0 File Offset: 0x0002BDA0
		public bool DisableSyntaxHelper
		{
			get
			{
				return this.disableSyntaxHelper;
			}
			set
			{
				if (this.disableSyntaxHelper == value)
				{
					return;
				}
				this.disableSyntaxHelper = value;
				this.OnPropertyChanged("DisableSyntaxHelper");
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000846 RID: 2118 RVA: 0x0002DBCD File Offset: 0x0002BDCD
		// (set) Token: 0x06000847 RID: 2119 RVA: 0x0002DBD8 File Offset: 0x0002BDD8
		public bool DisplayCapturesLast
		{
			get
			{
				return this.displayCapturesLast;
			}
			set
			{
				if (this.displayCapturesLast == value)
				{
					return;
				}
				this.displayCapturesLast = value;
				this.OnPropertyChanged("DisplayCapturesLast");
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000848 RID: 2120 RVA: 0x0002DC05 File Offset: 0x0002BE05
		// (set) Token: 0x06000849 RID: 2121 RVA: 0x0002DC10 File Offset: 0x0002BE10
		public bool DisableCopyPasteBlocks
		{
			get
			{
				return this.disableCopyPasteBlocks;
			}
			set
			{
				if (this.disableCopyPasteBlocks == value)
				{
					return;
				}
				this.disableCopyPasteBlocks = value;
				this.OnPropertyChanged("DisableCopyPasteBlocks");
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600084A RID: 2122 RVA: 0x0002DC3D File Offset: 0x0002BE3D
		// (set) Token: 0x0600084B RID: 2123 RVA: 0x0002DC48 File Offset: 0x0002BE48
		public bool EnableLogging
		{
			get
			{
				return this.enableLogging;
			}
			set
			{
				if (this.enableLogging == value)
				{
					return;
				}
				this.enableLogging = value;
				this.OnPropertyChanged("EnableLogging");
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600084C RID: 2124 RVA: 0x0002DC75 File Offset: 0x0002BE75
		// (set) Token: 0x0600084D RID: 2125 RVA: 0x0002DC80 File Offset: 0x0002BE80
		public bool LogToFile
		{
			get
			{
				return this.logToFile;
			}
			set
			{
				if (this.logToFile == value)
				{
					return;
				}
				this.logToFile = value;
				this.OnPropertyChanged("LogToFile");
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600084E RID: 2126 RVA: 0x0002DCAD File Offset: 0x0002BEAD
		// (set) Token: 0x0600084F RID: 2127 RVA: 0x0002DCB8 File Offset: 0x0002BEB8
		public int LogBufferSize
		{
			get
			{
				return this.logBufferSize;
			}
			set
			{
				if (this.logBufferSize == value)
				{
					return;
				}
				this.logBufferSize = value;
				this.OnPropertyChanged("LogBufferSize");
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000850 RID: 2128 RVA: 0x0002DCE5 File Offset: 0x0002BEE5
		// (set) Token: 0x06000851 RID: 2129 RVA: 0x0002DCF0 File Offset: 0x0002BEF0
		public bool BackupDB
		{
			get
			{
				return this.backupDB;
			}
			set
			{
				if (this.backupDB == value)
				{
					return;
				}
				this.backupDB = value;
				this.OnPropertyChanged("BackupDB");
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000852 RID: 2130 RVA: 0x0002DD1D File Offset: 0x0002BF1D
		// (set) Token: 0x06000853 RID: 2131 RVA: 0x0002DD28 File Offset: 0x0002BF28
		public bool IgnoreWordlistOnHitDedupe
		{
			get
			{
				return this.ignoreWordlistOnHitsDedupe;
			}
			set
			{
				if (this.ignoreWordlistOnHitsDedupe == value)
				{
					return;
				}
				this.ignoreWordlistOnHitsDedupe = value;
				this.OnPropertyChanged("IgnoreWordlistOnHitDedupe");
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000854 RID: 2132 RVA: 0x0002DD55 File Offset: 0x0002BF55
		// (set) Token: 0x06000855 RID: 2133 RVA: 0x0002DD60 File Offset: 0x0002BF60
		public int AutoSaveConfigTime
		{
			get
			{
				return this.autoSaveConfigTime;
			}
			set
			{
				if (this.autoSaveConfigTime == value)
				{
					return;
				}
				this.autoSaveConfigTime = value;
				this.OnPropertyChanged("AutoSaveConfigTime");
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x0002DD8D File Offset: 0x0002BF8D
		// (set) Token: 0x06000857 RID: 2135 RVA: 0x0002DD98 File Offset: 0x0002BF98
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

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x0002DDC5 File Offset: 0x0002BFC5
		// (set) Token: 0x06000859 RID: 2137 RVA: 0x0002DDD0 File Offset: 0x0002BFD0
		public bool AutoSaveConfigOnStacker
		{
			get
			{
				return this.autoSaveConfigOnStacker;
			}
			set
			{
				if (this.autoSaveConfigOnStacker == value)
				{
					return;
				}
				this.autoSaveConfigOnStacker = value;
				this.OnPropertyChanged("AutoSaveConfigOnStacker");
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x0002DDFD File Offset: 0x0002BFFD
		// (set) Token: 0x0600085B RID: 2139 RVA: 0x0002DE08 File Offset: 0x0002C008
		public bool LocalHTMLViewer
		{
			get
			{
				return this.localHTMLViewer;
			}
			set
			{
				if (this.localHTMLViewer == value)
				{
					return;
				}
				this.localHTMLViewer = value;
				this.OnPropertyChanged("LocalHTMLViewer");
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600085C RID: 2140 RVA: 0x0002DE35 File Offset: 0x0002C035
		// (set) Token: 0x0600085D RID: 2141 RVA: 0x0002DE40 File Offset: 0x0002C040
		public bool EnableCookiesInBrowser
		{
			get
			{
				return this.enableCookiesInBrowser;
			}
			set
			{
				if (this.enableCookiesInBrowser == value)
				{
					return;
				}
				this.enableCookiesInBrowser = value;
				this.OnPropertyChanged("EnableCookiesInBrowser");
			}
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0002DE70 File Offset: 0x0002C070
		public void Reset()
		{
			SBSettingsGeneral def = new SBSettingsGeneral();
			foreach (PropertyInfo prop in ((IEnumerable<PropertyInfo>)new List<PropertyInfo>(typeof(SBSettingsGeneral).GetProperties())))
			{
				prop.SetValue(this, prop.GetValue(def, null));
			}
		}

		// Token: 0x04000682 RID: 1666
		private bool displayLoliScriptOnLoad;

		// Token: 0x04000683 RID: 1667
		private bool recommendedBots = true;

		// Token: 0x04000684 RID: 1668
		private int startingWidth = 800;

		// Token: 0x04000685 RID: 1669
		private int startingHeight = 620;

		// Token: 0x04000686 RID: 1670
		private bool changeRunnerInterface;

		// Token: 0x04000687 RID: 1671
		private bool disableQuitWarning;

		// Token: 0x04000688 RID: 1672
		private bool disableNotSavedWarning;

		// Token: 0x04000689 RID: 1673
		private string defaultAuthor = "";

		// Token: 0x0400068A RID: 1674
		private bool liveConfigUpdates;

		// Token: 0x0400068B RID: 1675
		private bool disableHTMLView;

		// Token: 0x0400068C RID: 1676
		private bool alwaysOnTop;

		// Token: 0x0400068D RID: 1677
		private bool autoCreateRunner;

		// Token: 0x0400068E RID: 1678
		private bool persistDebuggerLog;

		// Token: 0x0400068F RID: 1679
		private bool disableDebuggerLog;

		// Token: 0x04000690 RID: 1680
		private bool sendDebuggerLogToNotepadPlus;

		// Token: 0x04000691 RID: 1681
		private bool disableSyntaxHelper;

		// Token: 0x04000692 RID: 1682
		private bool displayCapturesLast;

		// Token: 0x04000693 RID: 1683
		private bool disableCopyPasteBlocks;

		// Token: 0x04000694 RID: 1684
		private bool enableLogging;

		// Token: 0x04000695 RID: 1685
		private bool logToFile;

		// Token: 0x04000696 RID: 1686
		private int logBufferSize = 10000;

		// Token: 0x04000697 RID: 1687
		private bool backupDB = true;

		// Token: 0x04000698 RID: 1688
		private bool ignoreWordlistOnHitsDedupe;

		// Token: 0x04000699 RID: 1689
		private int autoSaveConfigTime = 1;

		// Token: 0x0400069A RID: 1690
		private bool scriptCompletion;

		// Token: 0x0400069B RID: 1691
		private bool autoSaveConfigOnStacker;

		// Token: 0x0400069C RID: 1692
		private bool localHTMLViewer;

		// Token: 0x0400069D RID: 1693
		private bool enableCookiesInBrowser;
	}
}
