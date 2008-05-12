using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Phanfare.ExternalAPI
{
	public class Year
	{
		public int YearOrdinal { get; set; }
		public int ChildCount { get; set; }
		public long[] AlbumIDs { get; set; }

		public bool IsTimeless { get { return YearOrdinal == 9999; } }

		public static Year FromXML( XmlElement el )
		{
			Year year = new Year();
			year.YearOrdinal = el.GetAttributeInt32( "year" );
			year.ChildCount = el.GetAttributeInt32( "child_count" );
			year.AlbumIDs = el.GetAttribute( "album_ids" ).ExtractIDs();
			return year;
		}
	}
}
