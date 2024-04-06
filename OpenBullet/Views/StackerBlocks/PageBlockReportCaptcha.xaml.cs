using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using CaptchaSharp.Enums;
using RuriLib;

namespace OpenBullet.Views.StackerBlocks
{
	// Token: 0x0200008B RID: 139
	public partial class PageBlockReportCaptcha : Page
	{
		// Token: 0x0600039F RID: 927 RVA: 0x000110AC File Offset: 0x0000F2AC
		public PageBlockReportCaptcha(BlockReportCaptcha block)
		{
			this.InitializeComponent();
			this.vm = block;
			base.DataContext = this.vm;
			foreach (string t in Enum.GetNames(typeof(CaptchaType)))
			{
				this.captchaTypeCombobox.Items.Add(t);
			}
			this.captchaTypeCombobox.SelectedIndex = (int)this.vm.Type;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00011122 File Offset: 0x0000F322
		private void captchaTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.Type = (CaptchaType)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x040002C0 RID: 704
		private BlockReportCaptcha vm;
	}
}
