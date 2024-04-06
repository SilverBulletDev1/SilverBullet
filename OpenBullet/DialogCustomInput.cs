using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using OpenBullet.ViewModels;
using RuriLib.Models;
using RuriLib.Runner;

namespace OpenBullet
{
	// Token: 0x02000041 RID: 65
	public class DialogCustomInput : Page, IComponentConnector
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00006511 File Offset: 0x00004711
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00006519 File Offset: 0x00004719
		private object Caller { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00006522 File Offset: 0x00004722
		// (set) Token: 0x0600016C RID: 364 RVA: 0x0000652A File Offset: 0x0000472A
		private string VariableName { get; set; }

		// Token: 0x0600016D RID: 365 RVA: 0x00006533 File Offset: 0x00004733
		public DialogCustomInput(object caller, string variableName, string question)
		{
			this.InitializeComponent();
			this.Caller = caller;
			this.VariableName = variableName;
			this.questionTextbox.Text = question;
			this.answerTextbox.Focus();
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00006568 File Offset: 0x00004768
		private void acceptButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.Caller.GetType() == typeof(StackerViewModel))
			{
				((StackerViewModel)this.Caller).BotData.Variables.Set(new CVar(this.VariableName, this.answerTextbox.Text, false, false));
			}
			else if (this.Caller.GetType() == typeof(RunnerViewModel))
			{
				((RunnerViewModel)this.Caller).CustomInputs.Add(new KeyValuePair<string, string>(this.VariableName, this.answerTextbox.Text));
			}
			((MainDialog)base.Parent).Close();
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000661C File Offset: 0x0000481C
		private void answerTextbox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == global::System.Windows.Input.Key.Return)
			{
				this.acceptButton_Click(sender, null);
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00006630 File Offset: 0x00004830
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocater = new Uri("/SilverBullet;component/views/dialogs/dialogcustominput.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocater);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00006660 File Offset: 0x00004860
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.questionTextbox = (TextBox)target;
				return;
			case 2:
				this.answerTextbox = (TextBox)target;
				this.answerTextbox.KeyDown += this.answerTextbox_KeyDown;
				return;
			case 3:
				this.acceptButton = (Button)target;
				this.acceptButton.Click += this.acceptButton_Click;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x04000145 RID: 325
		internal TextBox questionTextbox;

		// Token: 0x04000146 RID: 326
		internal TextBox answerTextbox;

		// Token: 0x04000147 RID: 327
		internal Button acceptButton;

		// Token: 0x04000148 RID: 328
		private bool _contentLoaded;
	}
}
