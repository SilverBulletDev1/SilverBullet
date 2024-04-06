using System;
using System.Windows;
using RuriLib.Interfaces;

namespace OpenBullet
{
	// Token: 0x02000013 RID: 19
	public class Alerter : IAlerter
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00002A4B File Offset: 0x00000C4B
		public bool YesOrNo(string message, string title)
		{
			return MessageBox.Show(message, title, MessageBoxButton.YesNo) == MessageBoxResult.Yes;
		}
	}
}
