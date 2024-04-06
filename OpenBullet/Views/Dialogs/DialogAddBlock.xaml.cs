using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using OpenBullet.Views.Main.Configs;
using PluginFramework;
using RuriLib;

namespace OpenBullet.Views.Dialogs
{
	// Token: 0x02000118 RID: 280
	public partial class DialogAddBlock : Page
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x0002B6AC File Offset: 0x000298AC
		// (set) Token: 0x06000785 RID: 1925 RVA: 0x0002B6B4 File Offset: 0x000298B4
		public object Caller { get; set; }

		// Token: 0x06000786 RID: 1926 RVA: 0x0002B6C0 File Offset: 0x000298C0
		public DialogAddBlock(object caller)
		{
			this.InitializeComponent();
			this.Caller = caller;
			for (int i = 0; i < SB.BlockPlugins.Count; i += 3)
			{
				this.pluginsGrid.RowDefinitions.Add(new RowDefinition
				{
					Height = GridLength.Auto
				});
			}
			for (int j = 0; j < SB.BlockPlugins.Count; j++)
			{
				IBlockPlugin plugin = SB.BlockPlugins[j];
				Button button = new Button();
				button.Background = plugin.LinearGradientBrush;
				if (plugin.LightForeground)
				{
					button.Foreground = new SolidColorBrush(Colors.Gainsboro);
				}
				button.Content = plugin.Name;
				button.SetValue(Grid.ColumnProperty, j % 3);
				button.SetValue(Grid.RowProperty, j / 3);
				button.Tag = plugin.GetType();
				button.Click += this.pluginButton_Click;
				this.pluginsGrid.Children.Add(button);
			}
			this.defaultSetLabel_MouseDown(this, null);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0002B7D0 File Offset: 0x000299D0
		private void pluginButton_Click(object sender, RoutedEventArgs e)
		{
			Type type = (e.OriginalSource as Button).Tag as Type;
			this.SendBack(Activator.CreateInstance(type) as BlockBase);
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0002B804 File Offset: 0x00029A04
		private void blockRequestButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendBack(new BlockRequest());
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0002B811 File Offset: 0x00029A11
		private void blockUtilityButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendBack(new BlockUtility());
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0002B81E File Offset: 0x00029A1E
		private void blockKeycheckButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendBack(new BlockKeycheck());
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0002B82B File Offset: 0x00029A2B
		private void blockParseButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendBack(new BlockParse());
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0002B838 File Offset: 0x00029A38
		private void blockFunctionButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendBack(new BlockFunction());
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0002B845 File Offset: 0x00029A45
		private void blockSolveCaptchaButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendBack(new BlockSolveCaptcha());
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0002B852 File Offset: 0x00029A52
		private void blockReportCaptchaButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendBack(new BlockReportCaptcha());
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0002B85F File Offset: 0x00029A5F
		private void blockBypassCFButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendBack(new BlockBypassCF());
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0002B86C File Offset: 0x00029A6C
		private void blockTCPButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendBack(new BlockTCP());
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0002B879 File Offset: 0x00029A79
		private void blockOcrButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendBack(new BlockOcr());
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0002B886 File Offset: 0x00029A86
		private void blockSpeechToTextButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendBack(new BlockSpeechToText());
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0002B893 File Offset: 0x00029A93
		private void blockWS_Click(object sender, RoutedEventArgs e)
		{
			this.SendBack(new BlockWebSocket());
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0002B8A0 File Offset: 0x00029AA0
		private void blockNavigateButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendBack(new SBlockNavigate());
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0002B8AD File Offset: 0x00029AAD
		private void blockBrowserActionButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendBack(new SBlockBrowserAction());
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0002B8BA File Offset: 0x00029ABA
		private void blockElementActionButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendBack(new SBlockElementAction());
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0002B8C7 File Offset: 0x00029AC7
		private void blockExecuteJSButton_Click(object sender, RoutedEventArgs e)
		{
			this.SendBack(new SBlockExecuteJS());
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0002B8D4 File Offset: 0x00029AD4
		private void SendBack(BlockBase block)
		{
			if (this.Caller.GetType() == typeof(Stacker))
			{
				((Stacker)this.Caller).AddBlock(block);
			}
			((MainDialog)base.Parent).Close();
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0002B913 File Offset: 0x00029B13
		private void defaultSetLabel_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.defaultSetLabel.Foreground = Utils.GetBrush("ForegroundMenuSelected");
			this.pluginsSetLabel.Foreground = Utils.GetBrush("ForegroundMain");
			this.blockSetTabControl.SelectedIndex = 0;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0002B94B File Offset: 0x00029B4B
		private void pluginsSetLabel_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.defaultSetLabel.Foreground = Utils.GetBrush("ForegroundMain");
			this.pluginsSetLabel.Foreground = Utils.GetBrush("ForegroundMenuSelected");
			this.blockSetTabControl.SelectedIndex = 1;
		}
	}
}
