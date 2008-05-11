using System;
using System.Collections.Generic;
using System.Text;
using Intel.UPNP;

namespace Phanfare.MediaServer
{
	partial class MediaServerDevice
	{
		public void ConnectionManager_GetCurrentConnectionIDs( out System.String ConnectionIDs )
		{
			ConnectionIDs = "0";
			Console.WriteLine( "ConnectionManager_GetCurrentConnectionIDs(" + ")" );
		}

		public void ConnectionManager_GetCurrentConnectionInfo( System.Int32 ConnectionID, out System.Int32 RcsID, out System.Int32 AVTransportID, out System.String ProtocolInfo, out System.String PeerConnectionManager, out System.Int32 PeerConnectionID, out DvConnectionManager.Enum_A_ARG_TYPE_Direction Direction, out DvConnectionManager.Enum_A_ARG_TYPE_ConnectionStatus Status )
		{
			//RcsID = 0;
			//AVTransportID = 0;
			//ProtocolInfo = "Sample String";
			//PeerConnectionManager = "Sample String";
			//PeerConnectionID = 0;
			//Direction = DvConnectionManager.Enum_A_ARG_TYPE_Direction.INPUT;
			//Status = DvConnectionManager.Enum_A_ARG_TYPE_ConnectionStatus.OK;
			//Console.WriteLine( "ConnectionManager_GetCurrentConnectionInfo(" + ConnectionID.ToString() + ")" );
			throw new UPnPCustomException( 706, "Invalid Connection Reference" );
		}

		public void ConnectionManager_GetProtocolInfo( out System.String Source, out System.String Sink )
		{
			Source =
				"http-get:*:image/jpeg:DLNA.ORG_PN=JPEG_LRG," +
				"http-get:*:image/jpeg:DLNA.ORG_PN=JPEG_MED," +
				"http-get:*:image/jpeg:DLNA.ORG_PN=JPEG_TN," +
				"http-get:*:image/jpeg:*," +
				"http-get:*:image/png:*,";
			Sink = string.Empty;
			Console.WriteLine( "ConnectionManager_GetProtocolInfo(" + ")" );
		}
	}
}
