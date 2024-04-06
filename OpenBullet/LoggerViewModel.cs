using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using OpenBullet.Views.CustomMessageBox;
using RuriLib;
using RuriLib.Interfaces;
using RuriLib.ViewModels;

namespace OpenBullet
{
	// Token: 0x02000029 RID: 41
	public class LoggerViewModel : ViewModelBase, ILogger
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00003549 File Offset: 0x00001749
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00003554 File Offset: 0x00001754
		public ObservableCollection<LogEntry> EntriesCollection
		{
			[CompilerGenerated]
			get
			{
				return this.<EntriesCollection>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (object.Equals(this.<EntriesCollection>k__BackingField, value))
				{
					return;
				}
				this.<EntriesCollection>k__BackingField = value;
				this.OnPropertyChanged("Entries");
				this.OnPropertyChanged("EntriesCollection");
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000AF RID: 175 RVA: 0x0000358F File Offset: 0x0000178F
		public IEnumerable<LogEntry> Entries
		{
			get
			{
				return this.EntriesCollection;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00003598 File Offset: 0x00001798
		public bool Enabled
		{
			get
			{
				bool flag;
				try
				{
					flag = SB.SBSettings.General.EnableLogging;
				}
				catch
				{
					flag = false;
				}
				return flag;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x000035D0 File Offset: 0x000017D0
		public int BufferSize
		{
			get
			{
				int num;
				try
				{
					num = SB.SBSettings.General.LogBufferSize;
				}
				catch
				{
					num = 0;
				}
				return num;
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003608 File Offset: 0x00001808
		public LoggerViewModel()
		{
			this.EntriesCollection = new ObservableCollection<LogEntry>();
			((CollectionView)CollectionViewSource.GetDefaultView(this.EntriesCollection)).Filter = new Predicate<object>(this.ErrorFilter);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003648 File Offset: 0x00001848
		public void Refresh()
		{
			try
			{
				CollectionViewSource.GetDefaultView(this.EntriesCollection).Refresh();
			}
			catch
			{
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x0000367C File Offset: 0x0000187C
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00003684 File Offset: 0x00001884
		public bool OnlyErrors
		{
			get
			{
				return this.onlyErrors;
			}
			set
			{
				if (this.onlyErrors == value)
				{
					return;
				}
				this.onlyErrors = value;
				this.OnPropertyChanged("OnlyErrors");
				this.Refresh();
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x000036B7 File Offset: 0x000018B7
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x000036C0 File Offset: 0x000018C0
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
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000036F4 File Offset: 0x000018F4
		private bool ErrorFilter(object item)
		{
			return (!(this.SearchString != string.Empty) || (item as LogEntry).LogString.ToLower().Contains(this.SearchString.ToLower())) && (!SB.Logger.OnlyErrors || (item as LogEntry).LogLevel == LogLevel.Error);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00003753 File Offset: 0x00001953
		public void Log(string message, LogLevel level, bool prompt = false, int timeout = 0)
		{
			this.Log(Components.Unknown, level, message, prompt, timeout, false);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003764 File Offset: 0x00001964
		public MessageBoxResult Log(Components component, LogLevel level, string message, bool prompt = false, int timeout = 0, bool isCancelButtonVisible = false)
		{
			if (prompt)
			{
				if (timeout == 0)
				{
					if (!isCancelButtonVisible)
					{
						CustomMsgBox.Show(message, level.ToString(), false);
						return MessageBoxResult.OK;
					}
					return CustomMsgBox.ShowWithCancel(message, level.ToString(), false);
				}
				else
				{
					Form w = new Form
					{
						Size = new global::System.Drawing.Size(0, 0)
					};
					Task.Delay(TimeSpan.FromSeconds((double)timeout)).ContinueWith(delegate(Task t)
					{
						w.Close();
					}, TaskScheduler.FromCurrentSynchronizationContext());
					global::System.Windows.Forms.MessageBox.Show(w, message, level.ToString());
				}
			}
			if (!this.Enabled)
			{
				return MessageBoxResult.None;
			}
			global::System.Windows.Application.Current.Dispatcher.Invoke(delegate
			{
				LogEntry entry = new LogEntry(component.ToString(), message, level);
				this.InsertEntry(entry);
				LoggerViewModel.LogToFile(entry);
			});
			return MessageBoxResult.None;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000386A File Offset: 0x00001A6A
		public void LogInfo(Components component, string message, bool prompt = false, int timeout = 0)
		{
			this.Log(component, LogLevel.Info, message, prompt, timeout, false);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000387A File Offset: 0x00001A7A
		public void LogWarning(Components component, string message, bool prompt = false, int timeout = 0)
		{
			this.Log(component, LogLevel.Warning, message, prompt, timeout, false);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000388A File Offset: 0x00001A8A
		public void LogError(Components component, string message, bool prompt = false, int timeout = 0)
		{
			this.Log(component, LogLevel.Error, message, prompt, timeout, false);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000389C File Offset: 0x00001A9C
		private void InsertEntry(LogEntry entry)
		{
			try
			{
				this.EntriesCollection.Insert(0, entry);
				int count = this.EntriesCollection.Count;
				if (count > this.BufferSize)
				{
					this.EntriesCollection.RemoveAt(count - 1);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000038F0 File Offset: 0x00001AF0
		private static void LogToFile(LogEntry entry)
		{
			try
			{
				if (SB.SBSettings.General.LogToFile)
				{
					File.AppendAllText(SB.logFile, string.Format("[{0}] ({1}) {2} - ", entry.LogTime, entry.LogLevel, entry.LogComponent) + entry.LogString + Environment.NewLine);
				}
			}
			catch
			{
			}
		}

		// Token: 0x040000D4 RID: 212
		private bool onlyErrors;

		// Token: 0x040000D5 RID: 213
		private string searchString = "";
	}
}
