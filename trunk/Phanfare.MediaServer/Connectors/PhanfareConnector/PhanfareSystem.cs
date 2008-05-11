using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Intel.UPNP;
using Phanfare.ExternalAPI;
using Phanfare.MediaServer.Utilities;

namespace Phanfare.MediaServer.Connectors.PhanfareConnector
{
	class PhanfareSystem : ContainerBase, IContentSystem
	{
		private MediaServerDevice _device;
		private UPnPService _upnpService;
		private bool _isLoggedIn;

		public PhanfareService Api;
		public Session Session;
		public long UserID;
		public string EmailAddress;
		private bool _showHidden;

		public PhanfareUser Self;
		public List<PhanfareUser> Users;
		public List<PhanfareGroup> Groups;

		private const string ApiKey = "z4JFdIqyWEYKw943UvZi";
		private const string ApiSecret = "8qzZ8RZ_Occ8i9IgUnyo";

		public const string PhotoHandler = "photos";

		private Dictionary<string, ContainerBase> _containerLookup;
		private Dictionary<string, ContentBase> _contentLookup;

		public PhanfareSystem( MediaServerDevice device, UPnPService upnpService )
			: base( null )
		{
			_device = device;
			_upnpService = upnpService;

			this.ID = "16";

			_containerLookup = new Dictionary<string, ContainerBase>();
			_contentLookup = new Dictionary<string, ContentBase>();
		}

		public string BaseURL
		{
			get
			{
				return "http://" + _upnpService.ParentDevice.LastEndPoint.ToString();
			}
		}
		public bool ShowHidden { get { return _showHidden; } }
		public override string Name { get { return "Users"; } }
		public bool IsLoggedIn { get { return _isLoggedIn; } }

		public bool Login()
		{
			_isLoggedIn = false;
			IniReader iniReader = new IniReader( _device.SettingsFilePath );
			this.EmailAddress = iniReader.ReadString( "Phanfare", "EmailAddress", string.Empty );
			string password = iniReader.ReadString( "Phanfare", "Password", string.Empty );
			if( ( this.EmailAddress == string.Empty ) ||
				( password == string.Empty ) )
				return false;
			password = Security.Decrypt( password );

			_showHidden = iniReader.ReadBoolean( "Security", "ShowHidden", false );

			this.Api = new PhanfareService( ApiKey, ApiSecret, false, false );
			this.Session = this.Api.Authenticate( this.EmailAddress, password );
			if( this.Session == null )
			{
				// Failed
				return false;
			}

			this.UserID = this.Session.UserID;
			this.Self = new PhanfareUser( this, this.UserID, true, this.Session.Profile );
			this.AddChild( this.UserID.ToString(), this.Self );

			// Pull friends/family
			PublicProfile[] profiles = this.Api.GetMyRelationships();
			List<PublicProfile> sourceProfiles = new List<PublicProfile>( profiles );
			sourceProfiles.Sort( new PublicProfileComparer() );
			this.Users = new List<PhanfareUser>( sourceProfiles.Count );
			List<PhanfareUser> family = new List<PhanfareUser>();
			List<PhanfareUser> friends = new List<PhanfareUser>();
			foreach( PublicProfile profile in sourceProfiles )
			{
				PhanfareUser user = new PhanfareUser( this, profile.UserID, false, profile );
				this.Users.Add( user );
				switch( profile.Relation )
				{
					case Relation.Friend:
						friends.Add( user );
						break;
					case Relation.Family:
						family.Add( user );
						break;
				}
			}

			// Get groups that the user is a member of
			GroupInfo[] groups = this.Api.GetMyGroups();
			List<GroupInfo> sourceGroups = new List<GroupInfo>( groups );
			sourceGroups.Sort( new GroupInfoComparer() );
			this.Groups = new List<PhanfareGroup>( sourceGroups.Count );

			// Build friends/family dummy grounds
			GroupInfo familyGroupInfo = new GroupInfo();
			familyGroupInfo.GroupID = this.Session.FamilyGroupID;
			familyGroupInfo.Name = "Family";
			PhanfareGroup familyGroup = new PhanfareGroup( this, familyGroupInfo, GroupType.FriendsFamily );
			this.AddChild( familyGroupInfo.GroupID.ToString(), familyGroup );
			this.Groups.Add( familyGroup );
			familyGroup.Populate( family );

			GroupInfo friendGroupInfo = new GroupInfo();
			friendGroupInfo.GroupID = this.Session.FriendGroupID;
			friendGroupInfo.Name = "Friends";
			PhanfareGroup friendGroup = new PhanfareGroup( this, friendGroupInfo, GroupType.FriendsFamily );
			this.AddChild( friendGroupInfo.GroupID.ToString(), friendGroup );
			this.Groups.Add( friendGroup );
			friendGroup.Populate( friends );

			// Add all the other groups
			foreach( GroupInfo groupInfo in sourceGroups )
			{
				PhanfareGroup group = new PhanfareGroup( this, groupInfo, GroupType.UserCreated );
				this.Groups.Add( group );
				this.AddChild( groupInfo.GroupID.ToString(), group );

				// Don't care?
				//PublicProfile[] members = this.ApiSession.GetGroupRelationships( groupInfo.group_id );
			}

			this.Self.Populate( this.Api.GetAlbumList( this.UserID ) );

			_isLoggedIn = true;
			return true;
		}

