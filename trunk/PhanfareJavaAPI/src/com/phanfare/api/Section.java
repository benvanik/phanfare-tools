package com.phanfare.api;

import org.json.me.JSONArray;
import org.json.me.JSONException;
import org.json.me.JSONObject;

public class Section {
	public long albumId;
	public long sectionId;
	public String name;
	public String description;
	public int imageCount;
	public ImageInfo[] images;

	public static Section fromJson(JSONObject object) {
		Section section = new Section();
		try {
			section.albumId = object.getLong("album_id");
			section.sectionId = object.getLong("section_id");
			section.name = object.getString("section_name");
			section.description = object.getString("section_descr");
			section.imageCount = object.getInt("num_images");
			JSONArray images = object.optJSONArray("image_infos");
			if (images != null) {
				int length = images.length();
				if (length == section.imageCount) {
					section.images = new ImageInfo[length];
					for (int n = 0; n < section.images.length; n++) {
						section.images[n] = ImageInfo.fromJson(images.getJSONObject(n));
					}
				}
			}
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
		return section;
	}
}
