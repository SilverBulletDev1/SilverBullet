using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using RuriLib;

namespace OpenBullet.Views.StackerBlocks
{
	// Token: 0x020000A1 RID: 161
	public partial class PageSBlockBrowserAction : Page
	{
		// Token: 0x06000434 RID: 1076 RVA: 0x00014E58 File Offset: 0x00013058
		public PageSBlockBrowserAction(SBlockBrowserAction block)
		{
			this.InitializeComponent();
			this.vm = block;
			base.DataContext = this.vm;
			foreach (string a in Enum.GetNames(typeof(BrowserAction)))
			{
				this.actionCombobox.Items.Add(a);
			}
			this.actionCombobox.SelectedIndex = (int)this.vm.Action;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00014F8C File Offset: 0x0001318C
		private void actionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.Action = (BrowserAction)((ComboBox)e.OriginalSource).SelectedIndex;
			if (this.vm.Action == BrowserAction.Open)
			{
				this.panelSUserAgent.Visibility = Visibility.Visible;
			}
			else
			{
				this.panelSUserAgent.Visibility = Visibility.Collapsed;
			}
			try
			{
				this.functionInfoTextblock.Text = this.infoDic[this.vm.Action.ToString()];
			}
			catch
			{
				this.functionInfoTextblock.Text = "No additional information available for this function";
			}
		}

		// Token: 0x04000373 RID: 883
		private SBlockBrowserAction vm;

		// Token: 0x04000374 RID: 884
		public Dictionary<string, string> infoDic = new Dictionary<string, string>
		{
			{ "Open", "Opens the browser assigned to the current bot. This will be disregarded if the browser is already opened." },
			{ "Close", "Closes the current tab without disposing of the webdriver (not recommended)." },
			{ "Quit", "Quits all the tabs and windows and disposes of the webdriver (recommended)." },
			{ "SendKeys", "Sends the keys directly to the browser as if you were typing them on your keyboard. You can use variables and <TAB> <ENTER> and <BACKSPACE> separated by || e.g. <USER>||<TAB>||<PASS>||<TAB>||<ENTER>. If you need other keys look here https://github.com/SeleniumHQ/selenium/blob/master/dotnet/src/webdriver/Keys.cs" },
			{ "MoveMouse", "Not Implemented Yet" },
			{ "Screenshot", "Takes a screenshot of the visible part of the page." },
			{ "Scroll", "Scrolls down by the specified amount in the input field." },
			{ "SwitchToTab", "Switches to the tab with the index in the input field." },
			{ "DOMtoSOURCE", "Puts the full DOM into the <SOURCE> fixed variable." },
			{ "GetCookies", "Sends all cookies from selenium to the cookie jar used in normal requests." },
			{ "SetCookies", "Sets all cookies from the cookie jar to selenium. You need to put the domain of the website in the input field (e.g. example.com)." }
		};
	}
}
