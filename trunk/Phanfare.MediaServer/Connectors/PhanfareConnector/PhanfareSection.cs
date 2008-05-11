using System;
using System.Collections.Generic;
using System.Text;
using Phanfare.ExternalAPI;

namespace Phanfare.MediaServer.Connectors.PhanfareConnector
{
	class PhanfareSection : ContainerBase
	{
		public PhanfareAlbum ParentAlbum { get { return ( PhanfareAlbum )this.Parent; } }
		public readonly Section Object;

		public List<PhanfareImage> Images;

		public PhanfareSection( PhanfareAlbum album, Section source )
			: base( album )
		{
			this.Object = source;

			this.Images = new List<PhanfareImage>();
		}

		public override string Name { get { return this.Object.Name; } }

		public override int GetChildCount()
		{
			if( this.ParentAlbum.IsCached == true )
				return base.GetChildCount();
			else
				return this.Object.ImageCount;
		}

		public void Populate( Section source )
		{
			bool showHidden = this.System.ShowHidden;
			foreach( ImageInfo sourceImage in source.Images )
			{
				// Ignore videos
				if( sourceImage.IsVideo == true )
					continue;
				if( ( sourceImage.IsHidden == true ) &&
					( showHidden == false ) )
					continue;
				PhanfareImage image = new PhanfareImage( this, sourceImage );
				this.Images.Add( image );
				this.AddChild( sourceImage.ImageID.ToString(), image );
			}
		}
	}
}
