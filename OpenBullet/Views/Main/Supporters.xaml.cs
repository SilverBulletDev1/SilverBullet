using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using OpenBullet.Views.UserControls;
using RuriLib;
using RuriLib.Models;

namespace OpenBullet.Views.Main
{
	// Token: 0x020000B4 RID: 180
	public partial class Supporters : Page
	{
		// Token: 0x06000481 RID: 1153 RVA: 0x0001668C File Offset: 0x0001488C
		public Supporters()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x000166A8 File Offset: 0x000148A8
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.wrapPanel.Children.Count <= 0)
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
						data = wc.DownloadString("https://raw.githubusercontent.com/mohamm4dx/SilverBullet/master/OpenBullet/Supporters.json");
					}
				}).ContinueWith(delegate(Task _)
				{
					if (string.IsNullOrWhiteSpace(data))
					{
						this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate
						{
							this.waitingLabel.Visibility = Visibility.Visible;
							this.waitingLabel.Content = "ERROR";
						}));
					}
					else
					{
						this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate
						{
							this.waitingLabel.Visibility = Visibility.Collapsed;
						}));
					}
					this.supporters = IOManager.DeserializeObject<SupportersModel[]>(data);
					this.Dispatcher.Invoke(delegate
					{
						SB.MainWindow.SilverZonePage.vm.SupportersBadge = ((this.supporters.Length > 99) ? "999+" : this.supporters.Length.ToString());
						int badge = this.supporters.Length + int.Parse(SB.MainWindow.SilverZonePage.vm.VerifiedMarketBadge.Replace("+", ""));
						SB.MainWindow.silverZoneBadged.Badge = ((badge > 99) ? "99+" : badge.ToString());
					});
					try
					{
						this.SetSupporters();
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
				this.waitingLabel.Content = "ERROR";
			}
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00016784 File Offset: 0x00014984
		private async void SetSupporters()
		{
			Supporters.<>c__DisplayClass4_0 CS$<>8__locals1 = new Supporters.<>c__DisplayClass4_0();
			CS$<>8__locals1.<>4__this = this;
			if (this.supporters != null && this.supporters.Length != 0)
			{
				CS$<>8__locals1.i = 0;
				while (CS$<>8__locals1.i < this.supporters.Length)
				{
					try
					{
						Dispatcher dispatcher = base.Dispatcher;
						DispatcherPriority dispatcherPriority = DispatcherPriority.Normal;
						ThreadStart threadStart;
						if ((threadStart = CS$<>8__locals1.<>9__0) == null)
						{
							Supporters.<>c__DisplayClass4_0 CS$<>8__locals2 = CS$<>8__locals1;
							ThreadStart threadStart2 = delegate
							{
								UserControlSupport uc = new UserControlSupport
								{
									Width = 200.0,
									Height = 200.0,
									SupportName = CS$<>8__locals1.<>4__this.supporters[CS$<>8__locals1.i].Name,
									Margin = new Thickness(0.0, 0.0, 8.0, 8.0),
									BackgroundButton = (SolidColorBrush)CS$<>8__locals1.<>4__this.brushConverter.ConvertFrom(CS$<>8__locals1.<>4__this.supporters[CS$<>8__locals1.i].Color),
									Url = CS$<>8__locals1.<>4__this.supporters[CS$<>8__locals1.i].Address
								};
								uc.SetImage(new Uri(CS$<>8__locals1.<>4__this.supporters[CS$<>8__locals1.i].Logo));
								if (!CS$<>8__locals1.<>4__this.wrapPanel.Children.OfType<UserControlSupport>().Any((UserControlSupport u) => u.Url == uc.Url))
								{
									CS$<>8__locals1.<>4__this.wrapPanel.Children.Add(uc);
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

		// Token: 0x040003B2 RID: 946
		private SupportersModel[] supporters;

		// Token: 0x040003B3 RID: 947
		private BrushConverter brushConverter = new BrushConverter();
	}
}
