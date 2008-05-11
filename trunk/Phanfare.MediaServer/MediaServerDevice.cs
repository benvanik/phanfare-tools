// Intel's UPnP .NET Framework Device Stack, Device Module
// Intel Device Builder Build#1.0.2777.24761

using System;
using System.IO;
using Intel.UPNP;
using Phanfare.MediaServer;
using Phanfare.MediaServer.Connectors.PhanfareConnector;
using Phanfare.MediaServer.Utilities;

namespace Phanfare.MediaServer
{
	public partial class MediaServerDevice
	{
		private UPnPDevice _upnpDevice;
		private DvContentDirectory _contentDirectory;
		private ContentHandler _contentHandler;
		private PhanfareSystem _phanfareSystem;

		public MediaServerDevice()
		{
			_upnpDevice = UPnPDevice.CreateRootDevice( 1800, 1.0, "\\" );
			_upnpDevice.FriendlyName = Environment.MachineName + " : Phanfare";
			_upnpDevice.Manufacturer = "Phanfare, Inc.";
			_upnpDevice.ManufacturerURL = "http://www.phanfare.com";
			_upnpDevice.ModelName = "Windows Media Connect Phanfare";
			_upnpDevice.ModelDescription = "Phanfare UPNP Media Server";
			_upnpDevice.ModelNumber = "2.0";
			_upnpDevice.HasPresentation = false;
			_upnpDevice.DeviceURN = "urn:schemas-upnp-org:device:MediaServer:1";
			_upnpDevice.Icon = Properties.Resources.IconLarge;
			_upnpDevice.Icon2 = Properties.Resources.Icon;

			IniReader iniReader = new IniReader( this.SettingsFilePath );
			string udn = iniReader.ReadString( "General", "UniqueDeviceName", string.Empty );
			if( string.IsNullOrEmpty( udn ) == true )
			{
				udn = Guid.NewGuid().ToString( "D" );
				iniReader.Write( "General", "UniqueDeviceName", udn );
			}
			_upnpDevice.UniqueDeviceName = udn;

			Phanfare.MediaServer.DvMediaReceiverRegistrar MediaReceiverRegistrar = new Phanfare.MediaServer.DvMediaReceiverRegistrar();
			MediaReceiverRegistrar.External_IsAuthorized = new Phanfare.MediaServer.DvMediaReceiverRegistrar.Delegate_IsAuthorized( MediaReceiverRegistrar_IsAuthorized );
			MediaReceiverRegistrar.External_IsValidated = new Phanfare.MediaServer.DvMediaReceiverRegistrar.Delegate_IsValidated( MediaReceiverRegistrar_IsValidated );
			MediaReceiverRegistrar.External_RegisterDevice = new Phanfare.MediaServer.DvMediaReceiverRegistrar.Delegate_RegisterDevice( MediaReceiverRegistrar_RegisterDevice );
			_upnpDevice.AddService( MediaReceiverRegistrar );

			Phanfare.MediaServer.DvConnectionManager ConnectionManager = new Phanfare.MediaServer.DvConnectionManager();
			ConnectionManager.External_GetCurrentConnectionIDs = new Phanfare.MediaServer.DvConnectionManager.Delegate_GetCurrentConnectionIDs( ConnectionManager_GetCurrentConnectionIDs );
			ConnectionManager.External_GetCurrentConnectionInfo = new Phanfare.MediaServer.DvConnectionManager.Delegate_GetCurrentConnectionInfo( ConnectionManager_GetCurrentConnectionInfo );
			ConnectionManager.External_GetProtocolInfo = new Phanfare.MediaServer.DvConnectionManager.Delegate_GetProtocolInfo( ConnectionManager_GetProtocolInfo );
			_upnpDevice.AddService( ConnectionManager );

			Phanfare.MediaServer.DvContentDirectory ContentDirectory = new Phanfare.MediaServer.DvContentDirectory();
			ContentDirectory.External_Browse = new Phanfare.MediaServer.DvContentDirectory.Delegate_Browse( ContentDirectory_Browse );
			ContentDirectory.External_GetSearchCapabilities = new Phanfare.MediaServer.DvContentDirectory.Delegate_GetSearchCapabilities( ContentDirectory_GetSearchCapabilities );
			ContentDirectory.External_GetSortCapabilities = new Phanfare.MediaServer.DvContentDirectory.Delegate_GetSortCapabilities( ContentDirectory_GetSortCapabilities );
			ContentDirectory.External_GetSystemUpdateID = new Phanfare.MediaServer.DvContentDirectory.Delegate_GetSystemUpdateID( ContentDirectory_GetSystemUpdateID );
			ContentDirectory.External_Search = new Phanfare.MediaServer.DvContentDirectory.Delegate_Search( ContentDirectory_Search );
			_upnpDevice.AddService( ContentDirectory );
			_contentDirectory = ContentDirectory;

			// Setting the initial value of evented variables
			MediaReceiverRegistrar.Evented_AuthorizationDeniedUpdateID = 0;
			MediaReceiverRegistrar.Evented_ValidationSucceededUpdateID = 0;
			MediaReceiverRegistrar.Evented_ValidationRevokedUpdateID = 0;
			MediaReceiverRegistrar.Evented_AuthorizationGrantedUpdateID = 0;
			ConnectionManager.Evented_SinkProtocolInfo = "Sample String";
			ConnectionManager.Evented_SourceProtocolInfo = "Sample String";
			ConnectionManager.Evented_CurrentConnectionIDs = "Sample String";
			ContentDirectory.Evented_ContainerUpdateIDs = "Sample String";
			ContentDirectory.Evented_SystemUpdateID = 0;

			_contentHandler = new ContentHandler();
			_upnpDevice.ContentHandler = _contentHandler;

			this.SetupSettingsWatcher();
		}

