using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Phanfare.MediaServer.Utilities
{
	public static class Security
	{
		private readonly static byte[] _iv = { 8, 7, 6, 5, 4, 3, 2, 1 };
		private readonly static byte[] _key;

		private readonly static TripleDESCryptoServiceProvider _des = new TripleDESCryptoServiceProvider();

		static Security()
		{
			_key = new byte[ 16 ];
			Array.Copy( Encoding.UTF8.GetBytes( "ASdvakl2lvv012Msadasdkmn4_adsal" ), _key, 16 );
		}

		public static string Encrypt( string input )
		{
			return Convert.ToBase64String( Encrypt( Encoding.UTF8.GetBytes( input ) ) );
		}

		public static byte[] Encrypt( byte[] input )
		{
			using( ICryptoTransform cryptoTransform = _des.CreateEncryptor( _key, _iv ) )
				return cryptoTransform.TransformFinalBlock( input, 0, input.Length );
		}

		public static string Decrypt( string input )
		{
			return Encoding.UTF8.GetString( Decrypt( Convert.FromBase64String( input ) ) );
		}

		public static byte[] Decrypt( byte[] input )
		{
			using( ICryptoTransform cryptoTransform = _des.CreateDecryptor( _key, _iv ) )
				return cryptoTransform.TransformFinalBlock( input, 0, input.Length );
		}
	}
}
