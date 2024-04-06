using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using MaterialDesignThemes.Wpf;
using OpenBullet.ViewModels;

namespace OpenBullet.Views.Main
{
	// Token: 0x020000B3 RID: 179
	public partial class SilverZone : Page
	{
		// Token: 0x0600047A RID: 1146 RVA: 0x000163D0 File Offset: 0x000145D0
		public SilverZone(SilverZoneViewModel viewModel = null)
		{
			this.InitializeComponent();
			base.DataContext = (this.vm = viewModel ?? new SilverZoneViewModel());
			this.menuOptionSupporters_MouseDown(this, null);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00016420 File Offset: 0x00014620
		private void menuOptionSupporters_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.supportersPage;
			this.menuOptionSelected(this.menuOptionSupporters);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0001643F File Offset: 0x0001463F
		private void menuOptionVerifiedMarket_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.Main.Content = this.verifiedMarketPage;
			this.menuOptionSelected(this.menuOptionVerifiedMarket);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00016460 File Offset: 0x00014660
		private void menuOptionSelected(object sender)
		{
			foreach (object child in this.topMenu.Children)
			{
				try
				{
					Badged badged = child as Badged;
					Label option;
					if (badged != null)
					{
						option = badged.Content as Label;
					}
					else
					{
						option = (Label)child;
					}
					option.Foreground = Utils.GetBrush("ForegroundMain");
				}
				catch
				{
				}
			}
			((Label)sender).Foreground = Utils.GetBrush("ForegroundGood");
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0001650C File Offset: 0x0001470C
		public int GetBadge()
		{
			int supCount;
			int veriMarketCount;
			using (WebClient wc = new WebClient())
			{
				wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:87.0) Gecko/20100101 Firefox/87.0");
				supCount = Regex.Matches(wc.DownloadString("https://raw.githubusercontent.com/mohamm4dx/SilverBullet/master/OpenBullet/Supporters.json"), "\"Name\":\"").Count;
				veriMarketCount = Regex.Matches(wc.DownloadString("https://raw.githubusercontent.com/mohamm4dx/SilverBullet/master/OpenBullet/VerifiedMarket.json"), "\"Content\":\"").Count;
			}
			if (this.vm != null)
			{
				this.vm.SupportersBadge = ((supCount > 99) ? "99+" : supCount.ToString());
				this.vm.VerifiedMarketBadge = ((veriMarketCount > 99) ? "99+" : veriMarketCount.ToString());
			}
			return supCount + veriMarketCount;
		}

		// Token: 0x040003AA RID: 938
		private Supporters supportersPage = new Supporters();

		// Token: 0x040003AB RID: 939
		private VerifiedMarket verifiedMarketPage = new VerifiedMarket();

		// Token: 0x040003AC RID: 940
		public SilverZoneViewModel vm;
	}
}
