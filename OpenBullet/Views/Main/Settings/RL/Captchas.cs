using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using CaptchaSharp.Exceptions;
using RuriLib;
using RuriLib.Enums;
using RuriLib.Functions.Captchas;
using RuriLib.ViewModels;

namespace OpenBullet.Views.Main.Settings.RL
{
	// Token: 0x020000F1 RID: 241
	public class Captchas : Page, IComponentConnector
	{
		// Token: 0x0600062E RID: 1582 RVA: 0x000203FC File Offset: 0x0001E5FC
		public Captchas()
		{
			this.InitializeComponent();
			base.DataContext = SB.Settings.RLSettings.Captchas;
			foreach (string i in Enum.GetNames(typeof(CaptchaServiceType)))
			{
				this.currentServiceCombobox.Items.Add(i);
			}
			this.currentServiceCombobox.SelectedIndex = (int)SB.Settings.RLSettings.Captchas.CurrentService;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00020480 File Offset: 0x0001E680
		private void currentServiceCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SB.Settings.RLSettings.Captchas.CurrentService = (CaptchaServiceType)this.currentServiceCombobox.SelectedIndex;
			Dictionary<CaptchaServiceType, int> dict = new Dictionary<CaptchaServiceType, int>
			{
				{
					CaptchaServiceType.TwoCaptcha,
					0
				},
				{
					CaptchaServiceType.AntiCaptcha,
					1
				},
				{
					CaptchaServiceType.CustomTwoCaptcha,
					2
				},
				{
					CaptchaServiceType.CapMonster,
					2
				},
				{
					CaptchaServiceType.DeathByCaptcha,
					3
				},
				{
					CaptchaServiceType.DeCaptcher,
					4
				},
				{
					CaptchaServiceType.ImageTyperz,
					5
				},
				{
					CaptchaServiceType.AzCaptcha,
					6
				},
				{
					CaptchaServiceType.CaptchasIO,
					7
				},
				{
					CaptchaServiceType.RuCaptcha,
					8
				},
				{
					CaptchaServiceType.SolveCaptcha,
					9
				},
				{
					CaptchaServiceType.SolveRecaptcha,
					10
				},
				{
					CaptchaServiceType.TrueCaptcha,
					11
				}
			};
			this.captchaServiceTabControl.SelectedIndex = dict[SB.Settings.RLSettings.Captchas.CurrentService];
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00020548 File Offset: 0x0001E748
		private async void checkBalanceButton_Click(object sender, RoutedEventArgs e)
		{
			IOManager.SaveSettings<RLSettingsViewModel>(SB.rlSettingsFile, SB.Settings.RLSettings);
			try
			{
				decimal balance = await Captchas.GetService(SB.Settings.RLSettings.Captchas).GetBalanceAsync(default(CancellationToken));
				this.balanceLabel.Content = balance;
				this.balanceLabel.Foreground = ((balance > 0m) ? Utils.GetBrush("ForegroundGood") : Utils.GetBrush("ForegroundBad"));
			}
			catch (BadAuthenticationException)
			{
				this.balanceLabel.Content = "WRONG TOKEN / CREDENTIALS";
				this.balanceLabel.Foreground = Utils.GetBrush("ForegroundBad");
			}
			catch
			{
				this.balanceLabel.Content = "AN ERROR OCCURRED";
				this.balanceLabel.Foreground = Utils.GetBrush("ForegroundBad");
			}
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00020580 File Offset: 0x0001E780
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/main/settings/rurilib/captchas.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x000205B0 File Offset: 0x0001E7B0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.currentServiceCombobox = (ComboBox)target;
				this.currentServiceCombobox.SelectionChanged += this.currentServiceCombobox_SelectionChanged;
				return;
			case 2:
				this.captchaServiceTabControl = (TabControl)target;
				return;
			case 3:
				this.checkBalanceButton = (Button)target;
				this.checkBalanceButton.Click += this.checkBalanceButton_Click;
				return;
			case 4:
				this.balanceLabel = (Label)target;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x0400051F RID: 1311
		internal ComboBox currentServiceCombobox;

		// Token: 0x04000520 RID: 1312
		internal TabControl captchaServiceTabControl;

		// Token: 0x04000521 RID: 1313
		internal Button checkBalanceButton;

		// Token: 0x04000522 RID: 1314
		internal Label balanceLabel;

		// Token: 0x04000523 RID: 1315
		private bool _contentLoaded;
	}
}
