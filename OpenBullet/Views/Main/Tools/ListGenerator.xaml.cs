using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Win32;
using OpenBullet.ViewModels;
using RuriLib.Models;

namespace OpenBullet.Views.Main.Tools
{
	// Token: 0x020000E3 RID: 227
	public partial class ListGenerator : Page
	{
		// Token: 0x060005C6 RID: 1478 RVA: 0x0001E289 File Offset: 0x0001C489
		public ListGenerator()
		{
			this.InitializeComponent();
			base.DataContext = this.vm;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001E2B9 File Offset: 0x0001C4B9
		private void lowercaseButton_Click(object sender, RoutedEventArgs e)
		{
			ListGeneratorViewModel listGeneratorViewModel = this.vm;
			listGeneratorViewModel.AllowedCharacters += "abcdefghijklmnopqrstuvwxyz";
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001E2D6 File Offset: 0x0001C4D6
		private void uppercaseButton_Click(object sender, RoutedEventArgs e)
		{
			ListGeneratorViewModel listGeneratorViewModel = this.vm;
			listGeneratorViewModel.AllowedCharacters += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001E2F3 File Offset: 0x0001C4F3
		private void digitsButton_Click(object sender, RoutedEventArgs e)
		{
			ListGeneratorViewModel listGeneratorViewModel = this.vm;
			listGeneratorViewModel.AllowedCharacters += "0123456789";
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001E310 File Offset: 0x0001C510
		private void clearButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.AllowedCharacters = "";
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0001E324 File Offset: 0x0001C524
		private void generateButton_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Text File |*.txt";
			sfd.Title = "Save Output List";
			sfd.ShowDialog();
			if (sfd.FileName != string.Empty)
			{
				this.sw = new StreamWriter(sfd.FileName);
				this.WriteCombinations(this.vm.Mask);
				this.sw.Close();
				this.sw.Dispose();
				if (this.vm.AutoImport)
				{
					Wordlist wordlist = new Wordlist("Generated" + this.rand.Next().ToString(), sfd.FileName, "Default", "", true, false, null);
					SB.MainWindow.WordlistManagerPage.AddWordlist(wordlist);
				}
			}
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001E3F4 File Offset: 0x0001C5F4
		private void WriteCombinations(string input)
		{
			if (input.Contains('*'))
			{
				foreach (char c in this.vm.AllowedCharacters)
				{
					this.WriteCombinations(new Regex("\\*").Replace(input, c.ToString(), 1));
				}
				return;
			}
			if ((this.vm.OnlyLuhn && ListGenerator.Luhn(input.Split(new char[] { ':' })[0])) || !this.vm.OnlyLuhn)
			{
				this.sw.WriteLine(input);
			}
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0001E490 File Offset: 0x0001C690
		private List<string> Generate(List<string> list)
		{
			if (list.Any((string s) => s.Contains('*')))
			{
				List<string> newList = new List<string>();
				foreach (string s2 in list)
				{
					foreach (char c in this.vm.AllowedCharacters)
					{
						newList.Add(new Regex("\\*").Replace(s2, c.ToString(), 1));
					}
				}
				return this.Generate(newList);
			}
			return list;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0001E55C File Offset: 0x0001C75C
		public static bool Luhn(string digits)
		{
			Func<char, bool> func;
			if ((func = ListGenerator.<>O.<0>__IsDigit) == null)
			{
				func = (ListGenerator.<>O.<0>__IsDigit = new Func<char, bool>(char.IsDigit));
			}
			if (digits.All(func))
			{
				return (from c in digits.Reverse<char>()
					select (int)(c - '0')).Select(delegate(int thisNum, int i)
				{
					if (i % 2 == 0)
					{
						return thisNum;
					}
					if ((thisNum *= 2) <= 9)
					{
						return thisNum;
					}
					return thisNum - 9;
				}).Sum() % 10 == 0;
			}
			return false;
		}

		// Token: 0x040004BC RID: 1212
		private ListGeneratorViewModel vm = new ListGeneratorViewModel();

		// Token: 0x040004BD RID: 1213
		private StreamWriter sw;

		// Token: 0x040004BE RID: 1214
		private Random rand = new Random();

		// Token: 0x020000E4 RID: 228
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040004C5 RID: 1221
			public static Func<char, bool> <0>__IsDigit;
		}
	}
}
