using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit;

namespace OpenBullet.Views.Main.Settings.OpenBullet
{
	// Token: 0x020000EC RID: 236
	public partial class Themes : Page
	{
		// Token: 0x06000608 RID: 1544 RVA: 0x0001F490 File Offset: 0x0001D690
		public Themes()
		{
			this.InitializeComponent();
			base.DataContext = SB.SBSettings.Themes;
			this.SetColors();
			this.SetColorPreviews();
			this.SetImagePreviews();
			SB.MainWindow.AllowsTransparency = SB.SBSettings.Themes.AllowTransparency;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001F4E4 File Offset: 0x0001D6E4
		public void SetColors()
		{
			this.SetAppColor("BackgroundMain", SB.SBSettings.Themes.BackgroundMain);
			this.SetAppColor("BackgroundSecondary", SB.SBSettings.Themes.BackgroundSecondary);
			this.SetAppColor("ForegroundMain", SB.SBSettings.Themes.ForegroundMain);
			this.SetAppColor("ForegroundGood", SB.SBSettings.Themes.ForegroundGood);
			this.SetAppColor("ForegroundBad", SB.SBSettings.Themes.ForegroundBad);
			this.SetAppColor("ForegroundCustom", SB.SBSettings.Themes.ForegroundCustom);
			this.SetAppColor("ForegroundRetry", SB.SBSettings.Themes.ForegroundRetry);
			this.SetAppColor("ForegroundToCheck", SB.SBSettings.Themes.ForegroundToCheck);
			this.SetAppColor("ForegroundMenuSelected", SB.SBSettings.Themes.ForegroundMenuSelected);
			this.SetAppColor("ForegroundOcrRate", SB.SBSettings.Themes.ForegroundOcrRate);
			SB.MainWindow.SetStyle();
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0001F600 File Offset: 0x0001D800
		private void SetColorPreviews()
		{
			this.BackgroundMain.SelectedColor = new Color?(this.GetAppColor("BackgroundMain"));
			this.BackgroundSecondary.SelectedColor = new Color?(this.GetAppColor("BackgroundSecondary"));
			this.ForegroundMain.SelectedColor = new Color?(this.GetAppColor("ForegroundMain"));
			this.ForegroundGood.SelectedColor = new Color?(this.GetAppColor("ForegroundGood"));
			this.ForegroundBad.SelectedColor = new Color?(this.GetAppColor("ForegroundBad"));
			this.ForegroundCustom.SelectedColor = new Color?(this.GetAppColor("ForegroundCustom"));
			this.ForegroundRetry.SelectedColor = new Color?(this.GetAppColor("ForegroundRetry"));
			this.ForegroundToCheck.SelectedColor = new Color?(this.GetAppColor("ForegroundToCheck"));
			this.ForegroundOcrRate.SelectedColor = new Color?(this.GetAppColor("ForegroundOcrRate"));
			this.ForegroundMenuSelected.SelectedColor = new Color?(this.GetAppColor("ForegroundMenuSelected"));
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0001F71B File Offset: 0x0001D91B
		public void SetAppColor(string resourceName, string color)
		{
			Application.Current.Resources[resourceName] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0001F73D File Offset: 0x0001D93D
		public Color GetAppColor(string resourceName)
		{
			return ((SolidColorBrush)Application.Current.Resources[resourceName]).Color;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0001F75C File Offset: 0x0001D95C
		private void SetImagePreviews()
		{
			try
			{
				this.backgroundImagePreview.Source = this.GetImageBrush(SB.SBSettings.Themes.BackgroundImage);
				this.backgroundLogoPreview.Source = this.GetImageBrush(SB.SBSettings.Themes.BackgroundLogo);
			}
			catch
			{
			}
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0001F7C0 File Offset: 0x0001D9C0
		private BitmapImage GetImageBrush(string file)
		{
			BitmapImage bitmapImage;
			try
			{
				if (File.Exists(file))
				{
					bitmapImage = new BitmapImage(new Uri(file));
				}
				else
				{
					bitmapImage = new BitmapImage(new Uri("pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/Images/Themes/empty.png", UriKind.Absolute));
				}
			}
			catch
			{
				bitmapImage = null;
			}
			return bitmapImage;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0001F828 File Offset: 0x0001DA28
		private void resetButton_Click(object sender, RoutedEventArgs e)
		{
			SB.SBSettings.Themes.BackgroundMain = "#222";
			SB.SBSettings.Themes.BackgroundSecondary = "#111";
			SB.SBSettings.Themes.ForegroundMain = "#dcdcdc";
			SB.SBSettings.Themes.ForegroundGood = "#adff2f";
			SB.SBSettings.Themes.ForegroundBad = "#ff6347";
			SB.SBSettings.Themes.ForegroundCustom = "#ff8c00";
			SB.SBSettings.Themes.ForegroundRetry = "#ffff00";
			SB.SBSettings.Themes.ForegroundToCheck = "#7fffd4";
			SB.SBSettings.Themes.ForegroundMenuSelected = "#1e90ff";
			SB.SBSettings.Themes.ForegroundOcrRate = "#ff8cc6ff";
			this.SetColors();
			this.SetColorPreviews();
			this.SetImagePreviews();
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0001F910 File Offset: 0x0001DB10
		private void loadBackgroundImage_MouseDown(object sender, MouseButtonEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";
			ofd.FilterIndex = 4;
			ofd.ShowDialog();
			SB.SBSettings.Themes.BackgroundImage = ofd.FileName;
			this.SetColors();
			this.SetImagePreviews();
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0001F960 File Offset: 0x0001DB60
		private void loadBackgroundLogo_MouseDown(object sender, MouseButtonEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";
			ofd.FilterIndex = 4;
			ofd.ShowDialog();
			SB.SBSettings.Themes.BackgroundLogo = ofd.FileName;
			this.SetColors();
			this.SetImagePreviews();
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0001F9B0 File Offset: 0x0001DBB0
		private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
		{
			if (e.NewValue != null)
			{
				SB.SBSettings.Themes.GetType().GetProperty(((ColorPicker)sender).Name.ToString()).SetValue(SB.SBSettings.Themes, this.ColorToHtml(e.NewValue.Value), null);
			}
			this.SetColors();
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0001FA1C File Offset: 0x0001DC1C
		private string ColorToHtml(Color color)
		{
			return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0001FA6F File Offset: 0x0001DC6F
		private void useImagesCheckbox_Checked(object sender, RoutedEventArgs e)
		{
			this.SetColors();
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0001FA6F File Offset: 0x0001DC6F
		private void useImagesCheckbox_Unchecked(object sender, RoutedEventArgs e)
		{
			this.SetColors();
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0001FA77 File Offset: 0x0001DC77
		private void backgroundImageOpacityUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			SB.MainWindow.SetStyle();
		}
	}
}
