using System;
using System.Collections.Generic;
using System.Text;
using Phanfare.ExternalAPI;

namespace Phanfare.MediaServer.Connectors.PhanfareConnector
{
	class PhanfareRandomSet : ContainerBase
	{
		public List<PhanfareImage> Images;

		public PhanfareRandomSet( ContainerBase parent )
			: base( parent )
		{
			this.Images = new List<PhanfareImage>();
		}

		public override string Name { get { return "Random"; } }

		public void Populate( ImageInfo[] sourceImages )
		{
			foreach( ImageInfo source in sourceImages )
			{
				PhanfareImage image = new PhanfareImage( this, source );
				this.AddChild( source.ImageID.ToString(), image );
				this.Images.Add( image );
			}
		}
	}
}
