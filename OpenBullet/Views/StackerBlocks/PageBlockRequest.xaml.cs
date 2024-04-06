using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Extreme.Net;
using RuriLib;
using RuriLib.Functions.Requests;

namespace OpenBullet.Views.StackerBlocks
{
	// Token: 0x0200009B RID: 155
	public partial class PageBlockRequest : Page
	{
		// Token: 0x0600040C RID: 1036 RVA: 0x0001399C File Offset: 0x00011B9C
		public PageBlockRequest(BlockRequest block)
		{
			this.InitializeComponent();
			this.vm = block;
			base.DataContext = this.vm;
			foreach (string i in Enum.GetNames(typeof(HttpMethod)))
			{
				this.methodCombobox.Items.Add(i);
			}
			this.methodCombobox.SelectedIndex = (int)this.vm.Method;
			foreach (string t in Enum.GetNames(typeof(RequestType)))
			{
				this.requestTypeCombobox.Items.Add(t);
			}
			this.requestTypeCombobox.SelectedIndex = (int)this.vm.RequestType;
			foreach (string t2 in Enum.GetNames(typeof(ResponseType)))
			{
				this.responseTypeCombobox.Items.Add(t2);
			}
			this.responseTypeCombobox.SelectedIndex = (int)this.vm.ResponseType;
			this.customCookiesRTB.AppendText(this.vm.GetCustomCookies());
			this.customHeadersRTB.AppendText(this.vm.GetCustomHeaders());
			this.multipartContentsRTB.AppendText(this.vm.GetMultipartContents());
			this.CheckBox_Click(null, null);
			foreach (string c in new List<string> { "application/x-www-form-urlencoded", "application/json", "text/plain" })
			{
				this.contentTypeCombobox.Items.Add(c);
			}
			foreach (string s in Enum.GetNames(typeof(SecurityProtocol)))
			{
				this.securityProtocolCombobox.Items.Add(s);
			}
			foreach (string p in this.vm.ProtocolVersions)
			{
				this.protocolVersionComboBox.Items.Add(p);
			}
			this.protocolVersionComboBox.Text = this.vm.ProtocolVersion.ToString();
			this.securityProtocolCombobox.SelectedIndex = (int)this.vm.SecurityProtocol;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00013C00 File Offset: 0x00011E00
		private void methodCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.Method = (HttpMethod)this.methodCombobox.SelectedIndex;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00013C18 File Offset: 0x00011E18
		private void requestTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.RequestType = (RequestType)this.requestTypeCombobox.SelectedIndex;
			switch (this.vm.RequestType)
			{
			case RequestType.Standard:
				this.requestTypeTabControl.SelectedIndex = 2;
				return;
			default:
				this.requestTypeTabControl.SelectedIndex = 1;
				return;
			case RequestType.Multipart:
				this.requestTypeTabControl.SelectedIndex = 3;
				return;
			case RequestType.Raw:
				this.requestTypeTabControl.SelectedIndex = 4;
				return;
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00013C90 File Offset: 0x00011E90
		private void responseTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.ResponseType = (ResponseType)this.responseTypeCombobox.SelectedIndex;
			ResponseType responseType = this.vm.ResponseType;
			if (responseType == ResponseType.File)
			{
				this.responseTypeTabControl.SelectedIndex = 1;
				return;
			}
			if (responseType != ResponseType.Base64String)
			{
				this.responseTypeTabControl.SelectedIndex = 0;
				return;
			}
			this.responseTypeTabControl.SelectedIndex = 2;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00013CED File Offset: 0x00011EED
		private void customCookiesRTB_LostFocus(object sender, RoutedEventArgs e)
		{
			this.vm.SetCustomCookies(this.customCookiesRTB.Lines());
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00013D05 File Offset: 0x00011F05
		private void customHeadersRTB_LostFocus(object sender, RoutedEventArgs e)
		{
			this.vm.SetCustomHeaders(this.customHeadersRTB.Lines());
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00013D1D File Offset: 0x00011F1D
		private void multipartContentsRTB_LostFocus(object sender, RoutedEventArgs e)
		{
			this.vm.SetMultipartContents(this.multipartContentsRTB.Lines());
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00013D35 File Offset: 0x00011F35
		private void securityProtocolCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.SecurityProtocol = (SecurityProtocol)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00013D54 File Offset: 0x00011F54
		private void AnalyzeLoginPage_Click(object sender, RoutedEventArgs e)
		{
			Transform analyzeRenderTransform = this.analyzeIcon.RenderTransform;
			Storyboard waitForAnalyze = (Storyboard)base.FindResource("WaitForAnalyze");
			try
			{
				waitForAnalyze.Begin();
				Tuple<string, string, string> tuple = null;
				try
				{
					Task task = this.analyzeTask;
					if (task != null)
					{
						task.Dispose();
					}
				}
				catch
				{
				}
				Action <>9__2;
				Action <>9__3;
				Func<Transform> <>9__4;
				Exception ex;
				this.analyzeTask = Task.Run<Tuple<string, string, string>>(() => tuple = this.vm.Analyze()).ContinueWith(delegate(Task<Tuple<string, string, string>> _)
				{
					try
					{
						if (string.IsNullOrWhiteSpace(tuple.Item1) || string.IsNullOrWhiteSpace(tuple.Item2))
						{
							Dispatcher dispatcher = this.Dispatcher;
							Action action;
							if ((action = <>9__2) == null)
							{
								action = (<>9__2 = delegate
								{
									waitForAnalyze.Stop();
									this.analyzeIcon.RenderTransform = analyzeRenderTransform;
								});
							}
							dispatcher.Invoke(action);
							SB.Logger.Log("URL or POSTDATA not found!", LogLevel.Error, true, 0);
						}
						else
						{
							this.vm.Url = tuple.Item1;
							this.vm.PostData = tuple.Item2;
							Dispatcher dispatcher2 = this.Dispatcher;
							Action action2;
							if ((action2 = <>9__3) == null)
							{
								action2 = (<>9__3 = delegate
								{
									waitForAnalyze.Stop();
									this.analyzeIcon.RenderTransform = analyzeRenderTransform;
								});
							}
							dispatcher2.Invoke(action2);
						}
					}
					catch (Exception ex2)
					{
						Exception ex3 = ex2;
						Exception ex = ex3;
						waitForAnalyze.Stop();
						Dispatcher dispatcher3 = this.Dispatcher;
						Func<Transform> func;
						if ((func = <>9__4) == null)
						{
							func = (<>9__4 = () => this.analyzeIcon.RenderTransform = analyzeRenderTransform);
						}
						dispatcher3.Invoke<Transform>(func);
						this.Dispatcher.Invoke(delegate
						{
							SB.Logger.Log(ex.Message, LogLevel.Error, true, 0);
						});
					}
				});
			}
			catch (Exception ex)
			{
				waitForAnalyze.Stop();
				this.analyzeIcon.RenderTransform = analyzeRenderTransform;
				Exception ex;
				SB.Logger.Log(ex.Message, LogLevel.Error, true, 0);
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00013E48 File Offset: 0x00012048
		private void ProtocolVersionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			string ver = this.protocolVersionComboBox.SelectedItem.ToString();
			int v = 1;
			int v2 = 0;
			try
			{
				v = int.Parse(ver.Split(new char[] { '.' })[0]);
			}
			catch
			{
			}
			try
			{
				v2 = int.Parse(ver.Split(new char[] { '.' })[1]);
			}
			catch
			{
			}
			this.vm.ProtocolVersion = new Version(v, v2);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00013ED8 File Offset: 0x000120D8
		private void ProtocolVersionComboBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			string ver = this.protocolVersionComboBox.Text;
			int v = 1;
			int v2 = 0;
			try
			{
				v = int.Parse(ver.Split(new char[] { '.' })[0]);
			}
			catch
			{
			}
			try
			{
				v2 = int.Parse(ver.Split(new char[] { '.' })[1]);
			}
			catch
			{
			}
			this.vm.ProtocolVersion = new Version(v, v2);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00013F60 File Offset: 0x00012160
		private void CheckBox_Click(object sender, RoutedEventArgs e)
		{
			this.expander.IsHitTestVisible = this.vm.UseAkamai;
			if (!this.vm.UseAkamai)
			{
				this.expander.IsExpanded = false;
			}
		}

		// Token: 0x0400032C RID: 812
		private BlockRequest vm;

		// Token: 0x0400032D RID: 813
		private Task analyzeTask;
	}
}
