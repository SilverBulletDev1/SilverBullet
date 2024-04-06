using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using RuriLib;

namespace OpenBullet.Views.StackerBlocks
{
	// Token: 0x0200009A RID: 154
	public partial class PageBlockRecaptcha : Page
	{
		// Token: 0x06000408 RID: 1032 RVA: 0x0001380E File Offset: 0x00011A0E
		public PageBlockRecaptcha(BlockRecaptcha block)
		{
			this.InitializeComponent();
			this.vm = block;
			base.DataContext = this.vm;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00013830 File Offset: 0x00011A30
		private void autoSiteKey_Click(object sender, RoutedEventArgs e)
		{
			if (this.vm.Url == string.Empty)
			{
				MessageBox.Show("You cannot use auto without setting a page where the reCaptcha is shown first!");
				return;
			}
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(this.vm.Url);
				httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
				using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (Stream stream = response.GetResponseStream())
					{
						using (StreamReader reader = new StreamReader(stream))
						{
							string html = reader.ReadToEnd();
							Regex r = new Regex("[^A-Za-z\\d][A-Za-z\\d\\-]{40}[^A-Za-z\\d]");
							this.vm.SiteKey = r.Match(html).Value.Replace("\"", "");
						}
					}
				}
			}
			catch
			{
				MessageBox.Show("Auto failed. Do it manually");
			}
		}

		// Token: 0x04000329 RID: 809
		private BlockRecaptcha vm;
	}
}
