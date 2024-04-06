using System;
using System.Runtime.CompilerServices;
using System.Windows;
using Newtonsoft.Json;
using RuriLib.ViewModels;

namespace OpenBullet.Models
{
	// Token: 0x02000157 RID: 343
	public class Source : ViewModelBase
	{
		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x00032655 File Offset: 0x00030855
		// (set) Token: 0x060009C6 RID: 2502 RVA: 0x00032660 File Offset: 0x00030860
		public int Id
		{
			[CompilerGenerated]
			get
			{
				return this.<Id>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (this.<Id>k__BackingField == value)
				{
					return;
				}
				this.<Id>k__BackingField = value;
				this.OnPropertyChanged("Id");
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060009C7 RID: 2503 RVA: 0x0003268D File Offset: 0x0003088D
		// (set) Token: 0x060009C8 RID: 2504 RVA: 0x00032698 File Offset: 0x00030898
		public string ApiUrl
		{
			get
			{
				return this.apiUrl;
			}
			set
			{
				if (string.Equals(this.apiUrl, value, StringComparison.Ordinal))
				{
					return;
				}
				this.apiUrl = value;
				this.OnPropertyChanged("ApiUrl");
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060009C9 RID: 2505 RVA: 0x000326C9 File Offset: 0x000308C9
		// (set) Token: 0x060009CA RID: 2506 RVA: 0x000326D4 File Offset: 0x000308D4
		public Source.AuthMode Auth
		{
			get
			{
				return this.auth;
			}
			set
			{
				if (this.auth == value)
				{
					return;
				}
				this.auth = value;
				this.OnPropertyChanged("Auth");
				this.OnPropertyChanged("ApiKeyVisible");
				this.OnPropertyChanged("UserPassVisible");
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060009CB RID: 2507 RVA: 0x00032717 File Offset: 0x00030917
		// (set) Token: 0x060009CC RID: 2508 RVA: 0x00032720 File Offset: 0x00030920
		public string ApiKey
		{
			get
			{
				return this.apiKey;
			}
			set
			{
				if (string.Equals(this.apiKey, value, StringComparison.Ordinal))
				{
					return;
				}
				this.apiKey = value;
				this.OnPropertyChanged("ApiKey");
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060009CD RID: 2509 RVA: 0x00032751 File Offset: 0x00030951
		// (set) Token: 0x060009CE RID: 2510 RVA: 0x0003275C File Offset: 0x0003095C
		public string Username
		{
			get
			{
				return this.username;
			}
			set
			{
				if (string.Equals(this.username, value, StringComparison.Ordinal))
				{
					return;
				}
				this.username = value;
				this.OnPropertyChanged("Username");
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060009CF RID: 2511 RVA: 0x0003278D File Offset: 0x0003098D
		// (set) Token: 0x060009D0 RID: 2512 RVA: 0x00032798 File Offset: 0x00030998
		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				if (string.Equals(this.password, value, StringComparison.Ordinal))
				{
					return;
				}
				this.password = value;
				this.OnPropertyChanged("Password");
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060009D1 RID: 2513 RVA: 0x000327C9 File Offset: 0x000309C9
		// (set) Token: 0x060009D2 RID: 2514 RVA: 0x000327D4 File Offset: 0x000309D4
		[JsonIgnore]
		public bool AuthInitialized
		{
			[CompilerGenerated]
			get
			{
				return this.<AuthInitialized>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (this.<AuthInitialized>k__BackingField == value)
				{
					return;
				}
				this.<AuthInitialized>k__BackingField = value;
				this.OnPropertyChanged("AuthInitialized");
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060009D3 RID: 2515 RVA: 0x00032801 File Offset: 0x00030A01
		[JsonIgnore]
		public Visibility ApiKeyVisible
		{
			get
			{
				if (this.Auth != Source.AuthMode.ApiKey)
				{
					return Visibility.Collapsed;
				}
				return Visibility.Visible;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060009D4 RID: 2516 RVA: 0x0003280E File Offset: 0x00030A0E
		[JsonIgnore]
		public Visibility UserPassVisible
		{
			get
			{
				if (this.Auth != Source.AuthMode.UserPass)
				{
					return Visibility.Collapsed;
				}
				return Visibility.Visible;
			}
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0003281C File Offset: 0x00030A1C
		public Source(int id)
		{
			this.Id = id;
		}

		// Token: 0x0400076C RID: 1900
		private string apiUrl = "";

		// Token: 0x0400076D RID: 1901
		private Source.AuthMode auth;

		// Token: 0x0400076E RID: 1902
		private string apiKey = "";

		// Token: 0x0400076F RID: 1903
		private string username = "";

		// Token: 0x04000770 RID: 1904
		private string password = "";

		// Token: 0x02000158 RID: 344
		public enum AuthMode
		{
			// Token: 0x04000773 RID: 1907
			ApiKey,
			// Token: 0x04000774 RID: 1908
			UserPass
		}
	}
}
