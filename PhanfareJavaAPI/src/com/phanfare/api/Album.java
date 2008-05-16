package com.phanfare.api;

import java.util.Date;

import org.json.me.JSONArray;
import org.json.me.JSONException;
import org.json.me.JSONObject;

public class Album {
	public final static int ALBUM_TYPE_DATED = 0;
	public final static int ALBUM_TYPE_TIMELESS = 1;

	public long albumId;
	public int type;
	public String name;
	public String description;
	public Date startDate;
	public Date endDate;
	public Date creationDate;
	public Date lastModified;
	public Boolean autoDate;
	public String url;
	public long ownerId;

	public ImageInfo leadImage;
	public Section[] sections;
	public Group[] groups;

	public static Album fromJson(JSONObject object) {
		Album album = new Album();
		try {
			album.albumId = object.getLong("album_id");
			String type = object.getString("album_type").toLowerCase();
			if (type.equals("dated") == true)
				album.type = Album.ALBUM_TYPE_DATED;
			else
				album.type = Album.ALBUM_TYPE_TIMELESS;
			album.name = object.getString("album_name");
			album.description = object.getString("album_descr");
			album.startDate = Utilities.convertJsonDate(object.getString("album_start_date"));
			album.endDate = Utilities.convertJsonDate(object.getString("album_end_date"));
			album.creationDate = Utilities.convertJsonDate(object.getString("album_creation_date"));
			album.lastModified = Utilities.convertJsonDate(object.getString("album_last_modified"));
			album.autoDate = new Boolean( object.getBoolean("auto_date") );
			album.url = object.getString("album_url");
			album.ownerId = object.getLong("owner_uid");

			JSONObject leadImage = object.optJSONObject("lead_image");
			if (leadImage != null)
				album.leadImage = ImageInfo.fromJson(leadImage);

			JSONArray sections = object.getJSONArray("sections");
			album.sections = new Section[sections.length()];
			for (int n = 0; n < album.sections.length; n++) {
				album.sections[n] = Section.fromJson(sections.getJSONObject(n));
			}

			JSONArray groups = object.getJSONArray("groups");
			album.groups = new Group[groups.length()];
			for (int n = 0; n < album.groups.length; n++) {
				album.groups[n] = Group.fromJson(groups.getJSONObject(n));
			}
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
		return album;
	}

	public boolean isPrivate() {
		return this.groups.length == 0;
	}

	public boolean isSharedWith(long groupId) {
		for (int n = 0; n < this.groups.length; n++) {
			if (this.groups[n].groupId == groupId) {
				return true;
			}
		}
		return false;
	}
}
