using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using AngleSharp.Text;
using RuriLib;

namespace OpenBullet.Views.UserControls
{
	// Token: 0x0200006B RID: 107
	public partial class UserControlMarket : UserControl
	{
		// Token: 0x06000294 RID: 660 RVA: 0x0000B5C2 File Offset: 0x000097C2
		public UserControlMarket()
		{
			this.InitializeComponent();
			base.DataContext = this;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000B5F4 File Offset: 0x000097F4
		public void SetIcon(Uri imgSource)
		{
			BitmapImage bitmap = new BitmapImage();
			bitmap.BeginInit();
			bitmap.UriSource = imgSource;
			bitmap.EndInit();
			this.icon.Source = bitmap;
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000B626 File Offset: 0x00009826
		// (set) Token: 0x06000297 RID: 663 RVA: 0x0000B62E File Offset: 0x0000982E
		public string Seller { get; set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000B637 File Offset: 0x00009837
		// (set) Token: 0x06000299 RID: 665 RVA: 0x0000B63F File Offset: 0x0000983F
		public string Date { get; set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000B648 File Offset: 0x00009848
		// (set) Token: 0x0600029B RID: 667 RVA: 0x0000B650 File Offset: 0x00009850
		public string Category { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000B659 File Offset: 0x00009859
		// (set) Token: 0x0600029D RID: 669 RVA: 0x0000B661 File Offset: 0x00009861
		public string ContentMarket { get; set; }

		// Token: 0x0600029E RID: 670 RVA: 0x0000B66C File Offset: 0x0000986C
		public void SetContent(string content)
		{
			string[] contents = content.Split(new char[] { '\n' });
			if (contents == null || content.Length == 0)
			{
				contents = new string[] { content };
			}
			foreach (string cont in contents)
			{
				string tmpCont = cont;
				DockPanel dockPanel = new DockPanel();
				this.contentPanel.Children.Add(dockPanel);
				MatchCollection urls = this.regexUrl.Matches(tmpCont);
				for (int i = 0; i < urls.Count; i++)
				{
					string url = urls[i].Value;
					string text = tmpCont.GetUntilOrEmpty(url);
					tmpCont = tmpCont.ReplaceFirst(text + url, string.Empty);
					TextBlock link = this.CreateHyperLink(url, text);
					dockPanel.Children.Add(link);
				}
				if (!string.IsNullOrWhiteSpace(tmpCont))
				{
					dockPanel.Children.Add(this.CreateTextBlock(tmpCont));
				}
				else if (string.IsNullOrEmpty(cont))
				{
					dockPanel.Children.Add(this.CreateTextBlock(cont));
				}
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000B788 File Offset: 0x00009988
		private TextBlock CreateHyperLink(string url, string text)
		{
			TextBlock textBlock = new TextBlock();
			textBlock.Text = text;
			textBlock.Margin = new Thickness(0.0, 0.0, 3.0, 0.0);
			textBlock.FontSize = 13.5;
			Uri uri = new Uri(url);
			Hyperlink hyperLink = new Hyperlink(new Run(uri.Host.Contains("t.me") ? uri.AbsolutePath.Replace("/", "") : (uri.Host + uri.AbsolutePath)))
			{
				Cursor = Cursors.Hand,
				NavigateUri = uri,
				Foreground = (this.converter.ConvertFrom("#FF3CE6EC") as SolidColorBrush)
			};
			hyperLink.RequestNavigate += this.Hyperlink_RequestNavigate;
			textBlock.Inlines.Add(hyperLink);
			return textBlock;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000B876 File Offset: 0x00009A76
		private TextBlock CreateTextBlock(string text)
		{
			return new TextBlock
			{
				Text = text
			};
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000B884 File Offset: 0x00009A84
		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			try
			{
				Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
				e.Handled = true;
			}
			catch
			{
			}
		}

		// Token: 0x04000206 RID: 518
		private BrushConverter converter = new BrushConverter();

		// Token: 0x0400020B RID: 523
		private Regex regexUrl = new Regex("\\b(?:https?://|www\\.)\\S+\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
	}
}
