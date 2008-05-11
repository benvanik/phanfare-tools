using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Phanfare.MediaServer
{
	static class Didl
	{
		public static string Begin()
		{
			return "<DIDL-Lite xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:upnp=\"urn:schemas-upnp-org:metadata-1-0/upnp/\" xmlns=\"urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/\">";
		}

		public static string End()
		{
			return "</DIDL-Lite>";
		}

		public static string GetContainer( string id, string parentId, bool restricted, bool searchable, string title, string upnpClass, int childCount )
		{
			return string.Format(
				"<container id=\"{0}\" restricted=\"{2}\" parentID=\"{1}\" searchable=\"{3}\" {4}>" +
				"<dc:title>{5}</dc:title><upnp:class>{6}</upnp:class></container>",
				id, parentId,
				( restricted == true ) ? "true" : "false",
				( searchable == true ) ? "true" : "false",
				( childCount > 0 ) ? "childCount=\"" + childCount + "\"" : "",
				HttpUtility.HtmlEncode( title ), upnpClass );
		}
	}
}
