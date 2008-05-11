using System;
using System.Collections.Generic;
using System.Text;

namespace Phanfare.MediaServer.Connectors
{
	interface IContentSystem
	{
		string BaseURL { get; }
		bool ShowHidden { get; }

		void RegisterContainer( string id, ContainerBase container );
		void RegisterContent( string id, ContentBase content );
		ContainerBase LookupContainer( string id );
		ContentBase LookupContent( string id );

		bool EnsurePopulated( ContainerBase container );

		HandleResult Handle( string getWhat );
	}
}
