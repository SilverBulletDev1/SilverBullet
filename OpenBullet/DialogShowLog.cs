using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using RuriLib;

namespace OpenBullet
{
	// Token: 0x02000033 RID: 51
	public class DialogShowLog : Page, IComponentConnector
	{
		// Token: 0x06000100 RID: 256 RVA: 0x000047DC File Offset: 0x000029DC
		public DialogShowLog(List<LogEntry> log)
		{
			this.InitializeComponent();
			base.DataContext = this.vm;
			this.logRTB.Font = new Font("Consolas", 10f);
			this.logRTB.BackColor = Color.FromArgb(22, 22, 22);
			foreach (LogEntry entry in log)
			{
				this.logRTB.AppendText(entry.LogString + Environment.NewLine, entry.LogColor);
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004898 File Offset: 0x00002A98
		private void searchButton_Click(object sender, RoutedEventArgs e)
		{
			SB.Logger.LogInfo(Components.Stacker, "Seaching for " + this.vm.SearchString, false, 0);
			this.logRTB.SelectAll();
			this.logRTB.SelectionBackColor = Color.FromArgb(22, 22, 22);
			this.logRTB.DeselectAll();
			if (this.vm.SearchString == string.Empty)
			{
				return;
			}
			int sstart = this.logRTB.SelectionStart;
			int startIndex = 0;
			this.vm.Indexes.Clear();
			int index;
			while ((index = this.logRTB.Text.IndexOf(this.vm.SearchString, startIndex, StringComparison.InvariantCultureIgnoreCase)) != -1)
			{
				this.logRTB.Select(index, this.vm.SearchString.Length);
				this.logRTB.SelectionColor = Color.White;
				this.logRTB.SelectionBackColor = Color.Navy;
				startIndex = index + this.vm.SearchString.Length;
				this.vm.Indexes.Add(startIndex);
				if (this.vm.Indexes.Count == 1)
				{
					this.logRTB.ScrollToCaret();
				}
			}
			this.vm.UpdateTotalSearchMatches();
			this.logRTB.SelectionStart = sstart;
			this.logRTB.SelectionLength = 0;
			this.logRTB.SelectionColor = Color.Black;
			SB.Logger.LogInfo(Components.Stacker, string.Format("Found {0} matches", this.vm.Indexes.Count), true, 0);
			if (this.vm.Indexes.Count > 0)
			{
				this.vm.CurrentSearchMatch = 1;
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00004A4C File Offset: 0x00002C4C
		private void previousMatchButton_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (this.vm.TotalSearchMatches == 0)
			{
				return;
			}
			if (this.vm.CurrentSearchMatch == 1)
			{
				this.vm.CurrentSearchMatch = this.vm.Indexes.Count;
			}
			else
			{
				FullLogViewModel fullLogViewModel = this.vm;
				int currentSearchMatch = fullLogViewModel.CurrentSearchMatch;
				fullLogViewModel.CurrentSearchMatch = currentSearchMatch - 1;
			}
			this.logRTB.DeselectAll();
			this.logRTB.Select(this.vm.Indexes[this.vm.CurrentSearchMatch - 1], 0);
			this.logRTB.ScrollToCaret();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00004AE8 File Offset: 0x00002CE8
		private void nextMatchButton_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (this.vm.TotalSearchMatches == 0)
			{
				return;
			}
			if (this.vm.CurrentSearchMatch == this.vm.Indexes.Count)
			{
				this.vm.CurrentSearchMatch = 1;
			}
			else
			{
				FullLogViewModel fullLogViewModel = this.vm;
				int currentSearchMatch = fullLogViewModel.CurrentSearchMatch;
				fullLogViewModel.CurrentSearchMatch = currentSearchMatch + 1;
			}
			this.logRTB.DeselectAll();
			this.logRTB.Select(this.vm.Indexes[this.vm.CurrentSearchMatch - 1], 0);
			this.logRTB.ScrollToCaret();
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00004B84 File Offset: 0x00002D84
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/dialogs/dialogshowlog.xaml", UriKind.Relative);
			global::System.Windows.Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004BB4 File Offset: 0x00002DB4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.logRTB = (global::System.Windows.Forms.RichTextBox)target;
				return;
			case 2:
				this.searchButton = (global::System.Windows.Controls.Button)target;
				this.searchButton.Click += this.searchButton_Click;
				return;
			case 3:
				this.previousMatchButton = (global::System.Windows.Controls.Image)target;
				this.previousMatchButton.MouseDown += this.previousMatchButton_MouseDown;
				return;
			case 4:
				this.nextMatchButton = (global::System.Windows.Controls.Image)target;
				this.nextMatchButton.MouseDown += this.nextMatchButton_MouseDown;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x04000101 RID: 257
		public FullLogViewModel vm = new FullLogViewModel();

		// Token: 0x04000102 RID: 258
		internal global::System.Windows.Forms.RichTextBox logRTB;

		// Token: 0x04000103 RID: 259
		internal global::System.Windows.Controls.Button searchButton;

		// Token: 0x04000104 RID: 260
		internal global::System.Windows.Controls.Image previousMatchButton;

		// Token: 0x04000105 RID: 261
		internal global::System.Windows.Controls.Image nextMatchButton;

		// Token: 0x04000106 RID: 262
		private bool _contentLoaded;
	}
}
