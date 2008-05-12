using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Phanfare.ExternalAPI
{
	internal delegate T FromXMLDelegate<T>( XmlElement element );

	internal static class Extensions
	{
		public static long[] ExtractIDs( this string listing )
		{
			string[] idsRaw = listing.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );
			long[] result = new long[ idsRaw.Length ];
			for( int n = 0; n < idsRaw.Length; n++ )
				result[ n ] = long.Parse( idsRaw[ n ] );
			return result;
		}

		public static string ToOrderedString<T>( this T[] list )
		{
			if( list.Length == 0 )
				return string.Empty;
			else if( list.Length == 1 )
				return list[ 0 ].ToString();
			else
			{
				StringBuilder sb = new StringBuilder( list.Length * 8 );
				bool first = true;
				foreach( T item in list )
				{
					if( first == false )
						sb.Append( ',' );
					else
						first = false;
					sb.Append( item );
				}
				return sb.ToString();
			}
		}

		public static T GetAttributeEnum<T>( this XmlElement element, string attributeName )
		{
			string value = element.GetAttribute( attributeName );
			return ( T )Enum.Parse( typeof( T ), value, true );
		}

		public static DateTime GetAttributeDate( this XmlElement element, string attributeName )
		{
			string value = element.GetAttribute( attributeName );
			return DateTime.Parse( value );
		}

		public static int GetAttributeInt32( this XmlElement element, string attributeName )
		{
			string value = element.GetAttribute( attributeName );
			return int.Parse( value );
		}

		public static long GetAttributeInt64( this XmlElement element, string attributeName )
		{
			string value = element.GetAttribute( attributeName );
			return long.Parse( value );
		}

		public static bool GetAttributeBoolean( this XmlElement element, string attributeName )
		{
			string value = element.GetAttribute( attributeName );
			return ( ( value == "1" ) || ( string.Compare( value, "true", true ) == 0 ) );
		}

		public static T[] ReadList<T>( this XmlElement element, string counterName, FromXMLDelegate<T> del )
		{
			int count = element.GetAttributeInt32( counterName );
			T[] result = new T[ count ];
			int n = 0;
			foreach( XmlNode node in element.ChildNodes )
				result[ n++ ] = del( node as XmlElement );
			return result;
		}

		public static T[] ReadList<T>( this XmlElement element, string counterName, FromXMLDelegate<T> del, out int actualCount )
		{
			actualCount = element.GetAttributeInt32( counterName );
			if( element.ChildNodes.Count != actualCount )
				return null;
			T[] result = new T[ actualCount ];
			int n = 0;
			foreach( XmlNode node in element.ChildNodes )
				result[ n++ ] = del( node as XmlElement );
			return result;
		}
	}
}
