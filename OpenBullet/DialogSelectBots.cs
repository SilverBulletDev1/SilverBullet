using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using RuriLib.Runner;

namespace OpenBullet
{
	// Token: 0x02000030 RID: 48
	public class DialogSelectBots : Page, IComponentConnector
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x000041E9 File Offset: 0x000023E9
		// (set) Token: 0x060000EA RID: 234 RVA: 0x000041F1 File Offset: 0x000023F1
		public object Caller { get; set; }

		// Token: 0x060000EB RID: 235 RVA: 0x000041FA File Offset: 0x000023FA
		public DialogSelectBots(object caller, int initial = 1)
		{
			this.InitializeComponent();
			this.Caller = caller;
			this.botsNumberTextbox.Text = initial.ToString();
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004224 File Offset: 0x00002424
		private void selectButton_Click(object sender, RoutedEventArgs e)
		{
			int bots = 1;
			int.TryParse(this.botsNumberTextbox.Text, out bots);
			if (this.Caller.GetType() == typeof(RunnerViewModel))
			{
				(this.Caller as RunnerViewModel).BotsAmount = bots;
			}
			((MainDialog)base.Parent).Close();
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004284 File Offset: 0x00002484
		private void botsNumberTextbox_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Key == Key.Return)
				{
					this.selectButton_Click(null, null);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000042B8 File Offset: 0x000024B8
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				this.botsNumberTextbox.CaretIndex = this.botsNumberTextbox.Text.Length;
				this.botsNumberTextbox.Focus();
			}
			catch
			{
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004304 File Offset: 0x00002504
		private void botsNumberTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			try
			{
				Regex regex = new Regex("[^0-9]+");
				e.Handled = regex.IsMatch(e.Text);
				if (!e.Handled)
				{
					TextBox textBox = (TextBox)sender;
					string value = textBox.Text;
					if (textBox.SelectedText != string.Empty)
					{
						value = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectedText.Length);
					}
					int botsAmount = int.Parse(value + e.Text);
					e.Handled = botsAmount > 400 || botsAmount <= 0;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000043B4 File Offset: 0x000025B4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/dialogs/dialogselectbots.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000043E4 File Offset: 0x000025E4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((DialogSelectBots)target).Loaded += this.Page_Loaded;
				return;
			case 2:
				this.botsNumberTextbox = (TextBox)target;
				this.botsNumberTextbox.KeyDown += this.botsNumberTextbox_KeyDown;
				this.botsNumberTextbox.PreviewTextInput += this.botsNumberTextbox_PreviewTextInput;
				return;
			case 3:
				this.selectButton = (Button)target;
				this.selectButton.Click += this.selectButton_Click;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x040000F5 RID: 245
		private const int Maximum = 400;

		// Token: 0x040000F6 RID: 246
		private const int Minimum = 1;

		// Token: 0x040000F7 RID: 247
		internal TextBox botsNumberTextbox;

		// Token: 0x040000F8 RID: 248
		internal Button selectButton;

		// Token: 0x040000F9 RID: 249
		private bool _contentLoaded;
	}
}
