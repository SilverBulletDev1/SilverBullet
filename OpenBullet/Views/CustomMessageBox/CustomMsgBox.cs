using System;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro.IconPacks;

namespace OpenBullet.Views.CustomMessageBox
{
	// Token: 0x02000119 RID: 281
	public sealed class CustomMsgBox
	{
		// Token: 0x0600079D RID: 1949 RVA: 0x0002BC94 File Offset: 0x00029E94
		public static void Show(string message, bool isRTL = false)
		{
			using (CustomMsgBoxWindow msg = new CustomMsgBoxWindow())
			{
				msg.Title = "Message";
				msg.TitleLabel.Content = "Message";
				msg.Message.Text = message;
				msg.BorderBrush = new SolidColorBrush(Color.FromRgb(3, 169, 244));
				msg.BtnCancel.Visibility = Visibility.Collapsed;
				if (isRTL)
				{
					msg.FlowDirection = FlowDirection.RightToLeft;
				}
				msg.BtnOk.Focus();
				msg.ShowDialog();
			}
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0002BD30 File Offset: 0x00029F30
		public static void Show(string message, string title, bool isRTL = false)
		{
			using (CustomMsgBoxWindow msg = new CustomMsgBoxWindow())
			{
				msg.Title = title;
				msg.TitleLabel.Content = title;
				msg.Message.Text = message;
				msg.BorderBrush = new SolidColorBrush(Color.FromRgb(3, 169, 244));
				msg.BtnCancel.Visibility = Visibility.Collapsed;
				msg.MsgIcon.Kind = PackIconMaterialKind.InformationOutline;
				msg.MsgIcon.Foreground = Brushes.LightSteelBlue;
				if (isRTL)
				{
					msg.FlowDirection = FlowDirection.RightToLeft;
				}
				msg.BtnOk.Focus();
				msg.ShowDialog();
			}
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0002BDE4 File Offset: 0x00029FE4
		public static void ShowError(string errorMessage, bool isRTL = false)
		{
			try
			{
				using (CustomMsgBoxWindow msg = new CustomMsgBoxWindow())
				{
					msg.Title = "Error";
					msg.TitleLabel.Content = "Error";
					msg.Message.Text = errorMessage;
					msg.BorderBrush = Brushes.Red;
					msg.BtnCancel.Visibility = Visibility.Collapsed;
					msg.MsgIcon.Kind = PackIconMaterialKind.CloseBoxOutline;
					msg.MsgIcon.Foreground = Brushes.Orange;
					if (isRTL)
					{
						msg.FlowDirection = FlowDirection.RightToLeft;
					}
					msg.BtnOk.Focus();
					msg.ShowDialog();
				}
			}
			catch (Exception)
			{
				MessageBox.Show(errorMessage);
			}
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0002BEA8 File Offset: 0x0002A0A8
		public static void ShowError(string errorMessage, string errorTitle, bool isRTL = false)
		{
			try
			{
				using (CustomMsgBoxWindow msg = new CustomMsgBoxWindow())
				{
					msg.Title = errorTitle;
					msg.TitleLabel.Content = errorTitle;
					msg.Message.Text = errorMessage;
					msg.BorderBrush = Brushes.Red;
					msg.BtnCancel.Visibility = Visibility.Collapsed;
					msg.MsgIcon.Kind = PackIconMaterialKind.CloseBoxOutline;
					msg.MsgIcon.Foreground = Brushes.Red;
					if (isRTL)
					{
						msg.FlowDirection = FlowDirection.RightToLeft;
					}
					msg.BtnOk.Focus();
					msg.ShowDialog();
				}
			}
			catch (Exception)
			{
				MessageBox.Show(errorMessage);
			}
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0002BF64 File Offset: 0x0002A164
		public static void ShowWarning(string warningMessage, bool isRTL = false)
		{
			try
			{
				using (CustomMsgBoxWindow msg = new CustomMsgBoxWindow())
				{
					msg.Title = "Warning";
					msg.TitleLabel.Content = "Warning";
					msg.Message.Text = warningMessage;
					msg.BorderBrush = Brushes.Orange;
					msg.BtnCancel.Visibility = Visibility.Collapsed;
					msg.MsgIcon.Kind = PackIconMaterialKind.AlertBoxOutline;
					msg.MsgIcon.Foreground = Brushes.Orange;
					if (isRTL)
					{
						msg.FlowDirection = FlowDirection.RightToLeft;
					}
					msg.BtnOk.Focus();
					msg.ShowDialog();
				}
			}
			catch (Exception)
			{
				MessageBox.Show(warningMessage);
			}
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0002C028 File Offset: 0x0002A228
		public static void ShowWarning(string warningMessage, string warningTitle, bool isRTL = false)
		{
			try
			{
				using (CustomMsgBoxWindow msg = new CustomMsgBoxWindow())
				{
					msg.Title = warningTitle;
					msg.TitleLabel.Content = warningTitle;
					msg.Message.Text = warningMessage;
					msg.BorderBrush = Brushes.Orange;
					msg.BtnCancel.Visibility = Visibility.Collapsed;
					msg.MsgIcon.Kind = PackIconMaterialKind.AlertBoxOutline;
					msg.MsgIcon.Foreground = Brushes.Orange;
					if (isRTL)
					{
						msg.FlowDirection = FlowDirection.RightToLeft;
					}
					msg.BtnOk.Focus();
					msg.ShowDialog();
				}
			}
			catch (Exception)
			{
				MessageBox.Show(warningMessage, warningTitle);
			}
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0002C0E4 File Offset: 0x0002A2E4
		public static MessageBoxResult ShowWithCancel(string message, bool isRTL = false)
		{
			MessageBoxResult messageBoxResult;
			try
			{
				using (CustomMsgBoxWindow msg = new CustomMsgBoxWindow())
				{
					msg.Title = "Message";
					msg.TitleLabel.Content = "Message";
					msg.Message.Text = message;
					msg.BorderBrush = new SolidColorBrush(Color.FromRgb(3, 169, 244));
					if (isRTL)
					{
						msg.FlowDirection = FlowDirection.RightToLeft;
					}
					msg.BtnOk.Focus();
					msg.ShowDialog();
					messageBoxResult = ((msg.Result == MessageBoxResult.OK) ? MessageBoxResult.OK : MessageBoxResult.Cancel);
				}
			}
			catch (Exception)
			{
				MessageBox.Show(message);
				messageBoxResult = MessageBoxResult.Cancel;
			}
			return messageBoxResult;
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0002C19C File Offset: 0x0002A39C
		public static MessageBoxResult ShowWithCancel(string message, string title, bool isRTL = false)
		{
			MessageBoxResult messageBoxResult;
			try
			{
				using (CustomMsgBoxWindow msg = new CustomMsgBoxWindow())
				{
					msg.Title = title;
					msg.TitleLabel.Content = title;
					msg.Message.Text = message;
					msg.BorderBrush = new SolidColorBrush(Color.FromRgb(3, 169, 244));
					if (isRTL)
					{
						msg.FlowDirection = FlowDirection.RightToLeft;
					}
					msg.BtnOk.Focus();
					msg.ShowDialog();
					messageBoxResult = ((msg.Result == MessageBoxResult.OK) ? MessageBoxResult.OK : MessageBoxResult.Cancel);
				}
			}
			catch (Exception)
			{
				MessageBox.Show(message);
				messageBoxResult = MessageBoxResult.Cancel;
			}
			return messageBoxResult;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0002C24C File Offset: 0x0002A44C
		public static MessageBoxResult ShowWithCancel(string message, bool isError, bool isRTL = false)
		{
			MessageBoxResult messageBoxResult;
			try
			{
				using (CustomMsgBoxWindow msg = new CustomMsgBoxWindow())
				{
					msg.Title = "Message";
					msg.TitleLabel.Content = "Message";
					msg.Message.Text = message;
					if (isRTL)
					{
						msg.FlowDirection = FlowDirection.RightToLeft;
					}
					msg.BtnOk.Focus();
					msg.ShowDialog();
					messageBoxResult = ((msg.Result == MessageBoxResult.OK) ? MessageBoxResult.OK : MessageBoxResult.Cancel);
				}
			}
			catch (Exception)
			{
				MessageBox.Show(message);
				messageBoxResult = MessageBoxResult.Cancel;
			}
			return messageBoxResult;
		}

		// Token: 0x0400064C RID: 1612
		private const string MessageBoxTitle = "Message";
	}
}
