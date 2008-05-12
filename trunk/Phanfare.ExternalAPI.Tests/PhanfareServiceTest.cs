using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phanfare.ExternalAPI;
using System.Configuration;

namespace Phanfare.ExternalAPI.Tests
{
	/// <summary>
	///This is a test class for PhanfareServiceTest and is intended
	///to contain all PhanfareServiceTest Unit Tests
	///</summary>
	[TestClass()]
	public class PhanfareServiceTest
	{
		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext { get; set; }

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		[ClassInitialize()]
		public static void MyClassInitialize( TestContext testContext )
		{
			AppSettingsReader reader = new AppSettingsReader();
			ApiKey = ( string )reader.GetValue( "ApiKey", typeof( string ) );
			ApiSecret = ( string )reader.GetValue( "ApiSecret", typeof( string ) );
			EmailAddress = ( string )reader.GetValue( "EmailAddress", typeof( string ) );
			Password = ( string )reader.GetValue( "Password", typeof( string ) );
		}

		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion

		private static string ApiKey;
		private static string ApiSecret;

		private static string EmailAddress;
		private static string Password;

		/// <summary>
		///A test for Authenticate
		///</summary>
		[TestMethod()]
		public void AuthenticateTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;

			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			string emailAddress = EmailAddress + "BAD";
			string password = Password;
			try
			{
				Session actual = target.Authenticate( emailAddress, password );
				Assert.Fail( "Authenticate returned a session when it should have thrown" );
			}
			catch
			{
			}

			emailAddress = EmailAddress;
			password = Password + "BAD";
			try
			{
				Session actual = target.Authenticate( emailAddress, password );
				Assert.Fail( "Authenticate returned a session when it should have thrown" );
			}
			catch
			{
			}

			emailAddress = EmailAddress;
			password = Password;
			try
			{
				Session actual = target.Authenticate( emailAddress, password );
				Assert.IsNotNull( actual );
				Assert.IsNotNull( actual.SessionCookie );
				Assert.IsNotNull( actual.Profile );
				Assert.AreNotEqual( actual.FamilyGroupID, 0 );
				Assert.AreNotEqual( actual.FriendGroupID, 0 );
				Assert.AreNotEqual( actual.UserID, 0 );
			}
			catch
			{
				Assert.Fail( "Authenticate threw an exception" );
			}
		}

		#region GetAlbumList

		/// <summary>
		///A test for GetAlbumList
		///</summary>
		[TestMethod()]
		public void GetAlbumListTest1()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			Session session = target.Authenticate( EmailAddress, Password );

