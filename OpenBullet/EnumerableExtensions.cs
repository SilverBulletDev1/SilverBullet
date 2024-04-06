using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace OpenBullet
{
	// Token: 0x0200001C RID: 28
	public static class EnumerableExtensions
	{
		// Token: 0x06000066 RID: 102 RVA: 0x00002C28 File Offset: 0x00000E28
		public static void SaveToFile<T>(this IEnumerable<T> items, string fileName, Func<T, string> mapping)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentNullException("The filename must not be empty");
			}
			File.WriteAllLines(fileName, items.Select((T i) => mapping(i)));
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002C70 File Offset: 0x00000E70
		public static void CopyToClipboard<T>(this IEnumerable<T> items, Func<T, string> mapping)
		{
			Clipboard.SetText(string.Join(Environment.NewLine, items.Select((T i) => mapping(i))));
		}
	}
}
