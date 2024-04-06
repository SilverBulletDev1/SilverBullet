using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using RuriLib;
using RuriLib.Functions.Conditions;
using RuriLib.Functions.Conversions;

namespace OpenBullet.Views.StackerBlocks
{
	// Token: 0x020000A0 RID: 160
	public partial class PageBlockUtility : Page
	{
		// Token: 0x06000427 RID: 1063 RVA: 0x00014634 File Offset: 0x00012834
		public PageBlockUtility(BlockUtility block)
		{
			this.InitializeComponent();
			this.block = block;
			base.DataContext = this.block;
			foreach (string g in Enum.GetNames(typeof(UtilityGroup)))
			{
				this.groupCombobox.Items.Add(g);
			}
			this.groupCombobox.SelectedIndex = (int)block.Group;
			foreach (string a in Enum.GetNames(typeof(ListAction)))
			{
				this.listActionCombobox.Items.Add(a);
			}
			this.listActionCombobox.SelectedIndex = (int)block.ListAction;
			foreach (string a2 in Enum.GetNames(typeof(VarAction)))
			{
				this.varActionCombobox.Items.Add(a2);
			}
			this.varActionCombobox.SelectedIndex = (int)block.VarAction;
			foreach (string c in Enum.GetNames(typeof(Encoding)))
			{
				this.conversionFromCombobox.Items.Add(c);
			}
			this.conversionFromCombobox.SelectedIndex = (int)block.ConversionFrom;
			foreach (string c2 in Enum.GetNames(typeof(Encoding)))
			{
				this.conversionToCombobox.Items.Add(c2);
			}
			this.conversionToCombobox.SelectedIndex = (int)block.ConversionTo;
			foreach (string a3 in Enum.GetNames(typeof(FileAction)))
			{
				this.fileActionCombobox.Items.Add(a3);
			}
			this.fileActionCombobox.SelectedIndex = (int)block.FileAction;
			foreach (string a4 in Enum.GetNames(typeof(FolderAction)))
			{
				this.folderActionCombobox.Items.Add(a4);
			}
			this.folderActionCombobox.SelectedIndex = (int)block.FolderAction;
			foreach (string c3 in Enum.GetNames(typeof(Comparer)))
			{
				this.removeComparerCombobox.Items.Add(c3);
			}
			this.removeComparerCombobox.SelectedIndex = (int)block.ListElementComparer;
			this.messageRTB.AppendText(block.GetMessages());
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x000148A8 File Offset: 0x00012AA8
		private void groupCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.block.Group = (UtilityGroup)((ComboBox)e.OriginalSource).SelectedIndex;
			switch (this.block.Group)
			{
			case UtilityGroup.List:
				this.groupTabControl.SelectedIndex = 1;
				return;
			case UtilityGroup.Variable:
				this.groupTabControl.SelectedIndex = 2;
				return;
			case UtilityGroup.Conversion:
				this.groupTabControl.SelectedIndex = 3;
				return;
			case UtilityGroup.File:
				this.groupTabControl.SelectedIndex = 4;
				return;
			case UtilityGroup.Folder:
				this.groupTabControl.SelectedIndex = 5;
				return;
			case UtilityGroup.Telegram:
				this.groupTabControl.SelectedIndex = 6;
				return;
			default:
				this.groupTabControl.SelectedIndex = 0;
				return;
			}
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00014954 File Offset: 0x00012B54
		private void listActionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.block.ListAction = (ListAction)((ComboBox)e.OriginalSource).SelectedIndex;
			switch (this.block.ListAction)
			{
			case ListAction.Join:
				this.listActionTabControl.SelectedIndex = 1;
				return;
			case ListAction.Sort:
				this.listActionTabControl.SelectedIndex = 2;
				return;
			case ListAction.Concat:
			case ListAction.Zip:
				this.listActionTabControl.SelectedIndex = 3;
				return;
			case ListAction.Map:
				this.listActionTabControl.SelectedIndex = 3;
				return;
			case ListAction.Add:
				this.listActionTabControl.SelectedIndex = 4;
				return;
			case ListAction.Remove:
				this.listActionTabControl.SelectedIndex = 5;
				return;
			case ListAction.RemoveValues:
				this.listActionTabControl.SelectedIndex = 6;
				return;
			default:
				this.listActionTabControl.SelectedIndex = 0;
				return;
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00014A17 File Offset: 0x00012C17
		private void conversionFromCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.block.ConversionFrom = (Encoding)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00014A34 File Offset: 0x00012C34
		private void conversionToCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.block.ConversionTo = (Encoding)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00014A51 File Offset: 0x00012C51
		private void fileActionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.block.FileAction = (FileAction)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00014A6E File Offset: 0x00012C6E
		private void folderActionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.block.FolderAction = (FolderAction)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00014A8B File Offset: 0x00012C8B
		private void removeComparerCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.block.ListElementComparer = (Comparer)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00014AA8 File Offset: 0x00012CA8
		private void varActionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.block.VarAction = (VarAction)((ComboBox)e.OriginalSource).SelectedIndex;
			if (this.block.VarAction != VarAction.Split)
			{
				this.varActionTabControl.SelectedIndex = 0;
				return;
			}
			this.varActionTabControl.SelectedIndex = 1;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00014AF6 File Offset: 0x00012CF6
		private void TelegramActionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.block == null)
			{
				return;
			}
			this.block.TelegramAction = (TelegramAction)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00014B1C File Offset: 0x00012D1C
		private void messageRTB_LostFocus(object sender, RoutedEventArgs e)
		{
			this.block.SetMessages(this.messageRTB.Lines());
		}

		// Token: 0x04000354 RID: 852
		private BlockUtility block;
	}
}
