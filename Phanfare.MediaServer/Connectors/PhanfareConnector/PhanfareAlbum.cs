using System;
using System.Collections.Generic;
using System.Text;
using Phanfare.ExternalAPI;

namespace Phanfare.MediaServer.Connectors.PhanfareConnector
{
	class PhanfareAlbum : ContainerBase
	{
		public PhanfareUser ParentUser { get { return this.Parent as PhanfareUser; } }
		public PhanfareGroup ParentGroup { get { return this.Parent as PhanfareGroup; } }
		public readonly Album Object;

		public List<PhanfareSection> Sections;
		public bool IsCached;
		public DateTime LastTimePopulated;

		public PhanfareAlbum( PhanfareUser user, Album source )
			: base( user )
		{
			this.Object = source;
			this.Sections = new List<PhanfareSection>( source.Sections.Length );
		}

		public PhanfareAlbum( PhanfareGroup group, Album source )
			: base( group )
		{
			this.Object = source;
			this.Sections = new List<PhanfareSection>( source.Sections.Length );
		}

		public override string Name { get { return this.Object.Name; } }
		public override bool FlattenSingleChild { get { return true; } }
		public bool HasMultipleSections { get { return this.Object.Sections.Length > 1; } }

		public void Populate( Album source )
		{
			lock( this )
			{
				if( this.IsCached == false )
				{
					foreach( Section sourceSection in source.Sections )
					{
						PhanfareSection section = new PhanfareSection( this, sourceSection );
						this.Sections.Add( section );
						this.AddChild( sourceSection.SectionID.ToString(), section );
						section.Populate( sourceSection );
					}
					this.IsCached = true;
					this.LastTimePopulated = DateTime.Now;
				}
				else
				{
					this.LastTimePopulated = DateTime.Now;
					// merge
				}
			}
		}

		#region Random

		protected override RandomContentResult OnRandomContentRequested( uint startingIndex, uint requestCount )
		{
			// Request over entire album - we are NOT random, though; this is an album-level slideshow

			uint totalCount = 0;
			uint index = 0;
			List<ContentBase> content = new List<ContentBase>();
			foreach( PhanfareSection section in this.Sections )
			{
				foreach( PhanfareImage image in section.Images )
				{
					if( index >= startingIndex )
					{
						if( content.Count < requestCount )
							content.Add( image );
					}
					totalCount++;
					index++;
				}
			}

			return new RandomContentResult( content, startingIndex, ( uint )content.Count, totalCount );
		}

		#endregion
	}
}
