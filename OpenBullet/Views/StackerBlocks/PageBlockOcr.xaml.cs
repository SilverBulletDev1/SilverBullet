using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using AngleSharp.Text;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.MetaData;
using Microsoft.Win32;
using OpenBullet.Models;
using OpenBullet.Views.Main.Configs;
using OpenBullet.Views.UserControls.Filters;
using RuriLib;
using Tesseract;

namespace OpenBullet.Views.StackerBlocks
{
	// Token: 0x02000088 RID: 136
	public partial class PageBlockOcr : global::System.Windows.Controls.Page
	{
		// Token: 0x0600036A RID: 874 RVA: 0x0000E89C File Offset: 0x0000CA9C
		public PageBlockOcr(BlockOcr block)
		{
			this.InitializeComponent();
			this.vm = block;
			base.DataContext = this.vm;
			this.customHeadersRTB.AppendText(this.vm.GetCustomHeaders());
			try
			{
				foreach (string filter in this.vm.GetFilters().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
				{
					this.filterLB.Items.Add(filter.Trim());
				}
			}
			catch
			{
			}
			this.vm.PixelFormats.Add("Default");
			this.vm.PixelFormats.AddRange(Enum.GetNames(typeof(PixelFormat)));
			Enum.GetNames(typeof(EngineMode)).ToList<string>().ForEach(delegate(string e)
			{
				this.EngineModeComboBox.Items.Add(e);
			});
			Enum.GetNames(typeof(PageSegMode)).ToList<string>().ForEach(delegate(string p)
			{
				this.PageSegModeComboBox.Items.Add(p);
			});
			this.InitFilterControls();
			this.SetItemToComboBox();
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00003B20 File Offset: 0x00001D20
		private void InitFilterControls()
		{
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000E9CC File Offset: 0x0000CBCC
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			this.LoadTessData();
			this.LoadFilters();
			this.SetItemToComboBox();
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00003B20 File Offset: 0x00001D20
		private void Page_Initialized(object sender, EventArgs e)
		{
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000E9E0 File Offset: 0x0000CBE0
		private void customHeadersRTB_LostFocus(object sender, RoutedEventArgs e)
		{
			this.vm.SetCustomHeaders(this.customHeadersRTB.Lines());
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000E9F8 File Offset: 0x0000CBF8
		private void filterLB_LostFocus(object sender, RoutedEventArgs e)
		{
			try
			{
				this.vm.SetFilters(this.filterLB.Items.OfType<string>().ToArray<string>());
			}
			catch
			{
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000EA3C File Offset: 0x0000CC3C
		private void btnAddFilter_Click(object sender, RoutedEventArgs e)
		{
			this.filterLB.Items.Add(this.filterComboBox.Text + ": ");
			this.filterLB.SelectedIndex = this.filterLB.Items.Count - 1;
			this.filterLB_LostFocus(null, null);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000EA94 File Offset: 0x0000CC94
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
					if (file.Name.Contains(".") && !this.vm.Languages.Contains(file.Name.Split(new char[] { '.' })[0]))
					{
						this.vm.Languages.Add(file.Name.Split(new char[] { '.' })[0]);
					}
				}
				try
				{
					this.vm.Languages = new ObservableCollection<string>(this.vm.Languages.Distinct<string>());
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

		// Token: 0x06000372 RID: 882 RVA: 0x0000EBA0 File Offset: 0x0000CDA0
		private void LoadFilters()
		{
			try
			{
				foreach (ValueTuple<string, string, Type> processor in this.vm.Processors)
				{
					if (!this.filterComboBox.Items.Contains(processor.Item1))
					{
						this.filterComboBox.Items.Add(processor.Item1);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000EC34 File Offset: 0x0000CE34
		private void SetItemToComboBox()
		{
			try
			{
				this.LangComboBox.SelectedIndex = this.LangComboBox.Items.IndexOf(this.vm.OcrLang);
				this.EngineModeComboBox.SelectedIndex = this.EngineModeComboBox.Items.IndexOf(this.vm.Engine);
				this.PageSegModeComboBox.SelectedIndex = this.PageSegModeComboBox.Items.IndexOf(this.vm.PageSeg);
			}
			catch
			{
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000ECC8 File Offset: 0x0000CEC8
		private void btnfilterUp_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int selectedIndex = this.filterLB.SelectedIndex;
				if (selectedIndex > 0)
				{
					object itemToMoveUp = this.filterLB.Items[selectedIndex];
					this.filterLB.Items.RemoveAt(selectedIndex);
					this.filterLB.Items.Insert(selectedIndex - 1, itemToMoveUp);
					this.filterLB.SelectedIndex = selectedIndex - 1;
					this.vm.SetFilters(this.filterLB.Items.OfType<string>().ToArray<string>());
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000ED60 File Offset: 0x0000CF60
		private void btnfilterDown_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int selectedIndex = this.filterLB.SelectedIndex;
				if (selectedIndex + 1 < this.filterLB.Items.Count)
				{
					object itemToMoveDown = this.filterLB.Items[selectedIndex];
					this.filterLB.Items.RemoveAt(selectedIndex);
					this.filterLB.Items.Insert(selectedIndex + 1, itemToMoveDown);
					this.filterLB.SelectedIndex = selectedIndex + 1;
					this.filterLB_LostFocus(null, null);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000EDF0 File Offset: 0x0000CFF0
		private void btnfilterRemove_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int selectedIndex = this.filterLB.SelectedIndex;
				if (selectedIndex > -1)
				{
					this.filterLB.Items.RemoveAt(selectedIndex);
					this.filterTabControl.SelectedIndex = -1;
					this.filterLB_LostFocus(null, null);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000EE48 File Offset: 0x0000D048
		private void btnfilterClone_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this.filterLB.Items.Count != 0)
				{
					this.filterLB.Items.Add(this.filterLB.SelectedItem.ToString());
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000EEA0 File Offset: 0x0000D0A0
		private void btnfilterClear_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (global::System.Windows.Forms.MessageBox.Show("Do you want to clear the list of filters?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.No)
				{
					this.filterLB.Items.Clear();
					this.vm.SetFilters(this.filterLB.Items.OfType<string>().ToArray<string>());
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000EF0C File Offset: 0x0000D10C
		private void filterLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				int seletedIndex = this.filterLB.SelectedIndex;
				this.lastSelectedIndex = seletedIndex;
				if (seletedIndex != -1)
				{
					string selectedFilter = this.filterLB.Items[seletedIndex++].ToString();
					if (!selectedFilter.Contains(":"))
					{
						selectedFilter += ": ";
					}
					string filterType = selectedFilter.Split(new char[] { ':' })[0].Trim();
					if (seletedIndex > -1)
					{
						this.filterGroupBox.Visibility = Visibility.Visible;
						this.filterGroupBox.Header = filterType;
					}
					string text = filterType.ToLower();
					if (text != null)
					{
						switch (text.Length)
						{
						case 3:
							if (!(text == "hue"))
							{
								goto IL_9AC;
							}
							this.SetInInputTextAndBoolean(seletedIndex - 1, "0", false, "Degrees", "Rotate (Any integer between 0 and 360)");
							goto IL_9C4;
						case 4:
						{
							char c = text[0];
							if (c <= 'm')
							{
								switch (c)
								{
								case 'b':
									if (!(text == "blur"))
									{
										goto IL_9AC;
									}
									this.SetSize(seletedIndex - 1);
									goto IL_9C4;
								case 'c':
									if (!(text == "crop"))
									{
										goto IL_9AC;
									}
									this.SetCropLayer(seletedIndex - 1, new string[]
									{
										this.controlCropLayer.LeftTextBox.Text,
										this.controlCropLayer.TopTextBox.Text,
										this.controlCropLayer.RightTextBox.Text,
										this.controlCropLayer.BottomTextBox.Text,
										"Percentage"
									});
									goto IL_9C4;
								case 'd':
									goto IL_9AC;
								case 'e':
									if (!(text == "edge"))
									{
										goto IL_9AC;
									}
									goto IL_7A3;
								default:
									if (c != 'm')
									{
										goto IL_9AC;
									}
									if (!(text == "mean"))
									{
										goto IL_9AC;
									}
									goto IL_691;
								}
							}
							else if (c != 't')
							{
								if (c != 'z')
								{
									goto IL_9AC;
								}
								if (!(text == "zoom"))
								{
									goto IL_9AC;
								}
								this.SetInInputTextAndBoolean(seletedIndex - 1, "0", false, "Zoom Factor", "NearestNeighbor");
								goto IL_9C4;
							}
							else
							{
								if (!(text == "tint"))
								{
									goto IL_9AC;
								}
								goto IL_6D9;
							}
							break;
						}
						case 5:
						{
							char c = text[0];
							if (c != 'a')
							{
								if (c != 'g')
								{
									if (c != 's')
									{
										goto IL_9AC;
									}
									if (!(text == "scale"))
									{
										goto IL_9AC;
									}
									goto IL_65D;
								}
								else
								{
									if (!(text == "gamma"))
									{
										goto IL_9AC;
									}
									goto IL_691;
								}
							}
							else
							{
								if (!(text == "alpha"))
								{
									goto IL_9AC;
								}
								goto IL_65D;
							}
							break;
						}
						case 6:
						{
							char c = text[2];
							if (c <= 'o')
							{
								if (c != 'd')
								{
									if (c != 'o')
									{
										goto IL_9AC;
									}
									if (!(text == "smooth"))
									{
										goto IL_9AC;
									}
									goto IL_691;
								}
								else
								{
									if (!(text == "median"))
									{
										goto IL_9AC;
									}
									this.SetInInput(seletedIndex - 1, new string[] { "0" }, "ksize", false);
									goto IL_9C4;
								}
							}
							else if (c != 's')
							{
								if (c != 't')
								{
									goto IL_9AC;
								}
								if (!(text == "rotate"))
								{
									goto IL_9AC;
								}
								this.SetInInput(seletedIndex - 1, new string[] { "0" }, "Degrees", false);
								goto IL_9C4;
							}
							else
							{
								if (!(text == "resize"))
								{
									goto IL_9AC;
								}
								goto IL_681;
							}
							break;
						}
						case 7:
							if (!(text == "sharpen"))
							{
								goto IL_9AC;
							}
							goto IL_691;
						case 8:
						{
							char c = text[2];
							if (c <= 'l')
							{
								if (c != 'g')
								{
									if (c != 'l')
									{
										goto IL_9AC;
									}
									if (!(text == "halftone"))
									{
										goto IL_9AC;
									}
									this.SetInInputBoolean(seletedIndex - 1, "False", "Comic Mode");
									goto IL_9C4;
								}
								else
								{
									if (!(text == "vignette"))
									{
										goto IL_9AC;
									}
									goto IL_6D9;
								}
							}
							else if (c != 'n')
							{
								if (c != 't')
								{
									if (c != 'x')
									{
										goto IL_9AC;
									}
									if (!(text == "pixelate"))
									{
										goto IL_9AC;
									}
									goto IL_681;
								}
								else
								{
									if (!(text == "cvtcolor"))
									{
										goto IL_9AC;
									}
									this.SetControl(seletedIndex - 1, "CvtColor", new ControlText<global::System.Windows.Controls.TextBox>[]
									{
										new ControlText<global::System.Windows.Controls.TextBox>(this.controlCvtColor.dstCnTextBox, this.controlCvtColor.dstCnTextBox.Text)
									});
									goto IL_9C4;
								}
							}
							else
							{
								if (!(text == "contrast"))
								{
									goto IL_9AC;
								}
								goto IL_65D;
							}
							break;
						}
						case 9:
						{
							char c = text[0];
							if (c <= 'c')
							{
								if (c != 'a')
								{
									if (c != 'c')
									{
										goto IL_9AC;
									}
									if (!(text == "constrain"))
									{
										goto IL_9AC;
									}
									this.SetSize(seletedIndex - 1);
									goto IL_9C4;
								}
								else
								{
									if (!(text == "alignment"))
									{
										goto IL_9AC;
									}
									this.SetInInput(seletedIndex - 1, new string[] { "4" }, "Alignment size(must be a power of two)", false);
									goto IL_9C4;
								}
							}
							else if (c != 's')
							{
								if (c != 't')
								{
									goto IL_9AC;
								}
								if (!(text == "threshold"))
								{
									goto IL_9AC;
								}
								this.SetThreshold(seletedIndex - 1, new string[] { "0", "255", "Binary" });
								goto IL_9C4;
							}
							else
							{
								if (!(text == "sharpenex"))
								{
									goto IL_9AC;
								}
								goto IL_691;
							}
							break;
						}
						case 10:
						{
							char c = text[2];
							if (c != 'i')
							{
								switch (c)
								{
								case 'l':
									if (!(text == "coloralpha"))
									{
										goto IL_9AC;
									}
									goto IL_6D9;
								case 'm':
								case 'o':
								case 'p':
								case 'q':
									goto IL_9AC;
								case 'n':
									if (!(text == "contrastex"))
									{
										goto IL_9AC;
									}
									goto IL_65D;
								case 'r':
									if (!(text == "morphology"))
									{
										goto IL_9AC;
									}
									this.SetMorphology(seletedIndex - 1);
									goto IL_9C4;
								case 's':
									if (!(text == "resolution"))
									{
										goto IL_9AC;
									}
									this.SetResolution(seletedIndex - 1, new string[] { "0", "0", "Inch" });
									goto IL_9C4;
								case 't':
									if (!(text == "saturation"))
									{
										goto IL_9AC;
									}
									goto IL_65D;
								default:
									goto IL_9AC;
								}
							}
							else
							{
								if (!(text == "brightness"))
								{
									goto IL_9AC;
								}
								goto IL_65D;
							}
							break;
						}
						case 11:
							if (!(text == "entropycrop"))
							{
								goto IL_9AC;
							}
							break;
						case 12:
						{
							char c = text[0];
							if (c != 'b')
							{
								if (c != 'g')
								{
									if (c != 'r')
									{
										goto IL_9AC;
									}
									if (!(text == "replacecolor"))
									{
										goto IL_9AC;
									}
									this.SetReplaceColor(seletedIndex - 1, new string[] { "0,0,0", "|", "0,0,0", "0" });
									goto IL_9C4;
								}
								else
								{
									if (!(text == "gaussianblur"))
									{
										goto IL_9AC;
									}
									goto IL_6FD;
								}
							}
							else if (!(text == "binarization"))
							{
								goto IL_9AC;
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
							goto IL_9AC;
						case 14:
						{
							char c = text[0];
							if (c != 'c')
							{
								if (c != 'r')
								{
									goto IL_9AC;
								}
								if (!(text == "roundedcorners"))
								{
									goto IL_9AC;
								}
								goto IL_7A3;
							}
							else
							{
								if (!(text == "colorthreshold"))
								{
									goto IL_9AC;
								}
								goto IL_691;
							}
							break;
						}
						case 15:
						{
							char c = text[0];
							if (c != 'b')
							{
								if (c != 'g')
								{
									goto IL_9AC;
								}
								if (!(text == "gaussiansharpen"))
								{
									goto IL_9AC;
								}
								goto IL_6FD;
							}
							else
							{
								if (!(text == "backgroundcolor"))
								{
									goto IL_9AC;
								}
								goto IL_6D9;
							}
							break;
						}
						case 17:
							if (!(text == "adaptivethreshold"))
							{
								goto IL_9AC;
							}
							this.SetAdaptiveThreshold(seletedIndex - 1, new string[] { "1", "MeanC", "Binary", "1", "1" });
							goto IL_9C4;
						case 27:
							if (!(text == "fastnlmeansdenoisingcolored"))
							{
								goto IL_9AC;
							}
							this.SetFastNlMeansDenoisingColored(seletedIndex - 1, new string[] { "3", "3" });
							goto IL_9C4;
						default:
							goto IL_9AC;
						}
						this.SetInInput(seletedIndex - 1, new string[] { "0" }, "Threshold", false);
						goto IL_9C4;
						IL_65D:
						this.SetInInput(seletedIndex - 1, new string[] { "0" }, "Percentage", false);
						goto IL_9C4;
						IL_681:
						this.SetSize(seletedIndex - 1);
						goto IL_9C4;
						IL_691:
						this.SetInInput(seletedIndex - 1, new string[] { "0" }, "Value", false);
						goto IL_9C4;
						IL_6D9:
						this.SetInInput(seletedIndex - 1, new string[] { "0,0,0" }, "Color(R,G,B)", true);
						goto IL_9C4;
						IL_6FD:
						this.SetInInput(seletedIndex - 1, new string[] { "0" }, "Size", false);
						goto IL_9C4;
						IL_7A3:
						this.SetInInput(seletedIndex - 1, new string[] { "0" }, "Radius", false);
						goto IL_9C4;
					}
					IL_9AC:
					this.filterTabControl.SelectedIndex = -1;
					this.filterGroupBox.Visibility = Visibility.Collapsed;
					IL_9C4:;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000F900 File Offset: 0x0000DB00
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
				this.SetCaretIndexAndSelect(this.inputControl.InputTextBox, "0", 1);
			}
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000F9D4 File Offset: 0x0000DBD4
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

		// Token: 0x0600037C RID: 892 RVA: 0x0000FA74 File Offset: 0x0000DC74
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

		// Token: 0x0600037D RID: 893 RVA: 0x0000FB64 File Offset: 0x0000DD64
		private void SetInInputBoolean(int index, string defValue, string label = "Value")
		{
			this.inputControl.SetInputType(UserControlInput.InputType.Boolean);
			this.inputControl.label.Content = label + ":";
			this.filterTabControl.SelectIndexByHeaderName("Input");
			this.inputControl.InputComboBox.SelectedIndex = (this.GetFilterValue(index, new string[] { defValue }, 0).ToBoolean(false) ? 1 : 0);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000FBD8 File Offset: 0x0000DDD8
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
			this.SetCaretIndexAndSelect(this.resizeControl.WidthTextBox, "0", 1);
			this.SetCaretIndexAndSelect(this.resizeControl.HeightTextBox, "0", 1);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000FCBC File Offset: 0x0000DEBC
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

		// Token: 0x06000380 RID: 896 RVA: 0x0000FE4C File Offset: 0x0000E04C
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

		// Token: 0x06000381 RID: 897 RVA: 0x0000FF04 File Offset: 0x0000E104
		private void SetFastNlMeansDenoisingColored(int index, string[] defValues)
		{
			this.filterTabControl.SelectIndexByHeaderName("FastNlMeansDenoisingColored");
			string strength = this.GetFilterValues(index, defValues, ',')[0];
			string colorStrength = this.GetFilterValues(index, defValues, ',')[1];
			this.SetTextInTextBox(this.controlFastNlMeansDenoisingColored.StrengthTextBox, strength);
			this.SetTextInTextBox(this.controlFastNlMeansDenoisingColored.ColorStrengthTextBox, colorStrength);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000FF60 File Offset: 0x0000E160
		private void SetBlur(int index, string[] defValues)
		{
			this.filterTabControl.SelectIndexByHeaderName("Blur");
			string radius = this.GetFilterValues(index, defValues, ',')[0];
			string sigma = this.GetFilterValues(index, defValues, ',')[1];
			string channels = this.GetFilterValues(index, defValues, ',')[2];
			this.SetTextInTextBox(this.blurControl.RadiusTextBox, radius);
			this.SetTextInTextBox(this.blurControl.SigmaTextBox, sigma);
			this.blurControl.ChannelsComboBox.SelectedItem = channels;
			this.SetCaretIndexAndSelect(this.blurControl.RadiusTextBox, "0", 1);
			this.SetCaretIndexAndSelect(this.blurControl.SigmaTextBox, "0", 1);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00010008 File Offset: 0x0000E208
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

		// Token: 0x06000384 RID: 900 RVA: 0x00010084 File Offset: 0x0000E284
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

		// Token: 0x06000385 RID: 901 RVA: 0x00010164 File Offset: 0x0000E364
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

		// Token: 0x06000386 RID: 902 RVA: 0x00010260 File Offset: 0x0000E460
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
			this.SetCaretIndexAndSelect(this.controlMorphology.IterationsTextBox, "0", 1);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00010458 File Offset: 0x0000E658
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

		// Token: 0x06000388 RID: 904 RVA: 0x000104F0 File Offset: 0x0000E6F0
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

		// Token: 0x06000389 RID: 905 RVA: 0x00010553 File Offset: 0x0000E753
		private void SetTextInTextBox(global::System.Windows.Controls.TextBox textBox, string text)
		{
			if (textBox.Text != text)
			{
				textBox.Text = text;
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0001056C File Offset: 0x0000E76C
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
				this.filterLB.Items[index] = split[0] + ": " + value;
				this.filterLB.SelectedIndex = index;
			}
			catch
			{
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00010688 File Offset: 0x0000E888
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

		// Token: 0x0600038C RID: 908 RVA: 0x00010790 File Offset: 0x0000E990
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

		// Token: 0x0600038D RID: 909 RVA: 0x00010930 File Offset: 0x0000EB30
		private string[] GetFilterColors(int index, string[] defaultValues)
		{
			string val = string.Empty;
			defaultValues.ToList<string>().ForEach(delegate(string dv)
			{
				if (!dv.EndsWith("|"))
				{
					val = val + dv + "|";
					return;
				}
				if (dv != "|")
				{
					val += dv;
				}
			});
			return val.Trim().TrimEnd(new char[] { '|' }).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00010994 File Offset: 0x0000EB94
		private void SetCaretIndexAndSelect(global::System.Windows.Controls.TextBox textBox, string defVal = "0", int index = 1)
		{
			try
			{
				if (textBox.Text == defVal)
				{
					textBox.CaretIndex = index;
					textBox.SelectAll();
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600038F RID: 911 RVA: 0x000109D4 File Offset: 0x0000EBD4
		private void inputControl_SetFilter(object sender, EventArgs e)
		{
			if (this.filterTabControl.SelectedIndex != this.filterTabControl.GetIndexByItemName((e as TextChangedEventArgs).Source.ToString()))
			{
				return;
			}
			this.SetFilter(this.lastSelectedIndex, sender as string[]);
			this.filterLB_LostFocus(null, null);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00010A24 File Offset: 0x0000EC24
		private void MenuItem_Click(object sender, RoutedEventArgs e)
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
					File.ReadAllLines(dialog.FileName).ToList<string>().ForEach(delegate(string f)
					{
						this.filterLB.Items.Add(f);
					});
					this.filterLB_LostFocus(null, null);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00010AA0 File Offset: 0x0000ECA0
		private void MenuItem_Click_1(object sender, RoutedEventArgs e)
		{
			try
			{
				global::System.Windows.Clipboard.SetText(this.vm.Filters[this.filterLB.SelectedIndex].ToString());
			}
			catch
			{
			}
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00010AE8 File Offset: 0x0000ECE8
		private void MenuItem_Click_2(object sender, RoutedEventArgs e)
		{
			try
			{
				string item = global::System.Windows.Clipboard.GetText();
				string item2 = item;
				if (item.Contains(":"))
				{
					item2 = item.Split(new char[] { ':' })[0].Trim();
				}
				if (this.filterComboBox.Items.Contains(item2))
				{
					this.filterLB.Items.Add(item);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00010B60 File Offset: 0x0000ED60
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

		// Token: 0x06000394 RID: 916 RVA: 0x00010BB8 File Offset: 0x0000EDB8
		private void MenuItem_Click_3(object sender, RoutedEventArgs e)
		{
			try
			{
				string item = this.filterLB.SelectedItem.ToString();
				(new ConfigOcrSettings(true).DataContext as ConfigSettings).FilterList.Add(item);
			}
			catch
			{
			}
		}

		// Token: 0x0400029B RID: 667
		private BlockOcr vm;

		// Token: 0x0400029C RID: 668
		private int lastSelectedIndex = -1;
	}
}
