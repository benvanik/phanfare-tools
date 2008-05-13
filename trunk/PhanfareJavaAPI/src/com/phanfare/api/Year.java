package com.phanfare.api;

import org.json.me.JSONArray;
import org.json.me.JSONException;
import org.json.me.JSONObject;

public class Year {
	public int yearOrdinal;
	public long[] albumIds;

	public boolean isTimeless() {
		return this.yearOrdinal == 9999;
	}

	public static Year fromJson(JSONObject object) {
		Year year = new Year();
		try {
			year.yearOrdinal = object.getInt("year");
			JSONArray albumIds = object.getJSONArray("album_ids");
			year.albumIds = new long[albumIds.length()];
			for (int n = 0; n < year.albumIds.length; n++) {
				year.albumIds[n] = Long.parseLong(albumIds.getString(n));
			}
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
		return year;
	}
}