			long userId = session.UserID;
			Album[] actual = target.GetAlbumList( userId );
			Assert.IsNotNull( actual );
			Assert.AreNotEqual( actual.Length, 0 );
		}

		/// <summary>
		///A test for GetAlbumList
		///</summary>
		[TestMethod()]
		public void GetAlbumListTest2()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			Session session = target.Authenticate( EmailAddress, Password );

			long userId = session.UserID;
			int year = 2008;
			Album[] actual = target.GetAlbumList( userId, true, year );
			Assert.IsNotNull( actual );
			Assert.AreNotEqual( actual.Length, 0 );
		}

		/// <summary>
		///A test for GetAlbumList
		///</summary>
		[TestMethod()]
		public void GetAlbumListTest3()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			Session session = target.Authenticate( EmailAddress, Password );

			long userId = session.UserID;
			DateTime modifiedAfter = DateTime.Now.Subtract( new TimeSpan( 20, 0, 0, 0 ) );
			Album[] actual = target.GetAlbumList( userId, true, modifiedAfter );
			Assert.IsNotNull( actual );
			Assert.AreNotEqual( actual.Length, 0 );
		}

		/// <summary>
		///A test for GetAlbumList
		///</summary>
		[TestMethod()]
		public void GetAlbumListTest4()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			Session session = target.Authenticate( EmailAddress, Password );

			long userId = session.UserID;
			DateTime modifiedAfter = DateTime.Now.Subtract( new TimeSpan( 20, 0, 0, 0 ) );
			int year = 2008;
			Album[] actual = target.GetAlbumList( userId, true, modifiedAfter, year );
			Assert.IsNotNull( actual );
			Assert.AreNotEqual( actual.Length, 0 );
		}

		/// <summary>
		///A test for GetAlbumList
		///</summary>
		[TestMethod()]
		public void GetAlbumListTest5()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			Session session = target.Authenticate( EmailAddress, Password );

			long userId = session.UserID;
			Album[] actual = target.GetAlbumList( userId, true, new long[] { 1002792, } );
			Assert.IsNotNull( actual );
			Assert.AreNotEqual( actual.Length, 0 );
		}

		#endregion

		#region GetNewsFeed

		/// <summary>
		///A test for GetNewsFeed
		///</summary>
		[TestMethod()]
		public void GetNewsFeedTest3()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			Session session = target.Authenticate( EmailAddress, Password );

			DateTime showSince = new DateTime();
			NewsItem[] actual = target.GetNewsFeed( showSince );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetNewsFeed
		///</summary>
		[TestMethod()]
		public void GetNewsFeedTest2()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			Session session = target.Authenticate( EmailAddress, Password );

			DateTime showSince = DateTime.Now.Subtract( new TimeSpan( 5, 0, 0, 0, 0 ) );
			NewsItem[] actual = target.GetNewsFeed( showSince );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetNewsFeed
		///</summary>
		[TestMethod()]
		public void GetNewsFeedTest1()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			Session session = target.Authenticate( EmailAddress, Password );

			int maximumItems = 1;
			NewsItem[] actual = target.GetNewsFeed( maximumItems );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetNewsFeed
		///</summary>
		[TestMethod()]
		public void GetNewsFeedTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			Session session = target.Authenticate( EmailAddress, Password );

			Nullable<int> maximumItems = 1;
			Nullable<DateTime> showSince = DateTime.Now.Subtract( new TimeSpan( 5, 0, 0, 0 ) );
			NewsItem[] actual = target.GetNewsFeed( maximumItems, showSince );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		#endregion

		#region GetYearList

		/// <summary>
		///A test for GetYearList
		///</summary>
		[TestMethod()]
		public void GetYearListTest1()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			Session session = target.Authenticate( EmailAddress, Password );

			long userId = session.UserID;
			Year[] actual = target.GetYearList( userId );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		#endregion







		/// <summary>
		///A test for UpdateSection
		///</summary>
		[TestMethod()]
		public void UpdateSectionTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			long sectionId = 0;
			string fieldToUpdate = string.Empty;
			object fieldValue = null;
			target.UpdateSection( userId, albumId, sectionId, fieldToUpdate, fieldValue );
			Assert.Inconclusive( "A method that does not return a value cannot be verified." );
		}

		/// <summary>
		///A test for UpdateImageCaption
		///</summary>
		[TestMethod()]
		public void UpdateImageCaptionTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			long sectionId = 0;
			long imageId = 0;
			string caption = string.Empty;
			target.UpdateImageCaption( userId, albumId, sectionId, imageId, caption );
			Assert.Inconclusive( "A method that does not return a value cannot be verified." );
		}

		/// <summary>
		///A test for UpdateImage
		///</summary>
		[TestMethod()]
		public void UpdateImageTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			long sectionId = 0;
			long imageId = 0;
			string fieldToUpdate = string.Empty;
			object fieldValue = null;
			target.UpdateImage( userId, albumId, sectionId, imageId, fieldToUpdate, fieldValue );
			Assert.Inconclusive( "A method that does not return a value cannot be verified." );
		}

		/// <summary>
		///A test for UpdateAlbum
		///</summary>
		[TestMethod()]
		public void UpdateAlbumTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			string fieldToUpdate = string.Empty;
			object fieldValue = null;
			target.UpdateAlbum( userId, albumId, fieldToUpdate, fieldValue );
			Assert.Inconclusive( "A method that does not return a value cannot be verified." );
		}

		/// <summary>
		///A test for NewSection
		///</summary>
		[TestMethod()]
		public void NewSectionTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			Section section = null;
			Section expected = null;
			Section actual;
			actual = target.NewSection( userId, albumId, section );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for NewImage
		///</summary>
		[TestMethod()]
		public void NewImageTest2()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			long sectionId = 0;
			ImageInfo image = null;
			bool externalLinks = false;
			ImageInfo expected = null;
			ImageInfo actual;
			actual = target.NewImage( userId, albumId, sectionId, image, externalLinks );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for NewImage
		///</summary>
		[TestMethod()]
		public void NewImageTest1()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			long sectionId = 0;
			ImageInfo image = null;
			ImageInfo expected = null;
			ImageInfo actual;
			actual = target.NewImage( userId, albumId, sectionId, image );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for NewImage
		///</summary>
		[TestMethod()]
		public void NewImageTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			long sectionId = 0;
			ImageInfo image = null;
			bool setAsProfilePicture = false;
			bool externalLinks = false;
			ImageInfo expected = null;
			ImageInfo actual;
			actual = target.NewImage( userId, albumId, sectionId, image, setAsProfilePicture, externalLinks );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for NewAlbum
		///</summary>
		[TestMethod()]
		public void NewAlbumTest1()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			Album album = null;
			long[] groups = null;
			Album expected = null;
			Album actual;
			actual = target.NewAlbum( userId, album, groups );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for NewAlbum
		///</summary>
		[TestMethod()]
		public void NewAlbumTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			Album album = null;
			Album expected = null;
			Album actual;
			actual = target.NewAlbum( userId, album );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for NewAccount
		///</summary>
		[TestMethod()]
		public void NewAccountTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			string emailAddress = string.Empty;
			string firstName = string.Empty;
			string lastName = string.Empty;
			string promotionCode = string.Empty;
			target.NewAccount( emailAddress, firstName, lastName, promotionCode );
			Assert.Inconclusive( "A method that does not return a value cannot be verified." );
		}

		/// <summary>
		///A test for HideImage
		///</summary>
		[TestMethod()]
		public void HideImageTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			long sectionId = 0;
			long imageId = 0;
			bool isHidden = false;
			target.HideImage( userId, albumId, sectionId, imageId, isHidden );
			Assert.Inconclusive( "A method that does not return a value cannot be verified." );
		}

		/// <summary>
		///A test for GetSectionImages
		///</summary>
		[TestMethod()]
		public void GetSectionImagesTest1()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			long sectionId = 0;
			bool externalLinks = false;
			ImageInfo[] expected = null;
			ImageInfo[] actual;
			actual = target.GetSectionImages( userId, albumId, sectionId, externalLinks );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetSectionImages
		///</summary>
		[TestMethod()]
		public void GetSectionImagesTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			long sectionId = 0;
			ImageInfo[] expected = null;
			ImageInfo[] actual;
			actual = target.GetSectionImages( userId, albumId, sectionId );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetRandomImages
		///</summary>
		[TestMethod()]
		public void GetRandomImagesTest2()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			RandomMode mode = new RandomMode();
			int imageCount = 0;
			ImageInfo[] expected = null;
			ImageInfo[] actual;
			actual = target.GetRandomImages( userId, mode, imageCount );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetRandomImages
		///</summary>
		[TestMethod()]
		[DeploymentItem( "Phanfare.ExternalAPI.dll" )]
		public void GetRandomImagesTest1()
		{
			PrivateObject param0 = null;
			PhanfareService_Accessor target = new PhanfareService_Accessor( param0 );
			string parameterName = string.Empty;
			long value = 0;
			RandomMode mode = new RandomMode();
			int imageCount = 0;
			bool externalLinks = false;
			ImageInfo[] expected = null;
			ImageInfo[] actual;
			actual = target.GetRandomImages( parameterName, value, mode, imageCount, externalLinks );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetRandomImages
		///</summary>
		[TestMethod()]
		public void GetRandomImagesTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			RandomMode mode = new RandomMode();
			int imageCount = 0;
			bool externalLinks = false;
			ImageInfo[] expected = null;
			ImageInfo[] actual;
			actual = target.GetRandomImages( userId, mode, imageCount, externalLinks );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetMyRelationships
		///</summary>
		[TestMethod()]
		public void GetMyRelationshipsTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			PublicProfile[] expected = null;
			PublicProfile[] actual;
			actual = target.GetMyRelationships();
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetMyGroups
		///</summary>
		[TestMethod()]
		public void GetMyGroupsTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			GroupInfo[] expected = null;
			GroupInfo[] actual;
			actual = target.GetMyGroups();
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetMobileAlbum
		///</summary>
		[TestMethod()]
		public void GetMobileAlbumTest1()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			Album expected = null;
			Album actual;
			actual = target.GetMobileAlbum( userId );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetMobileAlbum
		///</summary>
		[TestMethod()]
		public void GetMobileAlbumTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			bool useExisting = false;
			Album expected = null;
			Album actual;
			actual = target.GetMobileAlbum( userId, useExisting );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetImageInfo
		///</summary>
		[TestMethod()]
		public void GetImageInfoTest1()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			long sectionId = 0;
			long imageId = 0;
			bool externalLinks = false;
			ImageInfo expected = null;
			ImageInfo actual;
			actual = target.GetImageInfo( userId, albumId, sectionId, imageId, externalLinks );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetImageInfo
		///</summary>
		[TestMethod()]
		public void GetImageInfoTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			long sectionId = 0;
			long imageId = 0;
			ImageInfo expected = null;
			ImageInfo actual;
			actual = target.GetImageInfo( userId, albumId, sectionId, imageId );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetImageData
		///</summary>
		[TestMethod()]
		public void GetImageDataTest1()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			ImageRendition rendition = null;
			byte[] buffer = null;
			byte[] bufferExpected = null;
			int expected = 0;
			int actual;
			actual = target.GetImageData( rendition, ref buffer );
			Assert.AreEqual( bufferExpected, buffer );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetImageData
		///</summary>
		[TestMethod()]
		public void GetImageDataTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			ImageRendition rendition = null;
			byte[] expected = null;
			byte[] actual;
			actual = target.GetImageData( rendition );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetGroupYearList
		///</summary>
		[TestMethod()]
		public void GetGroupYearListTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long groupId = 0;
			Year[] expected = null;
			Year[] actual;
			actual = target.GetGroupYearList( groupId );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetGroupRelationships
		///</summary>
		[TestMethod()]
		public void GetGroupRelationshipsTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long groupId = 0;
			PublicProfile[] expected = null;
			PublicProfile[] actual;
			actual = target.GetGroupRelationships( groupId );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetGroupRandomImages
		///</summary>
		[TestMethod()]
		public void GetGroupRandomImagesTest1()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long groupId = 0;
			RandomMode mode = new RandomMode();
			int imageCount = 0;
			bool externalLinks = false;
			ImageInfo[] expected = null;
			ImageInfo[] actual;
			actual = target.GetGroupRandomImages( groupId, mode, imageCount, externalLinks );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetGroupRandomImages
		///</summary>
		[TestMethod()]
		public void GetGroupRandomImagesTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long groupId = 0;
			RandomMode mode = new RandomMode();
			int imageCount = 0;
			ImageInfo[] expected = null;
			ImageInfo[] actual;
			actual = target.GetGroupRandomImages( groupId, mode, imageCount );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetGroupAlbumList
		///</summary>
		[TestMethod()]
		public void GetGroupAlbumListTest3()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long groupId = 0;
			Album[] expected = null;
			Album[] actual;
			actual = target.GetGroupAlbumList( groupId );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetGroupAlbumList
		///</summary>
		[TestMethod()]
		public void GetGroupAlbumListTest2()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long groupId = 0;
			DateTime modifiedAfter = DateTime.MinValue;
			int year = 0;
			Album[] expected = null;
			Album[] actual;
			actual = target.GetGroupAlbumList( groupId, true, modifiedAfter, year );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetGroupAlbumList
		///</summary>
		[TestMethod()]
		public void GetGroupAlbumListTest1()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long groupId = 0;
			int year = 0;
			Album[] expected = null;
			Album[] actual;
			actual = target.GetGroupAlbumList( groupId, true, year );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetGroupAlbumList
		///</summary>
		[TestMethod()]
		public void GetGroupAlbumListTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long groupId = 0;
			DateTime modifiedAfter = new DateTime();
			Album[] expected = null;
			Album[] actual;
			actual = target.GetGroupAlbumList( groupId, true, modifiedAfter );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetAlbumImages
		///</summary>
		[TestMethod()]
		public void GetAlbumImagesTest1()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			ImageInfo[] expected = null;
			ImageInfo[] actual;
			actual = target.GetAlbumImages( userId, albumId );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetAlbumImages
		///</summary>
		[TestMethod()]
		public void GetAlbumImagesTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			bool externalLinks = false;
			ImageInfo[] expected = null;
			ImageInfo[] actual;
			actual = target.GetAlbumImages( userId, albumId, externalLinks );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for GetAlbum
		///</summary>
		[TestMethod()]
		public void GetAlbumTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			Album expected = null;
			Album actual;
			actual = target.GetAlbum( userId, albumId );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}

		/// <summary>
		///A test for DeleteSection
		///</summary>
		[TestMethod()]
		public void DeleteSectionTest1()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			long sectionId = 0;
			target.DeleteSection( userId, albumId, sectionId );
			Assert.Inconclusive( "A method that does not return a value cannot be verified." );
		}

		/// <summary>
		///A test for DeleteSection
		///</summary>
		[TestMethod()]
		public void DeleteSectionTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			long sectionId = 0;
			bool deleteForever = false;
			target.DeleteSection( userId, albumId, sectionId, deleteForever );
			Assert.Inconclusive( "A method that does not return a value cannot be verified." );
		}

		/// <summary>
		///A test for DeleteImage
		///</summary>
		[TestMethod()]
		public void DeleteImageTest1()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			long sectionId = 0;
			long imageId = 0;
			bool deleteForever = false;
			target.DeleteImage( userId, albumId, sectionId, imageId, deleteForever );
			Assert.Inconclusive( "A method that does not return a value cannot be verified." );
		}

		/// <summary>
		///A test for DeleteImage
		///</summary>
		[TestMethod()]
		public void DeleteImageTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			long sectionId = 0;
			long imageId = 0;
			target.DeleteImage( userId, albumId, sectionId, imageId );
			Assert.Inconclusive( "A method that does not return a value cannot be verified." );
		}

		/// <summary>
		///A test for DeleteAlbum
		///</summary>
		[TestMethod()]
		public void DeleteAlbumTest1()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			bool deleteForever = false;
			target.DeleteAlbum( userId, albumId, deleteForever );
			Assert.Inconclusive( "A method that does not return a value cannot be verified." );
		}

		/// <summary>
		///A test for DeleteAlbum
		///</summary>
		[TestMethod()]
		public void DeleteAlbumTest()
		{
			string apiKey = ApiKey;
			string apiSecret = ApiSecret;
			PhanfareService target = new PhanfareService( apiKey, apiSecret );
			long userId = 0;
			long albumId = 0;
			target.DeleteAlbum( userId, albumId );
			Assert.Inconclusive( "A method that does not return a value cannot be verified." );
		}

		/// <summary>
		///A test for GetGroupAlbumList
		///</summary>
		[TestMethod()]
		public void GetGroupAlbumListTest4()
		{
			string apiKey = string.Empty; // TODO: Initialize to an appropriate value
			string apiSecret = string.Empty; // TODO: Initialize to an appropriate value
			PhanfareService target = new PhanfareService( apiKey, apiSecret ); // TODO: Initialize to an appropriate value
			long groupId = 0; // TODO: Initialize to an appropriate value
			bool externalLinks = false; // TODO: Initialize to an appropriate value
			long[] albumIds = null; // TODO: Initialize to an appropriate value
			Album[] expected = null; // TODO: Initialize to an appropriate value
			Album[] actual;
			actual = target.GetGroupAlbumList( groupId, externalLinks, albumIds );
			Assert.AreEqual( expected, actual );
			Assert.Inconclusive( "Verify the correctness of this test method." );
		}
	}
}
