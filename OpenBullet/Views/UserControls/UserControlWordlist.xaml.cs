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
using RuriLib.Models;

namespace OpenBullet.Views.UserControls
{
	// Token: 0x0200006D RID: 109
	public partial class UserControlWordlist : UserControl, IControl
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000BCB9 File Offset: 0x00009EB9
		// (set) Token: 0x060002BA RID: 698 RVA: 0x0000BCC1 File Offset: 0x00009EC1
		public Wordlist Wordlist { get; set; }

		// Token: 0x060002BB RID: 699 RVA: 0x0000BCCA File Offset: 0x00009ECA
		public UserControlWordlist()
		{
			this.InitializeComponent();
			base.DataContext = this;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000BCDF File Offset: 0x00009EDF
		[return: Dynamic]
		public dynamic GetValue()
		{
			return this.Wordlist;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000BCE8 File Offset: 0x00009EE8
		public void SetValue(dynamic value)
		{
			if (UserControlWordlist.<>o__6.<>p__0 == null)
			{
				UserControlWordlist.<>o__6.<>p__0 = CallSite<Func<CallSite, object, Wordlist>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Wordlist), typeof(UserControlWordlist)));
			}
			this.Wordlist = UserControlWordlist.<>o__6.<>p__0.Target(UserControlWordlist.<>o__6.<>p__0, value);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000BD3C File Offset: 0x00009F3C
		private void Choose_Click(object sender, RoutedEventArgs e)
		{
			new MainDialog(new DialogSelectWordlist(this), "Select a Wordlist").ShowDialog();
			if (this.Wordlist != null)
			{
				this.WordlistName.Text = this.Wordlist.Name;
			}
		}
	}
}
