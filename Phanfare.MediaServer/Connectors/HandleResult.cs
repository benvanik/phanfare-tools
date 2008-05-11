using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Phanfare.MediaServer.Connectors
{
	class HandleResult
	{
		public readonly string TagData;
		public readonly byte[] Buffer;
		public readonly Stream Stream;
		public readonly long Length;

		public HandleResult( string tagData, byte[] buffer )
			: this( tagData, buffer, buffer.Length )
		{
		}

		public HandleResult( string tagData, byte[] buffer, int length )
		{
			this.TagData = tagData;
			this.Buffer = buffer;
			this.Length = length;
		}

		public HandleResult( string tagData, Stream stream, long length )
		{
			this.TagData = tagData;
			this.Stream = stream;
			this.Length = length;
		}
	}
}
