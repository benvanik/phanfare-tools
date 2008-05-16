package com.phanfare.api;

import java.util.Date;

import org.json.me.JSONArray;
import org.json.me.JSONException;
import org.json.me.JSONObject;

public class NewsItem {
	public final static int NEWS_TYPE_NEW_ALBUM = 0;
	public final static int NEWS_TYPE_NEW_MEDIA = 1;

	public int type;
	public Date timestamp;
	public long sourceId;
	public String sourceName;
	public String siteName;
	public long albumId;
	public String albumName;
	public String albumUrl;
	public String albumSlideshowUrl;

	public ImageInfo[] leadImages;

	public static NewsItem fromJson(JSONObject object) {
		NewsItem item = new NewsItem();
		try {
			String type = object.getString("type").toLowerCase();
			if (type.equals("newalbum") == true)
				item.type = NewsItem.NEWS_TYPE_NEW_ALBUM;
			else
				item.type = NewsItem.NEWS_TYPE_NEW_MEDIA;
			item.timestamp = Utilities.convertJsonDate(object.getString("timestamp"));
			item.sourceId = object.getLong("source_uid");
			item.sourceName = object.getString("source_name");
			item.siteName = object.getString("site_name");
			item.albumId = object.getLong("album_id");
			item.albumName = object.getString("album_name");
			item.albumUrl = object.getString("album_url");
			item.albumSlideshowUrl = object.getString("album_slideshow_url");
			JSONArray images = object.getJSONArray("images");
			item.leadImages = new ImageInfo[images.length()];
			for (int n = 0; n < item.leadImages.length; n++) {
				item.leadImages[n] = ImageInfo.fromJson(images.getJSONObject(n));
			}

		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
		return item;
	}
}
