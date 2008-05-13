package com.phanfare.api;

import org.json.me.JSONException;
import org.json.me.JSONObject;

public class PublicProfile {
	public final static int RELATION_OTHER = 0;
	public final static int RELATION_SELF = 1;
	public final static int RELATION_FRIEND = 2;
	public final static int RELATION_FAMILY = 3;

	public long userId;
	public String firstName;
	public String lastName;
	public int relation;
	public boolean isPremium;
	public String imageUrl;

	public static PublicProfile fromJson(JSONObject object) {
		PublicProfile profile = new PublicProfile();
		try {
			profile.userId = object.getLong("uid");
			profile.firstName = object.getString("first_name");
			profile.lastName = object.getString("last_name");
			String relation = object.getString("relation").toLowerCase();
			if (relation.equals("self") == true)
				profile.relation = PublicProfile.RELATION_SELF;
			else if (relation.equals("friend") == true)
				profile.relation = PublicProfile.RELATION_FRIEND;
			else if (relation.equals("family") == true)
				profile.relation = PublicProfile.RELATION_FAMILY;
			else
				profile.relation = PublicProfile.RELATION_OTHER;
			profile.isPremium = object.getBoolean("premium");
			profile.imageUrl = object.getString("image_url");
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
		return profile;
	}
}
