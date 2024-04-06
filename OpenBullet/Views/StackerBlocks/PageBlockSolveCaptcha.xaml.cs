using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using CaptchaSharp.Enums;
using RuriLib;

namespace OpenBullet.Views.StackerBlocks
{
	// Token: 0x0200008C RID: 140
	public partial class PageBlockSolveCaptcha : Page
	{
		// Token: 0x060003A3 RID: 931 RVA: 0x000111A4 File Offset: 0x0000F3A4
		public PageBlockSolveCaptcha(BlockSolveCaptcha block)
		{
			this.InitializeComponent();
			this.vm = block;
			base.DataContext = this.vm;
			foreach (string t in Enum.GetNames(typeof(CaptchaType)))
			{
				this.captchaTypeCombobox.Items.Add(t);
			}
			this.captchaTypeCombobox.SelectedIndex = (int)this.vm.Type;
			foreach (string g in Enum.GetNames(typeof(CaptchaLanguageGroup)))
			{
				this.textLanguageGroupCombobox.Items.Add(g);
				this.imageLanguageGroupCombobox.Items.Add(g);
			}
			this.textLanguageGroupCombobox.SelectedIndex = (int)this.vm.LanguageGroup;
			this.imageLanguageGroupCombobox.SelectedIndex = (int)this.vm.LanguageGroup;
			foreach (string g2 in Enum.GetNames(typeof(CaptchaLanguage)))
			{
				this.textLanguageCombobox.Items.Add(g2);
				this.imageLanguageCombobox.Items.Add(g2);
			}
			this.textLanguageCombobox.SelectedIndex = (int)this.vm.Language;
			this.imageLanguageCombobox.SelectedIndex = (int)this.vm.Language;
			foreach (string s in Enum.GetNames(typeof(CharacterSet)))
			{
				this.charSetCombobox.Items.Add(s);
			}
			this.charSetCombobox.SelectedIndex = (int)this.vm.CharSet;
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00011350 File Offset: 0x0000F550
		private void captchaTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.Type = (CaptchaType)((ComboBox)e.OriginalSource).SelectedIndex;
			Dictionary<CaptchaType, int> dict = new Dictionary<CaptchaType, int>
			{
				{
					CaptchaType.TextCaptcha,
					0
				},
				{
					CaptchaType.ImageCaptcha,
					1
				},
				{
					CaptchaType.ReCaptchaV2,
					2
				},
				{
					CaptchaType.ReCaptchaV3,
					3
				},
				{
					CaptchaType.FunCaptcha,
					4
				},
				{
					CaptchaType.HCaptcha,
					5
				},
				{
					CaptchaType.KeyCaptcha,
					6
				},
				{
					CaptchaType.GeeTest,
					7
				},
				{
					CaptchaType.Capy,
					8
				}
			};
			this.captchaTypeTabControl.SelectedIndex = dict[this.vm.Type];
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x000113E2 File Offset: 0x0000F5E2
		private void languageGroupCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.LanguageGroup = (CaptchaLanguageGroup)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x000113FF File Offset: 0x0000F5FF
		private void languageCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.Language = (CaptchaLanguage)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0001141C File Offset: 0x0000F61C
		private void charSetCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.CharSet = (CharacterSet)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x040002C3 RID: 707
		private BlockSolveCaptcha vm;
	}
}
