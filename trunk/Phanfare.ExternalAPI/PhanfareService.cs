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
		private string _sessionCookie;
		private bool _asJson;

		private const string BaseUrl = "www.phanfare.com/api/?";

		public string SessionCookie { get { return _sessionCookie; } }

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
			_sessionCookie = string.Empty;
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
			_sessionCookie = s.SessionCookie;
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
	}
}
