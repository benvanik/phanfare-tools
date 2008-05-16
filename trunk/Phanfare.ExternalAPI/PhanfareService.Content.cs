using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;

namespace Phanfare.ExternalAPI
{
	public enum RandomMode
	{
		Random,
		NewestAlbumCreationDate,
		NewestAlbumStartDate,
		NewestImages
	}

	public partial class PhanfareService
	{
		public Year[] GetYearList( long userId )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );

			return this.GetYearList( "target_uid", userId );
		}

		public Year[] GetGroupYearList( long groupId )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "groupId", groupId );

			return this.GetYearList( "group_id", groupId );
		}

		private Year[] GetYearList( string parameterName, long value )
		{
			Hashtable ht = this.MethodCall( "getyearlist" );
			ht[ parameterName ] = value;

			XmlDocument doc = this.MakeRequest( ht );
			return this.ReadResponseList<Year>( doc, "years", "num_years", Year.FromXML );
		}

		public Album[] GetAlbumList( long userId )
		{
			return this.GetAlbumList( userId, false, null, null, null );
		}

		public Album[] GetAlbumList( long userId, bool externalLinks )
		{
			return this.GetAlbumList( userId, externalLinks, null, null, null );
		}

		public Album[] GetAlbumList( long userId, bool externalLinks, DateTime modifiedAfter )
		{
			return this.GetAlbumList( userId, externalLinks, modifiedAfter, null, null );
		}

		public Album[] GetAlbumList( long userId, bool externalLinks, int year )
		{
			return this.GetAlbumList( userId, externalLinks, null, year, null );
		}

		public Album[] GetAlbumList( long userId, bool externalLinks, long[] albumIds )
		{
			return this.GetAlbumList( userId, externalLinks, null, null, albumIds );
		}

		public Album[] GetAlbumList( long userId, bool externalLinks, DateTime modifiedAfter, int year )
		{
			return this.GetAlbumList( userId, externalLinks, modifiedAfter, year, null );
		}

		public Album[] GetAlbumList( long userId, bool externalLinks, DateTime? modifiedAfter, int? year, long[] albumIds )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );

			return this.GetAlbumList( "target_uid", userId, externalLinks, modifiedAfter, year, albumIds );
		}

		public Album[] GetGroupAlbumList( long groupId )
		{
			return this.GetGroupAlbumList( groupId, false, null, null, null );
		}

		public Album[] GetGroupAlbumList( long groupId, bool externalLinks )
		{
			return this.GetGroupAlbumList( groupId, externalLinks, null, null, null );
		}

		public Album[] GetGroupAlbumList( long groupId, bool externalLinks, DateTime modifiedAfter )
		{
			return this.GetGroupAlbumList( groupId, externalLinks, modifiedAfter, null, null );
		}

		public Album[] GetGroupAlbumList( long groupId, bool externalLinks, int year )
		{
			return this.GetGroupAlbumList( groupId, externalLinks, null, year, null );
		}

		public Album[] GetGroupAlbumList( long groupId, bool externalLinks, long[] albumIds )
		{
			return this.GetGroupAlbumList( groupId, externalLinks, null, null, albumIds );
		}

		public Album[] GetGroupAlbumList( long groupId, bool externalLinks, DateTime modifiedAfter, int year )
		{
			return this.GetGroupAlbumList( groupId, externalLinks, modifiedAfter, year, null );
		}

		public Album[] GetGroupAlbumList( long groupId, bool externalLinks, DateTime? modifiedAfter, int? year, long[] albumIds )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "groupId", groupId );

			return this.GetAlbumList( "group_id", groupId, externalLinks, modifiedAfter, year, albumIds );
		}

		private Album[] GetAlbumList( string parameterName, long value, bool externalLinks, DateTime? modifiedAfter, int? year, long[] albumIds )
		{
			if( ( albumIds != null ) && ( albumIds.Length == 0 ) )
				return new Album[ 0 ];

			Hashtable ht = this.MethodCall( "getalbumlist" );
			ht[ parameterName ] = value;
			if( modifiedAfter != null )
				ht[ "modified_after" ] = modifiedAfter.Value.ToUniversalTime();
			if( year != null )
				ht[ "year" ] = year.Value;
			if( externalLinks == true )
				ht[ "external_links" ] = externalLinks;
			if( albumIds != null )
				ht[ "album_ids" ] = albumIds.ToOrderedString();

			XmlDocument doc = this.MakeRequest( ht );
			return this.ReadResponseList<Album>( doc, "albums", "num_albums", Album.FromXML );
		}

		public Album GetAlbum( long userId, long albumId )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );
			this.AssertParameterValidID( "albumId", albumId );

			Hashtable ht = this.MethodCall( "getalbum" );
			ht[ "target_uid" ] = userId;
			ht[ "album_id" ] = albumId;

			XmlDocument doc = this.MakeRequest( ht );
			return this.ReadResponseItem<Album>( doc, Album.FromXML );
		}

		public Album GetMobileAlbum( long userId )
		{
			return this.GetMobileAlbum( userId, false );
		}

		public Album GetMobileAlbum( long userId, bool useExisting )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );

			Hashtable ht = this.MethodCall( "getalbum" );
			ht[ "method" ] = "getalbum";
			ht[ "target_uid" ] = userId;
			ht[ "album_id" ] = 1;
			ht[ "mobile" ] = 1;
			ht[ "use_existing_mobile" ] = useExisting;

			XmlDocument doc = this.MakeRequest( ht );
			return this.ReadResponseItem<Album>( doc, Album.FromXML );
		}

		public ImageInfo GetImageInfo( long userId, long albumId, long sectionId, long imageId )
		{
			return this.GetImageInfo( userId, albumId, sectionId, imageId, false );
		}

		public ImageInfo GetImageInfo( long userId, long albumId, long sectionId, long imageId, bool externalLinks )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );
			this.AssertParameterValidID( "albumId", albumId );
			this.AssertParameterValidID( "sectionId", sectionId );
			this.AssertParameterValidID( "imageId", imageId );

			Hashtable ht = this.MethodCall( "getimageinfo" );
			ht[ "target_uid" ] = userId;
			ht[ "album_id" ] = albumId;
			ht[ "section_id" ] = sectionId;
			ht[ "image_id" ] = imageId;
			ht[ "external_links" ] = externalLinks;

			XmlDocument doc = this.MakeRequest( ht );
			return this.ReadResponseItem<ImageInfo>( doc, ImageInfo.FromXML );
		}

		public ImageInfo[] GetAlbumImages( long userId, long albumId )
		{
			return this.GetAlbumImages( userId, albumId, false );
		}

		public ImageInfo[] GetAlbumImages( long userId, long albumId, bool externalLinks )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );
			this.AssertParameterValidID( "albumId", albumId );

			Hashtable ht = this.MethodCall( "getimages" );
			ht[ "target_uid" ] = userId;
			ht[ "album_id" ] = albumId;
			ht[ "image_mode" ] = "specificalbum";
			ht[ "external_links" ] = externalLinks;

			XmlDocument doc = this.MakeRequest( ht );
			return this.ReadResponseList<ImageInfo>( doc, "images", "num_images", ImageInfo.FromXML );
		}

		public ImageInfo[] GetSectionImages( long userId, long albumId, long sectionId )
		{
			return this.GetSectionImages( userId, albumId, sectionId, false, null, null );
		}

		public ImageInfo[] GetSectionImages( long userId, long albumId, long sectionId, bool externalLinks )
		{
			return this.GetSectionImages( userId, albumId, sectionId, externalLinks, null, null );
		}

		public ImageInfo[] GetSectionImages( long userId, long albumId, long sectionId, bool externalLinks, int? startIndex, int? itemCount )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );
			this.AssertParameterValidID( "albumId", albumId );
			this.AssertParameterValidID( "sectionId", sectionId );

			Hashtable ht = this.MethodCall( "getsectionimages" );
			ht[ "target_uid" ] = userId;
			ht[ "album_id" ] = albumId;
			ht[ "section_id" ] = sectionId;
			ht[ "external_links" ] = externalLinks;
			if( startIndex != null )
				ht[ "start_index" ] = startIndex.Value;
			if( itemCount != null )
				ht[ "items_requested" ] = itemCount.Value;

			XmlDocument doc = this.MakeRequest( ht );
			return this.ReadResponseList<ImageInfo>( doc, "images", "num_images", ImageInfo.FromXML );
		}

		public ImageInfo[] GetRandomImages( long userId, RandomMode mode, int imageCount )
		{
			return this.GetRandomImages( userId, mode, imageCount, false );
		}

		public ImageInfo[] GetRandomImages( long userId, RandomMode mode, int imageCount, bool externalLinks )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );

			return this.GetRandomImages( "target_uid", userId, mode, imageCount, externalLinks );
		}

		public ImageInfo[] GetGroupRandomImages( long groupId, RandomMode mode, int imageCount )
		{
			return this.GetGroupRandomImages( groupId, mode, imageCount, false );
		}

		public ImageInfo[] GetGroupRandomImages( long groupId, RandomMode mode, int imageCount, bool externalLinks )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "groupId", groupId );

			return this.GetRandomImages( "group_id", groupId, mode, imageCount, externalLinks );
		}

		private ImageInfo[] GetRandomImages( string parameterName, long value, RandomMode mode, int imageCount, bool externalLinks )
		{
			Hashtable ht = this.MethodCall( "getimages" );
			ht[ parameterName ] = value;
			ht[ "image_mode" ] = mode.ToString();
			ht[ "num_images" ] = imageCount;
			ht[ "external_links" ] = externalLinks;

			XmlDocument doc = this.MakeRequest( ht );
			return this.ReadResponseList<ImageInfo>( doc, "images", "num_images", ImageInfo.FromXML );
		}

		public Album NewAlbum( long userId, Album album )
		{
			return this.NewAlbum( userId, album, new long[ 0 ] );
		}

		public Album NewAlbum( long userId, Album album, long[] groups )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );
			this.AssertParameterNotNull( "album", album );
			this.AssertParameterNotNull( "groups", groups );

			Hashtable ht = this.MethodCall( "newalbum" );
			ht[ "target_uid" ] = userId;

			if( string.IsNullOrEmpty( album.Name ) == false )
				ht[ "album_name" ] = album.Name;
			if( string.IsNullOrEmpty( album.Description ) == false )
				ht[ "album_description" ] = album.Description;
			if( album.StartDate != null )
				ht[ "album_start_date" ] = album.StartDate.Value.ToUniversalTime();
			if( album.EndDate != null )
				ht[ "album_end_date" ] = album.EndDate.Value.ToUniversalTime();
			if( album.AutoDate != null )
				ht[ "auto_date" ] = album.AutoDate.Value ? "1" : "0";
			ht[ "type" ] = album.Type.ToString();
			if( groups.Length > 0 )
			{
				StringBuilder sb = new StringBuilder();
				foreach( long groupId in groups )
				{
					if( sb.Length > 0 )
						sb.Append( ',' );
					sb.Append( groupId.ToString() );
				}
				ht[ "groups" ] = sb.ToString();
			}

			XmlDocument doc = this.MakeRequest( ht );
			return this.ReadResponseItem<Album>( doc, Album.FromXML );
		}

		public Section NewSection( long userId, long albumId, Section section )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );
			this.AssertParameterValidID( "albumId", albumId );
			this.AssertParameterNotNull( "section", section );

			Hashtable ht = this.MethodCall( "newsection" );
			ht[ "target_uid" ] = userId;
			ht[ "album_id" ] = albumId;

			if( string.IsNullOrEmpty( section.Name ) == false )
				ht[ "section_name" ] = section.Name;
			if( string.IsNullOrEmpty( section.Description ) == false )
				ht[ "section_description" ] = section.Description;

			XmlDocument doc = this.MakeRequest( ht );
			return this.ReadResponseItem<Section>( doc, Section.FromXML );
		}

		public void DeleteAlbum( long userId, long albumId )
		{
			this.DeleteAlbum( userId, albumId, false );
		}

		public void DeleteAlbum( long userId, long albumId, bool deleteForever )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );
			this.AssertParameterValidID( "albumId", albumId );

			Hashtable ht = this.MethodCall( "deletealbum" );
			ht[ "target_uid" ] = userId;
			ht[ "album_id" ] = albumId;
			ht[ "to_trash" ] = !deleteForever;

			XmlDocument doc = this.MakeRequest( ht );
		}

		public void DeleteSection( long userId, long albumId, long sectionId )
		{
			this.DeleteSection( userId, albumId, sectionId, false );
		}

		public void DeleteSection( long userId, long albumId, long sectionId, bool deleteForever )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );
			this.AssertParameterValidID( "albumId", albumId );
			this.AssertParameterValidID( "sectionId", sectionId );

			Hashtable ht = this.MethodCall( "deletesection" );
			ht[ "target_uid" ] = userId;
			ht[ "album_id" ] = albumId;
			ht[ "section_id" ] = sectionId;
			ht[ "to_trash" ] = !deleteForever;

			XmlDocument doc = this.MakeRequest( ht );
		}

		public void DeleteImage( long userId, long albumId, long sectionId, long imageId )
		{
			this.DeleteImage( userId, albumId, sectionId, imageId, false );
		}

		public void DeleteImage( long userId, long albumId, long sectionId, long imageId, bool deleteForever )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );
			this.AssertParameterValidID( "albumId", albumId );
			this.AssertParameterValidID( "sectionId", sectionId );
			this.AssertParameterValidID( "imageId", imageId );

			Hashtable ht = this.MethodCall( "deleteimage" );
			ht[ "target_uid" ] = userId;
			ht[ "album_id" ] = albumId;
			ht[ "section_id" ] = sectionId;
			ht[ "image_id" ] = imageId;
			ht[ "to_trash" ] = !deleteForever;

			XmlDocument doc = this.MakeRequest( ht );
		}

		public void UpdateAlbum( long userId, long albumId, string fieldToUpdate, object fieldValue )
		{
			// album_name, album_description, album_start_date, album_end_date, album_type, groups

			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );
			this.AssertParameterValidID( "albumId", albumId );
			this.AssertParameterNotNullOrEmpty( "fieldToUpdate", fieldToUpdate );
			this.AssertParameterNotNull( "fieldValue", fieldValue );

			Hashtable ht = this.MethodCall( "updatealbum" );
			ht[ "target_uid" ] = userId;
			ht[ "album_id" ] = albumId;
			ht[ "field_to_update" ] = fieldToUpdate;
			ht[ "field_value" ] = fieldValue;

			XmlDocument doc = this.MakeRequest( ht );
		}

		public void UpdateSection( long userId, long albumId, long sectionId, string fieldToUpdate, object fieldValue )
		{
			// section_name, section_description

			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );
			this.AssertParameterValidID( "albumId", albumId );
			this.AssertParameterValidID( "sectionId", sectionId );
			this.AssertParameterNotNullOrEmpty( "fieldToUpdate", fieldToUpdate );
			this.AssertParameterNotNull( "fieldValue", fieldValue );

			Hashtable ht = this.MethodCall( "updatesection" );
			ht[ "target_uid" ] = userId;
			ht[ "album_id" ] = albumId;
			ht[ "section_id" ] = sectionId;
			ht[ "field_to_update" ] = fieldToUpdate;
			ht[ "field_value" ] = fieldValue;

			XmlDocument doc = this.MakeRequest( ht );
		}

		public void UpdateImage( long userId, long albumId, long sectionId, long imageId, string fieldToUpdate, object fieldValue )
		{
			// caption, image_date, hide

			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );
			this.AssertParameterValidID( "albumId", albumId );
			this.AssertParameterValidID( "sectionId", sectionId );
			this.AssertParameterValidID( "imageId", imageId );
			this.AssertParameterNotNullOrEmpty( "fieldToUpdate", fieldToUpdate );
			this.AssertParameterNotNull( "fieldValue", fieldValue );

			Hashtable ht = this.MethodCall( "updateimage" );
			ht[ "target_uid" ] = userId;
			ht[ "album_id" ] = albumId;
			ht[ "section_id" ] = sectionId;
			ht[ "image_id" ] = imageId;
			ht[ "field_to_update" ] = fieldToUpdate;
			ht[ "field_value" ] = fieldValue;

			XmlDocument doc = this.MakeRequest( ht );
		}

		public void UpdateImageCaption( long userId, long albumId, long sectionId, long imageId, string caption )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );
			this.AssertParameterValidID( "albumId", albumId );
			this.AssertParameterValidID( "sectionId", sectionId );
			this.AssertParameterValidID( "imageId", imageId );
			this.AssertParameterNotNull( "caption", caption );

			Hashtable ht = this.MethodCall( "updatecaption" );
			ht[ "target_uid" ] = userId;
			ht[ "album_id" ] = albumId;
			ht[ "section_id" ] = sectionId;
			ht[ "image_id" ] = imageId;
			ht[ "caption" ] = caption;

			XmlDocument doc = this.MakeRequest( ht );
		}

		public void HideImage( long userId, long albumId, long sectionId, long imageId, bool isHidden )
		{
			this.AssertSessionValid();
			this.AssertParameterValidID( "userId", userId );
			this.AssertParameterValidID( "albumId", albumId );
			this.AssertParameterValidID( "sectionId", sectionId );
			this.AssertParameterValidID( "imageId", imageId );

			Hashtable ht = this.MethodCall( "hideimage" );
			ht[ "target_uid" ] = userId;
			ht[ "album_id" ] = albumId;
			ht[ "section_id" ] = sectionId;
			ht[ "image_id" ] = imageId;
			ht[ "hidden" ] = isHidden;

			XmlDocument doc = this.MakeRequest( ht );
		}
	}
}
