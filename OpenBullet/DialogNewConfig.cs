using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using OpenBullet.Repositories;
using OpenBullet.Views.Main.Configs;
using RuriLib.Functions.Files;
using RuriLib.ViewModels;

namespace OpenBullet
{
	// Token: 0x02000043 RID: 67
	public class DialogNewConfig : Page, IComponentConnector
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00006B7D File Offset: 0x00004D7D
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00006B85 File Offset: 0x00004D85
		public object Caller { get; set; }

		// Token: 0x06000181 RID: 385 RVA: 0x00006B90 File Offset: 0x00004D90
		public DialogNewConfig(object caller)
		{
			this.InitializeComponent();
			this.Caller = caller;
			this.authorTextbox.Text = SB.SBSettings.General.DefaultAuthor;
			this.nameTextbox.Focus();
			this.categoryCombobox.Items.Add(ConfigRepository.defaultCategory);
			foreach (object category2 in (from c in SB.ConfigManager.ConfigsCollection
				select c.Category into category
				where category != ConfigRepository.defaultCategory
				select category).Distinct<string>())
			{
				this.categoryCombobox.Items.Add(category2);
			}
			this.categoryCombobox.SelectedIndex = 0;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006C9C File Offset: 0x00004E9C
		private void acceptButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.Caller.GetType() == typeof(ConfigManager))
			{
				if (this.nameTextbox.Text.Trim() == string.Empty)
				{
					MessageBox.Show("The name cannot be blank");
					return;
				}
				if (this.nameTextbox.Text != Files.MakeValidFileName(this.nameTextbox.Text, true))
				{
					MessageBox.Show("The name contains invalid characters");
					return;
				}
				if (string.IsNullOrWhiteSpace(this.categoryCombobox.Text))
				{
					this.categoryCombobox.Text = ConfigRepository.defaultCategory;
				}
				else if (this.categoryCombobox.Text != Files.MakeValidFileName(this.categoryCombobox.Text, true))
				{
					MessageBox.Show("The category contains invalid characters");
					return;
				}
				try
				{
					((ConfigManager)this.Caller).CreateConfig(this.nameTextbox.Text, this.categoryCombobox.Text, this.authorTextbox.Text);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
					return;
				}
			}
			((MainDialog)base.Parent).Close();
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006DDC File Offset: 0x00004FDC
		private void textbox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				this.acceptButton_Click(this, new RoutedEventArgs());
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00006DF4 File Offset: 0x00004FF4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/dialogs/dialognewconfig.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00006E24 File Offset: 0x00005024
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.nameTextbox = (TextBox)target;
				this.nameTextbox.KeyDown += this.textbox_KeyDown;
				return;
			case 2:
				this.categoryCombobox = (ComboBox)target;
				this.categoryCombobox.KeyDown += this.textbox_KeyDown;
				return;
			case 3:
				this.authorTextbox = (TextBox)target;
				this.authorTextbox.KeyDown += this.textbox_KeyDown;
				return;
			case 4:
				this.acceptButton = (Button)target;
				this.acceptButton.Click += this.acceptButton_Click;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x04000154 RID: 340
		internal TextBox nameTextbox;

		// Token: 0x04000155 RID: 341
		internal ComboBox categoryCombobox;

		// Token: 0x04000156 RID: 342
		internal TextBox authorTextbox;

		// Token: 0x04000157 RID: 343
		internal Button acceptButton;

		// Token: 0x04000158 RID: 344
		private bool _contentLoaded;
	}
}
