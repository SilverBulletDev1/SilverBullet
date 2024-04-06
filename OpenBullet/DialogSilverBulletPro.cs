using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace OpenBullet
{
	// Token: 0x02000032 RID: 50
	public class DialogSilverBulletPro : Page, IComponentConnector
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x000045FD File Offset: 0x000027FD
		public DialogSilverBulletPro()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000460C File Offset: 0x0000280C
		private void Buy_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.Start("tg://resolve?domain=mohamm4dx");
			}
			catch
			{
				Process.Start("https://t.me/mohamm4dx");
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004644 File Offset: 0x00002844
		private void Buy1_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.Start("tg://resolve?domain=AlirezaCR");
			}
			catch
			{
				Process.Start("https://t.me/AlirezaCR");
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000467C File Offset: 0x0000287C
		private void Channel_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.Start("tg://resolve?domain=SilverBullet_Soft");
			}
			catch
			{
				Process.Start("https://t.me/mohamm4dx");
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000046B4 File Offset: 0x000028B4
		private void Image_MouseDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				Process.Start("https://t.me/SilverBullet_Soft/8");
			}
			catch
			{
				Process.Start("https://t.me/mohamm4dx");
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000046EC File Offset: 0x000028EC
		private void TextBox_Loaded(object sender, RoutedEventArgs e)
		{
			(sender as TextBox).Text = "\ud83d\udecd Order SilverBullet Pro now!\r\n\r\n❓ What is the SilverBullet Pro?: It's a paid, highly optimized and enhanced version of the SilverBullet (OB-based) with a noob-friendly UI and faster in action than other mod releases!\r\n\r\nℹ\ufe0f Changes log:\r\n\r\n\ud83c\udd95 Added a new block selection menu (similar to OB2) and you can search your desired function with a search bar\r\n\ud83c\udd95 Added a new Proxy organizer, you can group your proxies and use them for your each desired runner\r\n\ud83c\udd95 Added an ability to set your desired icon on your config and pre-view it freely\r\n\ud83c\udd95 Added a large Real User-Agent database that includes Console User-Agents too\r\n\ud83c\udd95 Added an Encoding option for parse your inputs with your desired encoding\r\n\ud83c\udd95 Added Puppeteer (replaced with selenium)\r\n\ud83c\udd95 Added a newer Config to Checker compiler\r\n\ud83c\udd95 Added a stable WebSocket request block (supports HTTP/S, SOCKS4 and SOCKS5 proxies)\r\n\ud83c\udd95 Added HTTP request block with supporting HTTP/2 and HTTP/3\r\n\ud83c\udd95 Added Lambda Parser: Runtime parser for string expressions (formulas, method calls, properties/fields/arrays accessors), also Lambda Parser builds dynamic LINQ expression tree and compiles it to the lambda delegate. Types are resolved at run-time like in dynamic languages\r\n\ud83c\udd95 Added a new light-weight browser (Edge Chromium-based engine) and you'll not grab old browser errors\r\n\ud83c\udd95 Added a Script Auto-Complete ability\r\n\ud83c\udd95 Added C# and NodeJS interpreters\r\n\ud83c\udd95 Added an ability to import your custom C#, Python and NodeJS libraries in app\r\n\ud83c\udd95 Added FTP request block (supports HTTP/S, SOCKS4 and SOCKS5 proxies)\r\n\ud83c\udd95 Added SMTP request block (supports HTTP/S, SOCKS4 and SOCKS5 proxies)\r\n\ud83c\udd95 Added POP3 request block (supports HTTP/S, SOCKS4 and SOCKS5 proxies)\r\n\ud83c\udd95 Added IMAP request block (supports HTTP/S, SOCKS4 and SOCKS5 proxies)\r\n\ud83c\udd95 Added SSH request block (supports HTTP/S, SOCKS4 and SOCKS5 proxies)\r\n\ud83c\udd95 Added Shell Command block\r\n\ud83c\udd95 Added built-in Undo and Redo abilities in Stacker section while changing your blocks\r\n\ud83c\udd95 Added an ability to add your custom plug-ins without re-starting the app\r\n\ud83c\udd95 Added UNIX Timestamp with different units like millisecond, microsecond and nanosecond\r\n\ud83c\udd95 Added a customizable GUID with your desired format\r\n\ud83c\udd95 Added a new Parse method called XPath for parsing XML files and responses\r\n\ud83c\udd95 Added new commands in LoliScript like PRINT (with customizable text colors), STOP, BREAK, RETURN and etc.\r\n\ud83c\udd95 Added an execution priority on each Keychains of the KEY_CHECK block to guide the bot to continue or stop executing remained blocks\r\n\ud83c\udd95 Added an automatic option that automatically detects type of your variables/captures, you can also re-define your variables/captures with another type of data\r\n\ud83c\udd95 Added support of Long Integers (Int64) in Random Integer block\r\n\ud83c\udd95 Added an option to import your RSA settings as XML parameters\r\n\ud83c\udd95 Added a simple CF Bypass block (supports HTTP/S & SOCKS proxies [without proxy authentication support and single-threaded])\r\n\ud83c\udd95 Added Config Converter option in \"Tools\" tab of the sidebar\r\n\ud83c\udd95 Added an ability for change your language to your desired one (Multi-Language)\r\n\ud83c\udd95 Added safe-mode for Parse, Lambda and few amounts of Function blocks\r\n\ud83c\udd95 Added NativeLibInvoke block (for invoking functions through native libraries)\r\n\ud83c\udd95 Added \"ESLEIF\" command in SVBScript\r\n\ud83c\udd95 Added a feature to load different proxy list for each Runner task\r\n\ud83c\udd95 Added Selenium\r\n\ud83c\udd95 Added a new engine for OCR\r\n\ud83c\udd95 Added a feature to use wordlist with range and combination modes\r\n\ud83c\udd95 Added old OCR config to new one while converting old config to SB Pro configs\r\n\ud83c\udd95 Added Words to Numbers and vice versa block\r\n\ud83c\udd95 Added new filters for OCR engine\r\n\ud83c\udd95 Added Runner Scheduler\r\n\r\n\ud83d\uded2 Price (lifetime): $179\r\n\r\n\ud83c\udd94 Buy from: t.me/mohamm4dx / t.me/AlirezaCR\r\n\r\n\ud83d\udce3: https://t.me/SilverBullet_Soft";
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004700 File Offset: 0x00002900
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/dialogs/dialogsilverbulletpro.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004730 File Offset: 0x00002930
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((Button)target).Click += this.Buy_Click;
				return;
			case 2:
				((Button)target).Click += this.Channel_Click;
				return;
			case 3:
				((Image)target).MouseDown += this.Image_MouseDown;
				return;
			case 4:
				((Button)target).Click += this.Buy1_Click;
				return;
			case 5:
				((TextBox)target).Loaded += this.TextBox_Loaded;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x04000100 RID: 256
		private bool _contentLoaded;
	}
}
