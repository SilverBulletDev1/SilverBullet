using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PluginFramework;
using PluginFramework.Attributes;
using RuriLib.Interfaces;
using RuriLib.Models;
using RuriLib.ViewModels;

namespace OpenBullet.Plugins
{
	// Token: 0x0200014C RID: 332
	public static class Check
	{
		// Token: 0x060009A1 RID: 2465 RVA: 0x00031F68 File Offset: 0x00030168
		public static bool InputProperty(PropertyInfo property)
		{
			if (property.GetCustomAttributes().Count((Attribute a) => a is InputField) != 1)
			{
				return false;
			}
			InputField attribute = property.GetCustomAttribute<InputField>();
			if (!Check._requiredPropertyTypes.ContainsKey(attribute.GetType()))
			{
				throw new Exception(string.Format("Unknown attribute type {0}", attribute.GetType()));
			}
			Type requiredType = Check._requiredPropertyTypes[attribute.GetType()];
			if (property.PropertyType != requiredType)
			{
				throw new Exception(string.Format("The property {0} must be of type {1}", property.Name, requiredType));
			}
			return true;
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0003200C File Offset: 0x0003020C
		public static bool Method(IPlugin plugin, MethodInfo method)
		{
			if (method.GetCustomAttributes().Count((Attribute a) => a is Button) != 1)
			{
				return false;
			}
			method.GetCustomAttribute<Button>();
			ParameterInfo[] parameters = method.GetParameters();
			if (parameters.Length > 1)
			{
				return false;
			}
			if (parameters.Length == 1)
			{
				if (!parameters.Any((ParameterInfo p) => p.ParameterType == typeof(IApplication)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400075B RID: 1883
		private static Dictionary<Type, Type> _requiredPropertyTypes = new Dictionary<Type, Type>
		{
			{
				typeof(InfoText),
				typeof(string)
			},
			{
				typeof(Text),
				typeof(string)
			},
			{
				typeof(Numeric),
				typeof(int)
			},
			{
				typeof(Checkbox),
				typeof(bool)
			},
			{
				typeof(TextMulti),
				typeof(string[])
			},
			{
				typeof(FilePicker),
				typeof(string)
			},
			{
				typeof(Dropdown),
				typeof(string)
			},
			{
				typeof(WordlistPicker),
				typeof(Wordlist)
			},
			{
				typeof(ConfigPicker),
				typeof(ConfigViewModel)
			}
		};
	}
}
