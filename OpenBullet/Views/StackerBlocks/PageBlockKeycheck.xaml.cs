using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using MahApps.Metro.IconPacks;
using OpenBullet.ViewModels;
using RuriLib;
using RuriLib.Functions.Conditions;
using RuriLib.Models;

namespace OpenBullet.Views.StackerBlocks
{
	// Token: 0x02000096 RID: 150
	public partial class PageBlockKeycheck : Page
	{
		// Token: 0x060003DE RID: 990 RVA: 0x00012ACE File Offset: 0x00010CCE
		public PageBlockKeycheck(BlockKeycheck block)
		{
			this.InitializeComponent();
			this.vm = new BlockKeycheckViewModel(block);
			base.DataContext = this.vm;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00012B00 File Offset: 0x00010D00
		private void addKeychainImage_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.vm.AddKeychain();
			try
			{
				this.keychainsScrollViewer.ScrollToEnd();
			}
			catch
			{
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00012B38 File Offset: 0x00010D38
		private void keychainTypeCombobox_Loaded(object sender, RoutedEventArgs e)
		{
			KeychainViewModel kc = this.vm.GetKeychainById((int)((ComboBox)e.OriginalSource).Tag);
			if (kc.TypeInitialized)
			{
				return;
			}
			kc.TypeInitialized = true;
			foreach (string t in Enum.GetNames(typeof(KeyChain.KeychainType)))
			{
				((ComboBox)e.OriginalSource).Items.Add(t);
			}
			((ComboBox)e.OriginalSource).SelectedIndex = (int)kc.Type;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00012BC8 File Offset: 0x00010DC8
		private void keychainModeCombobox_Loaded(object sender, RoutedEventArgs e)
		{
			KeychainViewModel kc = this.vm.GetKeychainById((int)((ComboBox)e.OriginalSource).Tag);
			if (kc.ModeInitialized)
			{
				return;
			}
			kc.ModeInitialized = true;
			foreach (string i in Enum.GetNames(typeof(KeyChain.KeychainMode)))
			{
				((ComboBox)e.OriginalSource).Items.Add(i);
			}
			((ComboBox)e.OriginalSource).SelectedIndex = (int)kc.Mode;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00012C58 File Offset: 0x00010E58
		private void customTypeCombobox_Loaded(object sender, RoutedEventArgs e)
		{
			KeychainViewModel kc = this.vm.GetKeychainById((int)((ComboBox)e.OriginalSource).Tag);
			if (kc.CustomTypeInitialized)
			{
				return;
			}
			kc.CustomTypeInitialized = true;
			foreach (string i in SB.Settings.Environment.GetCustomKeychainNames())
			{
				((ComboBox)e.OriginalSource).Items.Add(i);
			}
			if (((ComboBox)e.OriginalSource).Items.IndexOf(kc.CustomType) > 0)
			{
				((ComboBox)e.OriginalSource).SelectedValue = kc.CustomType;
				return;
			}
			((ComboBox)e.OriginalSource).Text = kc.CustomType;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00012D40 File Offset: 0x00010F40
		private void keychainTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.GetKeychainById((int)((ComboBox)e.OriginalSource).Tag).Type = (KeyChain.KeychainType)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00012D77 File Offset: 0x00010F77
		private void keychainModeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.GetKeychainById((int)((ComboBox)e.OriginalSource).Tag).Mode = (KeyChain.KeychainMode)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00012DAE File Offset: 0x00010FAE
		private void customTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.GetKeychainById((int)((ComboBox)e.OriginalSource).Tag).CustomType = (string)(sender as ComboBox).SelectedItem;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00012DE5 File Offset: 0x00010FE5
		private void removeKeychainImage_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.vm.RemoveKeychainById((int)((Image)e.OriginalSource).Tag);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00012E08 File Offset: 0x00011008
		private void conditionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			KeyFullId tag = (KeyFullId)((ComboBox)e.OriginalSource).Tag;
			this.vm.GetKeychainById(tag.ParentId).GetKeyById(tag.KeyId).Comparer = (Comparer)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00012E5C File Offset: 0x0001105C
		private void conditionCombobox_Loaded(object sender, RoutedEventArgs e)
		{
			KeyFullId tag = (KeyFullId)((ComboBox)e.OriginalSource).Tag;
			if (tag.ConditionInitialized)
			{
				return;
			}
			tag.ConditionInitialized = true;
			foreach (string c in Enum.GetNames(typeof(Comparer)))
			{
				((ComboBox)e.OriginalSource).Items.Add(c);
			}
			((ComboBox)e.OriginalSource).SelectedIndex = (int)this.vm.GetKeychainById(tag.ParentId).GetKeyById(tag.KeyId).Comparer;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00012EFC File Offset: 0x000110FC
		private void leftTermCombobox_Loaded(object sender, RoutedEventArgs e)
		{
			KeyFullId tag = (KeyFullId)((ComboBox)e.OriginalSource).Tag;
			if (tag.LeftTermInitialized)
			{
				return;
			}
			tag.LeftTermInitialized = true;
			foreach (string f in new string[] { "<SOURCE>", "<HEADERS(*)>", "<HEADERS{*}>", "<COOKIES(*)>", "<COOKIES{*}>", "<RESPONSECODE>", "<ADDRESS>" })
			{
				((ComboBox)e.OriginalSource).Items.Add(f);
			}
			try
			{
				((ComboBox)e.OriginalSource).SelectedValue = this.vm.GetKeychainById(tag.ParentId).GetKeyById(tag.KeyId).LeftTerm;
			}
			catch
			{
				((ComboBox)e.OriginalSource).SelectedIndex = 0;
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00012FF0 File Offset: 0x000111F0
		private void addKeyImage_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.vm.GetKeychainById((int)((Image)e.OriginalSource).Tag).AddKey();
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00013018 File Offset: 0x00011218
		private void removeKeyImage_MouseDown(object sender, MouseButtonEventArgs e)
		{
			KeyFullId tag = (KeyFullId)((Image)e.OriginalSource).Tag;
			this.vm.GetKeychainById(tag.ParentId).RemoveKeyById(tag.KeyId);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00013058 File Offset: 0x00011258
		private void customTypeCombobox_TextChanged(object sender, TextChangedEventArgs e)
		{
			ComboBox Combobox = sender as ComboBox;
			if (Combobox.Items.Count == 0)
			{
				return;
			}
			int id = (int)Combobox.Tag;
			this.vm.GetKeychainById(id).CustomType = Combobox.Text;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x000130A0 File Offset: 0x000112A0
		private void addKeychainImage_MouseEnter(object sender, MouseEventArgs e)
		{
			try
			{
				this.addKeychainIcon.Width = 16.5;
			}
			catch
			{
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x000130D8 File Offset: 0x000112D8
		private void addKeychainImage_MouseLeave(object sender, MouseEventArgs e)
		{
			try
			{
				this.addKeychainIcon.Width = 16.0;
			}
			catch
			{
			}
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x000131DC File Offset: 0x000113DC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 5:
				((Image)target).MouseDown += this.removeKeychainImage_MouseDown;
				return;
			case 6:
				((ComboBox)target).Loaded += this.keychainTypeCombobox_Loaded;
				((ComboBox)target).SelectionChanged += this.keychainTypeCombobox_SelectionChanged;
				return;
			case 7:
				((ComboBox)target).Loaded += this.keychainModeCombobox_Loaded;
				((ComboBox)target).SelectionChanged += this.keychainModeCombobox_SelectionChanged;
				return;
			case 8:
				((ComboBox)target).Loaded += this.customTypeCombobox_Loaded;
				((ComboBox)target).SelectionChanged += this.customTypeCombobox_SelectionChanged;
				((ComboBox)target).AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(this.customTypeCombobox_TextChanged));
				return;
			case 9:
				((Image)target).MouseDown += this.addKeyImage_MouseDown;
				return;
			case 10:
				((Image)target).MouseDown += this.removeKeyImage_MouseDown;
				return;
			case 11:
				((ComboBox)target).Loaded += this.leftTermCombobox_Loaded;
				return;
			case 12:
				((ComboBox)target).Loaded += this.conditionCombobox_Loaded;
				((ComboBox)target).SelectionChanged += this.conditionCombobox_SelectionChanged;
				return;
			default:
				return;
			}
		}

		// Token: 0x04000312 RID: 786
		public BlockKeycheckViewModel vm;

		// Token: 0x04000313 RID: 787
		private Random rand = new Random(1);
	}
}
