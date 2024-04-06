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
	// Token: 0x0200007B RID: 123
	public partial class UserControlEnumBox : UserControl
	{
		// Token: 0x06000303 RID: 771 RVA: 0x0000CE2E File Offset: 0x0000B02E
		public UserControlEnumBox()
		{
			this.InitializeComponent();
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000304 RID: 772 RVA: 0x0000CE3C File Offset: 0x0000B03C
		// (remove) Token: 0x06000305 RID: 773 RVA: 0x0000CE74 File Offset: 0x0000B074
		public event EventHandler SetFilter;

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000CEA9 File Offset: 0x0000B0A9
		// (set) Token: 0x06000307 RID: 775 RVA: 0x0000CEB1 File Offset: 0x0000B0B1
		public string TEnumName { get; set; }

		// Token: 0x06000308 RID: 776 RVA: 0x0000CEBC File Offset: 0x0000B0BC
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

		// Token: 0x06000309 RID: 777 RVA: 0x00003B20 File Offset: 0x00001D20
		private void UserControl_Initialized(object sender, EventArgs e)
		{
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000CF0C File Offset: 0x0000B10C
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
			setFilter(new string[] { this.EnumComboBox.SelectedItem.ToString() }, new TextChangedEventArgs(e.RoutedEvent, UndoAction.None)
			{
				Source = "Enum"
			});
		}

		// Token: 0x0400024D RID: 589
		public const string ControlName = "Enum";
	}
}
