using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using AngleSharp.Text;
using Extreme.Net;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.MetaData;
using Microsoft.Scripting.Utils;
using Microsoft.Win32;
using OpenBullet.Models;
using OpenBullet.Views.UserControls.Filters;
using RuriLib;
using RuriLib.Models;
using Tesseract;

namespace OpenBullet.Views.Main.Configs
{
	// Token: 0x020000F7 RID: 247
	public partial class ConfigOcrSettings : global::System.Windows.Controls.Page
	{
		// Token: 0x0600064D RID: 1613 RVA: 0x0002116C File Offset: 0x0001F36C
		public ConfigOcrSettings(bool sendFilter = false)
		{
			this.InitializeComponent();
			base.DataContext = SB.MainWindow.ConfigsPage.CurrentConfig.Config.Settings;
			if (sendFilter)
			{
				return;
			}
			this.blockOcr.Processors.ForEach(delegate(ValueTuple<string, string, Type> p)
			{
				this.filterBox.Items.Add(p.Item1);
			});
			this.OrigImage.SizeMode = PictureBoxSizeMode.Zoom;
			this.OrigImage.WaitOnLoad = true;
			this.ProcImage.SizeMode = PictureBoxSizeMode.Zoom;
			this.ProcImage.WaitOnLoad = true;
			this.LoadTessData();
			this.InitFilterControls();
			foreach (string i in Enum.GetNames(typeof(ProxyType)))
			{
				if (i != "Chain")
				{
					this.proxyTypeCombobox.Items.Add(i);
				}
			}
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00021254 File Offset: 0x0001F454
		private void LoadTessData()
		{
			try
			{
				if (!Directory.Exists(".\\tessdata"))
				{
					Directory.CreateDirectory(".\\tessdata");
				}
				foreach (FileInfo file in new DirectoryInfo(".\\tessdata").GetFiles("."))
				{
					if (file.Name.Contains(".") && !this.langBox.Items.Contains(file.Name.Split(new char[] { '.' })[0]))
					{
						this.langBox.Items.Add(file.Name.Split(new char[] { '.' })[0]);
					}
				}
				try
				{
					this.langBox.SelectedIndex = this.langBox.Items.IndexOf(this.blockOcr.OcrLang);
				}
				catch
				{
				}
			}
			catch (Exception)
			{
				global::System.Windows.Forms.MessageBox.Show("Missing folder \"tessdata\"! Please go make one and put your language files in it!", "NOTICE", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00021364 File Offset: 0x0001F564
		private void btnLoad_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(this.OcrUrl.Text))
				{
					CProxy proxy = null;
					if (this.chbProxy.IsChecked.GetValueOrDefault() && this.chbProxy.IsEnabled)
					{
						proxy = this.blockOcr.CreateProxy(this.proxyTextbox.Text, this.proxyTypeCombobox.SelectedItem.ToString().ToEnum(ProxyType.Http));
					}
					Bitmap bmp = this.blockOcr.GetOcrImage(false, proxy);
					this.OrigImage.Image = bmp;
					this.ProcImage.Image = bmp.Clone() as Bitmap;
					this.imageFromFile = false;
				}
			}
			catch (Exception ex)
			{
				SB.Logger.LogError(Components.OcrTesting, ex.Message, true, 0);
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0002143C File Offset: 0x0001F63C
		private void btnfilterClear_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (SB.Logger.Log(Components.OcrTesting, LogLevel.Warning, "Do you want to clear the list of filters?", true, 0, true) != MessageBoxResult.Cancel)
				{
					this.GetSettings().FilterList.Clear();
					this.scrollFilterTabControl.Visibility = Visibility.Collapsed;
					this.ProcImage.Image = (Bitmap)this.OrigImage.Image.Clone();
					this.SetFilters();
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x000214BC File Offset: 0x0001F6BC
		private void btnfilterRemove_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int selectedIndex = this.filterLB.SelectedIndex;
				if (selectedIndex > -1)
				{
					this.GetSettings().FilterList.RemoveAt(selectedIndex);
					if (this.filterLB.Items.Count == 0)
					{
						this.ProcImage.Image = (Bitmap)this.OrigImage.Image.Clone();
					}
					this.scrollFilterTabControl.Visibility = Visibility.Collapsed;
					this.SetFilters();
				}
			}
			catch
			{
				this.scrollFilterTabControl.Visibility = Visibility.Collapsed;
				this.SetFilters();
			}
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00003B20 File Offset: 0x00001D20
		private void chbAutoLoad_Click(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00021558 File Offset: 0x0001F758
		private void chbisBase64_Click(object sender, RoutedEventArgs e)
		{
			this.proxyTextbox.IsEnabled = !(this.blockOcr.Base64 = this.chbisBase64.IsChecked.GetValueOrDefault()) && this.chbProxy.IsEnabled;
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x000215A4 File Offset: 0x0001F7A4
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.OrigImage.Image != null)
				{
					Bitmap bmp = this.LoadBmp();
					IEnumerable<string> ocr = this.blockOcr.GetOcr(bmp, this.engineComboBox.SelectedItem.ToString().ToEnum(EngineMode.Default), this.pageSegComboBox.SelectedItem.ToString().ToEnum(PageSegMode.SingleLine), this.GetSettings().EvaluateMathOCR);
					this.ProcImage.Image = this.blockOcr.ProcessedImage;
					this.resultOcrTextbox.Text = string.Empty;
					ocr.ToList<string>().ForEach(delegate(string o)
					{
						global::System.Windows.Controls.TextBox textBox = this.resultOcrTextbox;
						textBox.Text = textBox.Text + o + "\n";
					});
					this.resultOcrTextbox.Text = this.resultOcrTextbox.Text.TrimEnd(new char[] { '\n' });
					this.ocrRateTextblock.Text = "OCR Rate: " + this.blockOcr.OcrRate.ToString() + "%";
				}
			}
			catch (Exception ex)
			{
				global::System.Windows.MessageBox.Show(ex.Message, "ERROR");
			}
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x000216C0 File Offset: 0x0001F8C0
		private Bitmap LoadBmp()
		{
			return (Bitmap)this.OrigImage.Image.Clone();
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x000216D8 File Offset: 0x0001F8D8
		private void OcrUrl_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.blockOcr.Url = this.OcrUrl.Text;
			if (this.chbAutoLoad.IsChecked.GetValueOrDefault())
			{
				this.btnLoad_Click(null, null);
			}
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00021718 File Offset: 0x0001F918
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
			{
				Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png | All files (*.*)|*.*"
			};
			bool? flag = dialog.ShowDialog();
			bool flag2 = true;
			if ((flag.GetValueOrDefault() == flag2) & (flag != null))
			{
				global::System.Drawing.Image img = global::System.Drawing.Image.FromFile(dialog.FileName);
				this.OrigImage.Image = img;
				this.ProcImage.Image = img;
				this.imageFromFile = true;
				this.path = dialog.FileName;
			}
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0002178C File Offset: 0x0001F98C
		private void btnRefresh_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				CProxy proxy = null;
				Bitmap bmp;
				if (!this.imageFromFile)
				{
					if (this.chbProxy.IsChecked.GetValueOrDefault() && this.chbProxy.IsEnabled)
					{
						proxy = this.blockOcr.CreateProxy(this.proxyTextbox.Text, this.proxyTypeCombobox.SelectedItem.ToString().ToEnum(ProxyType.Http));
					}
					bmp = this.blockOcr.GetOcrImage(false, proxy);
				}
				else
				{
					bmp = (Bitmap)global::System.Drawing.Image.FromFile(this.path);
				}
				this.OrigImage.Image = bmp;
				Bitmap proc = this.blockOcr.ApplyFilters(bmp.Clone() as Bitmap, null);
				this.ProcImage.Image = proc;
			}
			catch (Exception ex)
			{
				global::System.Windows.Forms.MessageBox.Show(ex.Message, "NOTICE", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0002186C File Offset: 0x0001FA6C
		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			try
			{
				this.GetSettings().FilterList.Add(this.filterBox.SelectedItem.ToString());
				this.filterLB.SelectedIndex = this.filterLB.Items.IndexOf(this.filterBox.SelectedItem.ToString());
				this.SetFilters();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x000218E0 File Offset: 0x0001FAE0
		private void SetFilters()
		{
			try
			{
				this.blockOcr.SetFilters(this.filterLB.Items.OfType<string>().ToArray<string>());
			}
			catch
			{
			}
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00021924 File Offset: 0x0001FB24
		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog
				{
					Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
				};
				bool? flag = dialog.ShowDialog();
				bool flag2 = true;
				if ((flag.GetValueOrDefault() == flag2) & (flag != null))
				{
					this.ProcImage.Image.Save(dialog.FileName);
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0002198C File Offset: 0x0001FB8C
		private void InitFilterControls()
		{
			try
			{
				Enum.GetNames(typeof(EngineMode)).ToList<string>().ForEach(delegate(string e)
				{
					this.engineComboBox.Items.Add(e);
				});
			}
			catch
			{
			}
			try
			{
				Enum.GetNames(typeof(PageSegMode)).ToList<string>().ForEach(delegate(string p)
				{
					this.pageSegComboBox.Items.Add(p);
				});
			}
			catch
			{
			}
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00021A0C File Offset: 0x0001FC0C
		private void filterLB_LostFocus(object sender, RoutedEventArgs e)
		{
			if (this.filterLB.Items.Count == 0)
			{
				return;
			}
			this.blockOcr.SetFilters(this.filterLB.Items.OfType<string>().ToArray<string>());
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00021A44 File Offset: 0x0001FC44
		private void filterLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				global::System.Windows.Controls.ListBox filterLB = sender as global::System.Windows.Controls.ListBox;
				int seletedIndex = filterLB.SelectedIndex;
				this.lastSelectedIndex = seletedIndex;
				if (seletedIndex != -1)
				{
					string selectedFilter = filterLB.Items[seletedIndex++].ToString();
					if (!selectedFilter.Contains(":"))
					{
						selectedFilter += ": ";
					}
					string text = selectedFilter.Split(new char[] { ':' })[0].Trim();
					if (seletedIndex > -1)
					{
						this.scrollFilterTabControl.Visibility = Visibility.Visible;
					}
					string text2 = text.ToLower();
					if (text2 != null)
					{
						switch (text2.Length)
						{
						case 3:
							if (!(text2 == "hue"))
							{
								goto IL_987;
							}
							this.SetInInputTextAndBoolean(seletedIndex - 1, "0", false, "Degrees", "Rotate (Any integer between 0 and 360)");
							goto IL_99F;
						case 4:
						{
							char c = text2[0];
							if (c <= 'm')
							{
								switch (c)
								{
								case 'b':
									if (!(text2 == "blur"))
									{
										goto IL_987;
									}
									this.SetSize(seletedIndex - 1);
									goto IL_99F;
								case 'c':
									if (!(text2 == "crop"))
									{
										goto IL_987;
									}
									this.SetCropLayer(seletedIndex - 1, new string[]
									{
										this.controlCropLayer.LeftTextBox.Text,
										this.controlCropLayer.TopTextBox.Text,
										this.controlCropLayer.RightTextBox.Text,
										this.controlCropLayer.BottomTextBox.Text,
										"Percentage"
									});
									goto IL_99F;
								case 'd':
									goto IL_987;
								case 'e':
									if (!(text2 == "edge"))
									{
										goto IL_987;
									}
									goto IL_736;
								default:
									if (c != 'm')
									{
										goto IL_987;
									}
									if (!(text2 == "mean"))
									{
										goto IL_987;
									}
									goto IL_66C;
								}
							}
							else if (c != 't')
							{
								if (c != 'z')
								{
									goto IL_987;
								}
								if (!(text2 == "zoom"))
								{
									goto IL_987;
								}
								this.SetInInputTextAndBoolean(seletedIndex - 1, "0", false, "Zoom Factor", "NearestNeighbor");
								goto IL_99F;
							}
							else
							{
								if (!(text2 == "tint"))
								{
									goto IL_987;
								}
								goto IL_6B0;
							}
							break;
						}
						case 5:
						{
							char c = text2[0];
							if (c != 'a')
							{
								if (c != 'g')
								{
									if (c != 's')
									{
										goto IL_987;
									}
									if (!(text2 == "scale"))
									{
										goto IL_987;
									}
									goto IL_638;
								}
								else
								{
									if (!(text2 == "gamma"))
									{
										goto IL_987;
									}
									goto IL_66C;
								}
							}
							else
							{
								if (!(text2 == "alpha"))
								{
									goto IL_987;
								}
								goto IL_638;
							}
							break;
						}
						case 6:
						{
							char c = text2[2];
							if (c <= 'o')
							{
								if (c != 'd')
								{
									if (c != 'o')
									{
										goto IL_987;
									}
									if (!(text2 == "smooth"))
									{
										goto IL_987;
									}
									goto IL_66C;
								}
								else
								{
									if (!(text2 == "median"))
									{
										goto IL_987;
									}
									this.SetInInput(seletedIndex - 1, new string[] { "0" }, "ksize", false);
									goto IL_99F;
								}
							}
							else if (c != 's')
							{
								if (c != 't')
								{
									goto IL_987;
								}
								if (!(text2 == "rotate"))
								{
									goto IL_987;
								}
								this.SetInInput(seletedIndex - 1, new string[] { "0" }, "Degrees", false);
								goto IL_99F;
							}
							else
							{
								if (!(text2 == "resize"))
								{
									goto IL_987;
								}
								goto IL_65C;
							}
							break;
						}
						case 7:
							if (!(text2 == "sharpen"))
							{
								goto IL_987;
							}
							goto IL_66C;
						case 8:
						{
							char c = text2[2];
							if (c <= 'l')
							{
								if (c != 'g')
								{
									if (c != 'l')
									{
										goto IL_987;
									}
									if (!(text2 == "halftone"))
									{
										goto IL_987;
									}
									this.SetInInputBoolean(seletedIndex - 1, "False", "Comic Mode");
									goto IL_99F;
								}
								else
								{
									if (!(text2 == "vignette"))
									{
										goto IL_987;
									}
									goto IL_6B0;
								}
							}
							else if (c != 'n')
							{
								if (c != 't')
								{
									if (c != 'x')
									{
										goto IL_987;
									}
									if (!(text2 == "pixelate"))
									{
										goto IL_987;
									}
									goto IL_65C;
								}
								else
								{
									if (!(text2 == "cvtcolor"))
									{
										goto IL_987;
									}
									this.SetControl(seletedIndex - 1, "CvtColor", new ControlText<global::System.Windows.Controls.TextBox>[]
									{
										new ControlText<global::System.Windows.Controls.TextBox>(this.controlCvtColor.dstCnTextBox, this.controlCvtColor.dstCnTextBox.Text)
									});
									goto IL_99F;
								}
							}
							else
							{
								if (!(text2 == "contrast"))
								{
									goto IL_987;
								}
								goto IL_638;
							}
							break;
						}
						case 9:
						{
							char c = text2[0];
							if (c <= 'c')
							{
								if (c != 'a')
								{
									if (c != 'c')
									{
										goto IL_987;
									}
									if (!(text2 == "constrain"))
									{
										goto IL_987;
									}
									this.SetSize(seletedIndex - 1);
									goto IL_99F;
								}
								else
								{
									if (!(text2 == "alignment"))
									{
										goto IL_987;
									}
									this.SetInInput(seletedIndex - 1, new string[] { "4" }, "Alignment size(must be a power of two)", false);
									goto IL_99F;
								}
							}
							else if (c != 's')
							{
								if (c != 't')
								{
									goto IL_987;
								}
								if (!(text2 == "threshold"))
								{
									goto IL_987;
								}
								this.SetThreshold(seletedIndex - 1, new string[] { "0", "255", "Binary" });
								goto IL_99F;
							}
							else
							{
								if (!(text2 == "sharpenex"))
								{
									goto IL_987;
								}
								goto IL_66C;
							}
							break;
						}
						case 10:
						{
							char c = text2[0];
							if (c <= 'c')
							{
								if (c != 'b')
								{
									if (c != 'c')
									{
										goto IL_987;
									}
									if (!(text2 == "contrastex"))
									{
										goto IL_987;
									}
									goto IL_638;
								}
								else
								{
									if (!(text2 == "brightness"))
									{
										goto IL_987;
									}
									goto IL_638;
								}
							}
							else if (c != 'm')
							{
								if (c != 'r')
								{
									if (c != 's')
									{
										goto IL_987;
									}
									if (!(text2 == "saturation"))
									{
										goto IL_987;
									}
									goto IL_638;
								}
								else
								{
									if (!(text2 == "resolution"))
									{
										goto IL_987;
									}
									this.SetResolution(seletedIndex - 1, new string[] { "0", "0", "Inch" });
									goto IL_99F;
								}
							}
							else
							{
								if (!(text2 == "morphology"))
								{
									goto IL_987;
								}
								this.SetMorphology(seletedIndex - 1);
								goto IL_99F;
							}
							break;
						}
						case 11:
							if (!(text2 == "entropycrop"))
							{
								goto IL_987;
							}
							break;
						case 12:
						{
							char c = text2[0];
							if (c != 'b')
							{
								if (c != 'g')
								{
									if (c != 'r')
									{
										goto IL_987;
									}
									if (!(text2 == "replacecolor"))
									{
										goto IL_987;
									}
									this.SetReplaceColor(seletedIndex - 1, new string[] { "0,0,0", "|", "0,0,0", "0" });
									goto IL_99F;
								}
								else
								{
									if (!(text2 == "gaussianblur"))
									{
										goto IL_987;
									}
									goto IL_6D4;
								}
							}
							else if (!(text2 == "binarization"))
							{
								goto IL_987;
							}
							break;
						}
						case 13:
						case 16:
						case 18:
						case 19:
						case 20:
						case 21:
						case 22:
						case 23:
						case 24:
						case 25:
						case 26:
							goto IL_987;
						case 14:
						{
							char c = text2[0];
							if (c != 'c')
							{
								if (c != 'r')
								{
									goto IL_987;
								}
								if (!(text2 == "roundedcorners"))
								{
									goto IL_987;
								}
								goto IL_736;
							}
							else
							{
								if (!(text2 == "colorthreshold"))
								{
									goto IL_987;
								}
								goto IL_66C;
							}
							break;
						}
						case 15:
						{
							char c = text2[0];
							if (c != 'b')
							{
								if (c != 'g')
								{
									goto IL_987;
								}
								if (!(text2 == "gaussiansharpen"))
								{
									goto IL_987;
								}
								goto IL_6D4;
							}
							else
							{
								if (!(text2 == "backgroundcolor"))
								{
									goto IL_987;
								}
								goto IL_6B0;
							}
							break;
						}
						case 17:
							if (!(text2 == "adaptivethreshold"))
							{
								goto IL_987;
							}
							this.SetAdaptiveThreshold(seletedIndex - 1, new string[] { "1", "MeanC", "Binary", "1", "1" });
							goto IL_99F;
						case 27:
							if (!(text2 == "fastnlmeansdenoisingcolored"))
							{
								goto IL_987;
							}
							this.SetFastNlMeansDenoisingColored(seletedIndex - 1, new string[] { "3", "3" });
							goto IL_99F;
						default:
							goto IL_987;
						}
						this.SetInInput(seletedIndex - 1, new string[] { "0" }, "Threshold", false);
						goto IL_99F;
						IL_638:
						this.SetInInput(seletedIndex - 1, new string[] { "0" }, "Percentage", false);
						goto IL_99F;
						IL_65C:
						this.SetSize(seletedIndex - 1);
						goto IL_99F;
						IL_66C:
						this.SetInInput(seletedIndex - 1, new string[] { "0" }, "Value", false);
						goto IL_99F;
						IL_6B0:
						this.SetInInput(seletedIndex - 1, new string[] { "0,0,0" }, "Color(R,G,B)", true);
						goto IL_99F;
						IL_6D4:
						this.SetInInput(seletedIndex - 1, new string[] { "0" }, "Size", false);
						goto IL_99F;
						IL_736:
						this.SetInInput(seletedIndex - 1, new string[] { "0" }, "Radius", false);
						goto IL_99F;
					}
					IL_987:
					this.filterTabControl.SelectedIndex = -1;
					this.scrollFilterTabControl.Visibility = Visibility.Collapsed;
					IL_99F:;
				}
			}
			catch (IndexOutOfRangeException)
			{
			}
			catch (ArgumentOutOfRangeException)
			{
			}
			catch (ArgumentException)
			{
			}
			catch (Exception ex)
			{
				global::System.Windows.MessageBox.Show(ex.Message, "ERROR");
			}
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00022474 File Offset: 0x00020674
		private void SetInInput(int index, string[] defValues, string label = "Value", bool color = false)
		{
			this.inputControl.SetInputType(UserControlInput.InputType.Text);
			this.inputControl.label.Content = label + ":";
			this.filterTabControl.SelectIndexByHeaderName("Input");
			string input = string.Empty;
			if (color)
			{
				string[] colors = this.GetFilterColors(index, defValues);
				input = string.Concat(new string[]
				{
					colors[0],
					",",
					colors[1],
					",",
					colors[2]
				});
			}
			else
			{
				input = this.GetFilterValue(index, defValues, 0);
			}
			if (this.inputControl.InputTextBox.Text != input)
			{
				this.inputControl.InputTextBox.Text = input;
				this.SetCaretIndexAndSelect(this.inputControl.InputTextBox, 1);
			}
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00022544 File Offset: 0x00020744
		private void SetEnum<TEnum>(int index, string defValue, string label = "Select")
		{
			if (this.controlEnumBox.EnumComboBox.Items.Count == 0 || this.controlEnumBox.TEnumName != typeof(TEnum).Name)
			{
				this.controlEnumBox.AddEnum<TEnum>();
			}
			this.filterTabControl.SelectIndexByHeaderName("Enum");
			this.controlEnumBox.label.Content = label + ":";
			string @enum = this.GetFilterValue(index, new string[] { defValue }, 0);
			this.controlEnumBox.EnumComboBox.SelectedItem = @enum;
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x000225E4 File Offset: 0x000207E4
		private void SetSize(int index)
		{
			this.filterTabControl.SelectIndexByHeaderName("Resize");
			string width = this.GetFilterValues(index, new string[] { "0", "0" }, ',')[0];
			string height = this.GetFilterValues(index, new string[] { "0", "0" }, ',')[1];
			if (!this.resizeControl.WidthTextBox.Text.Equals(width))
			{
				this.resizeControl.WidthTextBox.Text = width;
			}
			if (!this.resizeControl.HeightTextBox.Text.Equals(height))
			{
				this.resizeControl.HeightTextBox.Text = height;
			}
			this.SetCaretIndexAndSelect(this.resizeControl.WidthTextBox, 1);
			this.SetCaretIndexAndSelect(this.resizeControl.HeightTextBox, 1);
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x000226BC File Offset: 0x000208BC
		private void SetInputTextAndEnum<TEnum>(int index, string[] defValue, string labelInput = "Input", string labelSelect = "Select", bool reverse = false)
		{
			if (this.controlInputTextAndEnum.EnumComboBox.Items.Count == 0 || this.controlInputTextAndEnum.TEnumName != typeof(TEnum).Name)
			{
				this.controlInputTextAndEnum.AddEnum<TEnum>();
			}
			this.filterTabControl.SelectIndexByHeaderName("InputTextAndEnum");
			this.controlInputTextAndEnum.Reverse = reverse;
			this.controlInputTextAndEnum.labelInput.Content = labelInput + ":";
			this.controlInputTextAndEnum.labelSelect.Content = labelSelect + ":";
			string input = this.GetFilterValues(index, defValue, ',')[reverse ? 1 : 0];
			string @enum = this.GetFilterValues(index, defValue, ',')[(!reverse) ? 1 : 0];
			this.controlInputTextAndEnum.EnumComboBox.SelectedItem = @enum;
			this.SetTextInTextBox(this.controlInputTextAndEnum.InputTextBox, input);
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x000227AC File Offset: 0x000209AC
		private void SetInInputBoolean(int index, string defValue, string label = "Value")
		{
			this.inputControl.SetInputType(UserControlInput.InputType.Boolean);
			this.inputControl.label.Content = label + ":";
			this.filterTabControl.SelectIndexByHeaderName("Input");
			this.inputControl.InputComboBox.SelectedIndex = (this.GetFilterValue(index, new string[] { defValue }, 0).ToBoolean(false) ? 1 : 0);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00022820 File Offset: 0x00020A20
		private void SetInInputTextAndBoolean(int index, string defValue, bool defBoolean, string labelVal = "Value", string labelBool = "Grayscale")
		{
			this.controlInputTextAndBool.label.Content = labelVal + ":";
			this.controlInputTextAndBool.CheckBox.Content = labelBool;
			this.filterTabControl.SelectIndexByHeaderName("InputTextAndBoolean");
			string inputText = this.GetFilterValues(index, new string[]
			{
				defValue,
				defBoolean.ToString()
			}, ',')[0];
			this.SetTextInTextBox(this.controlInputTextAndBool.InputTextBox, inputText);
			this.controlInputTextAndBool.CheckBox.IsChecked = new bool?(this.GetFilterValues(index, new string[]
			{
				defValue,
				defBoolean.ToString()
			}, ',')[1].ToBoolean(false));
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x000228D8 File Offset: 0x00020AD8
		private void SetBlur(int index, string[] defValues)
		{
			this.filterTabControl.SelectIndexByHeaderName("Blur");
			string radius = this.GetFilterValues(index, defValues, ',')[0];
			string sigma = this.GetFilterValues(index, defValues, ',')[1];
			string channels = this.GetFilterValues(index, defValues, ',')[2];
			this.SetTextInTextBox(this.blurControl.RadiusTextBox, radius);
			this.SetTextInTextBox(this.blurControl.SigmaTextBox, sigma);
			this.blurControl.ChannelsComboBox.SelectedItem = channels;
			this.SetCaretIndexAndSelect(this.blurControl.RadiusTextBox, 1);
			this.SetCaretIndexAndSelect(this.blurControl.SigmaTextBox, 1);
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00022978 File Offset: 0x00020B78
		private void SetThreshold(int index, string[] defValues)
		{
			this.filterTabControl.SelectIndexByHeaderName("Threshold");
			string thresold = this.GetFilterValues(index, defValues, ',')[0];
			string maxVal = this.GetFilterValues(index, defValues, ',')[1];
			string thresholdType = this.GetFilterValues(index, defValues, ',')[2];
			this.SetTextInTextBox(this.controlThreshold.ThreshTextBox, thresold);
			this.SetTextInTextBox(this.controlThreshold.MaxValueTextBox, maxVal);
			this.controlThreshold.ThresholdTypeComboBox.SelectedItem = thresholdType;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x000229F4 File Offset: 0x00020BF4
		private void SetAdaptiveThreshold(int index, string[] defValues)
		{
			this.filterTabControl.SelectIndexByHeaderName("AdaptiveThreshold");
			string maxValue = this.GetFilterValues(index, defValues, ',')[0];
			string adaptiveMethod = this.GetFilterValues(index, defValues, ',')[1];
			string thresholdType = this.GetFilterValues(index, defValues, ',')[2];
			string blockSize = this.GetFilterValues(index, defValues, ',')[3];
			string constant = this.GetFilterValues(index, defValues, ',')[4];
			this.SetTextInTextBox(this.controlAdaptiveThreshold.MaxValueTextBox, maxValue);
			try
			{
				this.controlAdaptiveThreshold.AdaptiveMethodComboBox.SelectedItem = adaptiveMethod;
			}
			catch
			{
			}
			try
			{
				this.controlAdaptiveThreshold.ThresholdTypeComboBox.SelectedItem = thresholdType;
			}
			catch
			{
			}
			this.SetTextInTextBox(this.controlAdaptiveThreshold.BlockSizeTextBox, blockSize);
			this.SetTextInTextBox(this.controlAdaptiveThreshold.ConstantTextBox, constant);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00022AD4 File Offset: 0x00020CD4
		private void SetFastNlMeansDenoisingColored(int index, string[] defValues)
		{
			this.filterTabControl.SelectIndexByHeaderName("FastNlMeansDenoisingColored");
			string strength = this.GetFilterValues(index, defValues, ',')[0];
			string colorStrength = this.GetFilterValues(index, defValues, ',')[1];
			this.SetTextInTextBox(this.controlFastNlMeansDenoisingColored.StrengthTextBox, strength);
			this.SetTextInTextBox(this.controlFastNlMeansDenoisingColored.ColorStrengthTextBox, colorStrength);
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00022B30 File Offset: 0x00020D30
		private void SetReplaceColor(int index, string[] defValues)
		{
			this.filterTabControl.SelectIndexByHeaderName("ReplaceColor");
			if (!string.IsNullOrWhiteSpace(this.controlReplaceColor.TargetTextBox.Text) && !string.IsNullOrWhiteSpace(this.controlReplaceColor.ReplacementTextBox.Text))
			{
				try
				{
					string value = this.filterLB.Items[index].ToString();
					if (!value.Contains(":"))
					{
						value += ": ";
					}
					string[] val = value.Split(new char[] { ':' }, 2)[1].Trim().Split(new char[] { ',' });
					defValues = new string[]
					{
						val[0],
						val[1],
						val[2],
						"|",
						val[3],
						val[4],
						val[5],
						val[6]
					};
				}
				catch
				{
				}
			}
			string colorTarget = string.Concat(new string[]
			{
				defValues[0],
				",",
				defValues[1],
				",",
				defValues[2]
			});
			string colorFill = string.Concat(new string[]
			{
				defValues[4],
				",",
				defValues[5],
				",",
				defValues[6]
			});
			this.SetTextInTextBox(this.controlReplaceColor.TargetTextBox, colorTarget);
			this.SetTextInTextBox(this.controlReplaceColor.ReplacementTextBox, colorFill);
			this.SetTextInTextBox(this.controlReplaceColor.FuzzinessTextBox, defValues[7]);
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00022CC0 File Offset: 0x00020EC0
		private void SetCropLayer(int index, string[] defValues)
		{
			this.filterTabControl.SelectIndexByHeaderName("CropLayer");
			string value = this.filterLB.SelectedItem.ToString();
			CropMode cropMode = CropMode.Percentage;
			try
			{
				cropMode = this.GetFilterValues(index, value.Split(new char[] { ',' }), ',')[4].ToEnum(CropMode.Percentage);
			}
			catch
			{
			}
			string[] filterValues = this.GetFilterValues(index, defValues, ',');
			string left = filterValues[0];
			string top = filterValues[1];
			string right = filterValues[2];
			string bottom = filterValues[3];
			this.controlCropLayer.CropModeBox.SelectedItem = cropMode;
			this.SetTextInTextBox(this.controlCropLayer.LeftTextBox, left);
			this.SetTextInTextBox(this.controlCropLayer.TopTextBox, top);
			this.SetTextInTextBox(this.controlCropLayer.RightTextBox, right);
			this.SetTextInTextBox(this.controlCropLayer.BottomTextBox, bottom);
			this.controlCropLayer.CropModeBox.SelectedItem = cropMode;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00022DBC File Offset: 0x00020FBC
		private void SetMorphology(int index)
		{
			this.filterTabControl.SelectIndexByHeaderName("Morphology");
			string method = this.GetFilterValues(index, new string[] { "Erode" }, ',')[0];
			string iterations = this.GetFilterValues(index, new string[] { "Erode", "1", "Constant" }, ',')[1];
			string borderType = this.GetFilterValues(index, new string[] { "Erode", "1", "Constant" }, ',')[2];
			string mrophShapes = this.GetFilterValues(index, new string[] { "Erode", "1", "Constant", "Null" }, ',')[3];
			string sizeWidthKernel = this.GetFilterValues(index, new string[] { "Erode", "1", "Constant", "Null", "Null" }, ',')[4];
			string sizeHeightKernel = this.GetFilterValues(index, new string[] { "Erode", "1", "Constant", "Null", "Null", "Null" }, ',')[5];
			try
			{
				this.controlMorphology.MorphMethodComboBox.SelectedItem = method;
			}
			catch
			{
			}
			try
			{
				this.controlMorphology.BorderTypeComboBox.SelectedItem = borderType;
			}
			catch
			{
			}
			try
			{
				this.controlMorphology.MorphShapesComboBox.SelectedItem = mrophShapes;
			}
			catch
			{
			}
			this.SetTextInTextBox(this.controlMorphology.IterationsTextBox, iterations);
			this.SetTextInTextBox(this.controlMorphology.SizeWidthTextBox, sizeWidthKernel);
			this.SetTextInTextBox(this.controlMorphology.SizeHeightTextBox, sizeHeightKernel);
			this.SetCaretIndexAndSelect(this.controlMorphology.IterationsTextBox, 1);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00022FB0 File Offset: 0x000211B0
		private void SetResolution(int index, string[] defValues)
		{
			this.filterTabControl.SelectIndexByHeaderName("Resolution");
			string hori = this.GetFilterValues(index, defValues, ',')[0];
			string verti = this.GetFilterValues(index, defValues, ',')[1];
			string unit = this.GetFilterValues(index, defValues, ',')[2];
			this.controlResolution.HorizontalNumeric.Value = new int?(int.Parse(hori));
			this.controlResolution.VerticalNumeric.Value = new int?(int.Parse(verti));
			this.controlResolution.UnitComboBox.SelectedItem = unit.ToEnum(PropertyTagResolutionUnit.Inch);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00023048 File Offset: 0x00021248
		private void SetControl(int index, string controlName, ControlText<global::System.Windows.Controls.TextBox>[] controls = null)
		{
			this.filterTabControl.SelectIndexByHeaderName(controlName);
			int c = 0;
			for (;;)
			{
				int num = c;
				int? num2 = ((controls != null) ? new int?(controls.Length) : null);
				if (!((num < num2.GetValueOrDefault()) & (num2 != null)))
				{
					break;
				}
				this.SetTextInTextBox(controls[c].Control, controls[c].Text);
				c++;
			}
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00010553 File Offset: 0x0000E753
		private void SetTextInTextBox(global::System.Windows.Controls.TextBox textBox, string text)
		{
			if (textBox.Text != text)
			{
				textBox.Text = text;
			}
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x000230AC File Offset: 0x000212AC
		private void SetFilter(int index, string[] values)
		{
			try
			{
				string selectedFilter = this.filterLB.Items[index].ToString();
				if (!selectedFilter.Contains(":"))
				{
					selectedFilter += ":";
				}
				string[] split = selectedFilter.Split(new char[] { ':' }, 2);
				string value = split[1];
				if (string.IsNullOrWhiteSpace(split[1]) && values.Length != 0)
				{
					for (int i = 0; i < values.Length; i++)
					{
						value = value + values[i] + ",";
					}
					value = value.Trim().TrimEnd(new char[] { ',' });
				}
				else if (values.Length != 0)
				{
					value = string.Empty;
					for (int j = 0; j < values.Length; j++)
					{
						value = value + values[j] + ",";
					}
					value = value.Trim().TrimEnd(new char[] { ',' });
				}
				this.GetSettings().FilterList[index] = split[0] + ": " + value;
				this.filterLB.SelectedIndex = index;
			}
			catch
			{
			}
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x000231C8 File Offset: 0x000213C8
		private string GetFilterValue(int index, string[] defaultValues, int parameterCount = 0)
		{
			string value = this.filterLB.Items[index].ToString();
			if (!value.Contains(":"))
			{
				value += ": ";
			}
			string val = value.Split(new char[] { ':' }, 2)[1].Trim();
			if (parameterCount > 0 && val.Split(new char[] { ',' }, parameterCount, StringSplitOptions.RemoveEmptyEntries).Length < parameterCount)
			{
				foreach (string defVal in defaultValues)
				{
					if (!val.EndsWith(","))
					{
						val += ",";
					}
					val = val + defVal + ",";
				}
			}
			if (!string.IsNullOrWhiteSpace(val))
			{
				return val.Trim().TrimEnd(new char[] { ',' });
			}
			foreach (string defValue in defaultValues)
			{
				val = val + defValue + ",";
			}
			return val.Trim().TrimEnd(new char[] { ',' });
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x000232D0 File Offset: 0x000214D0
		private string[] GetFilterValues(int index, string[] defaultValues, char split = ',')
		{
			string value = this.filterLB.Items[index].ToString();
			if (!value.Contains(":"))
			{
				value += ": ";
			}
			string val = value.Split(new char[] { ':' }, 2)[1].Trim();
			if (!string.IsNullOrWhiteSpace(val))
			{
				string[] vSplit = val.Split(new char[] { split });
				string val2 = string.Empty;
				for (int i = 0; i < defaultValues.Length; i++)
				{
					try
					{
						if (string.IsNullOrEmpty(vSplit[i]))
						{
							val2 = val2 + defaultValues[i] + split.ToString();
						}
						else
						{
							val2 = val2 + vSplit[i] + split.ToString();
						}
					}
					catch
					{
						val2 = val2 + defaultValues[i] + split.ToString();
					}
				}
				return ((val2 == "") ? val : val2).Trim().TrimEnd(new char[] { split }).Split(new char[] { split });
			}
			defaultValues.ToList<string>().ForEach(delegate(string dv)
			{
				val = val + dv + split.ToString();
			});
			return val.Trim().TrimEnd(new char[] { split }).Split(new char[] { split });
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00023470 File Offset: 0x00021670
		private string[] GetFilterColors(int index, string[] defaultValues)
		{
			string value = this.filterLB.Items[index].ToString();
			if (!value.Contains(":"))
			{
				value += ": ";
			}
			string[] val = value.Split(new char[] { ':' }, 2)[1].Trim().Split(new char[] { ',' });
			return new string[]
			{
				val[0],
				val[1],
				val[2],
				"|",
				val[3],
				val[4],
				val[5],
				val[6]
			};
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00023510 File Offset: 0x00021710
		private void SetCaretIndexAndSelect(global::System.Windows.Controls.TextBox textBox, int index = 1)
		{
			try
			{
				if (textBox.Text == "0")
				{
					textBox.CaretIndex = index;
					textBox.SelectAll();
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00023554 File Offset: 0x00021754
		private void inputControl_SetFilter(object sender, EventArgs e)
		{
			if (this.filterTabControl.SelectedIndex != this.filterTabControl.GetIndexByItemName((e as TextChangedEventArgs).Source.ToString()))
			{
				return;
			}
			this.SetFilter(this.lastSelectedIndex, sender as string[]);
			this.filterLB_LostFocus(null, null);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x000235A4 File Offset: 0x000217A4
		private void btnfilterUp_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int selectedIndex = this.filterLB.SelectedIndex;
				if (selectedIndex > 0)
				{
					string itemToMoveUp = this.filterLB.Items[selectedIndex].ToString();
					ConfigSettings settings = this.GetSettings();
					settings.FilterList.RemoveAt(selectedIndex);
					settings.FilterList.Insert(selectedIndex - 1, itemToMoveUp);
					this.filterLB.SelectedIndex = selectedIndex - 1;
					this.SetFilters();
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00023624 File Offset: 0x00021824
		private void btnfilterDown_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int selectedIndex = this.filterLB.SelectedIndex;
				if (selectedIndex + 1 < this.filterLB.Items.Count)
				{
					string itemToMoveDown = this.filterLB.Items[selectedIndex].ToString();
					ConfigSettings settings = this.GetSettings();
					settings.FilterList.RemoveAt(selectedIndex);
					settings.FilterList.Insert(selectedIndex + 1, itemToMoveDown);
					this.filterLB.SelectedIndex = selectedIndex + 1;
					this.filterLB_LostFocus(null, null);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x000236B4 File Offset: 0x000218B4
		private void btnfilterClone_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.filterLB.Items.Count != 0 && this.lastSelectedIndex != -1)
				{
					this.GetSettings().FilterList.Add(this.filterLB.SelectedItem.ToString());
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00023714 File Offset: 0x00021914
		private void btnApplyFilters_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.OrigImage.Image != null)
				{
					Bitmap proc = this.blockOcr.ApplyFilters(this.OrigImage.Image.Clone() as Bitmap, null);
					this.ProcImage.Image = proc;
				}
			}
			catch (Exception ex)
			{
				string[] array = new string[5];
				array[0] = ex.Message;
				array[1] = "\nInnerException: ";
				int num = 2;
				Exception innerException = ex.InnerException;
				array[num] = ((innerException != null) ? innerException.Message : null);
				array[3] = "\n";
				int num2 = 4;
				Exception innerException2 = ex.InnerException;
				string text;
				if (innerException2 == null)
				{
					text = null;
				}
				else
				{
					Exception innerException3 = innerException2.InnerException;
					text = ((innerException3 != null) ? innerException3.Message : null);
				}
				array[num2] = text;
				global::System.Windows.Forms.MessageBox.Show(string.Concat(array), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x000237E0 File Offset: 0x000219E0
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.filterLB.Items.Count != 0)
				{
					Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog
					{
						Filter = "Text|*.txt"
					};
					bool? flag = dialog.ShowDialog();
					bool flag2 = true;
					if ((flag.GetValueOrDefault() == flag2) & (flag != null))
					{
						using (StreamWriter SaveFile = new StreamWriter(dialog.FileName))
						{
							foreach (object item in ((IEnumerable)this.filterLB.Items))
							{
								SaveFile.WriteLine(item.ToString());
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x000238C4 File Offset: 0x00021AC4
		private void MenuItem1_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.filterLB.Items.Count != 0)
				{
					string filters = string.Empty;
					foreach (object item in this.filterLB.SelectedItems)
					{
						filters += ((item != null) ? item.ToString() : null);
						if (!item.Equals(this.filterLB.SelectedItems.OfType<string>().Last<string>()))
						{
							filters += "\n";
						}
					}
					global::System.Windows.Clipboard.SetText(filters);
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00023988 File Offset: 0x00021B88
		private void MenuItem_Click_1(object sender, RoutedEventArgs e)
		{
			try
			{
				Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
				{
					Filter = "Text|*.txt"
				};
				bool? flag = dialog.ShowDialog();
				bool flag2 = true;
				if ((flag.GetValueOrDefault() == flag2) & (flag != null))
				{
					string[] filters = File.ReadAllLines(dialog.FileName);
					this.GetSettings().FilterList.AddRange(filters);
					if (filters.Length != 0)
					{
						this.filterLB_LostFocus(null, null);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00023A04 File Offset: 0x00021C04
		private void sizeModeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				if (this.ProcImage != null)
				{
					this.ProcImage.SizeMode = (PictureBoxSizeMode)Enum.Parse(typeof(PictureBoxSizeMode), (this.sizeModeBox.SelectedItem as ComboBoxItem).Content.ToString(), true);
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00023A68 File Offset: 0x00021C68
		private ConfigSettings GetSettings()
		{
			return base.DataContext as ConfigSettings;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00023A78 File Offset: 0x00021C78
		private void MenuItem_Click_2(object sender, RoutedEventArgs e)
		{
			try
			{
				ConfigSettings settings = this.GetSettings();
				int selectedIndex = this.filterLB.SelectedIndex;
				string item = settings.FilterList[selectedIndex];
				if (!item.StartsWith("!"))
				{
					item = "!" + item;
					settings.FilterList[selectedIndex] = item;
					this.filterTabControl.SelectedIndex = -1;
					this.scrollFilterTabControl.Visibility = Visibility.Collapsed;
					this.filterLB_LostFocus(null, null);
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00023B04 File Offset: 0x00021D04
		private void MenuItem_Click_3(object sender, RoutedEventArgs e)
		{
			try
			{
				ConfigSettings settings = this.GetSettings();
				int selectedIndex = this.filterLB.SelectedIndex;
				string item = settings.FilterList[selectedIndex];
				if (item.StartsWith("!"))
				{
					settings.FilterList[selectedIndex] = item.Remove(0, 1);
					this.filterLB_LostFocus(null, null);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00023B70 File Offset: 0x00021D70
		private void langBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				this.blockOcr.OcrLang = this.langBox.SelectedItem.ToString();
			}
			catch
			{
			}
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00023BB0 File Offset: 0x00021DB0
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.GetSettings().FilterList.Count > 0 && this.blockOcr.Filters.Count == 0)
				{
					this.filterLB_LostFocus(null, null);
				}
			}
			catch
			{
			}
			try
			{
				this.chbisBase64_Click(null, null);
			}
			catch
			{
			}
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00023C1C File Offset: 0x00021E1C
		private void MenuItem_Click_4(object sender, RoutedEventArgs e)
		{
			try
			{
				try
				{
					string item = global::System.Windows.Clipboard.GetText();
					string item2 = item;
					if (item.Contains(":"))
					{
						item2 = item.Split(new char[] { ':' })[0].Trim();
					}
					if (this.filterBox.Items.Contains(item2))
					{
						this.GetSettings().FilterList.Add(item);
					}
				}
				catch
				{
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00023CA4 File Offset: 0x00021EA4
		private void ProcImage_MouseDoubleClick(object sender, global::System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				if (this.ProcImage.Image != null)
				{
					Color color = this.GetPixelInfo(e.X, e.Y);
					global::System.Windows.Clipboard.SetText(string.Concat(new string[]
					{
						color.R.ToString(),
						",",
						color.G.ToString(),
						",",
						color.B.ToString()
					}));
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00023D40 File Offset: 0x00021F40
		private void ProcImage_MouseMove(object sender, global::System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				if (this.ProcImage.Image != null && !this.clicked)
				{
					Color color = this.GetPixelInfo(e.X, e.Y);
					this.pixelInfo.Text = string.Concat(new string[]
					{
						e.X.ToString(),
						",",
						e.Y.ToString(),
						": RGBA(",
						color.R.ToString(),
						"-",
						color.G.ToString(),
						"-",
						color.B.ToString(),
						"-",
						color.A.ToString(),
						"),Saturation(",
						color.GetSaturation().ToString(),
						"),Brightness(",
						color.GetBrightness().ToString(),
						")"
					});
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00023E88 File Offset: 0x00022088
		private Color GetPixelInfo(int x, int y)
		{
			Color color;
			try
			{
				Bitmap img = (Bitmap)this.ProcImage.Image;
				float factorX = (float)this.ProcImage.Width / (float)img.Width;
				float factorY = (float)this.ProcImage.Height / (float)img.Height;
				color = img.GetPixel((int)((float)x / factorX), (int)((float)y / factorY));
			}
			catch (Exception ex)
			{
				if (!ex.Message.Contains("Parameter must be positive and < Height"))
				{
					SB.Logger.Log(ex.Message, LogLevel.Error, true, 0);
				}
				color = Color.Transparent;
			}
			return color;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00023F28 File Offset: 0x00022128
		private void pixelInfo_MouseDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				if (e.ClickCount == 2)
				{
					global::System.Windows.Clipboard.SetText(this.pixelInfo.Text);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00023F68 File Offset: 0x00022168
		private void ProcImage_MouseDown(object sender, global::System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				this.clicked = true;
			}
			catch
			{
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00023F94 File Offset: 0x00022194
		private void ProcImage_MouseLeave(object sender, EventArgs e)
		{
			try
			{
				this.clicked = false;
			}
			catch
			{
			}
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00023FC0 File Offset: 0x000221C0
		private void filterLB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			try
			{
				if (e.ChangedButton == MouseButton.Left && this.filterLB.Items.Count > 0 && this.filterLB.SelectedIndex > -1)
				{
					this.filterLB_SelectionChanged(this.filterLB, null);
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000541 RID: 1345
		private BlockOcr blockOcr = new BlockOcr();

		// Token: 0x04000542 RID: 1346
		private bool imageFromFile;

		// Token: 0x04000543 RID: 1347
		private string path;

		// Token: 0x04000544 RID: 1348
		private int lastSelectedIndex = -1;

		// Token: 0x04000545 RID: 1349
		private bool clicked;
	}
}
