using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using RuriLib;
using RuriLib.ViewModels;

namespace OpenBullet.Views.Main.Configs.OtherOptions
{
	// Token: 0x02000114 RID: 276
	public partial class Inputs : Page
	{
		// Token: 0x06000760 RID: 1888 RVA: 0x0002A8AD File Offset: 0x00028AAD
		public Inputs()
		{
			this.vm = SB.ConfigManager.CurrentConfig.Config.Settings;
			base.DataContext = this.vm;
			this.InitializeComponent();
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0002A8EC File Offset: 0x00028AEC
		private void clearInputsButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.CustomInputs.Clear();
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0002A8FE File Offset: 0x00028AFE
		private void addInputButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.CustomInputs.Add(new CustomInput(this.rand.Next()));
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0002A920 File Offset: 0x00028B20
		private void removeInputButton_Click(object sender, RoutedEventArgs e)
		{
			this.vm.RemoveCustomInputById((int)(sender as Button).Tag);
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0002A9EF File Offset: 0x00028BEF
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 4)
			{
				((Button)target).Click += this.removeInputButton_Click;
			}
		}

		// Token: 0x04000618 RID: 1560
		private ConfigSettings vm;

		// Token: 0x04000619 RID: 1561
		private Random rand = new Random();
	}
}
