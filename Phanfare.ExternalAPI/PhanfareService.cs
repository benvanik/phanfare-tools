using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Phanfare.ExternalAPI
{
	public partial class PhanfareService
	{
		private string _apiKey;
		private string _apiSecret;
		private bool _useHttps;
		private bool _asJson;

		private const string BaseUrl = "www.phanfare.com/api/?";

		public string SessionCookie { get; set; }

		public PhanfareService( string apiKey, string apiSecret )
			: this( apiKey, apiSecret, false, false )
		{
		}

		public PhanfareService( string apiKey, string apiSecret, bool useHttps, bool asJson )
		{
			this.AssertParameterNotNullOrEmpty( "apiKey", apiKey );
			this.AssertParameterNotNullOrEmpty( "apiSecret", apiSecret );

			_apiKey = apiKey;
			_apiSecret = apiSecret;
			_useHttps = useHttps;
			_asJson = asJson;
			this.SessionCookie = string.Empty;
		}

		public Session Authenticate( string emailAddress, string password )
		{
			this.AssertParameterNotNullOrEmpty( "emailAddress", emailAddress );
			this.AssertParameterNotNullOrEmpty( "password", password );

			Hashtable ht = this.MethodCall( "authenticate" );
			ht[ "email_address" ] = emailAddress;
			ht[ "password" ] = password;

			XmlDocument doc = this.MakeRequest( ht, true );
			Session s = Session.FromXML( ( XmlElement )doc.ChildNodes[ 1 ].FirstChild );
			if( s != null )
				this.SessionCookie = s.SessionCookie;
			return s;
		}

		public void NewAccount( string emailAddress, string firstName, string lastName, string promotionCode )
		{
			this.AssertParameterNotNullOrEmpty( "emailAddress", emailAddress );
			this.AssertParameterNotNullOrEmpty( "firstName", firstName );
			this.AssertParameterNotNullOrEmpty( "lastName", lastName );

			Hashtable ht = this.MethodCall( "newaccount" );
			ht[ "email_address" ] = emailAddress;
			ht[ "first_name" ] = firstName;
			ht[ "last_name" ] = lastName;
			ht[ "promotion_code" ] = promotionCode;

			XmlDocument doc = this.MakeRequest( ht );
		}

		public NewsItem[] GetNewsFeed()
		{
			return this.GetNewsFeed( null, null );
		}

		public NewsItem[] GetNewsFeed( int maximumItems )
		{
			return this.GetNewsFeed( maximumItems, null );
		}

		public NewsItem[] GetNewsFeed( DateTime showSince )
		{
			return this.GetNewsFeed( null, showSince );
		}

		public NewsItem[] GetNewsFeed( int? maximumItems, DateTime? showSince )
		{
			this.AssertSessionValid();

			Hashtable ht = this.MethodCall( "getnewsfeed" );
			if( maximumItems != null )
				ht[ "maximum_item_count" ] = maximumItems.Value;
			if( showSince != null )
				ht[ "show_since" ] = showSince.Value;

			XmlDocument doc = this.MakeRequest( ht );
			NewsItem[] items = this.ReadResponseList<NewsItem>( doc, "newsfeed", "num_items", NewsItem.FromXML );
			return items;
		}
	}
}
