using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

namespace Phanfare.MediaServer
{
	public partial class Service1 : ServiceBase
	{
		public MediaServerDevice Device;

		public Service1()
		{
			InitializeComponent();
		}

		protected override void OnStart( string[] args )
		{
			this.StartCore( args );
		}

		public void StartCore( string[] args )
		{
			if( this.Device != null )
				this.Device.Stop();
			this.Device = new MediaServerDevice();
			this.Device.Start();
		}

		protected override void OnStop()
		{
			if( this.Device != null )
				this.Device.Stop();
			this.Device = null;
		}
	}
}
