package com.phanfare.api;

import org.json.me.JSONException;
import org.json.me.JSONObject;

public class Group {
	public long groupId;

	public static Group fromJson(JSONObject object) {
		Group group = new Group();
		try {
			group.groupId = object.getLong("group_id");
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
		return group;
	}
}
