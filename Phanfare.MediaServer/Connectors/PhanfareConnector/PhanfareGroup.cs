using System;
using System.Collections.Generic;
using System.Text;
using Phanfare.ExternalAPI;

namespace Phanfare.MediaServer.Connectors.PhanfareConnector
{
	enum GroupType
	{
		FriendsFamily,
		UserCreated,
	}

	class PhanfareGroup : ContainerBase
	{
		public readonly GroupInfo Object;
		public readonly GroupType Type;

		public List<PhanfareUser> Users;
		public List<PhanfareAlbum> Albums;
		public bool IsCached;
		public DateTime LastTimePopulated;

		public PhanfareGroup( PhanfareSystem system, GroupInfo source, GroupType type )
			: base( system )
		{
			this.Object = source;
			this.Type = type;

			this.Users = new List<PhanfareUser>();
			this.Albums = new List<PhanfareAlbum>();
		}

		public override string Name { get { return this.Object.Name; } }

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

		public void Populate( IEnumerable<PhanfareUser> source )
		{
			bool showHidden = this.System.ShowHidden;
			lock( this )
			{
				if( this.IsCached == false )
				{
					foreach( PhanfareUser sourceUser in source )
					{
						this.Users.Add( sourceUser );
						this.AddChild( sourceUser.UserID.ToString(), sourceUser );
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
			// Request over entire group
			PhanfareRandomSet set = new PhanfareRandomSet( this );
			this.AddTemporaryChild( set );
			int zeroCount = 0;
			while( set.Images.Count < requestCount )
			{
				ImageInfo[] images = ( ( PhanfareSystem )this.System ).Api.GetGroupRandomImages( this.Object.GroupID, RandomMode.Random, ( int )requestCount - set.Images.Count );
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
