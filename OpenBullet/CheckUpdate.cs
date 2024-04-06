using System;
using System.Net;
using Newtonsoft.Json;

namespace OpenBullet
{
	// Token: 0x02000014 RID: 20
	public static class CheckUpdate
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00002A60 File Offset: 0x00000C60
		public static T Run<T>(string url = "https://raw.githubusercontent.com/mohamm4dx/SilverBullet/master/SilverBulletUpdater/SBUpdate.json")
		{
			string data = string.Empty;
			using (WebClient wc = new WebClient())
			{
				wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:87.0) Gecko/20100101 Firefox/87.0");
				data = wc.DownloadString(url);
			}
			return JsonConvert.DeserializeObject<T>(data);
		}
	}
}
