using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using RuriLib.Models;

namespace OpenBullet.Views.Dialogs
{
	// Token: 0x02000116 RID: 278
	public partial class DialogEditWordlist : Page
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x0002AA6D File Offset: 0x00028C6D
		// (set) Token: 0x0600076B RID: 1899 RVA: 0x0002AA75 File Offset: 0x00028C75
		public Wordlist WordList { get; private set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x0002AA7E File Offset: 0x00028C7E
		// (set) Token: 0x0600076D RID: 1901 RVA: 0x0002AA86 File Offset: 0x00028C86
		public DialogResult DialogResult { get; private set; }

		// Token: 0x0600076E RID: 1902 RVA: 0x0002AA90 File Offset: 0x00028C90
		public DialogEditWordlist(Wordlist wordlist)
		{
			this.WordList = wordlist;
			this.InitializeComponent();
			foreach (string i in SB.Settings.Environment.GetWordlistTypeNames())
			{
				this.wordlistType.Items.Add(i);
			}
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0002AB0C File Offset: 0x00028D0C
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				this.wordlistName.Text = this.WordList.Name;
				this.wordlistPath.Text = this.WordList.Path;
				this.wordlistPurpose.Text = this.WordList.Purpose;
				this.wordlistType.SelectedItem = this.WordList.Type;
			}
			catch
			{
			}
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0002AB88 File Offset: 0x00028D88
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.DialogResult = DialogResult.Cancel;
				((MainDialog)base.Parent).Close();
			}
			catch
			{
			}
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0002ABC4 File Offset: 0x00028DC4
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			try
			{
				this.DialogResult = DialogResult.OK;
				this.WordList.Name = this.wordlistName.Text;
				this.WordList.Path = this.wordlistPath.Text;
				this.WordList.Purpose = this.wordlistPurpose.Text;
				this.WordList.Type = this.wordlistType.SelectedItem.ToString();
				((MainDialog)base.Parent).Close();
			}
			catch
			{
			}
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0002AC5C File Offset: 0x00028E5C
		private void Page_KeyDown(object sender, global::System.Windows.Input.KeyEventArgs e)
		{
			try
			{
				if (e.Key == global::System.Windows.Input.Key.Return)
				{
					this.Button_Click_1(null, null);
				}
			}
			catch
			{
			}
		}
	}
}
