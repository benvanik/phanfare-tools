using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace Phanfare.MediaServer
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main( string[] args )
		{
			bool interactive = false;
			foreach( string arg in args )
			{
				switch( arg.ToLowerInvariant() )
				{
					case "/?":
					case "/help":
					case "-h":
					case "--help":
						Console.WriteLine( "Phanfare Media Server" );
						Console.WriteLine( " /?: usage" );
						Console.WriteLine( " /interactive, /i: run in console mode" );
						return;
					case "/i":
					case "/interactive":
						interactive = true;
						break;
				}
			}

			if( ( interactive == true ) ||
				( Debugger.IsAttached == true ) )
			{
				MediaServerDevice device = new MediaServerDevice();
				device.Start();
				Console.WriteLine( "Service started..." );
				if( interactive == true )
				{
					Console.WriteLine( "Press enter to exit" );
					Console.ReadLine();
				}
				else
					Thread.Sleep( int.MaxValue - 1 );
			}
			else
			{
				// Remove this to run under mono
				ServiceBase[] ServicesToRun;
				ServicesToRun = new ServiceBase[] 
			    { 
			        new Service1() 
			    };
				ServiceBase.Run( ServicesToRun );
			}
		}
	}
}
