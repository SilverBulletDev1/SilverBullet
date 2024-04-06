using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using OpenBullet.Views.UserControls;
using RuriLib;
using RuriLib.Models;

namespace OpenBullet.Views.Main
{
	// Token: 0x020000B9 RID: 185
	public partial class VerifiedMarket : Page
	{
		// Token: 0x06000492 RID: 1170 RVA: 0x00016D26 File Offset: 0x00014F26
		public VerifiedMarket()
		{
			this.InitializeComponent();
			this.itemsControl.ItemsSource = this.marketCollection;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00016D50 File Offset: 0x00014F50
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.marketCollection.Count <= 0)
				{
					this.waitingLabel.Visibility = Visibility.Visible;
				}
				else
				{
					this.waitingLabel.Visibility = Visibility.Collapsed;
				}
				string data = string.Empty;
				using (Task.Run(delegate
				{
					using (WebClient wc = new WebClient())
					{
						wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:87.0) Gecko/20100101 Firefox/87.0");
						data = wc.DownloadString("https://raw.githubusercontent.com/mohamm4dx/SilverBullet/master/OpenBullet/VerifiedMarket.json");
					}
				}).ContinueWith(delegate(Task _)
				{
					if (string.IsNullOrWhiteSpace(data))
					{
						this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate
						{
							this.waitingLabel.Visibility = Visibility.Visible;
							this.searchBoxDockPanel.Visibility = Visibility.Collapsed;
							this.waitingLabel.Content = "ERROR";
						}));
					}
					else
					{
						this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate
						{
							this.waitingLabel.Visibility = Visibility.Collapsed;
							this.searchBoxDockPanel.Visibility = Visibility.Visible;
						}));
					}
					this.markets = IOManager.DeserializeObject<Market[]>(data);
					this.Dispatcher.Invoke(delegate
					{
						SB.MainWindow.SilverZonePage.vm.VerifiedMarketBadge = ((this.markets.Length > 99) ? "99+" : this.markets.Length.ToString());
						int badge = this.markets.Length + int.Parse(SB.MainWindow.SilverZonePage.vm.SupportersBadge.Replace("+", ""));
						SB.MainWindow.silverZoneBadged.Badge = ((badge > 99) ? "99+" : badge.ToString());
					});
					try
					{
						this.SetMarkets();
					}
					catch
					{
					}
				}))
				{
				}
			}
			catch (InvalidOperationException)
			{
			}
			catch (NullReferenceException)
			{
			}
			catch (Exception)
			{
				this.waitingLabel.Visibility = Visibility.Visible;
				this.searchBoxDockPanel.Visibility = Visibility.Collapsed;
				this.waitingLabel.Content = "ERROR";
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00016E34 File Offset: 0x00015034
		private async void SetMarkets()
		{
			VerifiedMarket.<>c__DisplayClass4_0 CS$<>8__locals1 = new VerifiedMarket.<>c__DisplayClass4_0();
			CS$<>8__locals1.<>4__this = this;
			if (this.markets != null && this.markets.Length != 0)
			{
				CS$<>8__locals1.i = 0;
				while (CS$<>8__locals1.i < this.markets.Length)
				{
					try
					{
						Dispatcher dispatcher = base.Dispatcher;
						DispatcherPriority dispatcherPriority = DispatcherPriority.Normal;
						ThreadStart threadStart;
						if ((threadStart = CS$<>8__locals1.<>9__0) == null)
						{
							VerifiedMarket.<>c__DisplayClass4_0 CS$<>8__locals2 = CS$<>8__locals1;
							ThreadStart threadStart2 = delegate
							{
								UserControlMarket uc = new UserControlMarket
								{
									Date = CS$<>8__locals1.<>4__this.markets[CS$<>8__locals1.i].Date,
									Category = CS$<>8__locals1.<>4__this.markets[CS$<>8__locals1.i].Category,
									Seller = CS$<>8__locals1.<>4__this.markets[CS$<>8__locals1.i].Seller,
									Margin = new Thickness(0.0, 0.0, 0.0, 10.0),
									ContentMarket = CS$<>8__locals1.<>4__this.markets[CS$<>8__locals1.i].Content
								};
								uc.SetContent(uc.ContentMarket);
								uc.SetIcon(new Uri(CS$<>8__locals1.<>4__this.markets[CS$<>8__locals1.i].Icon));
								if (!CS$<>8__locals1.<>4__this.marketCollection.Any((UserControlMarket u) => u.Seller == uc.Seller && u.Date == uc.Date && u.ContentMarket == uc.ContentMarket))
								{
									CS$<>8__locals1.<>4__this.marketCollection.Add(uc);
								}
							};
							CS$<>8__locals2.<>9__0 = threadStart2;
							threadStart = threadStart2;
						}
						await dispatcher.BeginInvoke(dispatcherPriority, threadStart);
					}
					catch
					{
					}
					CS$<>8__locals1.i++;
				}
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00016E6B File Offset: 0x0001506B
		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == global::System.Windows.Input.Key.Return)
			{
				this.itemsControl.ItemsSource = this.marketCollection.Where((UserControlMarket m) => m.ContentMarket.ToLower().Contains(this.serachTextBox.Text.ToLower()));
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00016E98 File Offset: 0x00015098
		private void serachTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (this.serachTextBox.Text.Length == 0)
			{
				this.itemsControl.ItemsSource = this.marketCollection;
			}
		}

		// Token: 0x040003C2 RID: 962
		private Market[] markets;

		// Token: 0x040003C3 RID: 963
		private ObservableCollection<UserControlMarket> marketCollection = new ObservableCollection<UserControlMarket>();
	}
}
