using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using RuriLib;
using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
	// Token: 0x02000141 RID: 321
	public class StackerBlockViewModel : ViewModelBase
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x00030B60 File Offset: 0x0002ED60
		// (set) Token: 0x06000930 RID: 2352 RVA: 0x00030B68 File Offset: 0x0002ED68
		public int Id
		{
			get
			{
				return this.id;
			}
			set
			{
				if (this.id == value)
				{
					return;
				}
				this.id = value;
				this.OnPropertyChanged("Id");
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000931 RID: 2353 RVA: 0x00030B95 File Offset: 0x0002ED95
		// (set) Token: 0x06000932 RID: 2354 RVA: 0x00030BA0 File Offset: 0x0002EDA0
		public bool Selected
		{
			get
			{
				return this.selected;
			}
			set
			{
				if (this.selected == value)
				{
					return;
				}
				this.selected = value;
				this.OnPropertyChanged("Selected");
				this.OnPropertyChanged("BorderColor");
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000933 RID: 2355 RVA: 0x00030BD8 File Offset: 0x0002EDD8
		public SolidColorBrush BorderColor
		{
			get
			{
				return new SolidColorBrush(this.Selected ? Colors.White : Colors.Black);
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000934 RID: 2356 RVA: 0x00030BF3 File Offset: 0x0002EDF3
		public LinearGradientBrush Color
		{
			get
			{
				if (!this.Block.Disabled)
				{
					return this.GetBlockColor();
				}
				return Colors.Gray.GetLinearGradientBrush();
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x00030C13 File Offset: 0x0002EE13
		public SolidColorBrush Foreground
		{
			get
			{
				return new SolidColorBrush(this.Block.IsSelenium ? Colors.White : Colors.Black);
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x00030C33 File Offset: 0x0002EE33
		// (set) Token: 0x06000937 RID: 2359 RVA: 0x00030C3C File Offset: 0x0002EE3C
		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				if (this.height == value)
				{
					return;
				}
				this.height = value;
				this.OnPropertyChanged("Height");
				this.OnPropertyChanged("FontSize");
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x00030C74 File Offset: 0x0002EE74
		public int FontSize
		{
			get
			{
				return this.Height / 3;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x00030C7E File Offset: 0x0002EE7E
		// (set) Token: 0x0600093A RID: 2362 RVA: 0x00030C88 File Offset: 0x0002EE88
		public Page Page
		{
			get
			{
				return this.page;
			}
			set
			{
				if (object.Equals(this.page, value))
				{
					return;
				}
				this.page = value;
				this.OnPropertyChanged("Page");
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x00030CB8 File Offset: 0x0002EEB8
		// (set) Token: 0x0600093C RID: 2364 RVA: 0x00030CC0 File Offset: 0x0002EEC0
		public BlockBase Block
		{
			get
			{
				return this.block;
			}
			set
			{
				if (object.Equals(this.block, value))
				{
					return;
				}
				this.block = value;
				this.OnPropertyChanged("Color");
				this.OnPropertyChanged("Foreground");
				this.OnPropertyChanged("Block");
			}
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00030D08 File Offset: 0x0002EF08
		public void Disable()
		{
			if (this.block.GetType() == typeof(BlockLSCode))
			{
				return;
			}
			this.Block.Disabled = !this.Block.Disabled;
			this.OnPropertyChanged("Color");
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00030D56 File Offset: 0x0002EF56
		public void UpdateHeight(int height)
		{
			this.Height = height;
			this.OnPropertyChanged("Height");
			this.OnPropertyChanged("FontSize");
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00030D78 File Offset: 0x0002EF78
		public StackerBlockViewModel(BlockBase block, Random rand)
		{
			this.Id = rand.Next();
			this.Block = block;
			this.OnPropertyChanged("Label");
			this.OnPropertyChanged("Color");
			if (SB.BlockMappings.Any((ValueTuple<Type, Type, LinearGradientBrush> m) => m.Item1 == block.GetType()))
			{
				this.Page = Activator.CreateInstance(SB.BlockMappings.First((ValueTuple<Type, Type, LinearGradientBrush> m) => m.Item1 == block.GetType()).Item2, new object[] { block }) as Page;
				return;
			}
			throw new Exception("Tried to initialize a page for the block type " + block.GetType().Name + " but it wasn't found in the mappings");
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00030E3D File Offset: 0x0002F03D
		public LinearGradientBrush GetBlockColor()
		{
			return SB.BlockMappings.First((ValueTuple<Type, Type, LinearGradientBrush> m) => m.Item1 == this.block.GetType()).Item3;
		}

		// Token: 0x04000729 RID: 1833
		private int id;

		// Token: 0x0400072A RID: 1834
		private bool selected;

		// Token: 0x0400072B RID: 1835
		private int height;

		// Token: 0x0400072C RID: 1836
		private Page page;

		// Token: 0x0400072D RID: 1837
		private BlockBase block;
	}
}
