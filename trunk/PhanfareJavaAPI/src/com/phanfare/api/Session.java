package com.phanfare.api;

import org.json.me.JSONException;
import org.json.me.JSONObject;

public class Session {
	public String sessionCookie;
	public long userId;
	public boolean isPremium;
	public long friendGroupId;
	public long familyGroupId;
	public String websiteTitle;
	public String timelessHeader;
	public boolean isTimelessFirst;
	public PublicProfile profile;

	public static Session fromJson(JSONObject object) {
		Session session = new Session();
		try {
			session.sessionCookie = object.getString("cookie");
			session.userId = object.getLong("uid");
			session.isPremium = object.getBoolean("premium");
			session.friendGroupId = object.getLong("friend_group_id");
			session.familyGroupId = object.getLong("family_group_id");
			session.websiteTitle = object.getString("website_title");
			session.timelessHeader = object.getString("timeless_header");
			session.isTimelessFirst = object.getBoolean("timeless_first");
			session.profile = PublicProfile.fromJson(object.getJSONObject("profile"));
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
		return session;
	}
}
