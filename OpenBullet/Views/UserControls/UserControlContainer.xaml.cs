using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.CSharp.RuntimeBinder;
using OpenBullet.Plugins;

namespace OpenBullet.Views.UserControls
{
	// Token: 0x02000063 RID: 99
	public partial class UserControlContainer : UserControl, IControl
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000B0A6 File Offset: 0x000092A6
		// (set) Token: 0x06000274 RID: 628 RVA: 0x0000B0AE File Offset: 0x000092AE
		public string PropertyName { get; set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000B0B7 File Offset: 0x000092B7
		// (set) Token: 0x06000276 RID: 630 RVA: 0x0000B0BF File Offset: 0x000092BF
		public IControl UserControl { get; set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000B0C8 File Offset: 0x000092C8
		// (set) Token: 0x06000278 RID: 632 RVA: 0x0000B0D0 File Offset: 0x000092D0
		public string Label { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000B0D9 File Offset: 0x000092D9
		// (set) Token: 0x0600027A RID: 634 RVA: 0x0000B0E1 File Offset: 0x000092E1
		public string Tooltip { get; set; }

		// Token: 0x0600027B RID: 635 RVA: 0x0000B0EC File Offset: 0x000092EC
		public UserControlContainer(string propertyName, IControl userControl, string label, string tooltip)
		{
			this.InitializeComponent();
			base.DataContext = this;
			this.PropertyName = propertyName;
			this.UserControl = userControl;
			(this.UserControl as UserControl).SetValue(Grid.ColumnProperty, 1);
			this.Grid.Children.Add(this.UserControl as UserControl);
			this.Label = label;
			this.Tooltip = tooltip;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000B160 File Offset: 0x00009360
		[return: Dynamic]
		public dynamic GetValue()
		{
			return this.UserControl.GetValue();
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000B170 File Offset: 0x00009370
		public void SetValue(dynamic value)
		{
			if (UserControlContainer.<>o__18.<>p__0 == null)
			{
				UserControlContainer.<>o__18.<>p__0 = CallSite<Action<CallSite, IControl, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SetValue", null, typeof(UserControlContainer), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			UserControlContainer.<>o__18.<>p__0.Target(UserControlContainer.<>o__18.<>p__0, this.UserControl, value);
		}
	}
}
