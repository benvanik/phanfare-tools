using System;
using System.Collections.Generic;
using System.Text;
using Intel.UPNP;

namespace Phanfare.MediaServer
{
	partial class MediaServerDevice
	{
		public void MediaReceiverRegistrar_IsAuthorized( System.String DeviceID, out System.Int32 Result )
		{
			Result = 1;
			//Console.WriteLine( "MediaReceiverRegistrar_IsAuthorized(" + DeviceID.ToString() + ")" );
		}

		public void MediaReceiverRegistrar_IsValidated( System.String DeviceID, out System.Int32 Result )
		{
			Result = 1;
			//Console.WriteLine( "MediaReceiverRegistrar_IsValidated(" + DeviceID.ToString() + ")" );
		}

		public void MediaReceiverRegistrar_RegisterDevice( System.Byte[] RegistrationReqMsg, out System.Byte[] RegistrationRespMsg )
		{
			//RegistrationRespMsg = "Sample String";
			RegistrationRespMsg = null;
			//Console.WriteLine( "MediaReceiverRegistrar_RegisterDevice(" + RegistrationReqMsg.ToString() + ")" );
			throw new UPnPCustomException( 800, "This method has not been completely implemented..." );
		}
	}
}