		public void Start()
		{
			_upnpDevice.StartDevice( 41955 );

			_phanfareSystem = new PhanfareSystem( this, _contentDirectory.GetUPnPService() );
			try
			{
				if( _phanfareSystem.Login() == true )
				{
					Console.WriteLine( "Logged in to service" );
					_contentHandler.RegisterSystem( PhanfareSystem.PhotoHandler, _phanfareSystem );
				}
				else
				{
					// TODO: log event
					Console.WriteLine( "Failed to login; no configuration data" );
				}
			}
			catch( Exception ex )
			{
				// TODO: log exception (probably failure to log in)
				Console.WriteLine( "Failed to start system (login): " + ex.ToString() );
			}
		}

		public void Stop()
		{
			_upnpDevice.StopDevice();
		}

		#region Settings

		public const string SettingsFileName = "Phanfare.MediaServer.ini";
		public string SettingsFilePath
		{
			get
			{
				string executionPath = Path.GetDirectoryName( typeof( MediaServerDevice ).Assembly.Location );
				return Path.Combine( executionPath, SettingsFileName );
			}
		}

		private FileSystemWatcher _settingsWatcher;

		private void SetupSettingsWatcher()
		{
			string executionPath = Path.GetDirectoryName( typeof( MediaServerDevice ).Assembly.Location );
			_settingsWatcher = new FileSystemWatcher( executionPath, SettingsFileName );
			_settingsWatcher.NotifyFilter = NotifyFilters.LastWrite;
			_settingsWatcher.Changed += new FileSystemEventHandler( _settingsWatcher_Changed );
			_settingsWatcher.EnableRaisingEvents = true;
		}

		private void _settingsWatcher_Changed( object sender, FileSystemEventArgs e )
		{
			Console.WriteLine( "MediaServerDevice: reloading configuration (resetting device, updating ID)" );
			_updateId++;

			this.Stop();
			this.Start();
		}

		#endregion
	}
}

