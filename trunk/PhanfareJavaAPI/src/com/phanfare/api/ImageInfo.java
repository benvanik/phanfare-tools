package com.phanfare.api;

import java.util.Date;

import org.json.me.JSONArray;
import org.json.me.JSONException;
import org.json.me.JSONObject;

public class ImageInfo {
	public long albumId;
	public long sectionId;
	public long imageId;
	public String fileName;
	public String caption;
	public Date imageDate;
	public boolean isVideo;
	public boolean isHidden;
	public ImageRendition[] renditions;

	public static ImageInfo fromJson(JSONObject object) {
		ImageInfo image = new ImageInfo();
		try {
			image.albumId = object.getLong("album_id");
			image.sectionId = object.getLong("section_id");
			image.imageId = object.getLong("image_id");
			image.fileName = object.getString("filename");
			image.caption = object.getString("caption");
			image.imageDate = Utilities.convertJsonDate(object.getString("image_date"));
			image.isVideo = object.getBoolean("is_video");
			image.isHidden = object.getBoolean("hidden");
			JSONArray renditions = object.getJSONArray("renditions");
			image.renditions = new ImageRendition[renditions.length()];
			for (int n = 0; n < image.renditions.length; n++) {
				image.renditions[n] = ImageRendition.fromJson(renditions.getJSONObject(n));
			}
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
		return image;
	}
}
