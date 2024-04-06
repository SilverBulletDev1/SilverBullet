using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.UserControls
{
	// Token: 0x0200005C RID: 92
	public partial class ButtonContainer : UserControl
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000AA97 File Offset: 0x00008C97
		// (set) Token: 0x06000250 RID: 592 RVA: 0x0000AA9F File Offset: 0x00008C9F
		public string Text { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000AAA8 File Offset: 0x00008CA8
		// (set) Token: 0x06000252 RID: 594 RVA: 0x0000AAB0 File Offset: 0x00008CB0
		private string MethodName { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000AAB9 File Offset: 0x00008CB9
		// (set) Token: 0x06000254 RID: 596 RVA: 0x0000AAC1 File Offset: 0x00008CC1
		private PluginControl PluginControl { get; set; }

		// Token: 0x06000255 RID: 597 RVA: 0x0000AACA File Offset: 0x00008CCA
		public ButtonContainer(string text, string methodName, PluginControl pluginControl)
		{
			this.InitializeComponent();
			base.DataContext = this;
			this.Text = text;
			this.MethodName = methodName;
			this.PluginControl = pluginControl;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000AAF4 File Offset: 0x00008CF4
		private void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			this.PluginControl.RunMethod(this.MethodName);
		}
	}
}
