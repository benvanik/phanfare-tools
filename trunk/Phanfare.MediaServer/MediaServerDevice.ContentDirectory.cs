using System;
using System.Collections.Generic;
using System.Text;
using Intel.UPNP;
using Phanfare.MediaServer.Connectors;

namespace Phanfare.MediaServer
{
	public enum Agent
	{
		Xbox360,
		Playstation3,
		Other,
	}

	partial class MediaServerDevice
	{
		private const string RootObjectID = "0";
		private const string MusicItemObjectID = "4";
		private const string MusicGenreObjectID = "5";
		private const string MusicArtistObjectID = "6";
		private const string MusicAlbumObjectID = "7";
		private const string MusicPlaylistObjectID = "F";
		private const string MusicObjectID = "14";
		private const string VideosObjectID = "15";
		private const string PicturesObjectID = "16";

		private uint _updateId = ( uint )Environment.TickCount;

		public Agent GetRequestAgent()
		{
			HTTPMessage message = _contentDirectory.GetUPnPService().GetHttpMessage();
			if( message.UserAgent.Contains( "Playstation" ) ||
				message.AVClientInfo.Contains( "PLAYSTATION" ) )
				return Agent.Playstation3;
			else if( message.UserAgent.Contains( "Xbox" ) )
				return Agent.Xbox360;
			else
				return Agent.Other;
		}

		public void ContentDirectory_Browse( System.String ObjectID, DvContentDirectory.Enum_A_ARG_TYPE_BrowseFlag BrowseFlag, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, out System.String Result, out System.UInt32 NumberReturned, out System.UInt32 TotalMatches, out System.UInt32 UpdateID )
		{
			Console.WriteLine( "ContentDirectory_Browse(" + ObjectID.ToString() + BrowseFlag.ToString() + Filter.ToString() + StartingIndex.ToString() + RequestedCount.ToString() + SortCriteria.ToString() + ")" );
			TotalMatches = NumberReturned = 0;
			UpdateID = _updateId;
			Result = string.Empty;

			Agent agent = this.GetRequestAgent();

			if( BrowseFlag == DvContentDirectory.Enum_A_ARG_TYPE_BrowseFlag.BROWSEMETADATA )
			{
				if( agent == Agent.Xbox360 )
				{
					switch( ObjectID )
					{
						case RootObjectID:
							NumberReturned = TotalMatches = 1;
							Result = "<DIDL-Lite xmlns=\"urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:upnp=\"urn:schemas-upnp-org:metadata-1-0/upnp/\">" +
								  "<container id=\"0\" searchable=\"1\" parentID=\"-1\" restricted=\"0\" childCount=\"1\">" +
								  "<dc:title>root</dc:title>" +
								  "<upnp:class>object.container</upnp:class>" +
								  "</container></DIDL-Lite>";
							return;
						case PicturesObjectID:
							{
								if( _phanfareSystem.IsLoggedIn == true )
								{
									NumberReturned = TotalMatches = 1;
									StringBuilder sb = new StringBuilder();
									sb.Append( Didl.Begin() );
									sb.Append( Didl.GetContainer( "16", "0", true, true, "Pictures", "object.container.storageFolder", 0 ) );
									sb.Append( Didl.End() );
									Result = sb.ToString();
								}
								else
								{
									TotalMatches = NumberReturned = 1;
									Result = Didl.Begin() + Didl.GetContainer( "16/no", "16", true, false, "Not logged in", "object.container.storageFolder", 0 ) + Didl.End();
								}
							}
							return;
					}
				}
				else
				{
					switch( ObjectID )
					{
						case RootObjectID:
							NumberReturned = TotalMatches = 1;
							Result = "<DIDL-Lite xmlns=\"urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:upnp=\"urn:schemas-upnp-org:metadata-1-0/upnp/\">" +
								  "<container id=\"0\" searchable=\"1\" parentID=\"-1\" restricted=\"0\" childCount=\"1\">" +
								  "<dc:title>root</dc:title>" +
								  "<upnp:class>object.container</upnp:class>" +
								  "</container></DIDL-Lite>";
							return;
					}

					if( _phanfareSystem.IsLoggedIn == true )
					{
						if( ObjectID.StartsWith( PicturesObjectID ) == true )
						{
							ContainerBase container = _phanfareSystem.LookupContainer( ObjectID );
							NumberReturned = TotalMatches = 1;
							StringBuilder sb = new StringBuilder();
							sb.Append( Didl.Begin() );
							sb.Append( Didl.GetContainer( container.ID, container.Parent.ID, true, true, container.Name, "object.container.storageFolder", container.GetChildCount() ) );
							sb.Append( Didl.End() );
							Result = sb.ToString();
						}
					}
					else
					{
						TotalMatches = NumberReturned = 1;
						Result = Didl.Begin() + Didl.GetContainer( "16/no", "16", true, false, "Not logged in", "object.container.storageFolder", 0 ) + Didl.End();
					}
				}
			}
			else if( BrowseFlag == DvContentDirectory.Enum_A_ARG_TYPE_BrowseFlag.BROWSEDIRECTCHILDREN )
			{
				if( agent == Agent.Xbox360 )
				{
					switch( ObjectID )
					{
						case RootObjectID:
							NumberReturned = TotalMatches = 1;
							Result = "<DIDL-Lite xmlns=\"urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:upnp=\"urn:schemas-upnp-org:metadata-1-0/upnp/\">" +
								//"<container id=\"14\" parentID=\"\" childCount=\"0\"><dc:title>Music</dc:title><upnp:class>object.container.storageFolder</upnp:class></container>" +
								//"<container id=\"15\" parentID=\"\" childCount=\"0\"><dc:title>Videos</dc:title><upnp:class>object.container.storageFolder</upnp:class></container>" +
								"<container id=\"16\" parentID=\"\" childCount=\"0\"><dc:title>Pictures</dc:title><upnp:class>object.container.storageFolder</upnp:class></container>" +
								"</DIDL-Lite>";
							return;
						case PicturesObjectID:
							if( _phanfareSystem.IsLoggedIn == true )
							{
								EnumerationResult result = _phanfareSystem.Enumerate( StartingIndex, RequestedCount );
								NumberReturned = result.ReturnCount;
								TotalMatches = result.TotalCount;
								Result = result.Result;
							}
							else
							{
								TotalMatches = NumberReturned = 1;
								Result = Didl.Begin() + Didl.GetContainer( "16/no", "16", true, false, "Not logged in", "object.container.storageFolder", 0 ) + Didl.End();
							}
							return;
					}
				}
				else
				{
					switch( ObjectID )
					{
						case RootObjectID:
							if( _phanfareSystem.IsLoggedIn == true )
							{
								EnumerationResult result = _phanfareSystem.Enumerate( StartingIndex, RequestedCount );
								NumberReturned = result.ReturnCount;
								TotalMatches = result.TotalCount;
								Result = result.Result;
							}
							else
							{
								TotalMatches = NumberReturned = 1;
								Result = Didl.Begin() + Didl.GetContainer( "16/no", "16", true, false, "Not logged in", "object.container.storageFolder", 0 ) + Didl.End();
							}
							return;
					}
				}

				if( _phanfareSystem.IsLoggedIn == true )
				{
					if( ObjectID.StartsWith( PicturesObjectID ) == true )
					{
						ContainerBase container = _phanfareSystem.LookupContainer( ObjectID );
						EnumerationResult result = container.Enumerate( StartingIndex, RequestedCount );
						NumberReturned = result.ReturnCount;
						TotalMatches = result.TotalCount;
						Result = result.Result;
					}
				}
				else
				{
					TotalMatches = NumberReturned = 1;
					Result = Didl.Begin() + Didl.GetContainer( "16/no", "16", true, false, "Not logged in", "object.container.storageFolder", 0 ) + Didl.End();
				}
			}
		}

