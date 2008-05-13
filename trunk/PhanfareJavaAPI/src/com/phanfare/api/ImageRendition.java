package com.phanfare.api;

import java.util.Date;

import org.json.me.JSONException;
import org.json.me.JSONObject;

public class ImageRendition {
	public String type;
	public int width;
	public int height;
	public long fileSize;
	public Date createdDate;
	public String mediaType;
	public int quality;
	public String url;

	public static ImageRendition fromJson(JSONObject object) {
		ImageRendition rendition = new ImageRendition();
		try {
			rendition.type = object.getString("rendition_type");
			rendition.width = object.getInt("width");
			rendition.height = object.getInt("height");
			rendition.fileSize = object.getLong("filesize");
			rendition.createdDate = Utilities.convertJsonDate(object.getString("created_date"));
			rendition.mediaType = object.getString("media_type");
			rendition.quality = object.getInt("quality");
			rendition.url = object.getString("url");
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
		return rendition;
	}
}
