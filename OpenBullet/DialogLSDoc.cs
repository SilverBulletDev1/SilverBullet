using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace OpenBullet
{
	// Token: 0x0200002F RID: 47
	public class DialogLSDoc : Page, IComponentConnector
	{
		// Token: 0x060000E2 RID: 226 RVA: 0x00003E04 File Offset: 0x00002004
		public DialogLSDoc()
		{
			this.InitializeComponent();
			this.contentDisplay.TextArea.Foreground = new SolidColorBrush(Colors.Gainsboro);
			using (XmlReader reader = XmlReader.Create("LSHighlighting.xshd"))
			{
				this.contentDisplay.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
			}
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.Load("LSDoc.xml");
			}
			catch
			{
				MessageBox.Show("No documentation file found!");
				return;
			}
			this.main = doc.DocumentElement.SelectSingleNode("/doc");
			this.sectionComboBox.Items.Clear();
			this.sections = this.main.ChildNodes;
			foreach (object obj in this.sections)
			{
				XmlNode s = (XmlNode)obj;
				this.sectionComboBox.Items.Add(s.Attributes["name"].Value);
			}
			this.sectionComboBox.SelectedIndex = 0;
			this.currentSection = this.sections[0];
			this.SwitchPage();
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00003F6C File Offset: 0x0000216C
		private void sectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				this.currentSection = this.sections.Item(((ComboBox)e.OriginalSource).SelectedIndex);
				this.SwitchPage();
			}
			catch
			{
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00003FB8 File Offset: 0x000021B8
		private void SwitchPage()
		{
			this.items = this.currentSection.ChildNodes;
			this.menuPanel.Children.Clear();
			foreach (object obj in this.items)
			{
				XmlNode i = (XmlNode)obj;
				Label label = new Label();
				label.Content = i.Attributes["name"].Value;
				label.FontWeight = FontWeights.Bold;
				label.MouseDown += this.menuItem_Clicked;
				this.menuPanel.Children.Add(label);
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000407C File Offset: 0x0000227C
		private void menuItem_Clicked(object sender, MouseButtonEventArgs e)
		{
			try
			{
				for (int i = 0; i < this.items.Count; i++)
				{
					if (this.items[i].Attributes["name"].Value == ((TextBlock)e.OriginalSource).Text)
					{
						this.currentItem = this.items[i];
						break;
					}
				}
				this.DisplayContent();
			}
			catch
			{
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004108 File Offset: 0x00002308
		private void DisplayContent()
		{
			this.titleLabel.Content = this.currentItem.Attributes["name"];
			this.contentDisplay.Text = this.currentItem.InnerText;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004140 File Offset: 0x00002340
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/dialogs/dialoglsdoc.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004170 File Offset: 0x00002370
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.sectionComboBox = (ComboBox)target;
				this.sectionComboBox.SelectionChanged += this.sectionComboBox_SelectionChanged;
				return;
			case 2:
				this.menuPanel = (StackPanel)target;
				return;
			case 3:
				this.titleLabel = (Label)target;
				return;
			case 4:
				this.contentDisplay = (TextEditor)target;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x040000EA RID: 234
		private XmlNode main;

		// Token: 0x040000EB RID: 235
		private XmlNode currentSection;

		// Token: 0x040000EC RID: 236
		private XmlNode currentItem;

		// Token: 0x040000ED RID: 237
		private XmlNodeList sections;

		// Token: 0x040000EE RID: 238
		private XmlNodeList items;

		// Token: 0x040000EF RID: 239
		internal ComboBox sectionComboBox;

		// Token: 0x040000F0 RID: 240
		internal StackPanel menuPanel;

		// Token: 0x040000F1 RID: 241
		internal Label titleLabel;

		// Token: 0x040000F2 RID: 242
		internal TextEditor contentDisplay;

		// Token: 0x040000F3 RID: 243
		private bool _contentLoaded;
	}
}
