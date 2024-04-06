using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Extreme.Net;
using Newtonsoft.Json.Linq;
using OpenBullet.Repositories;
using RuriLib.Interfaces;
using RuriLib.Models;
using RuriLib.Models.Stats;
using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
	// Token: 0x0200012C RID: 300
	public class ProxyManagerViewModel : ViewModelBase, IProxyManager, IProxyChecker
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x0002E67B File Offset: 0x0002C87B
		// (set) Token: 0x06000895 RID: 2197 RVA: 0x0002E684 File Offset: 0x0002C884
		public ObservableCollection<CProxy> ProxiesCollection
		{
			get
			{
				return this._proxiesCollection;
			}
			private set
			{
				if (object.Equals(this._proxiesCollection, value))
				{
					return;
				}
				this._proxiesCollection = value;
				this.OnPropertyChanged("Proxies");
				this.OnPropertyChanged("ProxiesCollection");
				this.UpdateProperties();
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x0002E6C5 File Offset: 0x0002C8C5
		public IEnumerable<CProxy> Proxies
		{
			get
			{
				return this.ProxiesCollection;
			}
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0002E6D0 File Offset: 0x0002C8D0
		public ProxyManagerViewModel()
		{
			this._repo = new LiteDBRepository<CProxy>(SB.dataBaseFile, "proxies");
			this.ProxiesCollection = new ObservableCollection<CProxy>();
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x0002E7B4 File Offset: 0x0002C9B4
		// (set) Token: 0x06000898 RID: 2200 RVA: 0x0002E77C File Offset: 0x0002C97C
		public int Total
		{
			get
			{
				return this.total;
			}
			set
			{
				if (this.total == value)
				{
					return;
				}
				this.total = value;
				this.OnPropertyChanged("Stats");
				this.OnPropertyChanged("Total");
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600089B RID: 2203 RVA: 0x0002E7F4 File Offset: 0x0002C9F4
		// (set) Token: 0x0600089A RID: 2202 RVA: 0x0002E7BC File Offset: 0x0002C9BC
		public int Tested
		{
			get
			{
				return this.tested;
			}
			set
			{
				if (this.tested == value)
				{
					return;
				}
				this.tested = value;
				this.OnPropertyChanged("Stats");
				this.OnPropertyChanged("Tested");
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600089D RID: 2205 RVA: 0x0002E834 File Offset: 0x0002CA34
		// (set) Token: 0x0600089C RID: 2204 RVA: 0x0002E7FC File Offset: 0x0002C9FC
		public int Http
		{
			get
			{
				return this.http;
			}
			set
			{
				if (this.http == value)
				{
					return;
				}
				this.http = value;
				this.OnPropertyChanged("Stats");
				this.OnPropertyChanged("Http");
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x0002E874 File Offset: 0x0002CA74
		// (set) Token: 0x0600089E RID: 2206 RVA: 0x0002E83C File Offset: 0x0002CA3C
		public int Socks4
		{
			get
			{
				return this.socks4;
			}
			set
			{
				if (this.socks4 == value)
				{
					return;
				}
				this.socks4 = value;
				this.OnPropertyChanged("Stats");
				this.OnPropertyChanged("Socks4");
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x0002E8B4 File Offset: 0x0002CAB4
		// (set) Token: 0x060008A0 RID: 2208 RVA: 0x0002E87C File Offset: 0x0002CA7C
		public int Socks4a
		{
			get
			{
				return this.socks4a;
			}
			set
			{
				if (this.socks4a == value)
				{
					return;
				}
				this.socks4a = value;
				this.OnPropertyChanged("Stats");
				this.OnPropertyChanged("Socks4a");
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060008A3 RID: 2211 RVA: 0x0002E8F4 File Offset: 0x0002CAF4
		// (set) Token: 0x060008A2 RID: 2210 RVA: 0x0002E8BC File Offset: 0x0002CABC
		public int Socks5
		{
			get
			{
				return this.socks5;
			}
			set
			{
				if (this.socks5 == value)
				{
					return;
				}
				this.socks5 = value;
				this.OnPropertyChanged("Stats");
				this.OnPropertyChanged("Socks5");
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x0002E929 File Offset: 0x0002CB29
		// (set) Token: 0x060008A4 RID: 2212 RVA: 0x0002E8FC File Offset: 0x0002CAFC
		public int Chain
		{
			get
			{
				return this.chain;
			}
			set
			{
				if (this.chain == value)
				{
					return;
				}
				this.chain = value;
				this.OnPropertyChanged("Chain");
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x0002E96C File Offset: 0x0002CB6C
		// (set) Token: 0x060008A6 RID: 2214 RVA: 0x0002E934 File Offset: 0x0002CB34
		public int Working
		{
			get
			{
				return this.working;
			}
			set
			{
				if (this.working == value)
				{
					return;
				}
				this.working = value;
				this.OnPropertyChanged("Stats");
				this.OnPropertyChanged("Working");
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x0002E9A1 File Offset: 0x0002CBA1
		// (set) Token: 0x060008A8 RID: 2216 RVA: 0x0002E974 File Offset: 0x0002CB74
		public int NotWorking
		{
			get
			{
				return this.notWorking;
			}
			set
			{
				if (this.notWorking == value)
				{
					return;
				}
				this.notWorking = value;
				this.OnPropertyChanged("NotWorking");
			}
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0002E9AC File Offset: 0x0002CBAC
		public void UpdateProperties()
		{
			object obj = this.totalLock;
			lock (obj)
			{
				this.Total = this.ProxiesCollection.Count<CProxy>();
			}
			obj = this.testedLock;
			lock (obj)
			{
				this.Tested = this.ProxiesCollection.Count((CProxy x) => x.Working != ProxyWorking.UNTESTED);
			}
			obj = this.httpLock;
			lock (obj)
			{
				this.Http = this.ProxiesCollection.Count((CProxy x) => x.Type == ProxyType.Http);
			}
			obj = this.socks4Lock;
			lock (obj)
			{
				this.Socks4 = this.ProxiesCollection.Count((CProxy x) => x.Type == ProxyType.Socks4);
			}
			obj = this.socks4aLock;
			lock (obj)
			{
				this.Socks4a = this.ProxiesCollection.Count((CProxy x) => x.Type == ProxyType.Socks4a);
			}
			obj = this.socks5Lock;
			lock (obj)
			{
				this.Socks5 = this.ProxiesCollection.Count((CProxy x) => x.Type == ProxyType.Socks5);
			}
			obj = this.chainLock;
			lock (obj)
			{
				this.Chain = this.ProxiesCollection.Count((CProxy x) => x.Type == ProxyType.Chain);
			}
			obj = this.workingLock;
			lock (obj)
			{
				this.Working = this.ProxiesCollection.Count((CProxy x) => x.Working == ProxyWorking.YES);
			}
			obj = this.notWorkingLock;
			lock (obj)
			{
				this.NotWorking = this.ProxiesCollection.Count((CProxy x) => x.Working == ProxyWorking.NO);
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x0002ECC0 File Offset: 0x0002CEC0
		public ProxyManagerStats Stats
		{
			get
			{
				return new ProxyManagerStats(this.Total, this.Tested, this.Working, this.Http, this.Socks4, this.Socks4a, this.Socks5);
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x0002ECF1 File Offset: 0x0002CEF1
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x0002ECFC File Offset: 0x0002CEFC
		public int BotsAmount
		{
			get
			{
				return this.botsAmount;
			}
			set
			{
				if (this.botsAmount == value)
				{
					return;
				}
				this.botsAmount = value;
				this.OnPropertyChanged("BotsAmount");
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x0002ED29 File Offset: 0x0002CF29
		// (set) Token: 0x060008AF RID: 2223 RVA: 0x0002ED3C File Offset: 0x0002CF3C
		public string TestSite
		{
			get
			{
				return SB.Settings.ProxyManagerSettings.ActiveProxySiteUrl;
			}
			set
			{
				if (string.Equals(this.TestSite, value, StringComparison.Ordinal))
				{
					return;
				}
				SB.Settings.ProxyManagerSettings.ActiveProxySiteUrl = value;
				this.OnPropertyChanged("TestSite");
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0002ED76 File Offset: 0x0002CF76
		// (set) Token: 0x060008B1 RID: 2225 RVA: 0x0002ED88 File Offset: 0x0002CF88
		public string SuccessKey
		{
			get
			{
				return SB.Settings.ProxyManagerSettings.ActiveProxyKey;
			}
			set
			{
				if (string.Equals(this.SuccessKey, value, StringComparison.Ordinal))
				{
					return;
				}
				SB.Settings.ProxyManagerSettings.ActiveProxyKey = value;
				this.OnPropertyChanged("SuccessKey");
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x0002EDC2 File Offset: 0x0002CFC2
		// (set) Token: 0x060008B3 RID: 2227 RVA: 0x0002EDCC File Offset: 0x0002CFCC
		public bool OnlyUntested
		{
			get
			{
				return this.onlyUntested;
			}
			set
			{
				if (this.onlyUntested == value)
				{
					return;
				}
				this.onlyUntested = value;
				this.OnPropertyChanged("OnlyUntested");
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x0002EDF9 File Offset: 0x0002CFF9
		// (set) Token: 0x060008B5 RID: 2229 RVA: 0x0002EE04 File Offset: 0x0002D004
		public int Timeout
		{
			get
			{
				return this.timeout;
			}
			set
			{
				if (this.timeout == value)
				{
					return;
				}
				this.timeout = value;
				this.OnPropertyChanged("Timeout");
			}
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0002EE34 File Offset: 0x0002D034
		public async Task CheckAllAsync(IEnumerable<CProxy> proxies, CancellationToken cancellationToken, Action<ProxyCheckResult<ProxyResult>> onResult = null, IProgress<float> progress = null)
		{
			ProxyManagerViewModel.<>c__DisplayClass75_0 CS$<>8__locals1 = new ProxyManagerViewModel.<>c__DisplayClass75_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.onResult = onResult;
			CS$<>8__locals1.progress = progress;
			if (!cancellationToken.IsCancellationRequested)
			{
				CS$<>8__locals1.ss = new SemaphoreSlim(this.BotsAmount, this.BotsAmount);
				try
				{
					CS$<>8__locals1.total = proxies.Count<CProxy>();
					CS$<>8__locals1.current = 0;
					IEnumerable<Task> tasks = proxies.Select(delegate(CProxy proxy)
					{
						ProxyManagerViewModel.<>c__DisplayClass75_0.<<CheckAllAsync>b__0>d <<CheckAllAsync>b__0>d;
						<<CheckAllAsync>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<CheckAllAsync>b__0>d.<>4__this = CS$<>8__locals1;
						<<CheckAllAsync>b__0>d.proxy = proxy;
						<<CheckAllAsync>b__0>d.<>1__state = -1;
						<<CheckAllAsync>b__0>d.<>t__builder.Start<ProxyManagerViewModel.<>c__DisplayClass75_0.<<CheckAllAsync>b__0>d>(ref <<CheckAllAsync>b__0>d);
						return <<CheckAllAsync>b__0>d.<>t__builder.Task;
					});
					await Task.WhenAny(new Task[]
					{
						Task.WhenAll(tasks),
						ProxyManagerViewModel.AsTask(cancellationToken)
					});
					cancellationToken.ThrowIfCancellationRequested();
				}
				finally
				{
					if (CS$<>8__locals1.ss != null)
					{
						((IDisposable)CS$<>8__locals1.ss).Dispose();
					}
				}
			}
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0002EE98 File Offset: 0x0002D098
		public async Task<ProxyResult> CheckAsync(CProxy proxy, CancellationToken cancellationToken)
		{
			Task<ProxyResult> task = this.CheckProxy(proxy);
			await Task.WhenAny(new Task[]
			{
				task,
				ProxyManagerViewModel.AsTask(cancellationToken)
			});
			cancellationToken.ThrowIfCancellationRequested();
			return task.Result;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0002EEEC File Offset: 0x0002D0EC
		private Task<ProxyResult> CheckProxy(CProxy proxy)
		{
			ProxyManagerViewModel.<CheckProxy>d__77 <CheckProxy>d__;
			<CheckProxy>d__.<>t__builder = AsyncTaskMethodBuilder<ProxyResult>.Create();
			<CheckProxy>d__.<>4__this = this;
			<CheckProxy>d__.proxy = proxy;
			<CheckProxy>d__.<>1__state = -1;
			<CheckProxy>d__.<>t__builder.Start<ProxyManagerViewModel.<CheckProxy>d__77>(ref <CheckProxy>d__);
			return <CheckProxy>d__.<>t__builder.Task;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0002EF38 File Offset: 0x0002D138
		private async Task<bool> CheckWorking(CProxy proxy)
		{
			int timeout = this.Timeout * 1000;
			bool flag;
			using (HttpRequest request = new HttpRequest())
			{
				request.Proxy = proxy.GetClient();
				request.Proxy.ConnectTimeout = timeout;
				request.Proxy.ReadWriteTimeout = timeout;
				request.ConnectTimeout = timeout;
				request.KeepAliveTimeout = timeout;
				request.ReadWriteTimeout = timeout;
				flag = (await request.GetAsync(this.TestSite, null)).ToString().Contains(this.SuccessKey);
			}
			return flag;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0002EF84 File Offset: 0x0002D184
		private async Task<string> CheckCountry(CProxy proxy)
		{
			string text;
			using (HttpRequest request = new HttpRequest())
			{
				request.ConnectTimeout = this.Timeout;
				JObject json = JObject.Parse((await request.GetAsync("http://ip-api.com/json/" + proxy.Host, null)).ToString());
				if (json.Value<string>("status") == "success")
				{
					text = json.Value<string>("country");
				}
				else
				{
					text = "Unknown";
				}
			}
			return text;
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0002EFD0 File Offset: 0x0002D1D0
		public static Task AsTask(CancellationToken cancellationToken)
		{
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
			cancellationToken.Register(delegate
			{
				tcs.TrySetCanceled();
			}, false);
			return tcs.Task;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0002F00E File Offset: 0x0002D20E
		public void Add(CProxy proxy)
		{
			this.ProxiesCollection.Add(proxy);
			this._repo.Add(proxy);
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0002F028 File Offset: 0x0002D228
		public void AddRange(IEnumerable<CProxy> proxies)
		{
			foreach (CProxy p in proxies)
			{
				this.ProxiesCollection.Add(p);
			}
			this._repo.Add(proxies);
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0002F084 File Offset: 0x0002D284
		public void RefreshList()
		{
			this.ProxiesCollection = new ObservableCollection<CProxy>(this._repo.Get());
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0002F09C File Offset: 0x0002D29C
		public void Update(CProxy proxy)
		{
			this._repo.Update(proxy);
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0002F0AA File Offset: 0x0002D2AA
		public void Remove(CProxy proxy)
		{
			this.ProxiesCollection.Remove(proxy);
			this._repo.Remove(proxy);
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0002F0C8 File Offset: 0x0002D2C8
		public void Remove(IEnumerable<CProxy> proxies)
		{
			CProxy[] toRemove = proxies.ToArray<CProxy>();
			foreach (CProxy proxy in toRemove)
			{
				this.ProxiesCollection.Remove(proxy);
			}
			this._repo.Remove(toRemove);
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0002F109 File Offset: 0x0002D309
		public void RemoveAll()
		{
			this.ProxiesCollection.Clear();
			this._repo.RemoveAll();
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0002F121 File Offset: 0x0002D321
		public void RemoveNotWorking()
		{
			this.Remove(this.Proxies.Where((CProxy p) => p.Working == ProxyWorking.NO));
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0002F154 File Offset: 0x0002D354
		public void RemoveDuplicates()
		{
			IEnumerable<CProxy> duplicates = (from p in this.Proxies
				group p by p.Proxy into g
				where g.Count<CProxy>() > 1
				select g).SelectMany((IGrouping<string, CProxy> g) => g.OrderBy((CProxy p) => p.Proxy).Reverse<CProxy>().Skip(1));
			this.Remove(duplicates);
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0002F1DB File Offset: 0x0002D3DB
		public void RemoveUntested()
		{
			this.Remove(this.Proxies.Where((CProxy p) => p.Working == ProxyWorking.UNTESTED));
		}

		// Token: 0x040006B4 RID: 1716
		private LiteDBRepository<CProxy> _repo;

		// Token: 0x040006B5 RID: 1717
		private ObservableCollection<CProxy> _proxiesCollection;

		// Token: 0x040006B6 RID: 1718
		public object totalLock = new object();

		// Token: 0x040006B7 RID: 1719
		private int total;

		// Token: 0x040006B8 RID: 1720
		public object testedLock = new object();

		// Token: 0x040006B9 RID: 1721
		private int tested;

		// Token: 0x040006BA RID: 1722
		public object httpLock = new object();

		// Token: 0x040006BB RID: 1723
		private int http;

		// Token: 0x040006BC RID: 1724
		public object socks4Lock = new object();

		// Token: 0x040006BD RID: 1725
		private int socks4;

		// Token: 0x040006BE RID: 1726
		public object socks4aLock = new object();

		// Token: 0x040006BF RID: 1727
		private int socks4a;

		// Token: 0x040006C0 RID: 1728
		public object socks5Lock = new object();

		// Token: 0x040006C1 RID: 1729
		private int socks5;

		// Token: 0x040006C2 RID: 1730
		public object chainLock = new object();

		// Token: 0x040006C3 RID: 1731
		private int chain;

		// Token: 0x040006C4 RID: 1732
		public object workingLock = new object();

		// Token: 0x040006C5 RID: 1733
		private int working;

		// Token: 0x040006C6 RID: 1734
		public object notWorkingLock = new object();

		// Token: 0x040006C7 RID: 1735
		private int notWorking;

		// Token: 0x040006C8 RID: 1736
		private int botsAmount = 1;

		// Token: 0x040006C9 RID: 1737
		private bool onlyUntested = true;

		// Token: 0x040006CA RID: 1738
		private int timeout = 2;

		// Token: 0x040006CB RID: 1739
		public static readonly int maximumBots = 200;
	}
}