		public void ContentDirectory_GetSearchCapabilities( out System.String SearchCaps )
		{
			SearchCaps = "@id,@refID,dc:title,upnp:class";
			Console.WriteLine( "ContentDirectory_GetSearchCapabilities(" + ")" );
		}

		public void ContentDirectory_GetSortCapabilities( out System.String SortCaps )
		{
			SortCaps = "dc:title,dc:date";
			Console.WriteLine( "ContentDirectory_GetSortCapabilities(" + ")" );
		}

		public void ContentDirectory_GetSystemUpdateID( out System.UInt32 Id )
		{
			Id = _updateId;
			_updateId++;
			Console.WriteLine( "ContentDirectory_GetSystemUpdateID(" + ")" );
		}

		public void ContentDirectory_Search( System.String ContainerID, System.String SearchCriteria, System.String Filter, System.UInt32 StartingIndex, System.UInt32 RequestedCount, System.String SortCriteria, out System.String Result, out System.UInt32 NumberReturned, out System.UInt32 TotalMatches, out System.UInt32 UpdateID )
		{
			Console.WriteLine( "ContentDirectory_Search(" + ContainerID.ToString() + SearchCriteria.ToString() + Filter.ToString() + StartingIndex.ToString() + RequestedCount.ToString() + SortCriteria.ToString() + ")" );

			NumberReturned = 0;
			TotalMatches = 0;
			UpdateID = _updateId;
			Result = string.Empty;

			if( _phanfareSystem.IsLoggedIn == false )
				return;

			switch( ContainerID )
			{
				case PicturesObjectID:
					NumberReturned = TotalMatches = ( uint )( 1 + _phanfareSystem.Users.Count );
					{
						EnumerationResult result = _phanfareSystem.RandomContent( StartingIndex, RequestedCount );
						NumberReturned = result.ReturnCount;
						TotalMatches = result.TotalCount;
						Result = result.Result;
					}
					break;
				default:
					if( ContainerID.StartsWith( PicturesObjectID ) == true )
					{
						ContainerBase container = _phanfareSystem.LookupContainer( ContainerID );
						EnumerationResult result = container.RandomContent( StartingIndex, RequestedCount );
						NumberReturned = result.ReturnCount;
						TotalMatches = result.TotalCount;
						Result = result.Result;
					}
					break;
			}
		}
	}
}
