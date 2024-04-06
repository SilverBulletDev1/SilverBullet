using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OpenBullet.Views.UserControls.Filters
{
	// Token: 0x02000081 RID: 129
	public partial class UserControlInputTextAndEnum : UserControl
	{
		// Token: 0x0600032A RID: 810 RVA: 0x0000D753 File Offset: 0x0000B953
		public UserControlInputTextAndEnum()
		{
			this.InitializeComponent();
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600032B RID: 811 RVA: 0x0000D764 File Offset: 0x0000B964
		// (remove) Token: 0x0600032C RID: 812 RVA: 0x0000D79C File Offset: 0x0000B99C
		public event EventHandler SetFilter;

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0000D7D1 File Offset: 0x0000B9D1
		// (set) Token: 0x0600032E RID: 814 RVA: 0x0000D7D9 File Offset: 0x0000B9D9
		public bool Reverse { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000D7E2 File Offset: 0x0000B9E2
		// (set) Token: 0x06000330 RID: 816 RVA: 0x0000D7EA File Offset: 0x0000B9EA
		public string TEnumName { get; set; }

		// Token: 0x06000331 RID: 817 RVA: 0x0000D7F4 File Offset: 0x0000B9F4
		public void AddEnum<TEnum>()
		{
			this.EnumComboBox.Items.Clear();
			Type type = typeof(TEnum);
			Enum.GetNames(type).ToList<string>().ForEach(delegate(string e)
			{
				this.EnumComboBox.Items.Add(e);
			});
			this.TEnumName = type.Name;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00003B20 File Offset: 0x00001D20
		private void UserControl_Initialized(object sender, EventArgs e)
		{
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000D844 File Offset: 0x0000BA44
		private void EnumComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.EnumComboBox.Items.Count == 0)
			{
				return;
			}
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(this.GetInputs(), new TextChangedEventArgs(e.RoutedEvent, UndoAction.None)
			{
				Source = "InputTextAndEnum"
			});
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000D891 File Offset: 0x0000BA91
		private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			e.Source = "InputTextAndEnum";
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(this.GetInputs(), e);
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000D8B8 File Offset: 0x0000BAB8
		private string[] GetInputs()
		{
			if (this.Reverse)
			{
				return new string[]
				{
					this.EnumComboBox.SelectedItem.ToString(),
					this.InputTextBox.Text
				};
			}
			return new string[]
			{
				this.InputTextBox.Text,
				this.EnumComboBox.SelectedItem.ToString()
			};
		}

		// Token: 0x0400026D RID: 621
		public const string ControlName = "InputTextAndEnum";
	}
}
