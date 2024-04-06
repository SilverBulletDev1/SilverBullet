using System;
using System.Runtime.CompilerServices;

namespace OpenBullet.Plugins
{
	// Token: 0x0200014E RID: 334
	public interface IControl
	{
		// Token: 0x060009A9 RID: 2473
		[return: Dynamic]
		dynamic GetValue();

		// Token: 0x060009AA RID: 2474
		void SetValue(dynamic value);
	}
}
