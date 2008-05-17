package com.phanfare.util;

import java.util.Date;

public class ParameterList {
	private StringBuffer _sb;

	public ParameterList(String apiKey, String methodName) {
		_sb = new StringBuffer("method=");
		_sb.append(methodName);
		_sb.append("&api_key=");
		_sb.append(apiKey);
		_sb.append("&as_json=1");
	}

	public void put(String name, Object value) {
		String valueString;
		if (value != null) {
			if (value.getClass() == Boolean.class) {
				valueString = ((Boolean) value).booleanValue() ? "1" : "0";
			} else if (value.getClass() == Date.class) {
				valueString = ((Date) value).toString(); // TODO
			} else {
				valueString = value.toString();
			}
		} else {
			valueString = "";
		}
		_sb.append('&');
		_sb.append(name);
		_sb.append('=');
		_sb.append(valueString);
	}

	public String toString() {
		return _sb.toString();
	}
}
