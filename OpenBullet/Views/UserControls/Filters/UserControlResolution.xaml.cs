using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using ImageProcessor.Imaging.MetaData;
using Xceed.Wpf.Toolkit;

namespace OpenBullet.Views.UserControls.Filters
{
	// Token: 0x02000086 RID: 134
	public partial class UserControlResolution : UserControl
	{
		// Token: 0x06000358 RID: 856 RVA: 0x0000E352 File Offset: 0x0000C552
		public UserControlResolution()
		{
			this.InitializeComponent();
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000359 RID: 857 RVA: 0x0000E360 File Offset: 0x0000C560
		// (remove) Token: 0x0600035A RID: 858 RVA: 0x0000E398 File Offset: 0x0000C598
		public event EventHandler SetFilter;

		// Token: 0x0600035B RID: 859 RVA: 0x0000E3D0 File Offset: 0x0000C5D0
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			e.Source = "Resolution";
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.HorizontalNumeric.Value.GetValueOrDefault(0).ToString(),
				this.VerticalNumeric.Value.GetValueOrDefault(0).ToString(),
				this.UnitComboBox.SelectedItem.ToString()
			}, e);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000E450 File Offset: 0x0000C650
		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.HorizontalNumeric.Value.GetValueOrDefault(0).ToString(),
				this.VerticalNumeric.Value.GetValueOrDefault(0).ToString(),
				this.UnitComboBox.SelectedItem.ToString()
			}, new TextChangedEventArgs(e.RoutedEvent, UndoAction.None)
			{
				Source = "Resolution"
			});
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000E4DC File Offset: 0x0000C6DC
		private void UserControl_Initialized(object sender, EventArgs e)
		{
			try
			{
				Enum.GetNames(typeof(PropertyTagResolutionUnit)).ToList<string>().ForEach(delegate(string p)
				{
					this.UnitComboBox.Items.Add(p);
				});
			}
			catch
			{
			}
		}

		// Token: 0x0400028F RID: 655
		public const string ControlName = "Resolution";
	}
}
