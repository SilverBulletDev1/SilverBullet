using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using OpenBullet.Views.UserControls;

namespace OpenBullet.Views.Main
{
	// Token: 0x020000AC RID: 172
	public partial class PluginsSection : Page
	{
		// Token: 0x06000460 RID: 1120 RVA: 0x00015B28 File Offset: 0x00013D28
		public PluginsSection(IEnumerable<PluginControl> controls)
		{
			this.InitializeComponent();
			this.controls = controls.ToList<PluginControl>();
			foreach (PluginControl control in this.controls)
			{
				this.pluginSelector.Items.Add(control.Plugin.Name);
			}
			if (this.controls.Count > 0)
			{
				this.pluginSelector.SelectedIndex = 0;
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00015BC4 File Offset: 0x00013DC4
		private void pluginSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.Main.Content = this.controls.First((PluginControl c) => c.Plugin.Name == (string)this.pluginSelector.SelectedValue);
		}

		// Token: 0x04000393 RID: 915
		private List<PluginControl> controls;
	}
}
