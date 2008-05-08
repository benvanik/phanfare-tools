using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Phanfare.ExternalAPI
{
	public partial class PhanfareService
	{
		private const int BufferSize = 64 * 1024;
		private const int MethodTimeout = 2 * 60 * 1000;
		private const int DataTimeout = 3 * 60 * 1000;

		public Hashtable MethodCall( string methodName )
		{
			Hashtable ht = new Hashtable();
			ht[ "method" ] = methodName;
			return ht;
		}

		private void AssertSessionValid()
		{
			if( _sessionCookie == null )
				throw new PhanfareException( "Not logged in!" );
		}

		private void AssertParameterNotNull( string name, object value )
		{
			if( value == null )
				throw new ArgumentNullException( name );
		}

		private void AssertParameterNotNullOrEmpty( string name, string value )
		{
			if( value == null )
				throw new ArgumentNullException( name );
			if( value.Length == 0 )
				throw new ArgumentException( "Parameter must have a valid value.", name );
		}

		private void AssertParameterValidID( string name, long id )
		{
			if( id <= 0 )
				throw new ArgumentOutOfRangeException( name );
		}

		public Cookie GetCookie()
		{
			Cookie cookie = new Cookie();
			cookie.Domain = ".phanfare.com";
			cookie.Name = "phanfare2";
			cookie.Value = _sessionCookie;
			cookie.Expires = DateTime.Now.AddDays( 1 );
			return cookie;
		}

		private XmlDocument MakeRequest( Hashtable parameters )
		{
			return this.MakeRequest( parameters, _useHttps );
		}

		private XmlDocument MakeRequest( Hashtable parameters, bool secure )
		{
			HttpWebRequest request = ( WebRequest.Create( this.MakeURL( parameters, secure ) ) as HttpWebRequest );
			request.UserAgent = "Phanfare API v2";
			request.Timeout = MethodTimeout;
			request.ContentType = "multipart/form-data";
			request.Method = "GET";
			request.SendChunked = false;
			request.AutomaticDecompression = DecompressionMethods.GZip;
			request.CookieContainer = new CookieContainer( 1 );
			request.CookieContainer.Add( this.GetCookie() );

			string responseString;
			using( WebResponse response = request.GetResponse() )
			using( StreamReader reader = new StreamReader( response.GetResponseStream() ) )
				responseString = reader.ReadToEnd();

			return this.ParseResponse( responseString );
		}

		private XmlDocument MakeRequest( Hashtable parameters, Stream sourceStream, long length )
		{
			HttpWebRequest request = ( WebRequest.Create( this.MakeURL( parameters, false ) ) as HttpWebRequest );
			request.UserAgent = "Phanfare API v2";
			request.Timeout = DataTimeout;
			request.ContentType = "multipart/form-data";
			request.Method = "POST";
			request.AllowWriteStreamBuffering = true;
			request.AutomaticDecompression = DecompressionMethods.GZip;
			request.ContentLength = length;
			request.CookieContainer = new CookieContainer( 1 );
			request.CookieContainer.Add( this.GetCookie() );
			using( Stream requestStream = request.GetRequestStream() )
			{
				byte[] buffer = new byte[ BufferSize ];
				int read = 0;
				do
				{
					read = sourceStream.Read( buffer, 0, buffer.Length );
					if( read >= 0 )
						requestStream.Write( buffer, 0, read );
				} while( read > 0 );
			}

			string responseString;
			using( WebResponse response = request.GetResponse() )
			using( StreamReader reader = new StreamReader( response.GetResponseStream() ) )
				responseString = reader.ReadToEnd();

			return this.ParseResponse( responseString );
		}

		private string MakeURL( Hashtable parameters, bool secure )
		{
			StringBuilder url = new StringBuilder( 150 + ( parameters.Count * 50 ) );
			if( secure == true )
				url.Append( "https://" );
			else
				url.Append( "http://" );
			url.Append( BaseUrl );

			StringBuilder paramList = new StringBuilder( 50 + ( parameters.Count * 50 ) );
			paramList.AppendFormat( "api_key={0}", _apiKey );
			foreach( DictionaryEntry entry in parameters )
			{
				paramList.AppendFormat( "&{0}=", entry.Key );
				object value = entry.Value;
				if( value != null )
				{
					if( value is bool )
						paramList.Append( ( ( ( bool )value ) == true ) ? "1" : "0" );
					else if( value is DateTime )
						paramList.Append( ( ( DateTime )value ).ToString( "o" ) );
					else
						paramList.Append( value.ToString() );
				}
			}
			if( _asJson == true )
				paramList.Append( "&as_json=1" );
			string parameterString = paramList.ToString();

			string sig = GetStringMD5Hash( parameterString + _apiSecret );
			url.Append( Uri.EscapeUriString( parameterString ) );
			url.Append( "&sig=" + sig );
			return url.ToString();
		}

		private XmlDocument ParseResponse( string rawResponse )
		{
			XmlDocument doc = new XmlDocument();
			doc.Load( new StringReader( rawResponse ) );

			XmlNodeList rspCode = doc.GetElementsByTagName( "rsp" );

			if( rspCode.Count == 0 )
				throw new PhanfareException( "Invalid response from the Phanfare Service" );
			if( rspCode[ 0 ].Attributes[ "stat" ].Value == "fail" )
			{
				string errorCodeString = rspCode[ 0 ].Attributes[ "error_code" ].Value;
				int errorCode = -1;
				if( errorCodeString != string.Empty )
					errorCode = int.Parse( errorCodeString );

				string codeValue = rspCode[ 0 ].Attributes[ "code_value" ].Value;
				string message = rspCode[ 0 ].Attributes[ "msg" ].Value;

				throw new PhanfareException( errorCode, codeValue, message );
			}

			return doc;
		}

		private T ReadResponseItem<T>( XmlDocument doc, FromXMLDelegate<T> del )
		{
			XmlNode rootNode = doc.FirstChild.NextSibling.FirstChild;
			if( rootNode != null )
				return del( rootNode as XmlElement );
			else
				return default( T );
		}

		private T[] ReadResponseList<T>( XmlDocument doc, string listName, string counterName, FromXMLDelegate<T> del )
		{
			XmlElement rootNode = doc.FirstChild.NextSibling[ listName ] as XmlElement;
			if( rootNode == null )
				return new T[ 0 ];

			return rootNode.ReadList<T>( counterName, del );
		}

		[ThreadStatic]
		private static MD5 _md5;

		private static string GetStringMD5Hash( string input )
		{
			_md5 = _md5 ?? MD5.Create();
			byte[] hash = _md5.ComputeHash( Encoding.Default.GetBytes( input ) );
			return ToHexString( hash );
		}

		private readonly static string[] BATHS = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0A", "0B", "0C", "0D", "0E", "0F", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "1A", "1B", "1C", "1D", "1E", "1F", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2A", "2B", "2C", "2D", "2E", "2F", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "3A", "3B", "3C", "3D", "3E", "3F", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4A", "4B", "4C", "4D", "4E", "4F", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "5A", "5B", "5C", "5D", "5E", "5F", "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "6A", "6B", "6C", "6D", "6E", "6F", "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "7A", "7B", "7C", "7D", "7E", "7F", "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "8A", "8B", "8C", "8D", "8E", "8F", "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "9A", "9B", "9C", "9D", "9E", "9F", "A0", "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "AA", "AB", "AC", "AD", "AE", "AF", "B0", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "BA", "BB", "BC", "BD", "BE", "BF", "C0", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "CA", "CB", "CC", "CD", "CE", "CF", "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "DA", "DB", "DC", "DD", "DE", "DF", "E0", "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "EA", "EB", "EC", "ED", "EE", "EF", "F0", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "FA", "FB", "FC", "FD", "FE", "FF" };
		public static string ToHexString( byte[] value )
		{
			int length = ( value.Length << 1 ) + 1;
			StringBuilder sb = new StringBuilder( length, length );
			foreach( byte b in value )
				sb.Append( BATHS[ b ] );
			return sb.ToString();
		}
	}
}