		public void Update()
		{
			this.UpdateUser( this.Self );
			foreach( PhanfareUser user in this.Users )
			{
				if( user.IsCached == false )
					continue;
				this.UpdateUser( user );
			}
		}

		private void UpdateUser( PhanfareUser user )
		{
			Album[] albums = this.Api.GetAlbumList( user.UserID, false, user.LastTimePopulated );
			user.Populate( albums );
			foreach( PhanfareAlbum album in user.Albums )
			{
				if( ( album.IsCached == true ) &&
					( album.Object.LastModified > album.LastTimePopulated ) )
				{
					album.Populate( this.Api.GetAlbum( user.UserID, album.Object.AlbumID ) );
				}
			}
		}

		#region Containers

		public void RegisterContainer( string id, ContainerBase container )
		{
			_containerLookup[ id ] = container;
		}

		public void RegisterContent( string id, ContentBase content )
		{
			_contentLookup[ id ] = content;
		}

		public ContainerBase LookupContainer( string id )
		{
			return _containerLookup[ id ];
		}

		public ContentBase LookupContent( string id )
		{
			return _contentLookup[ id ];
		}

		#endregion

		#region Dynamic Population

		public bool EnsurePopulated( ContainerBase container )
		{
			if( container is PhanfareUser )
			{
				PhanfareUser user = ( PhanfareUser )container;
				if( user.IsCached == false )
					user.Populate( this.Api.GetAlbumList( user.UserID ) );
			}
			else if( container is PhanfareGroup )
			{
				PhanfareGroup group = ( PhanfareGroup )container;
				if( group.IsCached == false )
				{
					switch( group.Type )
					{
						case GroupType.FriendsFamily:
							PublicProfile[] profiles = this.Api.GetMyRelationships();
							List<PublicProfile> sourceProfiles = new List<PublicProfile>( profiles );
							sourceProfiles.Sort( new PublicProfileComparer() );
							// ... stuff
							break;
						case GroupType.UserCreated:
							group.Populate( this.Api.GetGroupAlbumList( group.Object.GroupID ) );
							break;
					}
				}
			}
			else if( container is PhanfareAlbum )
			{
				PhanfareAlbum album = ( PhanfareAlbum )container;
				if( album.IsCached == false )
					album.Populate( this.Api.GetAlbum( album.Object.OwnerID, album.Object.AlbumID ) );
			}
			else if( container is PhanfareSection )
			{
				// Ignored
			}
			return true;
		}

		#endregion

		#region Random

		protected override RandomContentResult OnRandomContentRequested( uint startingIndex, uint requestCount )
		{
			// Request over entire user collections
			return base.OnRandomContentRequested( startingIndex, requestCount );
		}

		#endregion

		#region File Handlers

		public HandleResult Handle( string getWhat )
		{
			Console.WriteLine( "PhanfareSystem::Handle( " + getWhat + " )" );
			if( getWhat.StartsWith( "16/" ) == true )
			{
				//16/uid/albumid/sectionid_imageid.renditiontype.jpg
				string contentId = Path.ChangeExtension( getWhat, null );
				string renditionType = Path.GetExtension( contentId ).Substring( 1 );
				contentId = Path.ChangeExtension( contentId, null );
				ContentBase content = this.LookupContent( contentId );
				PhanfareImage image = ( PhanfareImage )content;

				ImageRendition resultRendition = null;
				foreach( ImageRendition rendition in image.Object.Renditions )
				{
					if( rendition.Type == renditionType )
					{
						resultRendition = rendition;
						break;
					}
				}

				byte[] buffer = this.Api.GetImageData( resultRendition );
				HandleResult result = new HandleResult( "image/jpeg", buffer );
				return result;

				// This doesn't work - not sure if it's the Intel stack or the 360, but streaming
				// straight from the web to the 360 would be ideal
				//HttpWebRequest request = ( HttpWebRequest )HttpWebRequest.Create( resultRendition.url );
				//request.Timeout = 60000;
				//request.Method = "GET";
				//request.CookieContainer = new CookieContainer( 1 );
				//request.CookieContainer.Add( this.ApiSession.GetCookie() );
				//HttpWebResponse response = ( HttpWebResponse )request.GetResponse();

				//string contentType = response.Headers[ "Content-Type" ];
				//Stream responseStream = response.GetResponseStream();
				//HandleResult result = new HandleResult( contentType, responseStream, response.ContentLength );
				//return result;
			}
			return null;
		}

		#endregion
	}
}
