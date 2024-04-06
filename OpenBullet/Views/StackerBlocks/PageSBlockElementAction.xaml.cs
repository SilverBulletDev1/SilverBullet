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
	// Token: 0x020000A2 RID: 162
	public partial class PageSBlockElementAction : Page
	{
		// Token: 0x06000438 RID: 1080 RVA: 0x000150C8 File Offset: 0x000132C8
		public PageSBlockElementAction(SBlockElementAction block)
		{
			this.InitializeComponent();
			this.vm = block;
			base.DataContext = this.vm;
			foreach (string i in Enum.GetNames(typeof(ElementLocator)))
			{
				this.locatorCombobox.Items.Add(i);
			}
			this.locatorCombobox.SelectedIndex = (int)this.vm.Locator;
			foreach (string a in Enum.GetNames(typeof(ElementAction)))
			{
				this.actionCombobox.Items.Add(a);
			}
			this.actionCombobox.SelectedIndex = (int)this.vm.Action;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00015203 File Offset: 0x00013403
		private void locatorCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.Locator = (ElementLocator)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00015220 File Offset: 0x00013420
		private void actionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.Action = (ElementAction)((ComboBox)e.OriginalSource).SelectedIndex;
			try
			{
				this.functionInfoTextblock.Text = this.infoDic[this.vm.Action.ToString()];
			}
			catch
			{
				this.functionInfoTextblock.Text = "No additional information available for this function";
			}
		}

		// Token: 0x04000379 RID: 889
		private SBlockElementAction vm;

		// Token: 0x0400037A RID: 890
		public Dictionary<string, string> infoDic = new Dictionary<string, string>
		{
			{ "Clear", "Clears the text in the targeted input element." },
			{ "SendKeys", "Sends the text to the targeted input element." },
			{ "Submit", "Submits the targeted form." },
			{ "GetText", "Gets the innerHTML of the targeted element." },
			{ "GetAttribute", "From the targeted element, gets the attribute with the name specified in the input field." },
			{ "Screenshot", "Takes a screenshot of the targeted element, you can use this to screenshot a gift captcha and send it to be solved later with a Captcha Block." },
			{ "WaitForElement", "Waits until the element specified exists or not exists. You can set the timeout in seconds through the input field [Default Timeout = 10s]." }
		};
	}
}
