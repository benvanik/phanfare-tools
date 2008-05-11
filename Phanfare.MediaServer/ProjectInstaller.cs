using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;

namespace Phanfare.MediaServer
{
	[RunInstaller( true )]
	public partial class ProjectInstaller : Installer
	{
		public ProjectInstaller()
		{
			InitializeComponent();
		}

		protected override void OnAfterInstall( IDictionary savedState )
		{
			base.OnAfterInstall( savedState );

			try
			{
				ProcessStartInfo startInfo = new ProcessStartInfo( "net", "start \"Phanfare Media Server\"" );
				startInfo.CreateNoWindow = false;
				Process.Start( startInfo );
			}
			catch { }
		}
	}
}
