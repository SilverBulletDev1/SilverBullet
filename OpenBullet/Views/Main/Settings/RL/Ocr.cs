using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using RuriLib.ViewModels;

namespace OpenBullet.Views.Main.Settings.RL
{
	// Token: 0x020000EF RID: 239
	public class Ocr : Page, IComponentConnector
	{
		// Token: 0x0600061F RID: 1567 RVA: 0x0001FE1C File Offset: 0x0001E01C
		public Ocr()
		{
			this.InitializeComponent();
			base.DataContext = (this.vm = SB.Settings.RLSettings.Ocr);
			Enum.GetNames(typeof(VariableValueType)).ToList<string>().ForEach(delegate(string vt)
			{
				this.varValueType.Items.Add(vt);
			});
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0001FE78 File Offset: 0x0001E078
		private void btnVariableAdd_Click(object sender, RoutedEventArgs e)
		{
			this.vm.VariableList.Add(new TesseractVariable
			{
				Name = this.varName.Text,
				Value = this.varValue.Text,
				ValueType = (VariableValueType)Enum.Parse(typeof(VariableValueType), this.varValueType.SelectedItem.ToString(), true)
			});
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0001FEE8 File Offset: 0x0001E0E8
		private void btnVariableUp_Click(object sender, RoutedEventArgs e)
		{
			int selectedIndex = this.variableLB.SelectedIndex;
			if (selectedIndex > 0)
			{
				object itemToMoveUp = this.variableLB.Items[selectedIndex];
				this.vm.VariableList.RemoveAt(selectedIndex);
				this.vm.VariableList.Insert(selectedIndex - 1, (TesseractVariable)itemToMoveUp);
				this.variableLB.SelectedIndex = selectedIndex - 1;
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0001FF50 File Offset: 0x0001E150
		private void btnVariableDown_Click(object sender, RoutedEventArgs e)
		{
			int selectedIndex = this.variableLB.SelectedIndex;
			if (selectedIndex + 1 < this.variableLB.Items.Count)
			{
				object itemToMoveUp = this.variableLB.Items[selectedIndex];
				this.vm.VariableList.RemoveAt(selectedIndex);
				this.vm.VariableList.Insert(selectedIndex + 1, (TesseractVariable)itemToMoveUp);
				this.variableLB.SelectedIndex = selectedIndex + 1;
			}
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0001FFC8 File Offset: 0x0001E1C8
		private void btnVariableRemove_Click(object sender, RoutedEventArgs e)
		{
			if (this.variableLB.SelectedIndex == -1)
			{
				return;
			}
			this.vm.VariableList.RemoveAt(this.variableLB.SelectedIndex);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0001FFF4 File Offset: 0x0001E1F4
		private void btnVariableClear_Click(object sender, RoutedEventArgs e)
		{
			this.vm.VariableList.Clear();
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00020008 File Offset: 0x0001E208
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				TesseractVariable varItem = this.vm.VariableList[this.variableLB.SelectedIndex];
				Clipboard.SetText(string.Concat(new string[]
				{
					varItem.Name,
					":",
					varItem.Value,
					":",
					varItem.ValueType.ToString()
				}));
			}
			catch
			{
			}
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00020090 File Offset: 0x0001E290
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/main/settings/rurilib/ocr.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x000200C0 File Offset: 0x0001E2C0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.varName = (TextBox)target;
				return;
			case 2:
				this.varValue = (TextBox)target;
				return;
			case 3:
				this.varValueType = (ComboBox)target;
				return;
			case 4:
				this.variableLB = (ListBox)target;
				return;
			case 5:
				((MenuItem)target).Click += this.MenuItem_Click;
				return;
			case 6:
				this.btnVariableAdd = (Button)target;
				this.btnVariableAdd.Click += this.btnVariableAdd_Click;
				return;
			case 7:
				this.btnVariableUp = (Button)target;
				this.btnVariableUp.Click += this.btnVariableUp_Click;
				return;
			case 8:
				this.btnVariableDown = (Button)target;
				this.btnVariableDown.Click += this.btnVariableDown_Click;
				return;
			case 9:
				this.btnVariableRemove = (Button)target;
				this.btnVariableRemove.Click += this.btnVariableRemove_Click;
				return;
			case 10:
				this.btnVariableClear = (Button)target;
				this.btnVariableClear.Click += this.btnVariableClear_Click;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x04000511 RID: 1297
		private SettingsOcr vm;

		// Token: 0x04000512 RID: 1298
		internal TextBox varName;

		// Token: 0x04000513 RID: 1299
		internal TextBox varValue;

		// Token: 0x04000514 RID: 1300
		internal ComboBox varValueType;

		// Token: 0x04000515 RID: 1301
		internal ListBox variableLB;

		// Token: 0x04000516 RID: 1302
		internal Button btnVariableAdd;

		// Token: 0x04000517 RID: 1303
		internal Button btnVariableUp;

		// Token: 0x04000518 RID: 1304
		internal Button btnVariableDown;

		// Token: 0x04000519 RID: 1305
		internal Button btnVariableRemove;

		// Token: 0x0400051A RID: 1306
		internal Button btnVariableClear;

		// Token: 0x0400051B RID: 1307
		private bool _contentLoaded;
	}
}
