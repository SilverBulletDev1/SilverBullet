using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenBullet.Views.UserControls;
using PluginFramework;
using RuriLib;
using RuriLib.ViewModels;

namespace OpenBullet.Plugins
{
	// Token: 0x0200014F RID: 335
	public static class Loader
	{
		// Token: 0x060009AB RID: 2475 RVA: 0x000321B4 File Offset: 0x000303B4
		public static ValueTuple<IEnumerable<PluginControl>, IEnumerable<IBlockPlugin>, IEnumerable<string>> LoadPlugins(string folder)
		{
			List<PluginControl> plugins = new List<PluginControl>();
			List<IBlockPlugin> blockPlugins = new List<IBlockPlugin>();
			List<string> pluginNames = new List<string>();
			foreach (string dll in Directory.GetFiles(folder, "*.dll"))
			{
				Assembly asm = Assembly.LoadFrom(dll);
				string depFolder = Path.Combine(SB.pluginsFolder, Path.GetFileNameWithoutExtension(dll));
				if (Directory.Exists(depFolder))
				{
					Loader.Hook(new string[] { depFolder });
				}
				Loader.LoadDependencies(asm.GetReferencedAssemblies());
				foreach (Type type in asm.GetTypes())
				{
					if (type.GetInterface("IPlugin") == typeof(IPlugin))
					{
						pluginNames.Add(asm.GetName().Name);
						plugins.Add(new PluginControl(type, SB.App, type.GetTypeInfo().IsSubclassOf(typeof(ViewModelBase))));
					}
					else if (type.GetInterface("IBlockPlugin") == typeof(IBlockPlugin) && type.GetTypeInfo().IsSubclassOf(typeof(BlockBase)))
					{
						blockPlugins.Add(Activator.CreateInstance(type) as IBlockPlugin);
						pluginNames.Add(asm.GetName().Name);
					}
				}
			}
			return new ValueTuple<IEnumerable<PluginControl>, IEnumerable<IBlockPlugin>, IEnumerable<string>>(plugins, blockPlugins, pluginNames);
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x00032324 File Offset: 0x00030524
		public static void LoadDependencies(IEnumerable<AssemblyName> assemblies)
		{
			using (IEnumerator<AssemblyName> enumerator = assemblies.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AssemblyName asm = enumerator.Current;
					if (!AppDomain.CurrentDomain.GetAssemblies().Any((Assembly a) => a.GetName().FullName == asm.FullName))
					{
						try
						{
							AppDomain.CurrentDomain.Load(asm);
							Loader.LoadDependencies(Assembly.Load(asm).GetReferencedAssemblies());
						}
						catch
						{
						}
					}
				}
			}
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x000323C4 File Offset: 0x000305C4
		public static void Hook(params string[] folders)
		{
			AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs args)
			{
				Assembly loadedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((Assembly a) => a.FullName == args.Name);
				if (loadedAssembly != null)
				{
					return loadedAssembly;
				}
				AssemblyName n = new AssemblyName(args.Name);
				if (n.Name.EndsWith(".xmlserializers", StringComparison.OrdinalIgnoreCase))
				{
					return null;
				}
				if (n.Name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
				{
					return null;
				}
				string[] folders2 = folders;
				Func<string, bool> <>9__3;
				for (int i = 0; i < folders2.Length; i++)
				{
					string dir = folders2[i];
					IEnumerable<string> enumerable = new string[] { "*.dll", "*.exe" }.SelectMany((string g) => Directory.EnumerateFiles(dir, g));
					Func<string, bool> func;
					if ((func = <>9__3) == null)
					{
						func = (<>9__3 = delegate(string f)
						{
							bool flag;
							try
							{
								flag = n.Name.Equals(AssemblyName.GetAssemblyName(f).Name, StringComparison.OrdinalIgnoreCase);
							}
							catch (BadImageFormatException)
							{
								flag = false;
							}
							catch (Exception ex)
							{
								throw new ApplicationException("Error loading assembly " + f, ex);
							}
							return flag;
						});
					}
					string assy = enumerable.FirstOrDefault(func);
					if (assy != null)
					{
						return Assembly.LoadFrom(assy);
					}
				}
				throw new ApplicationException("Assembly " + args.Name + " not found");
			};
		}
	}
}
