using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.CSharp.RuntimeBinder;
using OpenBullet.Plugins;
using OpenBullet.Views.UserControls;
using PluginFramework;
using RuriLib;

namespace OpenBullet.Views.StackerBlocks
{
	// Token: 0x0200008F RID: 143
	public partial class BlockPluginPage : Page
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x000117D6 File Offset: 0x0000F9D6
		// (set) Token: 0x060003B3 RID: 947 RVA: 0x000117DE File Offset: 0x0000F9DE
		public IBlockPlugin BlockPlugin { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x000117E7 File Offset: 0x0000F9E7
		public BlockBase Block
		{
			get
			{
				return this.BlockPlugin as BlockBase;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x000117F4 File Offset: 0x0000F9F4
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x000117FC File Offset: 0x0000F9FC
		public ObservableCollection<UserControl> Controls { get; set; } = new ObservableCollection<UserControl>();

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x00011805 File Offset: 0x0000FA05
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x0001180D File Offset: 0x0000FA0D
		private List<PropertyInfo> ValidProperties { get; set; } = new List<PropertyInfo>();

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060003B9 RID: 953 RVA: 0x00011818 File Offset: 0x0000FA18
		// (remove) Token: 0x060003BA RID: 954 RVA: 0x00011850 File Offset: 0x0000FA50
		public event EventHandler AutoSave;

		// Token: 0x060003BB RID: 955 RVA: 0x00011888 File Offset: 0x0000FA88
		public BlockPluginPage(IBlockPlugin blockPlugin)
		{
			this.InitializeComponent();
			base.DataContext = this;
			this.BlockPlugin = blockPlugin;
			base.LostFocus += this.BlockPluginPage_LostFocus;
			foreach (PropertyInfo p2 in from p in this.BlockPlugin.GetType().GetProperties()
				where Check.InputProperty(p)
				select p)
			{
				this.ValidProperties.Add(p2);
				UserControlContainer inputField = Build.InputField(this.BlockPlugin, p2, null);
				inputField.LostFocus += this.InputField_LostFocus;
				this.Controls.Add(inputField);
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00011978 File Offset: 0x0000FB78
		private void BlockPluginPage_LostFocus(object sender, RoutedEventArgs e)
		{
			EventHandler autoSave = this.AutoSave;
			if (autoSave == null)
			{
				return;
			}
			autoSave(sender, e);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00011978 File Offset: 0x0000FB78
		private void InputField_LostFocus(object sender, RoutedEventArgs e)
		{
			EventHandler autoSave = this.AutoSave;
			if (autoSave == null)
			{
				return;
			}
			autoSave(sender, e);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0001198C File Offset: 0x0000FB8C
		public void SetPropertyValues()
		{
			using (List<PropertyInfo>.Enumerator enumerator = this.ValidProperties.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PropertyInfo property = enumerator.Current;
					object value = (from c in this.Controls
						where c is UserControlContainer
						select c as UserControlContainer).First((UserControlContainer c) => c.PropertyName == property.Name).GetValue();
					if (BlockPluginPage.<>o__20.<>p__0 == null)
					{
						BlockPluginPage.<>o__20.<>p__0 = CallSite<Action<CallSite, PropertyInfo, IBlockPlugin, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SetValue", null, typeof(BlockPluginPage), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					BlockPluginPage.<>o__20.<>p__0.Target(BlockPluginPage.<>o__20.<>p__0, property, this.BlockPlugin, value);
				}
			}
		}
	}
}
