using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.CSharp.RuntimeBinder;
using OpenBullet.Plugins;
using PluginFramework;
using RuriLib.Interfaces;
using RuriLib.ViewModels;

namespace OpenBullet.Views.UserControls
{
	// Token: 0x0200005D RID: 93
	public partial class PluginControl : UserControl
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000AB69 File Offset: 0x00008D69
		// (set) Token: 0x0600025A RID: 602 RVA: 0x0000AB71 File Offset: 0x00008D71
		public IPlugin Plugin { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000AB7A File Offset: 0x00008D7A
		// (set) Token: 0x0600025C RID: 604 RVA: 0x0000AB82 File Offset: 0x00008D82
		public ObservableCollection<UserControl> Controls { get; set; } = new ObservableCollection<UserControl>();

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000AB8B File Offset: 0x00008D8B
		// (set) Token: 0x0600025E RID: 606 RVA: 0x0000AB93 File Offset: 0x00008D93
		private List<PropertyInfo> ValidProperties { get; set; } = new List<PropertyInfo>();

		// Token: 0x0600025F RID: 607 RVA: 0x0000AB9C File Offset: 0x00008D9C
		public PluginControl(Type type, IApplication app, bool supportsPropertyChanged = false)
		{
			this.InitializeComponent();
			base.DataContext = this;
			object[] constructorParams = new object[0];
			PropertyInfo p;
			if (type.GetConstructors().Any((ConstructorInfo c) => c.GetParameters().Any((ParameterInfo p) => p.ParameterType == typeof(IApplication))))
			{
				constructorParams = new object[] { app };
			}
			this.Plugin = Activator.CreateInstance(type, constructorParams) as IPlugin;
			using (IEnumerator<PropertyInfo> enumerator = (from p in type.GetProperties()
				where Check.InputProperty(p)
				select p).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					p = enumerator.Current;
					this.ValidProperties.Add(p);
					this.Controls.Add(Build.InputField(this.Plugin, p, supportsPropertyChanged ? (this.Plugin as ViewModelBase) : null));
				}
			}
			foreach (MethodInfo i in from m in type.GetMethods()
				where Check.Method(this.Plugin, m)
				select m)
			{
				this.Controls.Add(Build.Button(i, this));
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000AD10 File Offset: 0x00008F10
		public void RunMethod(string methodName)
		{
			using (List<PropertyInfo>.Enumerator enumerator = this.ValidProperties.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PropertyInfo property = enumerator.Current;
					if (PluginControl.<>o__13.<>p__0 == null)
					{
						PluginControl.<>o__13.<>p__0 = CallSite<Action<CallSite, PropertyInfo, IPlugin, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SetValue", null, typeof(PluginControl), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					PluginControl.<>o__13.<>p__0.Target(PluginControl.<>o__13.<>p__0, property, this.Plugin, (from c in this.Controls
						where c is UserControlContainer
						select c as UserControlContainer).First((UserControlContainer c) => c.PropertyName == property.Name).GetValue());
				}
			}
			MethodInfo method = this.Plugin.GetType().GetMethod(methodName);
			ParameterInfo[] parameters = method.GetParameters();
			object[] passed = new object[0];
			if (parameters.Length == 1 && parameters.First<ParameterInfo>().ParameterType == typeof(IApplication))
			{
				passed = new object[] { SB.App };
			}
			method.Invoke(this.Plugin, passed);
		}
	}
}
