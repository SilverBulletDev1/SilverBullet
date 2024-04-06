using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Extreme.Net;
using Microsoft.Win32;
using OpenBullet.Views.Main;

namespace OpenBullet
{
	// Token: 0x02000046 RID: 70
	public class DialogAddProxies : Page, IComponentConnector
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00007528 File Offset: 0x00005728
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00007530 File Offset: 0x00005730
		public object Caller { get; set; }

		// Token: 0x06000197 RID: 407 RVA: 0x0000753C File Offset: 0x0000573C
		public DialogAddProxies(object caller)
		{
			this.InitializeComponent();
			this.Caller = caller;
			foreach (string i in Enum.GetNames(typeof(ProxyType)))
			{
				if (i != "Chain")
				{
					this.proxyTypeCombobox.Items.Add(i);
				}
			}
			this.proxyTypeCombobox.SelectedIndex = 0;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000075AC File Offset: 0x000057AC
		private void loadProxiesButton_MouseDown(object sender, MouseButtonEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog
			{
				Filter = "Proxy files | *.txt",
				FilterIndex = 1,
				Multiselect = true
			};
			bool? flag = ofd.ShowDialog();
			bool flag2 = true;
			if ((flag.GetValueOrDefault() == flag2) & (flag != null))
			{
				for (int i = 0; i < ofd.FileNames.Length; i++)
				{
					this.locationListBox.Items.Add(ofd.FileNames[i]);
				}
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00007624 File Offset: 0x00005824
		private void acceptButton_Click(object sender, RoutedEventArgs e)
		{
			string[] fileNames;
			if (this.locationListBox.SelectedItems == null)
			{
				fileNames = this.locationListBox.Items.OfType<string>().ToArray<string>();
			}
			else
			{
				fileNames = this.locationListBox.Items.OfType<string>().ToArray<string>();
			}
			List<string> lines = new List<string>();
			try
			{
				switch (this.modeTabControl.SelectedIndex)
				{
				case 0:
					if (fileNames.Length == 1)
					{
						SB.Logger.LogInfo(Components.ProxyManager, "Trying to load from file " + fileNames[0], false, 0);
						lines.AddRange(File.ReadAllLines(fileNames[0]));
					}
					else
					{
						if (fileNames.Length <= 1)
						{
							SB.Logger.LogError(Components.ProxyManager, "No file specified!", true, 0);
							return;
						}
						for (int i = 0; i < fileNames.Length; i++)
						{
							lines.AddRange(File.ReadAllLines(fileNames[i]));
						}
					}
					break;
				case 1:
					if (!(this.proxiesBox.Text != string.Empty))
					{
						SB.Logger.LogError(Components.ProxyManager, "The box is empty!", true, 0);
						return;
					}
					lines.AddRange(this.proxiesBox.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None));
					break;
				case 2:
				{
					if (!(this.urlTextbox.Text != string.Empty))
					{
						SB.Logger.LogError(Components.ProxyManager, "No URL specified!", true, 0);
						return;
					}
					string response = new HttpRequest().Get(this.urlTextbox.Text, null).ToString();
					lines.AddRange(response.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None));
					break;
				}
				}
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.ProxyManager, "There was an error: " + ex.Message, false, 0);
				return;
			}
			if (this.Caller.GetType() == typeof(ProxyManager))
			{
				((ProxyManager)this.Caller).AddProxies(lines, (ProxyType)Enum.Parse(typeof(ProxyType), this.proxyTypeCombobox.Text), this.usernameTextbox.Text, this.passwordTextbox.Text);
			}
			((MainDialog)base.Parent).Close();
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000788C File Offset: 0x00005A8C
		private void FileMode_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.fileMode.Foreground = Utils.GetBrush("ForegroundMenuSelected");
			this.pasteMode.Foreground = Utils.GetBrush("ForegroundMain");
			this.apiMode.Foreground = Utils.GetBrush("ForegroundMain");
			this.modeTabControl.SelectedIndex = 0;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000078E4 File Offset: 0x00005AE4
		private void PasteMode_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.fileMode.Foreground = Utils.GetBrush("ForegroundMain");
			this.pasteMode.Foreground = Utils.GetBrush("ForegroundMenuSelected");
			this.apiMode.Foreground = Utils.GetBrush("ForegroundMain");
			this.modeTabControl.SelectedIndex = 1;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000793C File Offset: 0x00005B3C
		private void ApiMode_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.fileMode.Foreground = Utils.GetBrush("ForegroundMain");
			this.pasteMode.Foreground = Utils.GetBrush("ForegroundMain");
			this.apiMode.Foreground = Utils.GetBrush("ForegroundMenuSelected");
			this.modeTabControl.SelectedIndex = 2;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00007994 File Offset: 0x00005B94
		private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				this.locationListBox.Items.RemoveAt(this.locationListBox.SelectedIndex);
			}
			catch
			{
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000079D4 File Offset: 0x00005BD4
		private void Grid_MouseDown_1(object sender, MouseButtonEventArgs e)
		{
			try
			{
				this.locationListBox.Items.Clear();
			}
			catch
			{
			}
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00007A08 File Offset: 0x00005C08
		private void locationListBox_DragEnter(object sender, DragEventArgs e)
		{
			try
			{
				if (e.Data.GetDataPresent(DataFormats.FileDrop))
				{
					e.Effects = DragDropEffects.Copy;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007A44 File Offset: 0x00005C44
		private void locationListBox_Drop(object sender, DragEventArgs e)
		{
			try
			{
				string[] locations = (string[])e.Data.GetData(DataFormats.FileDrop);
				Task.Run(delegate
				{
					for (int i = 0; i < locations.Length; i++)
					{
						try
						{
							string loc = locations[i];
							if (loc.EndsWith(".txt") && File.Exists(loc))
							{
								this.Dispatcher.Invoke<int>(() => this.locationListBox.Items.Add(loc));
							}
						}
						catch
						{
						}
					}
				});
			}
			catch
			{
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00007AA0 File Offset: 0x00005CA0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/dialogs/dialogaddproxies.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00007AD0 File Offset: 0x00005CD0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.fileMode = (Label)target;
				this.fileMode.MouseDown += this.FileMode_MouseDown;
				return;
			case 2:
				this.pasteMode = (Label)target;
				this.pasteMode.MouseDown += this.PasteMode_MouseDown;
				return;
			case 3:
				this.apiMode = (Label)target;
				this.apiMode.MouseDown += this.ApiMode_MouseDown;
				return;
			case 4:
				this.modeTabControl = (TabControl)target;
				return;
			case 5:
				this.fileTab = (TabItem)target;
				return;
			case 6:
				this.locationListBox = (ListBox)target;
				this.locationListBox.DragEnter += this.locationListBox_DragEnter;
				this.locationListBox.Drop += this.locationListBox_Drop;
				return;
			case 7:
				this.loadProxiesButton = (Image)target;
				this.loadProxiesButton.MouseDown += this.loadProxiesButton_MouseDown;
				return;
			case 8:
				((Grid)target).MouseDown += this.Grid_MouseDown;
				return;
			case 9:
				((Grid)target).MouseDown += this.Grid_MouseDown_1;
				return;
			case 10:
				this.pasteTab = (TabItem)target;
				return;
			case 11:
				this.proxiesBox = (TextBox)target;
				return;
			case 12:
				this.apiTab = (TabItem)target;
				return;
			case 13:
				this.urlTextbox = (TextBox)target;
				return;
			case 14:
				this.advancedWarning = (TextBlock)target;
				return;
			case 15:
				this.proxyTypeCombobox = (ComboBox)target;
				return;
			case 16:
				this.usernameTextbox = (TextBox)target;
				return;
			case 17:
				this.passwordTextbox = (TextBox)target;
				return;
			case 18:
				this.acceptButton = (Button)target;
				this.acceptButton.Click += this.acceptButton_Click;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x0400016A RID: 362
		internal Label fileMode;

		// Token: 0x0400016B RID: 363
		internal Label pasteMode;

		// Token: 0x0400016C RID: 364
		internal Label apiMode;

		// Token: 0x0400016D RID: 365
		internal TabControl modeTabControl;

		// Token: 0x0400016E RID: 366
		internal TabItem fileTab;

		// Token: 0x0400016F RID: 367
		internal ListBox locationListBox;

		// Token: 0x04000170 RID: 368
		internal Image loadProxiesButton;

		// Token: 0x04000171 RID: 369
		internal TabItem pasteTab;

		// Token: 0x04000172 RID: 370
		internal TextBox proxiesBox;

		// Token: 0x04000173 RID: 371
		internal TabItem apiTab;

		// Token: 0x04000174 RID: 372
		internal TextBox urlTextbox;

		// Token: 0x04000175 RID: 373
		internal TextBlock advancedWarning;

		// Token: 0x04000176 RID: 374
		internal ComboBox proxyTypeCombobox;

		// Token: 0x04000177 RID: 375
		internal TextBox usernameTextbox;

		// Token: 0x04000178 RID: 376
		internal TextBox passwordTextbox;

		// Token: 0x04000179 RID: 377
		internal Button acceptButton;

		// Token: 0x0400017A RID: 378
		private bool _contentLoaded;
	}
}
