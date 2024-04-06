using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.Win32;
using RuriLib;
using Xceed.Wpf.Toolkit;

namespace OpenBullet.Views.Main.Configs.OtherOptions
{
	// Token: 0x0200010F RID: 271
	public partial class Compile : Page
	{
		// Token: 0x06000744 RID: 1860 RVA: 0x00029D4C File Offset: 0x00027F4C
		public Compile()
		{
			this.InitializeComponent();
			this.vm = SB.ConfigManager.CurrentConfig.Config.Settings;
			base.DataContext = this.vm;
			this.vm.Title = Path.GetFileNameWithoutExtension(SB.MainWindow.ConfigsPage.CurrentConfig.FileName);
			this.vm.IconPath = "Icon\\svbfile.ico";
			this.compilerVersion.Content = "1.1";
			this.SetColors();
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00029DD8 File Offset: 0x00027FD8
		private void SetColors()
		{
			this.MessageColor.SelectedColor = new Color?(this.vm.MessageColor);
			this.AuthorColor.SelectedColor = new Color?(this.vm.AuthorColor);
			this.WordlistColor.SelectedColor = new Color?(this.vm.WordlistColor);
			this.BotsColor.SelectedColor = new Color?(this.vm.BotsColor);
			this.CustomInputColor.SelectedColor = new Color?(this.vm.CustomInputColor);
			this.CPMColor.SelectedColor = new Color?(this.vm.CPMColor);
			this.ProgressColor.SelectedColor = new Color?(this.vm.ProgressColor);
			this.HitsColor.SelectedColor = new Color?(this.vm.HitsColor);
			this.CustomInputColor.SelectedColor = new Color?(this.vm.CustomInputColor);
			this.ToCheckColor.SelectedColor = new Color?(this.vm.ToCheckColor);
			this.FailsColor.SelectedColor = new Color?(this.vm.FailsColor);
			this.RetriesColor.SelectedColor = new Color?(this.vm.RetriesColor);
			this.OcrRateColor.SelectedColor = new Color?(this.vm.OcrRateColor);
			this.ProxiesColor.SelectedColor = new Color?(this.vm.ProxiesColor);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00029F60 File Offset: 0x00028160
		private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
		{
			if (e.NewValue != null)
			{
				this.vm.GetType().GetProperty(((ColorPicker)sender).Name.ToString()).SetValue(this.vm, e.NewValue.Value, null);
			}
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00029FBC File Offset: 0x000281BC
		private void SelectIcon_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				OpenFileDialog dialog = new OpenFileDialog
				{
					Filter = "Icon | *.ico"
				};
				bool? flag = dialog.ShowDialog();
				bool flag2 = true;
				if (((flag.GetValueOrDefault() == flag2) & (flag != null)) && Path.GetExtension(dialog.FileName) == ".ico")
				{
					this.vm.IconPath = dialog.FileName;
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0002A034 File Offset: 0x00028234
		private void IconPath_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox textBox = sender as TextBox;
			if (File.Exists(textBox.Text) && Path.GetExtension(textBox.Text) == ".ico")
			{
				this.vm.IconPath = textBox.Text;
			}
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0002A07D File Offset: 0x0002827D
		private void Message_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.vm.Message = (sender as TextBox).Text;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0002A095 File Offset: 0x00028295
		private void HitInfoFormatTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.hitInfoFormatHint.Visibility = (((sender as TextBox).Text.Length == 0) ? Visibility.Visible : Visibility.Hidden);
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0002A0B8 File Offset: 0x000282B8
		private void SelectLicSource_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				OpenFileDialog dialog = new OpenFileDialog
				{
					Filter = "License Source (*.cs)|*.cs|License Source (*.txt)|*.txt"
				};
				bool? flag = dialog.ShowDialog();
				bool flag2 = true;
				if (((flag.GetValueOrDefault() == flag2) & (flag != null)) && Path.GetExtension(dialog.FileName) == ".cs")
				{
					this.vm.LicenseSource = dialog.FileName;
				}
			}
			catch
			{
			}
		}

		// Token: 0x040005FB RID: 1531
		private ConfigSettings vm;
	}
}
