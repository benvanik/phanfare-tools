using System;
using System.Collections.Generic;
using System.Text;
using Phanfare.ExternalAPI;

namespace Phanfare.MediaServer.Connectors.PhanfareConnector
{
	class PhanfareUser : ContainerBase
	{
		public readonly long UserID;
		public readonly bool IsSelf;
		public readonly PublicProfile Profile;

		public List<PhanfareAlbum> Albums;
		public bool IsCached;
		public DateTime LastTimePopulated;

		public PhanfareUser( PhanfareSystem system, long userId, bool isSelf, PublicProfile profile )
			: base( system )
		{
			this.UserID = userId;
			this.IsSelf = isSelf;
			this.Profile = profile;

			this.Albums = new List<PhanfareAlbum>();
		}

		public override string Name
		{
			get
			{
				//if( this.IsSelf == true )
				//    return "Me (" + ( ( PhanfareSystem )this.System ).EmailAddress + ")";
				//else
				return this.Profile.FirstName + " " + this.Profile.LastName;
			}
		}

		public void Populate( IEnumerable<Album> source )
		{
			bool showHidden = this.System.ShowHidden;
			lock( this )
			{
				if( this.IsCached == false )
				{
					List<Album> allAlbums = new List<Album>( source );
					AlbumComparer albumComparer = new AlbumComparer( true, false );
					allAlbums.Sort( albumComparer );
					foreach( Album sourceAlbum in allAlbums )
					{
						if( ( sourceAlbum.Groups.Length == 0 ) &&
							( showHidden == false ) )
							continue;
						PhanfareAlbum album = new PhanfareAlbum( this, sourceAlbum );
						this.Albums.Add( album );
						this.AddChild( sourceAlbum.AlbumID.ToString(), album );
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
			// Request over entire user
			PhanfareRandomSet set = new PhanfareRandomSet( this );
			this.AddTemporaryChild( set );
			int zeroCount = 0;
			while( set.Images.Count < requestCount )
			{
				ImageInfo[] images = ( ( PhanfareSystem )this.System ).Api.GetRandomImages( this.UserID, RandomMode.Random, ( int )requestCount - set.Images.Count );
				if( images.Length == 0 )
					zeroCount++;
				else
					set.Populate( images );
				if( zeroCount > 5 )
					break;
			}

			List<ContentBase> pickedList = new List<ContentBase>( set.Images.Count );
			foreach( PhanfareImage image in set.Images )
				pickedList.Add( image );
			return new RandomContentResult( pickedList, startingIndex, ( uint )pickedList.Count, requestCount * 2 );
		}

		#endregion
	}
}
