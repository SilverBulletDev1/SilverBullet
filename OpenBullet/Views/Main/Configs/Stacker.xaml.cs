using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using EO.WebBrowser;
using EO.WebEngine;
using EO.Wpf;
using Extreme.Net;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using MahApps.Metro.IconPacks;
using OpenBullet.Editor.CustomSearch;
using OpenBullet.ViewModels;
using OpenBullet.Views.CustomMessageBox;
using OpenBullet.Views.Dialogs;
using PluginFramework;
using RuriLib;
using RuriLib.LS;
using RuriLib.Models;
using RuriLib.Runner;
using RuriLib.ViewModels;
using SilverBullet.Tesseract;

namespace OpenBullet.Views.Main.Configs
{
	// Token: 0x020000FE RID: 254
	public partial class Stacker : Page
	{
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060006C1 RID: 1729 RVA: 0x00025A48 File Offset: 0x00023C48
		// (remove) Token: 0x060006C2 RID: 1730 RVA: 0x00025A80 File Offset: 0x00023C80
		public event Stacker.SaveConfigEventHandler SaveConfig;

		// Token: 0x060006C3 RID: 1731 RVA: 0x00025AB8 File Offset: 0x00023CB8
		protected virtual void OnSaveConfig()
		{
			Stacker.SaveConfigEventHandler saveConfig = this.SaveConfig;
			if (saveConfig != null)
			{
				saveConfig(this, EventArgs.Empty);
			}
			try
			{
				LogEntry log = SB.Logger.Entries.LastOrDefault<LogEntry>();
				if (log == null || (!log.LogString.StartsWith("Failed to save the config. Reason:") && log.LogLevel != LogLevel.Error))
				{
					this.iconSave.Foreground = this.bc.ConvertFrom("#FF5DF5A7") as Brush;
					this.RestoreForegroundIconSave();
				}
				else
				{
					this.iconSave.Foreground = this.bc.ConvertFrom("#FFF5645D") as Brush;
					this.RestoreForegroundIconSave();
				}
			}
			catch
			{
				try
				{
					this.iconSave.Foreground = Brushes.White;
				}
				catch
				{
				}
			}
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00025B90 File Offset: 0x00023D90
		private void RestoreForegroundIconSave()
		{
			try
			{
				try
				{
					Task task = this.taskResto;
					if (task != null)
					{
						task.Dispose();
					}
				}
				catch
				{
				}
				this.taskResto = Task.Run(delegate
				{
					Task.Delay(1099).Wait();
					base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate
					{
						this.iconSave.Foreground = Brushes.White;
					}));
				});
			}
			catch
			{
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00025BEC File Offset: 0x00023DEC
		public Stacker()
		{
			this.vm = SB.Stacker;
			base.DataContext = this.vm;
			this.InitializeComponent();
			this.loliScriptEditor.ShowLineNumbers = true;
			this.loliScriptEditor.TextArea.Foreground = new SolidColorBrush(Colors.Gainsboro);
			this.loliScriptEditor.TextArea.TextView.LinkTextForegroundBrush = new SolidColorBrush(Colors.DodgerBlue);
			using (XmlReader reader = XmlReader.Create("LSHighlighting.xshd"))
			{
				this.loliScriptEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
			}
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.Load("SyntaxHelper.xml");
				XmlNode main = doc.DocumentElement.SelectSingleNode("/doc");
				this.syntaxHelperItems = main.ChildNodes;
				this.loliScriptEditor.KeyDown += this.loliScriptEditor_KeyDown;
				this.loliScriptEditor.KeyUp += this.LoliScriptEditor_KeyUp;
			}
			catch
			{
			}
			try
			{
				doc.Load("ScriptCompletion.xml");
				this.scriptCompletion = doc.DocumentElement.SelectSingleNode("/Keywords").ChildNodes;
			}
			catch
			{
			}
			this.toolTipEditor = new TextEditor();
			this.toolTipEditor.TextArea.Foreground = Utils.GetBrush("ForegroundMain");
			this.toolTipEditor.Background = new SolidColorBrush(Color.FromArgb(22, 22, 22, 50));
			this.toolTipEditor.TextArea.TextView.LinkTextForegroundBrush = new SolidColorBrush(Colors.DodgerBlue);
			this.toolTipEditor.FontSize = 11.0;
			this.toolTipEditor.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
			this.toolTipEditor.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
			using (XmlReader reader2 = XmlReader.Create("LSHighlighting.xshd"))
			{
				this.toolTipEditor.SyntaxHighlighting = HighlightingLoader.Load(reader2, HighlightingManager.Instance);
			}
			this.toolTip = new ToolTip
			{
				Placement = PlacementMode.Relative,
				PlacementTarget = this.loliScriptEditor
			};
			this.toolTip.Content = this.toolTipEditor;
			this.loliScriptEditor.ToolTip = this.toolTip;
			this.vm.LS = new LoliScript(SB.ConfigManager.CurrentConfig.Config.Script);
			this.loliScriptEditor.Text = this.vm.LS.Script;
			if (!SB.SBSettings.General.DisplayLoliScriptOnLoad)
			{
				this.stackButton_Click(this, null);
			}
			this.logRTB.TextArea.TextView.LinkTextUnderline = false;
			this.logRTB.TextArea.TextView.LinkTextForegroundBrush = new SolidColorBrush(Colors.DodgerBlue);
			this.logRTB.Options.EnableEmailHyperlinks = false;
			this.logRTB.Options.EnableImeSupport = false;
			this.logRTB.Options.CutCopyWholeLine = false;
			this.logRTB.Options.EnableTextDragDrop = false;
			this.logRTB.Options.EnableVirtualSpace = false;
			this.logRTB.Options.ShowTabs = false;
			this.logRTB.TextArea.TextView.Triggers.Clear();
			this.logRTB.Options.EnableRectangularSelection = false;
			this.searchTextEditor = SearchTextEditor.Install(this.logRTB);
			foreach (string i in Enum.GetNames(typeof(ProxyType)))
			{
				if (i != "Chain")
				{
					this.proxyTypeCombobox.Items.Add(i);
				}
			}
			this.proxyTypeCombobox.SelectedIndex = 0;
			foreach (string t in SB.Settings.Environment.GetWordlistTypeNames())
			{
				this.testDataTypeCombobox.Items.Add(t);
			}
			this.testDataTypeCombobox.SelectedIndex = 0;
			this.debugger.WorkerSupportsCancellation = true;
			this.debugger.Status = WorkerStatus.Idle;
			this.debugger.DoWork += this.DebuggerCheck;
			this.debugger.RunWorkerCompleted += this.debuggerCompleted;
			this.SaveConfig += SB.MainWindow.ConfigsPage.ConfigManagerPage.OnSaveConfig;
			try
			{
				BrowserOptions options = new BrowserOptions
				{
					EnableWebSecurity = new bool?(false),
					AllowJavaScriptCloseWindow = new bool?(true),
					AllowJavaScriptAccessClipboard = new bool?(false),
					AllowZooming = new bool?(true)
				};
				EngineOptions.Default.SetDefaultBrowserOptions(options);
			}
			catch
			{
			}
			this.SetBrowserStatus("Initialized!");
			this.vm.Stack.CollectionChanged += this.Stack_CollectionChanged;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0002612C File Offset: 0x0002432C
		private void Stack_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (object obj in e.NewItems)
				{
					((StackerBlockViewModel)obj).Page.LostFocus += delegate
					{
						this.AutoSaveConfig();
					};
				}
			}
			this.AutoSaveConfig();
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x000261A4 File Offset: 0x000243A4
		private void TextArea_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Key == global::System.Windows.Input.Key.Space && e.KeyboardDevice.Modifiers == ModifierKeys.Control && this.vm.ScriptCompletion)
				{
					this.InvokeCompletionWindow(this.GetDataList(string.Empty), Brushes.White);
					e.Handled = true;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00026208 File Offset: 0x00024408
		private void TextArea_TextEntered(object sender, TextCompositionEventArgs e)
		{
			try
			{
				if (!(e.Text == "\n") && this.vm.ScriptCompletion)
				{
					if (this.completionWindow == null && e.Text == " ")
					{
						this.InvokeCompletionWindow(this.GetDataList(e.Text), Brushes.White);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x000080E9 File Offset: 0x000062E9
		private IEnumerable<Tuple<string, string>> GetDataList(string text)
		{
			return null;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0002627C File Offset: 0x0002447C
		private void TextArea_TextEntering(object sender, TextCompositionEventArgs e)
		{
			if (!this.vm.ScriptCompletion)
			{
				return;
			}
			if (e.Text.Length > 0 && this.completionWindow != null && char.IsLetter(e.Text[0]))
			{
				this.completionWindow.CompletionList.RequestInsertion(e);
			}
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x000262D4 File Offset: 0x000244D4
		private void InvokeCompletionWindow(IEnumerable<Tuple<string, string>> scriptAutoCompleteList, Brush foreground)
		{
			CompletionWindow completionWindow = new CompletionWindow(this.loliScriptEditor.TextArea);
			completionWindow.Background = (completionWindow.CompletionList.ListBox.Background = (completionWindow.CompletionList.Background = Brushes.Black));
			Control completionList = completionWindow.CompletionList;
			completionWindow.CompletionList.ListBox.Foreground = foreground;
			completionList.Foreground = foreground;
			IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
			if (scriptAutoCompleteList.Any<Tuple<string, string>>())
			{
				foreach (Tuple<string, string> autoCompleteScript in scriptAutoCompleteList)
				{
					data.Add(new LoliScriptCompletionData(autoCompleteScript.Item1, autoCompleteScript.Item2));
				}
				completionWindow.Show();
				completionWindow.Closed += delegate
				{
					completionWindow = null;
				};
			}
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x000263F0 File Offset: 0x000245F0
		private void ClearDebuggerLog(object sender, EventArgs e)
		{
			if (SB.SBSettings.General.SendDebuggerLogToNotepadPlus)
			{
				try
				{
					NotepadPlusExtensions.Clear();
				}
				catch
				{
				}
			}
			this.logRTB.Clear();
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00026434 File Offset: 0x00024634
		private void Image_MouseEnter(object sender, MouseEventArgs e)
		{
			try
			{
				PackIconBase packIconBase = VisualTreeHelper.GetChild((Grid)e.OriginalSource, 0) as PackIconBase;
				packIconBase.Width = 27.5;
				packIconBase.Height = 27.5;
			}
			catch (InvalidCastException)
			{
			}
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0002648C File Offset: 0x0002468C
		private void Image_MouseLeave(object sender, MouseEventArgs e)
		{
			try
			{
				PackIconBase packIconBase = VisualTreeHelper.GetChild((Grid)e.OriginalSource, 0) as PackIconBase;
				packIconBase.Width = 24.0;
				packIconBase.Height = 24.0;
			}
			catch (InvalidCastException)
			{
			}
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x000264E4 File Offset: 0x000246E4
		private void AddBlock_MouseDown(object sender, MouseButtonEventArgs e)
		{
			new MainDialog(new DialogAddBlock(this), "Add Block").ShowDialog();
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x000264FC File Offset: 0x000246FC
		public void AddBlock(BlockBase block)
		{
			int position;
			if (this.vm.CurrentBlockIndex == -1)
			{
				position = this.vm.Stack.Count;
			}
			else
			{
				position = ((this.vm.Stack.Count > 0) ? (this.vm.CurrentBlockIndex + 1) : 0);
			}
			SB.Logger.LogInfo(Components.Stacker, string.Format("Added a block of type {0} in position {1}", block.GetType(), position), false, 0);
			this.vm.AddBlock(block, position);
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00026580 File Offset: 0x00024780
		private void RemoveBlock_MouseDown(object sender, MouseButtonEventArgs e)
		{
			foreach (StackerBlockViewModel block in this.vm.SelectedBlocks)
			{
				this.vm.Stack.Remove(block);
			}
			this.vm.CurrentBlock = null;
			this.BlockInfo.Content = null;
			this.vm.UpdateHeights();
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00026608 File Offset: 0x00024808
		private void DisableBlock_MouseDown(object sender, MouseButtonEventArgs e)
		{
			foreach (StackerBlockViewModel stackerBlockViewModel in this.vm.SelectedBlocks)
			{
				stackerBlockViewModel.Disable();
			}
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00026660 File Offset: 0x00024860
		private void CloneBlock_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.vm.ConvertKeychains();
			foreach (StackerBlockViewModel block in this.vm.SelectedBlocks)
			{
				this.vm.AddBlock(IOManager.CloneBlock(block.Block), this.vm.Stack.IndexOf(block) + 1);
			}
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x000266E8 File Offset: 0x000248E8
		private void MoveUpBlock_MouseDown(object sender, MouseButtonEventArgs e)
		{
			foreach (StackerBlockViewModel block in this.vm.SelectedBlocks)
			{
				this.vm.MoveBlockUp(block);
			}
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00026748 File Offset: 0x00024948
		private void MoveDownBlock_MouseDown(object sender, MouseButtonEventArgs e)
		{
			foreach (StackerBlockViewModel block in this.vm.SelectedBlocks.AsEnumerable<StackerBlockViewModel>().Reverse<StackerBlockViewModel>())
			{
				this.vm.MoveBlockDown(block);
			}
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x000267AC File Offset: 0x000249AC
		private void SaveConfig_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.OnSaveConfig();
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x000267B4 File Offset: 0x000249B4
		private void Page_KeyDown(object sender, KeyEventArgs e)
		{
			if (Keyboard.Modifiers == ModifierKeys.Control)
			{
				global::System.Windows.Input.Key key = e.Key;
				if (key <= global::System.Windows.Input.Key.S)
				{
					if (key != global::System.Windows.Input.Key.C)
					{
						if (key != global::System.Windows.Input.Key.S)
						{
							return;
						}
						goto IL_195;
					}
					else
					{
						if (SB.SBSettings.General.DisableCopyPasteBlocks)
						{
							return;
						}
						try
						{
							Clipboard.SetText(IOManager.SerializeBlocks(this.vm.SelectedBlocks.Select((StackerBlockViewModel b) => b.Block).ToList<BlockBase>()));
							return;
						}
						catch
						{
							SB.Logger.LogError(Components.Stacker, "Exception while copying blocks", false, 0);
							return;
						}
					}
				}
				else if (key != global::System.Windows.Input.Key.V)
				{
					if (key != global::System.Windows.Input.Key.Z)
					{
						return;
					}
					if (this.vm.LastDeletedBlock != null)
					{
						this.vm.AddBlock(this.vm.LastDeletedBlock, this.vm.LastDeletedIndex);
						SB.Logger.LogInfo(Components.Stacker, string.Format("Readded block of type {0} in position {1}", this.vm.LastDeletedBlock.GetType(), this.vm.LastDeletedIndex), false, 0);
						this.vm.LastDeletedBlock = null;
						return;
					}
					SB.Logger.LogError(Components.Stacker, "Nothing to undo", false, 0);
					return;
				}
				if (SB.SBSettings.General.DisableCopyPasteBlocks)
				{
					return;
				}
				try
				{
					foreach (BlockBase block in IOManager.DeserializeBlocks(Clipboard.GetText()))
					{
						this.vm.AddBlock(block, -1);
					}
					return;
				}
				catch
				{
					SB.Logger.LogError(Components.Stacker, "Exception while pasting blocks", false, 0);
					return;
				}
				IL_195:
				this.vm.LS.Script = this.loliScriptEditor.Text;
				this.OnSaveConfig();
			}
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x000269A0 File Offset: 0x00024BA0
		private void startDebuggerButton_Click(object sender, RoutedEventArgs e)
		{
			this.AutoSaveConfig();
			switch (this.debugger.Status)
			{
			case WorkerStatus.Idle:
				if (this.vm.View == StackerView.Blocks)
				{
					this.vm.LS.FromBlocks(this.vm.GetList());
				}
				else
				{
					this.vm.LS.Script = this.loliScriptEditor.Text;
				}
				if (this.debuggerTabControl.SelectedIndex == 1)
				{
					this.logRTB.Focus();
				}
				this.vm.ControlsEnabled = false;
				if (!SB.SBSettings.General.PersistDebuggerLog)
				{
					this.ClearDebuggerLog(null, null);
				}
				this.dataRTB.Document.Blocks.Clear();
				if (!this.debugger.IsBusy)
				{
					this.debugger.RunWorkerAsync();
					SB.Logger.LogInfo(Components.Stacker, "Started the debugger", false, 0);
				}
				else
				{
					SB.Logger.LogError(Components.Stacker, "Cannot start the debugger (busy)", false, 0);
				}
				this.startDebuggerButtonLabel.Text = "Abort";
				this.startDebuggerButtonLabel.Margin = new Thickness(2.0, 0.0, 0.0, 0.0);
				this.startDebuggerButtonIcon.Kind = PackIconMaterialKind.Stop;
				this.startDebuggerButtonIcon.Height = 10.0;
				this.debugger.Status = WorkerStatus.Running;
				return;
			case WorkerStatus.Running:
				if (this.debugger.IsBusy)
				{
					this.debugger.CancelAsync();
					SB.Logger.LogInfo(Components.Stacker, "Sent Cancellation Request to the debugger", false, 0);
				}
				this.startDebuggerButtonLabel.Text = "Force";
				this.startDebuggerButtonLabel.Margin = new Thickness(2.0, 0.0, 0.0, 0.0);
				this.startDebuggerButtonIcon.Kind = PackIconMaterialKind.Stop;
				this.startDebuggerButtonIcon.Height = 10.0;
				this.debugger.Status = WorkerStatus.Stopping;
				return;
			case WorkerStatus.Stopping:
				this.debugger.Abort();
				SB.Logger.LogInfo(Components.Stacker, "Hard aborted the debugger", false, 0);
				this.startDebuggerButtonLabel.Text = "Start";
				this.startDebuggerButtonLabel.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
				this.startDebuggerButtonIcon.Kind = PackIconMaterialKind.Play;
				this.startDebuggerButtonIcon.Height = 13.0;
				this.debugger.Status = WorkerStatus.Idle;
				this.vm.ControlsEnabled = true;
				return;
			default:
				return;
			}
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00026C5C File Offset: 0x00024E5C
		private void DebuggerCheck(object sender, DoWorkEventArgs e)
		{
			if (this.vm.BotData != null && this.vm.BotData.BrowserOpen)
			{
				SB.Logger.LogInfo(Components.Stacker, "Quitting the previously opened browser", false, 0);
				this.vm.BotData.Driver.Quit();
				SB.Logger.LogInfo(Components.Stacker, "Quitted correctly", false, 0);
			}
			SB.Logger.LogInfo(Components.Stacker, "Converting Observables", false, 0);
			this.vm.ConvertKeychains();
			SB.Logger.LogInfo(Components.Stacker, "Initializing the request data", false, 0);
			CProxy proxy = null;
			if (this.vm.TestProxy.StartsWith("("))
			{
				try
				{
					proxy = new CProxy().Parse(this.vm.TestProxy, ProxyType.Http, "", "");
					goto IL_172;
				}
				catch
				{
					SB.Logger.LogError(Components.Stacker, "Invalid Proxy Syntax", true, 0);
					goto IL_172;
				}
			}
			string proxyAddress = string.Empty;
			string user = string.Empty;
			string pass = string.Empty;
			if (this.vm.TestProxy.Contains(":"))
			{
				string[] prox = this.vm.TestProxy.Split(new char[] { ':' });
				proxyAddress = prox[0] + ":" + prox[1];
				try
				{
					user = prox[2];
				}
				catch
				{
				}
				try
				{
					pass = prox[3];
				}
				catch
				{
				}
			}
			proxy = new CProxy(proxyAddress, this.vm.ProxyType);
			proxy.Username = user;
			proxy.Password = pass;
			IL_172:
			CData cData = new CData(this.vm.TestData, SB.Settings.Environment.GetWordlistType(this.vm.TestDataType));
			try
			{
				OcrEngine ocrEngine = this._ocrEngine;
				if (ocrEngine != null)
				{
					ocrEngine.DisposeEngines();
				}
			}
			catch
			{
			}
			StackerViewModel stackerViewModel = this.vm;
			BotData botData = new BotData(SB.Settings.RLSettings, this.vm.Config.Config.Settings, cData, proxy, this.vm.UseProxy, new Random(), 1, true);
			botData.BotsAmount = 1;
			OcrEngine ocrEngine2;
			if ((ocrEngine2 = this._ocrEngine) == null)
			{
				ocrEngine2 = (this._ocrEngine = new OcrEngine());
			}
			botData.OcrEngine = ocrEngine2;
			botData.Worker = this.debugger;
			stackerViewModel.BotData = botData;
			this.vm.LS.Reset();
			base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate
			{
				this.browserStatus.Text = "Idle";
			}));
			using (IEnumerator<CustomInput> enumerator = this.vm.BotData.ConfigSettings.CustomInputs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CustomInput input = enumerator.Current;
					SB.Logger.LogInfo(Components.Stacker, "Asking for user input: " + input.Description, false, 0);
					Application.Current.Dispatcher.Invoke(delegate
					{
						new MainDialog(new DialogCustomInput(this.vm, input.VariableName, input.Description), "Custom Input").ShowDialog();
					});
				}
			}
			SB.Logger.LogInfo(Components.Stacker, "Setting the first block as the current block", false, 0);
			string proxyEnabledText = (this.vm.UseProxy ? "ENABLED" : "DISABLED");
			this.vm.BotData.LogBuffer.Add(new LogEntry(string.Format("===== DEBUGGER STARTED FOR CONFIG {0} WITH DATA {1} AND PROXY {2} ({3}) {4} ====={5}", new object[]
			{
				this.vm.Config.Name,
				this.vm.TestData,
				this.vm.TestProxy,
				this.vm.ProxyType,
				proxyEnabledText,
				Environment.NewLine
			}), Colors.White));
			this.timer = new Stopwatch();
			this.timer.Start();
			if (this.vm.Config.Config.Settings.AlwaysOpen)
			{
				SB.Logger.LogInfo(Components.Stacker, "Opening the Browser", false, 0);
				SBlockBrowserAction.OpenBrowser(this.vm.BotData, "");
			}
			if (!this.vm.SBS)
			{
				while (!this.debugger.CancellationPending)
				{
					this.Process();
					if (!this.vm.LS.CanProceed)
					{
						goto IL_4DB;
					}
				}
				SB.Logger.LogInfo(Components.Stacker, "Found cancellation pending, aborting debugger", false, 0);
				return;
			}
			this.vm.SBSClear = true;
			for (;;)
			{
				Thread.Sleep(100);
				if (this.debugger.CancellationPending)
				{
					break;
				}
				if (this.vm.SBSClear)
				{
					this.vm.SBSEnabled = false;
					this.Process();
					SB.Logger.LogInfo(Components.Stacker, string.Format("Block processed in SBS mode, can proceed: {0}", this.vm.LS.CanProceed), false, 0);
					this.vm.SBSEnabled = true;
					this.vm.SBSClear = false;
				}
				if (!this.vm.LS.CanProceed)
				{
					goto Block_15;
				}
			}
			SB.Logger.LogInfo(Components.Stacker, "Found cancellation pending, aborting debugger", false, 0);
			return;
			Block_15:
			IL_4DB:
			if (this.vm.Config.Config.Settings.AlwaysQuit || (this.vm.Config.Config.Settings.QuitOnBanRetry && (this.vm.BotData.Status == BotStatus.BAN || this.vm.BotData.Status == BotStatus.RETRY)))
			{
				try
				{
					this.vm.BotData.Driver.Quit();
					this.vm.BotData.BrowserOpen = false;
					SB.Logger.LogInfo(Components.Stacker, "Successfully quit the browser", false, 0);
				}
				catch (Exception ex)
				{
					SB.Logger.LogError(Components.Stacker, "Cannot quit the browser - " + ex.Message, false, 0);
				}
			}
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00027250 File Offset: 0x00025450
		private void Process()
		{
			try
			{
				this.vm.LS.TakeStep(this.vm.BotData);
				SB.Logger.LogInfo(Components.Stacker, "Processed " + BlockBase.TruncatePretty(this.vm.LS.CurrentLine, 20), false, 0);
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.Stacker, "Processing of line " + BlockBase.TruncatePretty(this.vm.LS.CurrentLine, 20) + " failed, exception: " + ex.Message, false, 0);
			}
			this.PrintBotData();
			Task task = Task.Run(delegate
			{
				this.PrintLogBuffer();
			});
			task.Wait();
			task.Dispose();
			this.DisplayHTML();
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0002731C File Offset: 0x0002551C
		private void PrintLogBuffer()
		{
			if (this.vm.BotData.LogBuffer.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this.vm.BotData.LogBuffer.Count; i++)
			{
				LogEntry entry = this.vm.BotData.LogBuffer[i];
				if (!SB.SBSettings.General.DisableDebuggerLog)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(delegate
					{
						this.logRTB.AppendTextToEditor(entry.LogString, entry.LogColor);
						this.logRTB.TextArea.Caret.BringCaretToView();
						this.logRTB.ScrollToLine(this.logRTB.LineCount);
					}));
				}
				if (SB.SBSettings.General.SendDebuggerLogToNotepadPlus)
				{
					NotepadPlusExtensions.ShowText(entry.LogString + Environment.NewLine);
				}
			}
			this.vm.BotData.LogBuffer.Add(new LogEntry(Environment.NewLine, Colors.White));
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00027409 File Offset: 0x00025609
		private void PrintBotData()
		{
			Application.Current.Dispatcher.Invoke(delegate
			{
				this.dataRTB.Document.Blocks.Clear();
				this.dataRTB.AppendText(Environment.NewLine);
				this.dataRTB.AppendText("BOT STATUS: " + this.vm.BotData.StatusString + Environment.NewLine, Colors.White);
				this.dataRTB.AppendText("VARIABLES:" + Environment.NewLine, Colors.Yellow);
				if (SB.SBSettings.General.DisplayCapturesLast)
				{
					foreach (CVar variable in this.vm.BotData.Variables.All.Where((CVar v) => !v.Hidden && !v.IsCapture))
					{
						this.dataRTB.AppendText(variable.Name + string.Format(" ({0}) = ", variable.Type) + variable.ToString() + Environment.NewLine, Colors.Yellow);
					}
					using (IEnumerator<CVar> enumerator = this.vm.BotData.Variables.All.Where((CVar v) => !v.Hidden && v.IsCapture).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							CVar variable2 = enumerator.Current;
							this.dataRTB.AppendText(variable2.Name + string.Format(" ({0}) = ", variable2.Type) + variable2.ToString() + Environment.NewLine, Colors.Tomato);
						}
						return;
					}
				}
				foreach (CVar variable3 in this.vm.BotData.Variables.All.Where((CVar v) => !v.Hidden))
				{
					this.dataRTB.AppendText(variable3.Name + string.Format(" ({0}) = ", variable3.Type) + variable3.ToString() + Environment.NewLine, variable3.IsCapture ? Colors.Tomato : Colors.Yellow);
				}
			});
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00027426 File Offset: 0x00025626
		private void DisplayHTML()
		{
			if (SB.SBSettings.General.DisableHTMLView)
			{
				return;
			}
			base.Dispatcher.Invoke(delegate
			{
				if (this.vm.BotData.ResponseSource != string.Empty)
				{
					int? num = this.tempSrc;
					int hashCode = this.vm.BotData.ResponseSource.GetHashCode();
					if (!((num.GetValueOrDefault() == hashCode) & (num != null)))
					{
						if (SB.SBSettings.General.LocalHTMLViewer)
						{
							EO.Wpf.WebView webView = this.webView;
							if (webView != null)
							{
								webView.LoadHtml(this.vm.BotData.ResponseSource);
							}
							this.tempSrc = new int?(this.vm.BotData.ResponseSource.GetHashCode());
							return;
						}
						if (SB.SBSettings.General.EnableCookiesInBrowser)
						{
							string domain = new Uri(this.vm.BotData.Address).Host;
							foreach (KeyValuePair<string, string> c in this.vm.BotData.Cookies)
							{
								try
								{
									this.webView.Engine.CookieManager.SetCookie(this.vm.BotData.Address, new Cookie(c.Key, c.Value)
									{
										Domain = domain,
										Path = "/"
									});
								}
								catch
								{
								}
							}
							foreach (KeyValuePair<string, string> c2 in this.vm.BotData.GlobalCookies)
							{
								try
								{
									this.webView.Engine.CookieManager.SetCookie(this.vm.BotData.Address, new Cookie(c2.Key, c2.Value)
									{
										Domain = domain,
										Path = "/"
									});
								}
								catch
								{
								}
							}
						}
						if (!this.vm.BotData.IsImage)
						{
							EO.Wpf.WebView webView2 = this.webView;
							if (webView2 != null)
							{
								webView2.LoadHtml(this.vm.BotData.ResponseSource, this.vm.BotData.Address);
							}
						}
						else
						{
							EO.Wpf.WebView webView3 = this.webView;
							if (webView3 != null)
							{
								webView3.LoadUrl(this.vm.BotData.Address);
							}
						}
						this.tempSrc = new int?(this.vm.BotData.ResponseSource.GetHashCode());
					}
				}
			});
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00027454 File Offset: 0x00025654
		public void HideScriptErrors(WebBrowser wb, bool hide)
		{
			FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
			if (fiComWebBrowser == null)
			{
				return;
			}
			object objComWebBrowser = fiComWebBrowser.GetValue(wb);
			if (objComWebBrowser == null)
			{
				wb.Loaded += delegate(object o, RoutedEventArgs s)
				{
					this.HideScriptErrors(wb, hide);
				};
				return;
			}
			objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x000274F1 File Offset: 0x000256F1
		private void WebView_BeforeNavigate(object sender, BeforeNavigateEventArgs e)
		{
			this.SetBrowserStatus("Navigating...");
			if (e.Cancel)
			{
				this.SetBrowserStatus("Navigation cancelled");
			}
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00027511 File Offset: 0x00025711
		private void WebView_LoadCompleted(object sender, LoadCompletedEventArgs e)
		{
			this.SetBrowserStatus(string.Format("Navigation completed ({0})", e.HttpStatusCode));
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00027530 File Offset: 0x00025730
		private void debuggerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.timer.Stop();
			this.debugger.Status = WorkerStatus.Idle;
			this.startDebuggerButtonLabel.Text = "Start";
			this.startDebuggerButtonLabel.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
			this.startDebuggerButtonIcon.Kind = PackIconMaterialKind.Play;
			this.startDebuggerButtonIcon.Height = 13.0;
			this.vm.SBSEnabled = false;
			this.vm.ControlsEnabled = true;
			this.vm.BotData.LogBuffer.Clear();
			if (!this.vm.BotData.Data.IsValid)
			{
				this.vm.BotData.LogBuffer.Add(new LogEntry("WARNING: The test input data did not respect the validity regex for the selected wordlist type!", Colors.Tomato));
			}
			if (!this.vm.BotData.Data.RespectsRules(this.vm.Config.Config.Settings.DataRules.ToList<DataRule>()))
			{
				this.vm.BotData.LogBuffer.Add(new LogEntry("WARNING: The test input data did not respect the data rules of this config!", Colors.Tomato));
			}
			this.vm.BotData.LogBuffer.Add(new LogEntry(string.Format("===== DEBUGGER ENDED AFTER {0} SECOND(S) WITH STATUS: {1} =====", (double)this.timer.ElapsedMilliseconds / 1000.0, this.vm.BotData.StatusString), Colors.White));
			this.PrintLogBuffer();
			SB.Logger.LogInfo(Components.Stacker, "Debugger completed", false, 0);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x000276EA File Offset: 0x000258EA
		private void nextStepButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.SBSClear = true;
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x000276F8 File Offset: 0x000258F8
		private void proxyTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.ProxyType = (ProxyType)this.proxyTypeCombobox.SelectedIndex;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00027710 File Offset: 0x00025910
		private void searchButton_Click(object sender, RoutedEventArgs e)
		{
			SB.Logger.LogInfo(Components.Stacker, "Seaching for " + this.vm.SearchString, false, 0);
			this.logRTB.TextArea.ClearSelection();
			if (this.vm.SearchString == string.Empty)
			{
				this.vm.TotalSearchMatches = 0;
				this.vm.CurrentSearchMatch = 0;
				return;
			}
			this.logRTB.SelectionStart = 0;
			this.searchTextEditor.SearchPattern = this.vm.SearchString;
			this.searchTextEditor.UpdateSearch();
			this.searchTextEditor.DoSearch(true);
			this.vm.TotalSearchMatches = this.searchTextEditor.Count;
			SB.Logger.LogInfo(Components.Stacker, string.Format("Found {0} matches", this.vm.TotalSearchMatches), false, 0);
			if (this.vm.TotalSearchMatches > 0)
			{
				this.vm.CurrentSearchMatch = 1;
			}
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00027810 File Offset: 0x00025A10
		public static List<int> AllIndexesOf(string str, string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentException("the string to find may not be empty", "value");
			}
			List<int> indexes = new List<int>();
			int index = 0;
			for (;;)
			{
				index = str.IndexOf(value, index, StringComparison.InvariantCultureIgnoreCase);
				if (index == -1)
				{
					break;
				}
				indexes.Add(index);
				index += value.Length;
			}
			return indexes;
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00027860 File Offset: 0x00025A60
		private void previousMatchButton_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (this.vm.TotalSearchMatches == 0)
			{
				return;
			}
			if (this.vm.CurrentSearchMatch == 1)
			{
				this.vm.CurrentSearchMatch = this.vm.TotalSearchMatches;
			}
			else
			{
				StackerViewModel stackerViewModel = this.vm;
				int currentSearchMatch = stackerViewModel.CurrentSearchMatch;
				stackerViewModel.CurrentSearchMatch = currentSearchMatch - 1;
			}
			this.searchTextEditor.FindPrevious();
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x000278C4 File Offset: 0x00025AC4
		private void nextMatchButton_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (this.vm.TotalSearchMatches == 0)
			{
				return;
			}
			if (this.vm.CurrentSearchMatch == this.vm.TotalSearchMatches)
			{
				this.vm.CurrentSearchMatch = 1;
			}
			else
			{
				StackerViewModel stackerViewModel = this.vm;
				int currentSearchMatch = stackerViewModel.CurrentSearchMatch;
				stackerViewModel.CurrentSearchMatch = currentSearchMatch + 1;
			}
			this.searchTextEditor.FindNext();
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00027925 File Offset: 0x00025B25
		private void labelTextbox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (this.vm.CurrentBlock != null)
			{
				this.vm.CurrentBlock.Block.Label = this.labelTextbox.Text;
			}
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00027954 File Offset: 0x00025B54
		public static TextPointer GetTextPointAt(TextPointer from, int pos)
		{
			TextPointer ret = from;
			int i = 0;
			while (i < pos && ret != null)
			{
				if (ret.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.Text || ret.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.None)
				{
					i++;
				}
				if (ret.GetPositionAtOffset(1, LogicalDirection.Forward) == null)
				{
					return ret;
				}
				ret = ret.GetPositionAtOffset(1, LogicalDirection.Forward);
			}
			return ret;
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0002799C File Offset: 0x00025B9C
		private void testDataTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.testDataTypeCombobox.SelectedItem == null)
			{
				this.testDataTypeCombobox.SelectedIndex = this.testDataTypeCombobox.Items.IndexOf(this.vm.TestDataType);
				return;
			}
			this.vm.TestDataType = (string)this.testDataTypeCombobox.SelectedItem;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x000279F8 File Offset: 0x00025BF8
		private void blockClicked(object sender, RoutedEventArgs e)
		{
			ToggleButton toggle = sender as ToggleButton;
			StackerBlockViewModel block = this.vm.GetBlockById((int)toggle.Tag);
			if (Keyboard.Modifiers == ModifierKeys.Control)
			{
				block.Selected = !block.Selected;
			}
			else
			{
				this.vm.DeselectAll();
				block.Selected = true;
			}
			try
			{
				this.blockInfoScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
			}
			catch
			{
			}
			if ((this.vm.CurrentBlock = this.vm.SelectedBlocks.LastOrDefault<StackerBlockViewModel>()) == null)
			{
				return;
			}
			if (this.vm.CurrentBlock.Page != null)
			{
				this.BlockInfo.Content = this.vm.CurrentBlock.Page;
			}
			Keyboard.ClearFocus();
			if (this.vm.CurrentBlock.Page.Title == "PageBlockKeycheck")
			{
				this.blockInfoScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
			}
			this.labelTextbox.Text = this.vm.CurrentBlock.Block.Label;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00027B10 File Offset: 0x00025D10
		public void SetScript()
		{
			if (this.vm.View == StackerView.Blocks)
			{
				this.vm.LS.FromBlocks(this.vm.GetList());
			}
			else
			{
				this.vm.LS.Script = this.loliScriptEditor.Text;
			}
			this.vm.Config.Config.Script = this.vm.LS.Script;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00027B88 File Offset: 0x00025D88
		private void loliScriptButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.LS.FromBlocks(this.vm.GetList());
			this.loliScriptEditor.Text = this.vm.LS.Script;
			this.vm.View = StackerView.LoliScript;
			this.stackerTabControl.SelectedIndex = 0;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00027BE4 File Offset: 0x00025DE4
		private void stackButton_Click(object sender, RoutedEventArgs e)
		{
			Stacker.<>c__DisplayClass63_0 CS$<>8__locals1 = new Stacker.<>c__DisplayClass63_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.blocks = null;
			Action action = delegate
			{
				Stacker.<>c__DisplayClass63_0.<<stackButton_Click>b__0>d <<stackButton_Click>b__0>d;
				<<stackButton_Click>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<stackButton_Click>b__0>d.<>4__this = CS$<>8__locals1;
				<<stackButton_Click>b__0>d.<>1__state = -1;
				<<stackButton_Click>b__0>d.<>t__builder.Start<Stacker.<>c__DisplayClass63_0.<<stackButton_Click>b__0>d>(ref <<stackButton_Click>b__0>d);
			};
			try
			{
				if (this.vm.View != StackerView.LoliScript)
				{
					this.AutoSaveConfig();
				}
				Task task = this.taskSwitchView;
				if (task != null)
				{
					task.Dispose();
				}
				if (sender is Button)
				{
					this.stackerTabControl.BlurApply(0.0, 5.0, TimeSpan.FromSeconds(0.1), false);
					this.stackerTabControl.IsEnabled = false;
					this.taskSwitchView = Task.Run(action).ContinueWith(delegate(Task _)
					{
						Dispatcher dispatcher = CS$<>8__locals1.<>4__this.Dispatcher;
						Action action2;
						if ((action2 = CS$<>8__locals1.<>9__8) == null)
						{
							action2 = (CS$<>8__locals1.<>9__8 = delegate
							{
								CS$<>8__locals1.<>4__this.stackerTabControl.BlurDisable(TimeSpan.FromSeconds(0.1));
								CS$<>8__locals1.<>4__this.stackerTabControl.IsEnabled = true;
							});
						}
						dispatcher.Invoke(action2);
					});
				}
				else
				{
					this.taskSwitchView = Task.Run(action);
					this.taskSwitchView.Wait();
					this.taskSwitchView.Dispose();
				}
			}
			catch
			{
				this.stackerTabControl.IsEnabled = true;
				this.stackerTabControl.BlurDisable(TimeSpan.FromSeconds(0.1));
			}
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00027CF4 File Offset: 0x00025EF4
		private void loliScriptEditor_LostFocus(object sender, RoutedEventArgs e)
		{
			this.vm.LS.Script = this.loliScriptEditor.Text;
			this.toolTip.IsOpen = false;
			this.AutoSaveConfig();
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00027D24 File Offset: 0x00025F24
		private void loliScriptEditor_KeyDown(object sender, KeyEventArgs e)
		{
			if (Keyboard.Modifiers == ModifierKeys.Control)
			{
				if (e.Key == global::System.Windows.Input.Key.S)
				{
					this.vm.LS.Script = this.loliScriptEditor.Text;
					this.OnSaveConfig();
				}
				else if (e.Key == global::System.Windows.Input.Key.F)
				{
					this.Button_Click(null, null);
				}
			}
			if (SB.SBSettings.General.AutoSaveConfigOnStacker && this.vm.LS.Script != this.loliScriptEditor.Text)
			{
				this.vm.LS.Script = this.loliScriptEditor.Text;
				this.OnSaveConfig();
			}
			if (SB.SBSettings.General.DisableSyntaxHelper)
			{
				return;
			}
			DocumentLine line = this.loliScriptEditor.Document.GetLineByOffset(this.loliScriptEditor.CaretOffset);
			string blockLine = this.loliScriptEditor.Document.GetText(line.Offset, line.Length);
			while (blockLine.StartsWith(" ") || blockLine.StartsWith("\t"))
			{
				try
				{
					line = line.PreviousLine;
					blockLine = this.loliScriptEditor.Document.GetText(line.Offset, line.Length);
				}
				catch
				{
					break;
				}
			}
			if (!BlockParser.IsBlock(blockLine))
			{
				this.toolTip.IsOpen = false;
				return;
			}
			string blockName = BlockParser.GetBlockType(blockLine);
			Rect caret = this.loliScriptEditor.TextArea.Caret.CalculateCaretRectangle();
			this.toolTip.HorizontalOffset = caret.Right;
			this.toolTip.VerticalOffset = caret.Bottom;
			XmlNode node = null;
			for (int i = 0; i < this.syntaxHelperItems.Count; i++)
			{
				if (this.syntaxHelperItems[i].Attributes["name"].Value.ToUpper() == blockName.ToUpper())
				{
					node = this.syntaxHelperItems[i];
					break;
				}
			}
			if (node == null)
			{
				return;
			}
			this.toolTipEditor.Text = node.InnerText;
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00027F40 File Offset: 0x00026140
		private void LoliScriptEditor_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (SB.SBSettings.General.AutoSaveConfigOnStacker && this.vm.LS.Script != this.loliScriptEditor.Text)
				{
					this.vm.LS.Script = this.loliScriptEditor.Text;
					this.OnSaveConfig();
				}
			}
			catch
			{
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00013474 File Offset: 0x00011674
		private void openDocButton_Click(object sender, RoutedEventArgs e)
		{
			new MainDialog(new DialogLSDoc(), "LoliScript Documentation").Show();
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00027FB8 File Offset: 0x000261B8
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				FindReplaceDialog.ShowForFind(this.loliScriptEditor);
			}
			catch
			{
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00003B20 File Offset: 0x00001D20
		private void loliScriptEditor_KeyUp(object sender, KeyEventArgs e)
		{
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x00027FE8 File Offset: 0x000261E8
		private void debuggerTabControl_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.SystemKey == global::System.Windows.Input.Key.F10)
				{
					this.nextStepButton_Click(null, e);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0002801C File Offset: 0x0002621C
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				if ((e.OriginalSource as TextBox).Text == string.Empty)
				{
					this.searchButton_Click(sender, e);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00028064 File Offset: 0x00026264
		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Key == global::System.Windows.Input.Key.Return)
				{
					this.searchButton_Click(sender, e);
				}
				else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
				{
					if (e.Key == global::System.Windows.Input.Key.Next)
					{
						this.nextMatchButton_MouseDown(sender, null);
					}
					else if (e.Key == global::System.Windows.Input.Key.Prior)
					{
						this.previousMatchButton_MouseDown(sender, null);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x000280D0 File Offset: 0x000262D0
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			this.ClearDebuggerLog(sender, e);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00003B20 File Offset: 0x00001D20
		private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
		{
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x000280DC File Offset: 0x000262DC
		private void SetBrowserStatus(string status)
		{
			base.Dispatcher.Invoke<string>(() => this.browserStatus.Text = status);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00003B20 File Offset: 0x00001D20
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00028118 File Offset: 0x00026318
		private void MenuItem_Click_1(object sender, RoutedEventArgs e)
		{
			try
			{
				NotepadExtensions.ShowText(this.logRTB.Text, "Log");
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.Stacker, ex.Message, true, 0);
			}
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00028164 File Offset: 0x00026364
		private void loliScriptEditor_TextChanged(object sender, EventArgs e)
		{
			this.AutoSaveConfig();
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0002816C File Offset: 0x0002636C
		private void Compile_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.stackerTabControl.BlurApply(0.0, 5.0, TimeSpan.FromSeconds(0.1), false);
				this.stackerTabControl.IsEnabled = false;
				try
				{
					Task task = this.startCompileTask;
					if (task != null)
					{
						task.Dispose();
					}
				}
				catch
				{
				}
				this.startCompileTask = Task.Run(new Action(this.StartCompile)).ContinueWith(delegate(Task _)
				{
					base.Dispatcher.Invoke(delegate
					{
						this.stackerTabControl.BlurDisable(TimeSpan.FromSeconds(0.1));
						this.stackerTabControl.IsEnabled = true;
					});
				});
			}
			catch (Exception ex)
			{
				CustomMsgBox.ShowError(ex.Message, false);
			}
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0002821C File Offset: 0x0002641C
		private void StartCompile()
		{
			try
			{
				ConfigViewModel currentConfig = SB.MainWindow.ConfigsPage.CurrentConfig;
				if (string.IsNullOrWhiteSpace(currentConfig.Config.Script))
				{
					base.Dispatcher.Invoke(delegate
					{
						CustomMsgBox.ShowError("Script is empty!!", false);
					});
				}
				else if (currentConfig.Config.BlocksAmount == 0)
				{
					base.Dispatcher.Invoke(delegate
					{
						CustomMsgBox.ShowError("Blocks amount is zero!!", false);
					});
				}
				else
				{
					ConfigSettings settings = currentConfig.Config.Settings;
					if ((from i in settings.HitInfoFormat.Split(new char[] { '{' })
						where i.Contains(".")
						select i).Any((string i) => !i.StartsWith("hit.")))
					{
						base.Dispatcher.Invoke(delegate
						{
							CustomMsgBox.ShowError("Hit information format is invalid", false);
						});
					}
					else
					{
						string configName = Path.GetFileNameWithoutExtension(currentConfig.FileName);
						string dirCompile = "Compiled";
						if (!Directory.Exists(dirCompile))
						{
							Directory.CreateDirectory(dirCompile);
						}
						if (!Directory.Exists(dirCompile + "\\bin"))
						{
							Directory.CreateDirectory(dirCompile + "\\bin");
						}
						using (ScriptCompiler compiler = new ScriptCompiler(ScriptCompiler.CodeProviderLanguage.CS)
						{
							Output = dirCompile + "\\bin\\" + configName + ".exe",
							Title = settings.Title,
							IconPath = settings.IconPath,
							Message = settings.Message,
							MessageColor = settings.MessageColor.ConvertToString(),
							AuthorColor = settings.AuthorColor.ConvertToString(),
							BotsColor = settings.BotsColor.ConvertToString(),
							CPMColor = settings.CPMColor.ConvertToString(),
							CustomColor = settings.CustomColor.ConvertToString(),
							CustomInputColor = settings.CustomInputColor.ConvertToString(),
							FailsColor = settings.FailsColor.ConvertToString(),
							HitsColor = settings.HitsColor.ConvertToString(),
							OcrRateColor = settings.OcrRateColor.ConvertToString(),
							ProgressColor = settings.ProgressColor.ConvertToString(),
							ProxiesColor = settings.ProxiesColor.ConvertToString(),
							RetriesColor = settings.RetriesColor.ConvertToString(),
							ToCheckColor = settings.ToCheckColor.ConvertToString(),
							WordlistColor = settings.WordlistColor.ConvertToString(),
							SvbConfig = IOManager.SerializeConfig(currentConfig.Config),
							Config = currentConfig.Config,
							HitInformationFormat = settings.HitInfoFormat,
							LicenseSource = (string.IsNullOrWhiteSpace(settings.LicenseSource) ? string.Empty : File.ReadAllText(settings.LicenseSource))
						})
						{
							compiler.AddOption("/optimize");
							compiler.AddReferences(new string[] { "System.dll", "System.Drawing.dll", "System.Core.dll", "mscorlib.dll", "System.Linq.dll", "System.Collections.dll" });
							compiler.AddReferences((from d in Directory.GetFiles("bin", "*.dll")
								where !d.Contains("MahApps.Metro") && !d.Contains("Xceed.") && !d.Contains("MaterialDesign") && !d.Contains("WPFToolkit") && !d.Contains("ControlzEx.dll") && !d.Contains("ICSharpCode.AvalonEdit") && !d.Contains("CefSharp.Wpf")
								select d).ToArray<string>());
							string[] requiredPlugins2 = settings.RequiredPlugins;
							if (requiredPlugins2 != null && requiredPlugins2.Length != 0 && compiler.Supports(GeneratorSupport.Resources))
							{
								compiler.InjectPluginLoader();
								string[] plugins = Directory.GetFiles(SB.pluginsFolder, "*.dll");
								if (plugins.Length == 0)
								{
									base.Dispatcher.Invoke(delegate
									{
										CustomMsgBox.ShowError("Required plugins not found!", false);
									});
									return;
								}
								List<string> reqPlugins = new List<string>();
								settings.RequiredPlugins.Distinct<string>().ToList<string>().ForEach(delegate(string requiredPlugins)
								{
									string pluginName = SB.PluginNames.ToList<string>()[SB.BlockPlugins.IndexOf(SB.BlockPlugins.First((IBlockPlugin p) => p.Name == requiredPlugins))];
									reqPlugins.Add(pluginName);
									foreach (string plugin in plugins)
									{
										if (Path.GetFileNameWithoutExtension(plugin) == pluginName)
										{
											reqPlugins.Remove(pluginName);
											compiler.AddEmbeddedResource(plugin);
										}
									}
								});
								if (reqPlugins.Count > 0)
								{
									base.Dispatcher.Invoke(delegate
									{
										CustomMsgBox.ShowError("\"" + string.Join(", ", reqPlugins) + "\" Plugin(s) not found!", false);
									});
									return;
								}
							}
							ValueTuple<string, bool> result = compiler.GetResult(compiler.Execute());
							if (result.Item2)
							{
								base.Dispatcher.Invoke(delegate
								{
									CustomMsgBox.ShowError(result.Item1, false);
								});
							}
							else
							{
								compiler.CopyReferencesAndDependencies();
								compiler.CopySettings();
								compiler.CreateRunner(configName, currentConfig.Config);
								base.Dispatcher.Invoke(delegate
								{
									CustomMsgBox.Show(result.Item1, false);
								});
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Exception ex3;
				Exception ex2 = ex3;
				Exception ex = ex2;
				base.Dispatcher.Invoke(delegate
				{
					CustomMsgBox.ShowError(ex.Message, false);
				});
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x000287B4 File Offset: 0x000269B4
		public void AutoSaveConfig()
		{
			Task.Run(delegate
			{
				base.Dispatcher.Invoke(delegate
				{
					if (SB.MainWindow.ConfigsPage.ConfigManagerPage.CheckSaved())
					{
						return;
					}
					if (SB.SBSettings.General.AutoSaveConfigOnStacker)
					{
						this.OnSaveConfig();
					}
				});
			});
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00028E83 File Offset: 0x00027083
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 21)
			{
				((ToggleButton)target).Click += this.blockClicked;
			}
		}

		// Token: 0x0400059B RID: 1435
		private Stopwatch timer;

		// Token: 0x0400059C RID: 1436
		private StackerViewModel vm;

		// Token: 0x0400059D RID: 1437
		private AbortableBackgroundWorker debugger = new AbortableBackgroundWorker();

		// Token: 0x0400059E RID: 1438
		private XmlNodeList syntaxHelperItems;

		// Token: 0x0400059F RID: 1439
		private XmlNodeList scriptCompletion;

		// Token: 0x040005A0 RID: 1440
		private TextEditor toolTipEditor;

		// Token: 0x040005A1 RID: 1441
		private CompletionWindow completionWindow;

		// Token: 0x040005A2 RID: 1442
		private ToolTip toolTip;

		// Token: 0x040005A3 RID: 1443
		private Task taskSwitchView;

		// Token: 0x040005A4 RID: 1444
		private Task startCompileTask;

		// Token: 0x040005A5 RID: 1445
		private LoliScriptCompletionData.BlockParameters blockParameters;

		// Token: 0x040005A7 RID: 1447
		private BrushConverter bc = new BrushConverter();

		// Token: 0x040005A8 RID: 1448
		private SearchTextEditor searchTextEditor;

		// Token: 0x040005A9 RID: 1449
		private OcrEngine _ocrEngine;

		// Token: 0x040005AA RID: 1450
		private int? tempSrc;

		// Token: 0x040005AB RID: 1451
		private Task taskResto;

		// Token: 0x020000FF RID: 255
		// (Invoke) Token: 0x06000710 RID: 1808
		public delegate void SaveConfigEventHandler(object sender, EventArgs e);
	}
}
