using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Win32;
using RuriLib;
using Xceed.Wpf.Toolkit;

namespace OpenBullet.Views.Main.Tools
{
	// Token: 0x020000E2 RID: 226
	public partial class WordlistTools : Page
	{
		// Token: 0x060005BB RID: 1467 RVA: 0x0001DD56 File Offset: 0x0001BF56
		public WordlistTools()
		{
			this.InitializeComponent();
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001DD70 File Offset: 0x0001BF70
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				OpenFileDialog ofd = new OpenFileDialog();
				ofd.Filter = "Wordlist files | *.txt";
				ofd.FilterIndex = 1;
				bool? flag = ofd.ShowDialog();
				bool flag2 = false;
				if (!((flag.GetValueOrDefault() == flag2) & (flag != null)))
				{
					this.wordlistName = Path.GetFileNameWithoutExtension(ofd.FileName);
					try
					{
						this.locTextBox.Text = ofd.FileName;
						this.wordList.AddRange(File.ReadLines(ofd.FileName));
						this.loaded.Text = this.wordList.Count.ToString();
						string first = this.wordList.First<string>();
						this.recognizeWordlistType = SB.Settings.Environment.RecognizeWordlistType(first);
					}
					catch
					{
					}
				}
			}
			catch (Exception ex)
			{
				global::System.Windows.MessageBox.Show(ex.Message, "ERROR");
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001DE68 File Offset: 0x0001C068
		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			try
			{
				List<string> newList = this.wordList.Distinct<string>().ToList<string>();
				this.removedDup.Text = (this.wordList.Count - newList.Count).ToString();
				this.SaveFile("Remove Duplicate", newList.ToArray());
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001DED4 File Offset: 0x0001C0D4
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			try
			{
				List<string> newList = (from w in this.wordList.Select(delegate(string w)
					{
						string text;
						try
						{
							text = w.Split(new string[] { this.splitTextBox.Text }, StringSplitOptions.RemoveEmptyEntries)[this.splitIndex.Value.GetValueOrDefault() - 1];
						}
						catch
						{
							text = w;
						}
						return text;
					})
					where !w.Contains(this.splitTextBox.Text)
					select w).ToList<string>();
				this.splited.Text = newList.Count.ToString();
				this.SaveFile("Splitter", newList.ToArray());
			}
			catch (Exception ex)
			{
				SB.Logger.Log(ex.Message, LogLevel.Error, true, 0);
			}
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001DF64 File Offset: 0x0001C164
		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			try
			{
				StreamReader streamReader = new StreamReader(this.locTextBox.Text);
				string content = streamReader.ReadToEnd();
				streamReader.Close();
				content = Regex.Replace(content, this.currentSepTextBox.Text.Trim(), this.newSepTextBox.Text.Trim());
				this.changed.Text = this.wordList.Count((string w) => w.Contains(this.currentSepTextBox.Text)).ToString();
				SaveFileDialog saveDialog = new SaveFileDialog
				{
					Title = "Change Separator",
					Filter = "Text File|*.txt"
				};
				bool? flag = saveDialog.ShowDialog();
				bool flag2 = true;
				if ((flag.GetValueOrDefault() == flag2) & (flag != null))
				{
					StreamWriter streamWriter = new StreamWriter(saveDialog.FileName);
					streamWriter.Write(content);
					streamWriter.Close();
				}
			}
			catch (Exception ex)
			{
				SB.Logger.Log(ex.Message, LogLevel.Error, true, 0);
			}
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001E05C File Offset: 0x0001C25C
		private void SaveFile(string title, string[] contents)
		{
			SaveFileDialog saveDialog = new SaveFileDialog
			{
				Title = title,
				Filter = "Text File|*.txt"
			};
			bool? flag = saveDialog.ShowDialog();
			bool flag2 = true;
			if ((flag.GetValueOrDefault() == flag2) & (flag != null))
			{
				File.WriteAllLines(saveDialog.FileName, contents);
			}
		}

		// Token: 0x040004AF RID: 1199
		private string recognizeWordlistType;

		// Token: 0x040004B0 RID: 1200
		private string wordlistName;

		// Token: 0x040004B1 RID: 1201
		private List<string> wordList = new List<string>();
	}
}
