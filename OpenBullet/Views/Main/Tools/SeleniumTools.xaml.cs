using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.Main.Tools
{
	// Token: 0x020000E6 RID: 230
	public partial class SeleniumTools : Page
	{
		// Token: 0x060005D6 RID: 1494 RVA: 0x0001E736 File Offset: 0x0001C936
		public SeleniumTools()
		{
			this.InitializeComponent();
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0001E744 File Offset: 0x0001C944
		private void killChromedriversButton_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("cmd.exe", "/C C:\\Windows\\System32\\taskkill.exe /F /IM chromedriver.exe /T");
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0001E756 File Offset: 0x0001C956
		private void killGeckodriversButton_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("cmd.exe", "/C C:\\Windows\\System32\\taskkill.exe /F /IM geckodriver.exe /T");
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0001E768 File Offset: 0x0001C968
		private void deleteChromeCacheFoldersButton_Click(object sender, RoutedEventArgs e)
		{
			foreach (string dir in Directory.GetDirectories(Environment.ExpandEnvironmentVariables("%userprofile%") + "\\AppData\\Local\\Temp"))
			{
				if (dir.Contains("scopeddir") || dir.Contains("chromeurlfetcher") || dir.Contains("chromeBITS"))
				{
					try
					{
						Directory.Delete(dir, true);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0001E7E8 File Offset: 0x0001C9E8
		private void deleteFirefoxCacheFoldersButton_Click(object sender, RoutedEventArgs e)
		{
			foreach (string dir in Directory.GetDirectories(Environment.ExpandEnvironmentVariables("%userprofile%") + "\\AppData\\Local\\Temp"))
			{
				if (dir.Contains("rust_mozprofile"))
				{
					try
					{
						Directory.Delete(dir, true);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001E84C File Offset: 0x0001CA4C
		private void killChromesButton_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("cmd.exe", "/C C:\\Windows\\System32\\taskkill.exe /F /IM chrome.exe /T");
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0001E85E File Offset: 0x0001CA5E
		private void killFirefoxesButton_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("cmd.exe", "/C C:\\Windows\\System32\\taskkill.exe /F /IM firefox.exe /T");
		}
	}
}
