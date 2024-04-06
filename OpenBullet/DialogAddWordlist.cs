using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Microsoft.Win32;
using OpenBullet.Views.Main;
using RuriLib.Models;

namespace OpenBullet
{
	// Token: 0x02000045 RID: 69
	public class DialogAddWordlist : Page, IComponentConnector
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00006F06 File Offset: 0x00005106
		// (set) Token: 0x0600018B RID: 395 RVA: 0x00006F0E File Offset: 0x0000510E
		public object Caller { get; set; }

		// Token: 0x0600018C RID: 396 RVA: 0x00006F18 File Offset: 0x00005118
		public DialogAddWordlist(object caller)
		{
			this.InitializeComponent();
			this.Caller = caller;
			foreach (string i in SB.Settings.Environment.GetWordlistTypeNames())
			{
				this.typeCombobox.Items.Add(i);
			}
			SB.Settings.Environment.GetWordlistTypeNames().ForEach(delegate(string wt)
			{
				this.subTypeComboBox.Items.Add(wt);
			});
			this.typeCombobox.SelectedIndex = 0;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00006FCC File Offset: 0x000051CC
		private void acceptButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.Caller.GetType() == typeof(WordlistManager))
			{
				if (this.nameTextbox.Text.Trim() == string.Empty)
				{
					MessageBox.Show("The name cannot be blank");
					return;
				}
				string path = this.locationTextbox.Text;
				string cwd = Directory.GetCurrentDirectory();
				if (path.StartsWith(cwd))
				{
					path = path.Substring(cwd.Length + 1);
				}
				((WordlistManager)this.Caller).AddWordlist(new Wordlist(this.nameTextbox.Text, path, this.typeCombobox.Text, this.purposeTextbox.Text, true, false, this.SubWordlists.ToArray()));
			}
			((MainDialog)base.Parent).Close();
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000070A0 File Offset: 0x000052A0
		private void Image_MouseDown(object sender, MouseButtonEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Wordlist files | *.txt";
			ofd.FilterIndex = 1;
			bool? flag = ofd.ShowDialog();
			bool flag2 = false;
			if ((flag.GetValueOrDefault() == flag2) & (flag != null))
			{
				return;
			}
			this.locationTextbox.Text = ofd.FileName;
			this.nameTextbox.Text = Path.GetFileNameWithoutExtension(ofd.FileName);
			try
			{
				string first = File.ReadLines(ofd.FileName).First<string>();
				this.typeCombobox.Text = SB.Settings.Environment.RecognizeWordlistType(first);
			}
			catch
			{
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000714C File Offset: 0x0000534C
		private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(this.locationTextbox.Text))
				{
					MessageBox.Show("Please select main wordlist!", "NOTICE", MessageBoxButton.OK, MessageBoxImage.Hand);
				}
				else
				{
					OpenFileDialog ofd = new OpenFileDialog();
					ofd.Filter = "Sub Wordlist files | *.txt";
					ofd.FilterIndex = 1;
					bool? flag = ofd.ShowDialog();
					bool flag2 = false;
					if (!((flag.GetValueOrDefault() == flag2) & (flag != null)))
					{
						this.locationsSubWordlistsTextbox.Text = ofd.FileName;
						try
						{
							string first = File.ReadLines(ofd.FileName).First<string>();
							this.subTypeComboBox.Text = SB.Settings.Environment.RecognizeWordlistType(first);
						}
						catch
						{
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000721C File Offset: 0x0000541C
		private void selectSubButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(this.locationTextbox.Text))
				{
					MessageBox.Show("Please select main wordlist!", "NOTICE", MessageBoxButton.OK, MessageBoxImage.Hand);
				}
				else if (string.IsNullOrWhiteSpace(this.locationsSubWordlistsTextbox.Text))
				{
					MessageBox.Show("Please select sub wordlist!", "NOTICE", MessageBoxButton.OK, MessageBoxImage.Hand);
				}
				else
				{
					string path = this.locationsSubWordlistsTextbox.Text;
					string cwd = Directory.GetCurrentDirectory();
					if (path.StartsWith(cwd))
					{
						path = path.Substring(cwd.Length + 1);
					}
					SubWordlist sub = new SubWordlist(this.nameTextbox.Text, path, this.subTypeComboBox.Text, this.purposeTextbox.Text, true, false);
					this.SubWordlists.Add(sub);
					MessageBox.Show(string.Format("Added!\nTotal: {0}\nSubWordlist count: {1}", sub.Total, this.SubWordlists.Count));
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00007320 File Offset: 0x00005520
		private void CheckBox_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.selectSubButton.IsEnabled = (this.subTypeLabel.IsEnabled = (this.subTypeComboBox.IsEnabled = (this.locationsSubWordlistsTextbox.IsEnabled = (this.loadSubWordlistIco.IsEnabled = (sender as CheckBox).IsChecked.GetValueOrDefault()))));
			}
			catch
			{
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000739C File Offset: 0x0000559C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/dialogs/dialogaddwordlist.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000073CC File Offset: 0x000055CC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.locationTextbox = (TextBox)target;
				return;
			case 2:
				((Image)target).MouseDown += this.Image_MouseDown;
				return;
			case 3:
				((CheckBox)target).Click += this.CheckBox_Click;
				return;
			case 4:
				this.locationsSubWordlistsTextbox = (TextBox)target;
				return;
			case 5:
				this.loadSubWordlistIco = (Image)target;
				this.loadSubWordlistIco.MouseDown += this.Image_MouseDown_1;
				return;
			case 6:
				this.subTypeLabel = (Label)target;
				return;
			case 7:
				this.subTypeComboBox = (ComboBox)target;
				return;
			case 8:
				this.selectSubButton = (Button)target;
				this.selectSubButton.Click += this.selectSubButton_Click;
				return;
			case 9:
				this.nameTextbox = (TextBox)target;
				return;
			case 10:
				this.typeCombobox = (ComboBox)target;
				return;
			case 11:
				this.purposeTextbox = (TextBox)target;
				return;
			case 12:
				this.acceptButton = (Button)target;
				this.acceptButton.Click += this.acceptButton_Click;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x0400015D RID: 349
		private List<SubWordlist> SubWordlists = new List<SubWordlist>();

		// Token: 0x0400015E RID: 350
		internal TextBox locationTextbox;

		// Token: 0x0400015F RID: 351
		internal TextBox locationsSubWordlistsTextbox;

		// Token: 0x04000160 RID: 352
		internal Image loadSubWordlistIco;

		// Token: 0x04000161 RID: 353
		internal Label subTypeLabel;

		// Token: 0x04000162 RID: 354
		internal ComboBox subTypeComboBox;

		// Token: 0x04000163 RID: 355
		internal Button selectSubButton;

		// Token: 0x04000164 RID: 356
		internal TextBox nameTextbox;

		// Token: 0x04000165 RID: 357
		internal ComboBox typeCombobox;

		// Token: 0x04000166 RID: 358
		internal TextBox purposeTextbox;

		// Token: 0x04000167 RID: 359
		internal Button acceptButton;

		// Token: 0x04000168 RID: 360
		private bool _contentLoaded;
	}
}
