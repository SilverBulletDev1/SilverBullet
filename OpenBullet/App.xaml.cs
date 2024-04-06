using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using EO.Base;

namespace OpenBullet
{
	// Token: 0x0200004E RID: 78
	public partial class App : Application
	{
		// Token: 0x060001BF RID: 447 RVA: 0x00008030 File Offset: 0x00006230
		public App()
		{
			AppDomain.CurrentDomain.UnhandledException += delegate(object s, UnhandledExceptionEventArgs e)
			{
				this.OnUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");
			};
			base.Dispatcher.UnhandledException += delegate(object s, DispatcherUnhandledExceptionEventArgs e)
			{
				this.OnUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");
			};
			Application.Current.Dispatcher.UnhandledException += this.Dispatcher_UnhandledException;
			TaskScheduler.UnobservedTaskException += delegate(object s, UnobservedTaskExceptionEventArgs e)
			{
				this.OnUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
			};
			AppDomain.CurrentDomain.AssemblyResolve += this.OnAssemblyResolve;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000080B2 File Offset: 0x000062B2
		private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			e.Handled = true;
			this.OnUnhandledException(e.Exception, "Dispatcher_UnhandledException");
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000080CC File Offset: 0x000062CC
		public void OnUnhandledException(Exception ex, string @event)
		{
			File.AppendAllText(SB.logFile, string.Format("[FATAL][{0}] UHANDLED EXCEPTION{1}{2}", @event, Environment.NewLine, ex));
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000080E9 File Offset: 0x000062E9
		public Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
		{
			return null;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000080EC File Offset: 0x000062EC
		protected override void OnExit(ExitEventArgs e)
		{
			try
			{
				Runtime.Shutdown();
			}
			catch
			{
			}
			base.OnExit(e);
		}
	}
}
