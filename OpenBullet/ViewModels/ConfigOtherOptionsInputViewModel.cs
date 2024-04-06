using System;
using System.Collections.ObjectModel;
using System.Linq;
using RuriLib.ViewModels;

namespace OpenBullet.ViewModels
{
	// Token: 0x02000121 RID: 289
	public class ConfigOtherOptionsInputViewModel : ViewModelBase
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x0002CEA5 File Offset: 0x0002B0A5
		// (set) Token: 0x060007ED RID: 2029 RVA: 0x0002CEB0 File Offset: 0x0002B0B0
		public ObservableCollection<CustomInput> InputsList
		{
			get
			{
				return this.inputsList;
			}
			set
			{
				if (object.Equals(this.inputsList, value))
				{
					return;
				}
				this.inputsList = value;
				this.OnPropertyChanged("InputsList");
			}
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0002CEE0 File Offset: 0x0002B0E0
		public CustomInput GetInputById(int id)
		{
			return this.InputsList.Where((CustomInput x) => x.Id == id).First<CustomInput>();
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0002CF16 File Offset: 0x0002B116
		public void RemoveInputById(int id)
		{
			this.InputsList.Remove(this.GetInputById(id));
		}

		// Token: 0x0400066B RID: 1643
		private ObservableCollection<CustomInput> inputsList = new ObservableCollection<CustomInput>();
	}
}
