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
	// Token: 0x020000AE RID: 174
	public partial class RequiresPage : Page
	{
		// Token: 0x0600046A RID: 1130 RVA: 0x00015DC6 File Offset: 0x00013FC6
		public RequiresPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00015DE0 File Offset: 0x00013FE0
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
						data = wc.DownloadString("https://raw.githubusercontent.com/mohamm4dx/SilverBullet/master/OpenBullet/Requires.json");
					}
				}).ContinueWith(delegate(Task _)
				{
					if (string.IsNullOrWhiteSpace(data))
					{
						this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate
						{
							this.waitingLabel.Visibility = Visibility.Visible;
							this.waitingLabel.Content = "ERROR (CHECK YOUR NETWORK CONNECTION)";
						}));
					}
					else
					{
						this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate
						{
							this.waitingLabel.Visibility = Visibility.Collapsed;
						}));
					}
					this.requires = IOManager.DeserializeObject<Requires[]>(data);
					try
					{
						this.SetRequirements();
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
				this.waitingLabel.Content = "ERROR (CHECK YOUR NETWORK CONNECTION)";
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00015EBC File Offset: 0x000140BC
		private async void SetRequirements()
		{
			RequiresPage.<>c__DisplayClass4_0 CS$<>8__locals1 = new RequiresPage.<>c__DisplayClass4_0();
			CS$<>8__locals1.<>4__this = this;
			if (this.requires != null && this.requires.Length != 0)
			{
				CS$<>8__locals1.i = 0;
				while (CS$<>8__locals1.i < this.requires.Length)
				{
					try
					{
						Dispatcher dispatcher = base.Dispatcher;
						DispatcherPriority dispatcherPriority = DispatcherPriority.Normal;
						ThreadStart threadStart;
						if ((threadStart = CS$<>8__locals1.<>9__0) == null)
						{
							RequiresPage.<>c__DisplayClass4_0 CS$<>8__locals2 = CS$<>8__locals1;
							ThreadStart threadStart2 = delegate
							{
								UserControlSupport uc = new UserControlSupport
								{
									Width = CS$<>8__locals1.<>4__this.requires[CS$<>8__locals1.i].Width,
									Height = CS$<>8__locals1.<>4__this.requires[CS$<>8__locals1.i].Height,
									SupportName = CS$<>8__locals1.<>4__this.requires[CS$<>8__locals1.i].Name,
									Margin = new Thickness(0.0, 0.0, 8.0, 8.0),
									BackgroundButton = (SolidColorBrush)CS$<>8__locals1.<>4__this.brushConverter.ConvertFrom(CS$<>8__locals1.<>4__this.requires[CS$<>8__locals1.i].Color),
									Url = CS$<>8__locals1.<>4__this.requires[CS$<>8__locals1.i].Address,
									border = 
									{
										ToolTip = CS$<>8__locals1.<>4__this.requires[CS$<>8__locals1.i].ToolTip
									}
								};
								uc.SetImage(new Uri(CS$<>8__locals1.<>4__this.requires[CS$<>8__locals1.i].Image));
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

		// Token: 0x0400039A RID: 922
		private Requires[] requires;

		// Token: 0x0400039B RID: 923
		private BrushConverter brushConverter = new BrushConverter();
	}
}
