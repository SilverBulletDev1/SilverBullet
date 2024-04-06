using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using OpenBullet.Views.UserControls;
using PluginFramework.Attributes;
using RuriLib.ViewModels;

namespace OpenBullet.Plugins
{
	// Token: 0x0200014A RID: 330
	public static class Build
	{
		// Token: 0x0600099F RID: 2463 RVA: 0x00031B38 File Offset: 0x0002FD38
		public static UserControlContainer InputField(object plugin, PropertyInfo property, ViewModelBase viewModel = null)
		{
			InputField attribute = property.GetCustomAttribute<InputField>();
			object defaultValue = Convert.ChangeType(property.GetValue(plugin), property.PropertyType);
			IControl control;
			if (!(attribute is InfoText))
			{
				Text a = attribute as Text;
				if (a == null)
				{
					Numeric a2 = attribute as Numeric;
					if (a2 == null)
					{
						if (!(attribute is Checkbox))
						{
							TextMulti a3 = attribute as TextMulti;
							if (a3 == null)
							{
								FilePicker a4 = attribute as FilePicker;
								if (a4 == null)
								{
									Dropdown a5 = attribute as Dropdown;
									if (a5 == null)
									{
										if (!(attribute is WordlistPicker))
										{
											if (!(attribute is ConfigPicker))
											{
												throw new NotImplementedException();
											}
											control = new UserControlConfig();
										}
										else
										{
											control = new UserControlWordlist();
										}
									}
									else
									{
										if (Build.<>o__0.<>p__6 == null)
										{
											Build.<>o__0.<>p__6 = CallSite<Func<CallSite, Type, object, string[], ViewModelBase, UserControlDropdown>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Build), new CSharpArgumentInfo[]
											{
												CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
												CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
												CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
												CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
											}));
										}
										control = Build.<>o__0.<>p__6.Target(Build.<>o__0.<>p__6, typeof(UserControlDropdown), defaultValue, a5.options, viewModel);
									}
								}
								else
								{
									if (Build.<>o__0.<>p__5 == null)
									{
										Build.<>o__0.<>p__5 = CallSite<Func<CallSite, Type, object, string, UserControlFilePicker>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Build), new CSharpArgumentInfo[]
										{
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
											CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
										}));
									}
									control = Build.<>o__0.<>p__5.Target(Build.<>o__0.<>p__5, typeof(UserControlFilePicker), defaultValue, a4.filter);
								}
							}
							else
							{
								if (Build.<>o__0.<>p__4 == null)
								{
									Build.<>o__0.<>p__4 = CallSite<Func<CallSite, Type, object, bool, ViewModelBase, UserControlTextMulti>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Build), new CSharpArgumentInfo[]
									{
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
										CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
									}));
								}
								control = Build.<>o__0.<>p__4.Target(Build.<>o__0.<>p__4, typeof(UserControlTextMulti), defaultValue, a3.readOnly, viewModel);
							}
						}
						else
						{
							if (Build.<>o__0.<>p__3 == null)
							{
								Build.<>o__0.<>p__3 = CallSite<Func<CallSite, Type, object, ViewModelBase, UserControlCheckbox>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Build), new CSharpArgumentInfo[]
								{
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
									CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
								}));
							}
							control = Build.<>o__0.<>p__3.Target(Build.<>o__0.<>p__3, typeof(UserControlCheckbox), defaultValue, viewModel);
						}
					}
					else
					{
						if (Build.<>o__0.<>p__2 == null)
						{
							Build.<>o__0.<>p__2 = CallSite<Func<CallSite, Type, object, int, int, ViewModelBase, UserControlNumeric>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Build), new CSharpArgumentInfo[]
							{
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
								CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
							}));
						}
						control = Build.<>o__0.<>p__2.Target(Build.<>o__0.<>p__2, typeof(UserControlNumeric), defaultValue, a2.minimum, a2.maximum, viewModel);
					}
				}
				else
				{
					if (Build.<>o__0.<>p__1 == null)
					{
						Build.<>o__0.<>p__1 = CallSite<Func<CallSite, Type, object, bool, ViewModelBase, UserControlText>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Build), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
						}));
					}
					control = Build.<>o__0.<>p__1.Target(Build.<>o__0.<>p__1, typeof(UserControlText), defaultValue, a.readOnly, viewModel);
				}
			}
			else
			{
				if (Build.<>o__0.<>p__0 == null)
				{
					Build.<>o__0.<>p__0 = CallSite<Func<CallSite, Type, object, ViewModelBase, UserControlInfoText>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(Build), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
					}));
				}
				control = Build.<>o__0.<>p__0.Target(Build.<>o__0.<>p__0, typeof(UserControlInfoText), defaultValue, viewModel);
			}
			return new UserControlContainer(property.Name, control, attribute.label, attribute.tooltip);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x00031F4F File Offset: 0x0003014F
		public static ButtonContainer Button(MethodInfo method, PluginControl pluginControl)
		{
			return new ButtonContainer(method.GetCustomAttribute<Button>().text, method.Name, pluginControl);
		}
	}
}
