using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using OpenCvSharp;

namespace OpenBullet.Views.UserControls.Filters
{
	// Token: 0x02000082 RID: 130
	public partial class UserControlMorphology : UserControl
	{
		// Token: 0x06000339 RID: 825 RVA: 0x0000DA0C File Offset: 0x0000BC0C
		public UserControlMorphology()
		{
			this.InitializeComponent();
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600033A RID: 826 RVA: 0x0000DA1C File Offset: 0x0000BC1C
		// (remove) Token: 0x0600033B RID: 827 RVA: 0x0000DA54 File Offset: 0x0000BC54
		public event EventHandler SetFilter;

		// Token: 0x0600033C RID: 828 RVA: 0x0000DA8C File Offset: 0x0000BC8C
		private void UserControl_Initialized(object sender, EventArgs e)
		{
			try
			{
				Enum.GetNames(typeof(MorphTypes)).ToList<string>().ForEach(delegate(string m)
				{
					this.MorphMethodComboBox.Items.Add(m);
				});
			}
			catch
			{
			}
			try
			{
				Enum.GetNames(typeof(BorderTypes)).ToList<string>().ForEach(delegate(string c)
				{
					this.BorderTypeComboBox.Items.Add(c);
				});
			}
			catch
			{
			}
			try
			{
				this.MorphShapesComboBox.Items.Add("Null");
			}
			catch
			{
			}
			try
			{
				Enum.GetNames(typeof(MorphShapes)).ToList<string>().ForEach(delegate(string m)
				{
					this.MorphShapesComboBox.Items.Add(m);
				});
			}
			catch
			{
			}
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000DB6C File Offset: 0x0000BD6C
		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.MorphMethodComboBox.SelectedItem.ToString(),
				this.IterationsTextBox.Text,
				this.BorderTypeComboBox.SelectedItem.ToString(),
				this.MorphShapesComboBox.SelectedItem.ToString(),
				this.SizeWidthTextBox.Text,
				this.SizeHeightTextBox.Text
			}, new TextChangedEventArgs(e.RoutedEvent, UndoAction.None)
			{
				Source = "Morphology"
			});
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000DC0C File Offset: 0x0000BE0C
		private void IterationsTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			e.Source = "Morphology";
			EventHandler setFilter = this.SetFilter;
			if (setFilter == null)
			{
				return;
			}
			setFilter(new string[]
			{
				this.MorphMethodComboBox.SelectedItem.ToString(),
				this.IterationsTextBox.Text,
				this.BorderTypeComboBox.SelectedItem.ToString(),
				this.MorphShapesComboBox.SelectedItem.ToString(),
				this.SizeWidthTextBox.Text,
				this.SizeHeightTextBox.Text
			}, e);
		}

		// Token: 0x04000276 RID: 630
		public const string ControlName = "Morphology";
	}
}
