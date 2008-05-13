package com.phanfare.api;

import java.util.Date;

class Utilities {
	public static String joinIds(long[] list) {
		if (list.length == 0)
			return "";
		else if (list.length == 1)
			return Long.toString(list[0]);
		else {
			StringBuilder sb = new StringBuilder(list.length * 8);
			for (int n = 0; n < list.length; n++) {
				if (n > 0)
					sb.append(',');
				sb.append(list[n]);
			}
			return sb.toString();
		}
	}

	public static Date convertJsonDate(String source) {
		// /Date(1204588800000)/
		if (source == null) {
			return null;
		} else {
			source = source.substring(6, source.length() - 2);
			long ms = Long.parseLong(source);
			return new Date(ms);
		}
	}

	public static String urlEncode(String s) {
		if (s == null)
			return s;
		StringBuffer sb = new StringBuffer(s.length() * 3);
		try {
			char c;
			for (int i = 0; i < s.length(); i++) {
				c = s.charAt(i);
				if (c == '&') {
					sb.append("&amp;");
				} else if (c == ' ') {
					sb.append('+');
				} else if ((c >= ',' && c <= ';') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_'
						|| c == '?') {
					sb.append(c);
				} else {
					sb.append('%');
					if (c > 15) {
						// is it a non-control char, >x0F so 2 chars
						sb.append(Integer.toHexString((int) c));
						// just add % and the string
					} else {
						sb.append("0" + Integer.toHexString((int) c));
						// otherwise need to add a leading 0
					}
				}
			}

		} catch (Exception ex) {
			return (null);
		}
		return sb.toString();
	}
}
