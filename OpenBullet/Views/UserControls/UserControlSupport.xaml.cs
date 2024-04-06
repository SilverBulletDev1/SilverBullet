using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OpenBullet.Views.UserControls
{
	// Token: 0x0200006C RID: 108
	public partial class UserControlSupport : UserControl, INotifyPropertyChanged
	{
		// Token: 0x060002A4 RID: 676 RVA: 0x0000B921 File Offset: 0x00009B21
		public UserControlSupport()
		{
			this.InitializeComponent();
			base.DataContext = this;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060002A5 RID: 677 RVA: 0x0000B940 File Offset: 0x00009B40
		// (remove) Token: 0x060002A6 RID: 678 RVA: 0x0000B978 File Offset: 0x00009B78
		public event RoutedEventHandler Click;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060002A7 RID: 679 RVA: 0x0000B9B0 File Offset: 0x00009BB0
		// (remove) Token: 0x060002A8 RID: 680 RVA: 0x0000B9E8 File Offset: 0x00009BE8
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000BA1D File Offset: 0x00009C1D
		// (set) Token: 0x060002AA RID: 682 RVA: 0x0000BA28 File Offset: 0x00009C28
		public Stretch Stretch
		{
			get
			{
				return this._stretch;
			}
			set
			{
				if (this._stretch == value)
				{
					return;
				}
				if (!object.Equals(this._stretch, value))
				{
					this._stretch = value;
					this.RaisePropertyChanged("Stretch");
				}
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000BA6F File Offset: 0x00009C6F
		// (set) Token: 0x060002AC RID: 684 RVA: 0x0000BA78 File Offset: 0x00009C78
		public string SupportName
		{
			get
			{
				return this.supportName;
			}
			set
			{
				if (string.Equals(this.supportName, value, StringComparison.Ordinal))
				{
					return;
				}
				this.supportName = value;
				this.RaisePropertyChanged("SupportName");
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000BAA9 File Offset: 0x00009CA9
		// (set) Token: 0x060002AE RID: 686 RVA: 0x0000BAB4 File Offset: 0x00009CB4
		public SolidColorBrush BackgroundButton
		{
			get
			{
				return this.backgroundButton;
			}
			set
			{
				if (object.Equals(this.backgroundButton, value))
				{
					return;
				}
				this.backgroundButton = value;
				this.RaisePropertyChanged("BackgroundButton");
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000BAE4 File Offset: 0x00009CE4
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x0000BAEC File Offset: 0x00009CEC
		public string Url
		{
			[CompilerGenerated]
			get
			{
				return this.<Url>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				if (string.Equals(this.<Url>k__BackingField, value, StringComparison.Ordinal))
				{
					return;
				}
				this.<Url>k__BackingField = value;
				this.RaisePropertyChanged("Url");
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000BB20 File Offset: 0x00009D20
		public void SetImage(Uri imgSource)
		{
			BitmapImage bitmap = new BitmapImage();
			bitmap.BeginInit();
			bitmap.UriSource = imgSource;
			bitmap.EndInit();
			this.imageBrush.ImageSource = bitmap;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000BB52 File Offset: 0x00009D52
		private void Border_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (this._mouseDown)
			{
				RoutedEventHandler click = this.Click;
				if (click != null)
				{
					click(this, e);
				}
			}
			this._mouseDown = false;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000BB76 File Offset: 0x00009D76
		private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			this._mouseDown = true;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000BB7F File Offset: 0x00009D7F
		private void Border_MouseLeave(object sender, MouseEventArgs e)
		{
			this._mouseDown = false;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000BB88 File Offset: 0x00009D88
		private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000BBA4 File Offset: 0x00009DA4
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.Start(this.Url);
			}
			catch
			{
			}
		}

		// Token: 0x0400020F RID: 527
		private bool _mouseDown;

		// Token: 0x04000212 RID: 530
		private Stretch _stretch = Stretch.Fill;

		// Token: 0x04000213 RID: 531
		private string supportName;

		// Token: 0x04000214 RID: 532
		private SolidColorBrush backgroundButton;
	}
}
