using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using RuriLib;

namespace OpenBullet
{
	// Token: 0x0200002C RID: 44
	public partial class LogWindow : Window
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x000039B3 File Offset: 0x00001BB3
		public LogWindow()
		{
			this.InitializeComponent();
			base.DataContext = SB.Logger;
			base.Closing += this.LogWindowClosing;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000039DE File Offset: 0x00001BDE
		private void LogWindowClosing(object sender, CancelEventArgs e)
		{
			SB.LogWindow = null;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000039E8 File Offset: 0x00001BE8
		private void copyClick(object sender, RoutedEventArgs e)
		{
			string clipboardText = "";
			foreach (object obj in this.logListView.SelectedItems)
			{
				LogEntry selected = (LogEntry)obj;
				clipboardText = clipboardText + string.Format("[{0}] ({1}) {2} - ", selected.LogTime, selected.LogLevel, selected.LogComponent) + selected.LogString + Environment.NewLine;
			}
			Clipboard.SetText(clipboardText);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00003A84 File Offset: 0x00001C84
		private void copyAllButton_Click(object sender, RoutedEventArgs e)
		{
			string clipboardText = "";
			foreach (object obj in ((IEnumerable)this.logListView.Items))
			{
				LogEntry selected = (LogEntry)obj;
				clipboardText = clipboardText + string.Format("[{0}] ({1}) {2} - ", selected.LogTime, selected.LogLevel, selected.LogComponent) + selected.LogString + Environment.NewLine;
			}
			Clipboard.SetText(clipboardText);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003B20 File Offset: 0x00001D20
		private void ListViewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003B24 File Offset: 0x00001D24
		private void clearButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SB.Logger.EntriesCollection.Clear();
			}
			catch
			{
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003B58 File Offset: 0x00001D58
		private void searchButton_Click(object sender, RoutedEventArgs e)
		{
			SB.Logger.Refresh();
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00003C70 File Offset: 0x00001E70
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 3)
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = UIElement.MouseRightButtonDownEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.ListViewItem_MouseRightButtonDown);
				((Style)target).Setters.Add(eventSetter);
			}
		}
	}
}
