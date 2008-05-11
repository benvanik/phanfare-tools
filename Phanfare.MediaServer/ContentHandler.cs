using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Intel.UPNP;
using Phanfare.MediaServer.Connectors;

namespace Phanfare.MediaServer
{
	class ContentHandler : IContentHandler
	{
		private Dictionary<string, IContentSystem> _systems = new Dictionary<string, IContentSystem>();
		private Queue<MemoryCacheEntry> _memoryCache = new Queue<MemoryCacheEntry>();

		private const int MaximumMemoryEntries = 1;
		private class MemoryCacheEntry
		{
			public readonly string Url;
			public readonly string TagData;
			public readonly byte[] Buffer;
			public MemoryCacheEntry( string url, string tagData, byte[] buffer )
			{
				this.Url = url;
				this.TagData = tagData;
				this.Buffer = buffer;
			}
		}

		public void RegisterSystem( string prefix, IContentSystem system )
		{
			_systems[ prefix ] = system;
		}

		public HTTPMessage HandleContent( string GetWhat, System.Net.IPEndPoint local, HTTPMessage msg, HTTPSession WebSession )
		{
			foreach( MemoryCacheEntry entry in _memoryCache )
			{
				if( entry.Url == GetWhat )
				{
					Console.WriteLine( "ContentHandler::HandleContent: responding with cached copy for " + GetWhat );
					HTTPMessage message = new HTTPMessage();
					message.StatusCode = 200;
					message.StatusData = "OK";
					message.AddTag( "Content-Type", entry.TagData );
					message.BodyBuffer = entry.Buffer;
					WebSession.Send( message );
					return null;
				}
			}

			string getWhat = GetWhat.ToLowerInvariant();
			foreach( string prefix in _systems.Keys )
			{
				if( getWhat.StartsWith( prefix ) == true )
				{
					IContentSystem system = _systems[ prefix ];
					// Remove prefix
					getWhat = GetWhat.Remove( 0, prefix.Length + 1 );
					HandleResult result = _systems[ prefix ].Handle( getWhat );
					if( result.Buffer != null )
					{
						if( _memoryCache.Count >= MaximumMemoryEntries )
							_memoryCache.Dequeue();
						_memoryCache.Enqueue( new MemoryCacheEntry( GetWhat, result.TagData, result.Buffer ) );

						HTTPMessage message = new HTTPMessage();
						message.StatusCode = 200;
						message.StatusData = "OK";
						message.AddTag( "Content-Type", result.TagData );
						message.BodyBuffer = result.Buffer;
						WebSession.Send( message );
					}
					else
						WebSession.SendStreamObject( result.Stream, result.Length, result.TagData );
					break;
				}
			}

			return null;
		}
	}
}
