using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using RuriLib;
using RuriLib.Functions.Crypto;
using RuriLib.Functions.UserAgent;

namespace OpenBullet.Views.StackerBlocks
{
	// Token: 0x02000095 RID: 149
	public partial class PageBlockFunction : Page
	{
		// Token: 0x060003CF RID: 975 RVA: 0x00011C74 File Offset: 0x0000FE74
		public PageBlockFunction(BlockFunction block)
		{
			this.InitializeComponent();
			this.vm = block;
			base.DataContext = this.vm;
			foreach (string t in Enum.GetNames(typeof(BlockFunction.Function)))
			{
				this.functionTypeCombobox.Items.Add(t);
			}
			this.functionTypeCombobox.SelectedIndex = (int)this.vm.FunctionType;
			foreach (string h in Enum.GetNames(typeof(Hash)))
			{
				this.hashTypeCombobox.Items.Add(h);
				this.hmacHashTypeCombobox.Items.Add(h);
				this.kdfAlgorithmCombobox.Items.Add(h);
			}
			this.hashTypeCombobox.SelectedIndex = (int)this.vm.HashType;
			this.hmacHashTypeCombobox.SelectedIndex = (int)this.vm.HashType;
			this.kdfAlgorithmCombobox.SelectedIndex = (int)this.vm.KdfAlgorithm;
			foreach (string b in Enum.GetNames(typeof(UserAgent.Browser)))
			{
				this.randomUABrowserCombobox.Items.Add(b);
			}
			this.randomUABrowserCombobox.SelectedIndex = (int)this.vm.UserAgentBrowser;
			foreach (string i in Enum.GetNames(typeof(CipherMode)))
			{
				this.aesModeCombobox.Items.Add(i);
			}
			this.aesModeCombobox.SelectedIndex = this.vm.AesMode - CipherMode.CBC;
			foreach (string p in Enum.GetNames(typeof(PaddingMode)))
			{
				this.aesPaddingCombobox.Items.Add(p);
			}
			foreach (string d in Enum.GetNames(typeof(BlockFunction.DateToUnixTimeType)))
			{
				this.dateToUnixTimeCombobox.Items.Add(d);
			}
			this.encCombobox.Items.Add("utf-8");
			this.encCombobox.Items.Add("windows-1251");
			this.encCombobox.Items.Add(1251);
			foreach (string e in Enum.GetNames(typeof(BlockFunction.EncodingMethods)))
			{
				this.encFuncCombobox.Items.Add(e);
			}
			foreach (string e2 in Enum.GetNames(typeof(BlockFunction.ScryptMethods)))
			{
				if (e2 == "Encode")
				{
					this.scryptMethods.Items.Add(e2);
					break;
				}
			}
			foreach (string e3 in Enum.GetNames(typeof(BlockFunction.BCryptMethods)))
			{
				this.bcryptMethods.Items.Add(e3);
			}
			this.bcryptMethods.SelectedIndex = (int)this.vm.BCryptMeth;
			this.aesPaddingCombobox.SelectedIndex = this.vm.AesPadding - PaddingMode.None;
			this.dictionaryRTB.AppendText(this.vm.GetDictionary());
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00012284 File Offset: 0x00010484
		private void functionTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.FunctionType = (BlockFunction.Function)((ComboBox)e.OriginalSource).SelectedIndex;
			try
			{
				this.functionInfoTextblock.Text = this.infoDic[this.vm.FunctionType.ToString()];
			}
			catch
			{
				this.functionInfoTextblock.Text = "No additional information available for this function";
			}
			BlockFunction.Function functionType = this.vm.FunctionType;
			if (functionType <= BlockFunction.Function.Replace)
			{
				switch (functionType)
				{
				case BlockFunction.Function.Hash:
					this.functionTabControl.SelectedIndex = 1;
					return;
				case BlockFunction.Function.HMAC:
					this.functionTabControl.SelectedIndex = 2;
					return;
				case BlockFunction.Function.Translate:
					this.functionTabControl.SelectedIndex = 3;
					return;
				default:
					if (functionType == BlockFunction.Function.DateToUnixTime)
					{
						goto IL_161;
					}
					if (functionType == BlockFunction.Function.Replace)
					{
						this.functionTabControl.SelectedIndex = 5;
						return;
					}
					break;
				}
			}
			else if (functionType <= BlockFunction.Function.PBKDF2PKCS5)
			{
				if (functionType == BlockFunction.Function.RegexMatch)
				{
					this.functionTabControl.SelectedIndex = 6;
					return;
				}
				switch (functionType)
				{
				case BlockFunction.Function.Encoding:
					this.functionTabControl.SelectedIndex = 19;
					return;
				case BlockFunction.Function.UnixTimeToDate:
					goto IL_161;
				case BlockFunction.Function.RandomNum:
					this.functionTabControl.SelectedIndex = 7;
					return;
				case BlockFunction.Function.CountOccurrences:
					this.functionTabControl.SelectedIndex = 8;
					return;
				case BlockFunction.Function.RSAEncrypt:
					this.functionTabControl.SelectedIndex = 9;
					return;
				case BlockFunction.Function.RSAPKCS1PAD2:
					this.functionTabControl.SelectedIndex = 11;
					return;
				case BlockFunction.Function.CharAt:
					this.functionTabControl.SelectedIndex = 12;
					return;
				case BlockFunction.Function.Split:
					this.functionTabControl.SelectedIndex = 18;
					return;
				case BlockFunction.Function.Remove:
					this.functionTabControl.SelectedIndex = 17;
					return;
				case BlockFunction.Function.Substring:
					this.functionTabControl.SelectedIndex = 13;
					return;
				case BlockFunction.Function.GetRandomUA:
					this.functionTabControl.SelectedIndex = 14;
					return;
				case BlockFunction.Function.AESEncrypt:
				case BlockFunction.Function.AESDecrypt:
					this.functionTabControl.SelectedIndex = 15;
					return;
				case BlockFunction.Function.PBKDF2PKCS5:
					this.functionTabControl.SelectedIndex = 16;
					return;
				}
			}
			else
			{
				if (functionType == BlockFunction.Function.SCrypt)
				{
					this.functionTabControl.SelectedIndex = 20;
					return;
				}
				if (functionType == BlockFunction.Function.BCrypt)
				{
					this.functionTabControl.SelectedIndex = 21;
					return;
				}
			}
			this.functionTabControl.SelectedIndex = 0;
			return;
			IL_161:
			this.functionTabControl.SelectedIndex = 4;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x000124EC File Offset: 0x000106EC
		private void dictionaryRTB_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.vm.SetDictionary(this.dictionaryRTB.Lines());
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00012504 File Offset: 0x00010704
		private void hashTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.HashType = (Hash)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00012504 File Offset: 0x00010704
		private void hmacHashTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.HashType = (Hash)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00012521 File Offset: 0x00010721
		private void aesModeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.AesMode = ((ComboBox)e.OriginalSource).SelectedIndex + CipherMode.CBC;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00012540 File Offset: 0x00010740
		private void aesPaddingCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.AesPadding = ((ComboBox)e.OriginalSource).SelectedIndex + PaddingMode.None;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001255F File Offset: 0x0001075F
		private void kdfAlgorithmCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.KdfAlgorithm = (Hash)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0001257C File Offset: 0x0001077C
		private void randomUABrowserCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.vm.UserAgentBrowser = (UserAgent.Browser)((ComboBox)e.OriginalSource).SelectedIndex;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0001259C File Offset: 0x0001079C
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				this.SetItemToComboBox();
			}
			catch
			{
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x000125C4 File Offset: 0x000107C4
		private void SetItemToComboBox()
		{
			this.dateToUnixTimeCombobox.SelectedIndex = (int)this.vm.UnixTimeType;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x000125DC File Offset: 0x000107DC
		private void scryptMethods_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				int selectedIndex = this.scryptMethods.SelectedIndex;
				if (selectedIndex != 0)
				{
					if (selectedIndex != 1)
					{
						this.stackPanelEncode.Visibility = Visibility.Collapsed;
						this.dockPanelCompInput.Visibility = Visibility.Collapsed;
					}
					else
					{
						this.stackPanelEncode.Visibility = Visibility.Collapsed;
						this.dockPanelCompInput.Visibility = Visibility.Visible;
					}
				}
				else
				{
					this.dockPanelCompInput.Visibility = Visibility.Collapsed;
					this.stackPanelEncode.Visibility = Visibility.Visible;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00012660 File Offset: 0x00010860
		private void bcryptMethods_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				int selectedIndex = this.bcryptMethods.SelectedIndex;
				if (selectedIndex != 0)
				{
					if (selectedIndex != 2)
					{
						this.dockPanelBcryptSalt.Visibility = Visibility.Collapsed;
						this.dockPanelVerifyInput.Visibility = Visibility.Collapsed;
					}
					else
					{
						this.dockPanelBcryptSalt.Visibility = Visibility.Collapsed;
						this.dockPanelVerifyInput.Visibility = Visibility.Visible;
					}
				}
				else
				{
					this.dockPanelVerifyInput.Visibility = Visibility.Collapsed;
					this.dockPanelBcryptSalt.Visibility = Visibility.Visible;
				}
			}
			catch
			{
			}
		}

		// Token: 0x040002E8 RID: 744
		private BlockFunction vm;

		// Token: 0x040002E9 RID: 745
		public Dictionary<string, string> infoDic = new Dictionary<string, string>
		{
			{ "Constant", "This will just return anything written in the input string and store it in a variable, after possibly replacing all the input variables.\nUse this to chain constants and variables together." },
			{ "Base64Encode", "Encodes a string to Base64" },
			{ "Base64Decode", "Decodes a Base64 string to text" },
			{ "Hash", "The input string will be hashed with the selected function. Remember you can chain variables if you need a salt." },
			{ "HMAC", "The input string will be hashed with the selected Hashtype with the key you input." },
			{ "Translate", "Format like headers (this: that), one per line." },
			{ "DateToUnixTime", "This turns date to Unixtime. ex(input = 2020-01-01:01-01-01 to output = 1577840461" },
			{ "Length", "This will count the Length of the input Ex(Input = apple Output = 5)" },
			{ "ToLowercase", "Turns input to Lowercase Ex(A to a)" },
			{ "ToUppercase", "Turns input to Uppercase Ex(a to A)" },
			{ "Replace", "Replaces the Input from (Replace:) with the Input from (with:) from the Input string" },
			{ "RegexMatch", "Matches the input with the given Regex (Match:)" },
			{ "URLEncode", "URLEncodes the input. Ex(https://example.com to https%3A%2F%2Fexample.com)" },
			{ "URLDecode", "URLDecodes the input. Ex(https%3A%2F%2Fexample.com to https://example.com)" },
			{ "Unescape", "Unescapes the input." },
			{ "HTMLEntityEncode", "HTMLEntity Encodes the input. Ex(© to &#169;)" },
			{ "HTMLEntityDecode", "HTMLEntity Decodes the input. Ex(&#169; to ©)" },
			{ "UnixTimeToDate", "This turns Unixtime to date. ex(input = 1577840461 to output = 2020-01-01:01-01-01" },
			{ "CurrentUnixTime", "This Returns theh Current Unixtime. ex(1577840461)" },
			{ "UnixTimeToISO8601", "This Returns theh Current Unixtime. ex(1577840461 to 2020-01-01T01:01:01.000Z)" },
			{ "RandomNum", "Generates a random number based on ranges (min max) given." },
			{ "RandomString", "?l = Lowercase, ?u = Uppercase, ?d = Digit, ?f = Uppercase + Lowercase, ?s = Symbol, ?h = Hex (Lowercase), ?m = Upper + Digits,?n = Lower + Digits ?i = Lower + Upper + Digits, ?a = Any" },
			{ "EvaluateMathString", "Evaluating string '3 * 5 + Pow(2,3)' yield int 23" },
			{ "Ceil", "This function rounds up to the input to the next full integer. Ex(input = 2.9 Output = 3" },
			{ "Floor", "This function gets rid of the numbers after the decimal. Ex(input = 2.9 Output = 2" },
			{ "Round", "This function round input to the nearest integer. Ex(input = 2.5 Output = 3" },
			{ "Compute", "Calculates the value of a math expression, for example (6+3)*5 will return 45." },
			{ "ClearCookies", "Removes/Clears all the Cookies." },
			{ "CountOccurrences", "Counts the number of times that (Find:) input occurs in the input." },
			{ "RSAEncrypt", "Encrypts data with RSA. All parameters must be provided as base64 strings" },
			{ "RSADecrypt", "Decrypts data with RSA. All parameters must be provided as base64 strings" },
			{ "RSAPKCS1PAD", "Encrypts data with RSA Pkcs1Pad2. Modulus and exponent must be provided as HEX strings." },
			{ "Delay", "Write the amount of MILLISECONDS you want to wait in the input field" },
			{ "CharAt", "Returns the character at the specified index of the string in the input field" },
			{ "Substring", "Returns a new string that is a substring of Input String. The substring begins at the specified Start Index and Length." },
			{ "Reversestring", "Reverses the the characters if the Input String. Ex(input = abc output = cba" },
			{ "Trim", "Removes whitespace from both ends of a Input string." },
			{ "GetRandomUA", "Generates a random User Agent." },
			{ "AESEncrypt", "Encrypts data with AES. All parameters must be provided as base64 strings. Uses SHA-256 to get a 256 bit key" },
			{ "AESDecrypt", "Decrypts data with AES. All parameters must be provided as base64 strings. Uses SHA-256 to get a 256 bit key" },
			{ "PBKDF2PKCS5", "Generates a key based on a password. The salt, if provided, must be a base64 string" },
			{ "Ntlm", "Generates NTLM hash" },
			{ "Scrypt", "Scrypt encoder" }
		};
	}
}
