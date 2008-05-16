package com.phanfare.api;

import org.json.me.JSONException;
import org.json.me.JSONObject;

public class GroupInfo {
	public long groupId;
	public String name;
	public String description;
	public long ownerId;

	public static GroupInfo fromJson(JSONObject object) {
		GroupInfo group = new GroupInfo();
		try {
			group.groupId = object.getLong("group_id");
			group.name = object.getString("name");
			group.description = object.getString("descr");
			group.ownerId = object.getLong("owner_uid");
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
		return group;
	}
}
