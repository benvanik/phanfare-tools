package com.phanfare.api;

import java.io.ByteArrayOutputStream;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.Date;

import org.json.me.JSONArray;
import org.json.me.JSONException;
import org.json.me.JSONObject;

import com.phanfare.api.platform.PlatformUtilities;
import com.phanfare.util.MD5;
import com.phanfare.util.ParameterList;

public class PhanfareService {
	private final static String BASE_URL = "www.phanfare.com/api/?";

	private String _apiKey;
	private String _apiSecret;
	private boolean _useHttps;
	private PlatformUtilities _platform;

	public String sessionCookie;

	public PhanfareService(String apiKey, String apiSecret, PlatformUtilities platform) throws PhanfareException {
		this(apiKey, apiSecret, platform, false);
	}

	public PhanfareService(String apiKey, String apiSecret, PlatformUtilities platform, boolean useHttps)
			throws PhanfareException {
		this.assertParameterNotNullOrEmpty("apiKey", apiKey);
		this.assertParameterNotNullOrEmpty("apiSecret", apiSecret);
		this.assertParameterNotNull("platform", platform);
		_apiKey = apiKey;
		_apiSecret = apiSecret;
		_platform = platform;
		_useHttps = useHttps;
		this.sessionCookie = "";
	}

	public Session authenticate(String emailAddress, String password) throws PhanfareException {
		this.assertParameterNotNullOrEmpty("emailAddress", emailAddress);
		this.assertParameterNotNullOrEmpty("password", password);
		ParameterList ht = this.methodCall("authenticate");
		ht.put("email_address", emailAddress);
		ht.put("password", password);
		String responseString = this.makeRequest(ht, true);
		if (responseString == null)
			return null;
		try {
			JSONObject object = this.parseResponse(responseString);
			Session session = Session.fromJson(object);
			if (session != null)
				this.sessionCookie = session.sessionCookie;
			return session;
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	public boolean newAccount(String emailAddress, String firstName, String lastName, String promotionCode)
			throws PhanfareException {
		this.assertParameterNotNullOrEmpty("emailAddress", emailAddress);
		this.assertParameterNotNullOrEmpty("firstName", firstName);
		this.assertParameterNotNullOrEmpty("lastName", lastName);
		ParameterList ht = this.methodCall("authenticate");
		ht.put("email_address", emailAddress);
		ht.put("first_name", firstName);
		ht.put("last_name", lastName);
		ht.put("promotion_code", promotionCode);
		String responseString = this.makeRequest(ht);
		if (responseString == null)
			return false;
		try {
			this.parseResponse(responseString);
			return true;
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return false;
		}
	}

	public NewsItem[] getNewsFeed() throws PhanfareException {
		return this.getNewsFeed(null, null);
	}

	public NewsItem[] getNewsFeed(int maximumItems) throws PhanfareException {
		return this.getNewsFeed(new Integer(maximumItems), null);
	}

	public NewsItem[] getNewsFeed(Date showSince) throws PhanfareException {
		return this.getNewsFeed(null, showSince);
	}

	public NewsItem[] getNewsFeed(Integer maximumItems, Date showSince) throws PhanfareException {
		this.assertSessionValid();
		ParameterList ht = this.methodCall("getnewsfeed");
		if (maximumItems != null)
			ht.put("maximum_item_count", maximumItems);
		if (showSince != null)
			ht.put("show_since", showSince);
		String responseString = this.makeRequest(ht);
		if (responseString == null)
			return null;
		try {
			JSONObject object = this.parseResponse(responseString);
			JSONArray array = object.getJSONArray("newsfeed");
			NewsItem[] newsItems = new NewsItem[array.length()];
			for (int n = 0; n < newsItems.length; n++) {
				newsItems[n] = NewsItem.fromJson(array.getJSONObject(n));
			}
			return newsItems;
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	public GroupInfo[] getMyGroups() throws PhanfareException {
		this.assertSessionValid();
		ParameterList ht = this.methodCall("getmygroups");
		String responseString = this.makeRequest(ht);
		if (responseString == null)
			return null;
		try {
			JSONObject object = this.parseResponse(responseString);
			JSONArray array = object.getJSONArray("groupinfos");
			GroupInfo[] groupInfos = new GroupInfo[array.length()];
			for (int n = 0; n < groupInfos.length; n++) {
				groupInfos[n] = GroupInfo.fromJson(array.getJSONObject(n));
			}
			return groupInfos;
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	public PublicProfile[] getMyRelationships() throws PhanfareException {
		return this.getMyRelationships(false);
	}

	public PublicProfile[] getMyRelationships(boolean externalLinks) throws PhanfareException {
		this.assertSessionValid();
		return this.getRelationships(false, 0, externalLinks);
	}

	public PublicProfile[] getGroupRelationships(long groupId) throws PhanfareException {
		return this.getGroupRelationships(groupId, false);
	}

	public PublicProfile[] getGroupRelationships(long groupId, boolean externalLinks) throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("groupId", groupId);
		return this.getRelationships(true, groupId, externalLinks);
	}

	private PublicProfile[] getRelationships(boolean forGroup, long groupId, boolean externalLinks)
			throws PhanfareException {
		this.assertSessionValid();

		ParameterList ht;
		if (forGroup == true) {
			ht = methodCall("getgrouprelations");
			ht.put("group_id", new Long(groupId));
		} else {
			ht = methodCall("getmyrelations");
		}
		if (externalLinks == true)
			ht.put("external_links", new Boolean(externalLinks));

		String responseString = this.makeRequest(ht);
		if (responseString == null)
			return null;
		try {
			JSONObject object = this.parseResponse(responseString);
			JSONArray array = object.getJSONArray("profiles");
			PublicProfile[] profiles = new PublicProfile[array.length()];
			for (int n = 0; n < profiles.length; n++) {
				profiles[n] = PublicProfile.fromJson(array.getJSONObject(n));
			}
			return profiles;
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	public Year[] getYearList(long userId) throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		return this.getYearList("target_uid", userId);
	}

	public Year[] getGroupYearList(long groupId) throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("groupId", groupId);
		return this.getYearList("group_id", groupId);
	}

	private Year[] getYearList(String parameterName, long value) throws PhanfareException {
		ParameterList ht = this.methodCall("getyearlist");
		ht.put(parameterName, new Long(value));
		String responseString = this.makeRequest(ht);
		if (responseString == null)
			return null;
		try {
			JSONObject object = this.parseResponse(responseString);
			JSONArray array = object.getJSONArray("years");
			Year[] years = new Year[array.length()];
			for (int n = 0; n < years.length; n++) {
				years[n] = Year.fromJson(array.getJSONObject(n));
			}
			return years;
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	public Album[] getAlbumList(long userId) throws PhanfareException {
		return this.getAlbumList(userId, false, null, null, null);
	}

	public Album[] getAlbumList(long userId, boolean externalLinks) throws PhanfareException {
		return this.getAlbumList(userId, externalLinks, null, null, null);
	}

	public Album[] getAlbumList(long userId, boolean externalLinks, Date modifiedAfter) throws PhanfareException {
		return this.getAlbumList(userId, externalLinks, modifiedAfter, null, null);
	}

	public Album[] getAlbumList(long userId, boolean externalLinks, int year) throws PhanfareException {
		return this.getAlbumList(userId, externalLinks, null, new Integer(year), null);
	}

	public Album[] getAlbumList(long userId, boolean externalLinks, long[] albumIds) throws PhanfareException {
		return this.getAlbumList(userId, externalLinks, null, null, albumIds);
	}

	public Album[] getAlbumList(long userId, boolean externalLinks, Date modifiedAfter, int year)
			throws PhanfareException {
		return this.getAlbumList(userId, externalLinks, modifiedAfter, new Integer(year), null);
	}

	public Album[] getAlbumList(long userId, boolean externalLinks, Date modifiedAfter, Integer year, long[] albumIds)
			throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);

		return this.getAlbumList("target_uid", userId, externalLinks, modifiedAfter, year, albumIds);
	}

	public Album[] getGroupAlbumList(long groupId) throws PhanfareException {
		return this.getGroupAlbumList(groupId, false, null, null, null);
	}

	public Album[] getGroupAlbumList(long groupId, boolean externalLinks) throws PhanfareException {
		return this.getGroupAlbumList(groupId, externalLinks, null, null, null);
	}

	public Album[] getGroupAlbumList(long groupId, boolean externalLinks, Date modifiedAfter) throws PhanfareException {
		return this.getGroupAlbumList(groupId, externalLinks, modifiedAfter, null, null);
	}

	public Album[] getGroupAlbumList(long groupId, boolean externalLinks, int year) throws PhanfareException {
		return this.getGroupAlbumList(groupId, externalLinks, null, new Integer(year), null);
	}

	public Album[] getGroupAlbumList(long groupId, boolean externalLinks, long[] albumIds) throws PhanfareException {
		return this.getGroupAlbumList(groupId, externalLinks, null, null, albumIds);
	}

	public Album[] getGroupAlbumList(long groupId, boolean externalLinks, Date modifiedAfter, int year)
			throws PhanfareException {
		return this.getGroupAlbumList(groupId, externalLinks, modifiedAfter, new Integer(year), null);
	}

	public Album[] getGroupAlbumList(long groupId, boolean externalLinks, Date modifiedAfter, Integer year,
			long[] albumIds) throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("groupId", groupId);

		return this.getAlbumList("group_id", groupId, externalLinks, modifiedAfter, year, albumIds);
	}

	private Album[] getAlbumList(String parameterName, long value, boolean externalLinks, Date modifiedAfter,
			Integer year, long[] albumIds) throws PhanfareException {
		if ((albumIds != null) && (albumIds.length == 0))
			return new Album[0];

		ParameterList ht = this.methodCall("getalbumlist");
		ht.put(parameterName, new Long(value));
		if (modifiedAfter != null)
			ht.put("modified_after", modifiedAfter);
		if (year != null)
			ht.put("year", year);
		if (externalLinks == true)
			ht.put("external_links", new Boolean(externalLinks));
		if (albumIds != null)
			ht.put("album_ids", Utilities.joinIds(albumIds));
		String responseString = this.makeRequest(ht);
		if (responseString == null)
			return null;
		try {
			JSONObject object = this.parseResponse(responseString);
			JSONArray array = object.getJSONArray("albums");
			Album[] albums = new Album[array.length()];
			for (int n = 0; n < albums.length; n++) {
				albums[n] = Album.fromJson(array.getJSONObject(n));
			}
			return albums;
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	public Album getAlbum(long userId, long albumId) throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		this.assertParameterValidId("albumId", albumId);
		ParameterList ht = this.methodCall("getalbum");
		ht.put("target_uid", new Long(userId));
		ht.put("album_id", new Long(albumId));
		String responseString = this.makeRequest(ht);
		if (responseString == null)
			return null;
		try {
			JSONObject object = this.parseResponse(responseString);
			return Album.fromJson(object);
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	public Album getMobileAlbum(long userId, boolean useExisting) throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		ParameterList ht = this.methodCall("getalbum");
		ht.put("target_uid", new Long(userId));
		ht.put("album_id", new Long(1));
		ht.put("mobile", new Boolean(true));
		ht.put("use_existing_mobile", new Boolean(useExisting));
		String responseString = this.makeRequest(ht);
		if (responseString == null)
			return null;
		try {
			JSONObject object = this.parseResponse(responseString);
			return Album.fromJson(object);
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	public ImageInfo[] getAlbumImages(long userId, long albumId) throws PhanfareException {
		return this.getAlbumImages(userId, albumId, false);
	}

	public ImageInfo[] getAlbumImages(long userId, long albumId, boolean externalLinks) throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		this.assertParameterValidId("albumId", albumId);
		ParameterList ht = this.methodCall("getimages");
		ht.put("target_uid", new Long(userId));
		ht.put("album_id", new Long(albumId));
		ht.put("image_mode", "specificalbum");
		if (externalLinks == true)
			ht.put("external_links", new Boolean(externalLinks));
		String responseString = this.makeRequest(ht);
		if (responseString == null)
			return null;
		try {
			JSONObject object = this.parseResponse(responseString);
			JSONArray array = object.getJSONArray("images");
			ImageInfo[] images = new ImageInfo[array.length()];
			for (int n = 0; n < images.length; n++) {
				images[n] = ImageInfo.fromJson(array.getJSONObject(n));
			}
			return images;
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	public ImageInfo[] getSectionImages(long userId, long albumId, long sectionId) throws PhanfareException {
		return this.getSectionImages(userId, albumId, sectionId, false, null, null);
	}

	public ImageInfo[] getSectionImages(long userId, long albumId, long sectionId, boolean externalLinks)
			throws PhanfareException {
		return this.getSectionImages(userId, albumId, sectionId, externalLinks, null, null);
	}

	public ImageInfo[] getSectionImages(long userId, long albumId, long sectionId, boolean externalLinks,
			int startIndex, int itemCount) throws PhanfareException {
		return this.getSectionImages(userId, albumId, sectionId, externalLinks, new Integer(startIndex), new Integer(
				itemCount));
	}

	private ImageInfo[] getSectionImages(long userId, long albumId, long sectionId, boolean externalLinks,
			Integer startIndex, Integer itemCount) throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		this.assertParameterValidId("albumId", albumId);
		this.assertParameterValidId("sectionId", sectionId);
		ParameterList ht = this.methodCall("getsectionimages");
		ht.put("target_uid", new Long(userId));
		ht.put("album_id", new Long(albumId));
		ht.put("section_id", new Long(sectionId));
		if (startIndex != null)
			ht.put("start_index", startIndex);
		if (itemCount != null)
			ht.put("items_requested", itemCount);
		if (externalLinks == true)
			ht.put("external_links", new Boolean(externalLinks));
		String responseString = this.makeRequest(ht);
		if (responseString == null)
			return null;
		try {
			JSONObject object = this.parseResponse(responseString);
			JSONArray array = object.getJSONArray("images");
			ImageInfo[] images = new ImageInfo[array.length()];
			for (int n = 0; n < images.length; n++) {
				images[n] = ImageInfo.fromJson(array.getJSONObject(n));
			}
			return images;
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	public ImageInfo getImageInfo(long userId, long albumId, long sectionId, long imageId) throws PhanfareException {
		return this.getImageInfo(userId, albumId, sectionId, imageId, false);
	}

	public ImageInfo getImageInfo(long userId, long albumId, long sectionId, long imageId, boolean externalLinks)
			throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		this.assertParameterValidId("albumId", albumId);
		this.assertParameterValidId("sectionId", sectionId);
		this.assertParameterValidId("imageId", imageId);
		ParameterList ht = this.methodCall("getimageinfo");
		ht.put("target_uid", new Long(userId));
		ht.put("album_id", new Long(albumId));
		ht.put("section_id", new Long(sectionId));
		ht.put("image_id", new Long(imageId));
		ht.put("external_links", new Boolean(externalLinks));
		String responseString = this.makeRequest(ht);
		if (responseString == null)
			return null;
		try {
			JSONObject object = this.parseResponse(responseString);
			return ImageInfo.fromJson(object);
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	public final static int RANDOM_MODE_RANDOM = 0;
	public final static int RANDOM_MODE_NEWEST_ALBUM_CREATION_DATE = 1;
	public final static int RANDOM_MODE_NEWEST_ALBUM_START_DATE = 2;
	public final static int RANDOM_MODE_NEWEST_IMAGES = 3;

	public ImageInfo[] getRandomImages(long userId, int mode, int imageCount) throws PhanfareException {
		return this.getRandomImages(userId, mode, imageCount, false);
	}

	public ImageInfo[] getRandomImages(long userId, int mode, int imageCount, boolean externalLinks)
			throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		return this.getRandomImages("target_uid", userId, mode, imageCount, externalLinks);
	}

	public ImageInfo[] getGroupRandomImages(long groupId, int mode, int imageCount) throws PhanfareException {
		return this.getGroupRandomImages(groupId, mode, imageCount, false);
	}

	public ImageInfo[] getGroupRandomImages(long groupId, int mode, int imageCount, boolean externalLinks)
			throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("groupId", groupId);
		return this.getRandomImages("group_id", groupId, mode, imageCount, externalLinks);
	}

	private ImageInfo[] getRandomImages(String parameterName, long value, int mode, int imageCount,
			boolean externalLinks) throws PhanfareException {
		ParameterList ht = this.methodCall("getimages");
		ht.put(parameterName, new Long(value));
		switch (mode) {
		default:
		case RANDOM_MODE_RANDOM:
			ht.put("image_mode", "Random");
			break;
		case RANDOM_MODE_NEWEST_ALBUM_CREATION_DATE:
			ht.put("image_mode", "NewestAlbumCreationDate");
			break;
		case RANDOM_MODE_NEWEST_ALBUM_START_DATE:
			ht.put("image_mode", "NewestAlbumStartDate");
			break;
		case RANDOM_MODE_NEWEST_IMAGES:
			ht.put("image_mode", "NewestImages");
			break;
		}
		ht.put("num_images", new Integer(imageCount));
		if (externalLinks == true)
			ht.put("external_links", new Boolean(externalLinks));
		String responseString = this.makeRequest(ht);
		if (responseString == null)
			return null;
		try {
			JSONObject object = this.parseResponse(responseString);
			JSONArray array = object.getJSONArray("images");
			ImageInfo[] images = new ImageInfo[array.length()];
			for (int n = 0; n < images.length; n++) {
				images[n] = ImageInfo.fromJson(array.getJSONObject(n));
			}
			return images;
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	public Album newAlbum(long userId, Album album) throws PhanfareException {
		return this.newAlbum(userId, album, new long[0]);
	}

	public Album newAlbum(long userId, Album album, long[] groups) throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		this.assertParameterNotNull("album", album);
		this.assertParameterNotNull("groups", groups);
		ParameterList ht = this.methodCall("newalbum");
		ht.put("target_uid", new Long(userId));
		if ((album.name != null) && (album.name.length() > 0))
			ht.put("album_name", album.name);
		if ((album.description != null) && (album.description.length() > 0))
			ht.put("album_description", album.description);
		if (album.startDate != null)
			ht.put("album_start_date", album.startDate);
		if (album.endDate != null)
			ht.put("album_end_date", album.endDate);
		if (album.autoDate != null)
			ht.put("auto_date", album.autoDate);
		switch (album.type) {
		default:
		case Album.ALBUM_TYPE_DATED:
			ht.put("type", "Dated");
			break;
		case Album.ALBUM_TYPE_TIMELESS:
			ht.put("type", "Timeless");
			break;
		}
		if (groups.length > 0)
			ht.put("groups", Utilities.joinIds(groups));
		String responseString = this.makeRequest(ht);
		if (responseString == null)
			return null;
		try {
			JSONObject object = this.parseResponse(responseString);
			return Album.fromJson(object);
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	public Section newSection(long userId, long albumId, Section section) throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		this.assertParameterValidId("albumId", albumId);
		this.assertParameterNotNull("section", section);
		ParameterList ht = this.methodCall("newsection");
		ht.put("target_uid", new Long(userId));
		ht.put("album_id", new Long(albumId));
		if ((section.name != null) && (section.name.length() > 0))
			ht.put("section_name", section.name);
		if ((section.description != null) && (section.description.length() > 0))
			ht.put("section_description", section.description);
		String responseString = this.makeRequest(ht);
		if (responseString == null)
			return null;
		try {
			JSONObject object = this.parseResponse(responseString);
			return Section.fromJson(object);
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	public void deleteAlbum(long userId, long albumId) throws PhanfareException {
		this.deleteAlbum(userId, albumId, false);
	}

	public void deleteAlbum(long userId, long albumId, boolean deleteForever) throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		this.assertParameterValidId("albumId", albumId);
		ParameterList ht = this.methodCall("deletealbum");
		ht.put("target_uid", new Long(userId));
		ht.put("album_id", new Long(albumId));
		ht.put("to_trash", new Boolean(!deleteForever));
		this.makeRequest(ht);
	}

	public void deleteSection(long userId, long albumId, long sectionId) throws PhanfareException {
		this.deleteSection(userId, albumId, sectionId, false);
	}

	public void deleteSection(long userId, long albumId, long sectionId, boolean deleteForever)
			throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		this.assertParameterValidId("albumId", albumId);
		this.assertParameterValidId("sectionId", sectionId);
		ParameterList ht = this.methodCall("deletesection");
		ht.put("target_uid", new Long(userId));
		ht.put("album_id", new Long(albumId));
		ht.put("section_id", new Long(sectionId));
		ht.put("to_trash", new Boolean(!deleteForever));
		this.makeRequest(ht);
	}

	public void deleteImage(long userId, long albumId, long sectionId, long imageId) throws PhanfareException {
		this.deleteImage(userId, albumId, sectionId, imageId, false);
	}

	public void deleteImage(long userId, long albumId, long sectionId, long imageId, boolean deleteForever)
			throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		this.assertParameterValidId("albumId", albumId);
		this.assertParameterValidId("sectionId", sectionId);
		this.assertParameterValidId("imageId", imageId);
		ParameterList ht = this.methodCall("deleteimage");
		ht.put("target_uid", new Long(userId));
		ht.put("album_id", new Long(albumId));
		ht.put("section_id", new Long(sectionId));
		ht.put("image_id", new Long(imageId));
		ht.put("to_trash", new Boolean(!deleteForever));
		this.makeRequest(ht);
	}

	public void updateAlbum(long userId, long albumId, String fieldToUpdate, Object fieldValue)
			throws PhanfareException {
		// album_name, album_description, album_start_date, album_end_date,
		// album_type, groups
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		this.assertParameterValidId("albumId", albumId);
		this.assertParameterNotNullOrEmpty("fieldToUpdate", fieldToUpdate);
		this.assertParameterNotNull("fieldValue", fieldValue);
		ParameterList ht = this.methodCall("updatealbum");
		ht.put("target_uid", new Long(userId));
		ht.put("album_id", new Long(albumId));
		ht.put("field_to_update", fieldToUpdate);
		ht.put("field_value", fieldValue);
		this.makeRequest(ht);
	}

	public void updateSection(long userId, long albumId, long sectionId, String fieldToUpdate, Object fieldValue)
			throws PhanfareException {
		// section_name, section_description
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		this.assertParameterValidId("albumId", albumId);
		this.assertParameterValidId("sectionId", sectionId);
		this.assertParameterNotNullOrEmpty("fieldToUpdate", fieldToUpdate);
		this.assertParameterNotNull("fieldValue", fieldValue);
		ParameterList ht = this.methodCall("updatesection");
		ht.put("target_uid", new Long(userId));
		ht.put("album_id", new Long(albumId));
		ht.put("section_id", new Long(sectionId));
		ht.put("field_to_update", fieldToUpdate);
		ht.put("field_value", fieldValue);
		this.makeRequest(ht);
	}

	public void updateImage(long userId, long albumId, long sectionId, long imageId, String fieldToUpdate,
			Object fieldValue) throws PhanfareException {
		// caption, image_date, hide
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		this.assertParameterValidId("albumId", albumId);
		this.assertParameterValidId("sectionId", sectionId);
		this.assertParameterValidId("imageId", imageId);
		this.assertParameterNotNullOrEmpty("fieldToUpdate", fieldToUpdate);
		this.assertParameterNotNull("fieldValue", fieldValue);
		ParameterList ht = this.methodCall("updateimage");
		ht.put("target_uid", new Long(userId));
		ht.put("album_id", new Long(albumId));
		ht.put("section_id", new Long(sectionId));
		ht.put("image_id", new Long(imageId));
		ht.put("field_to_update", fieldToUpdate);
		ht.put("field_value", fieldValue);
		this.makeRequest(ht);
	}

	public void updateImageCaption(long userId, long albumId, long sectionId, long imageId, String caption)
			throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		this.assertParameterValidId("albumId", albumId);
		this.assertParameterValidId("sectionId", sectionId);
		this.assertParameterValidId("imageId", imageId);
		this.assertParameterNotNull("caption", caption);
		ParameterList ht = this.methodCall("updatecaption");
		ht.put("target_uid", new Long(userId));
		ht.put("album_id", new Long(albumId));
		ht.put("section_id", new Long(sectionId));
		ht.put("image_id", new Long(imageId));
		ht.put("caption", caption);
		this.makeRequest(ht);
	}

	public void hideImage(long userId, long albumId, long sectionId, long imageId, boolean isHidden)
			throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		this.assertParameterValidId("albumId", albumId);
		this.assertParameterValidId("sectionId", sectionId);
		this.assertParameterValidId("imageId", imageId);
		ParameterList ht = this.methodCall("hideimage");
		ht.put("target_uid", new Long(userId));
		ht.put("album_id", new Long(albumId));
		ht.put("section_id", new Long(sectionId));
		ht.put("image_id", new Long(imageId));
		ht.put("hidden", new Boolean(isHidden));
		this.makeRequest(ht);
	}

	public ImageInfo newImage(long userId, long albumId, long sectionId, ImageInfo image, InputStream sourceStream,
			long length) throws PhanfareException {
		return this.newImage(userId, albumId, sectionId, image, sourceStream, length, false, false);
	}

	public ImageInfo newImage(long userId, long albumId, long sectionId, ImageInfo image, InputStream sourceStream,
			long length, boolean externalLinks) throws PhanfareException {
		return this.newImage(userId, albumId, sectionId, image, sourceStream, length, externalLinks, false);
	}

	public ImageInfo newImage(long userId, long albumId, long sectionId, ImageInfo image, InputStream sourceStream,
			long length, boolean externalLinks, boolean setAsProfilePicture) throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterValidId("userId", userId);
		this.assertParameterValidId("albumId", albumId);
		this.assertParameterValidId("sectionId", sectionId);
		this.assertParameterNotNull("image", image);
		this.assertParameterNotNullOrEmpty("image.fileName", image.fileName);
		this.assertParameterNotNull("sourceStream", sourceStream);

		ParameterList ht = this.methodCall("newimage");
		ht.put("target_uid", new Long(userId));
		ht.put("album_id", new Long(albumId));
		ht.put("section_id", new Long(sectionId));
		ht.put("filename", image.fileName);
		if (image.caption != null)
			ht.put("caption", image.caption);
		if (image.imageDate != null)
			ht.put("image_date", image.imageDate);
		ht.put("is_video", new Boolean(image.isVideo));
		ht.put("hidden", new Boolean(image.isHidden));
		ht.put("set_as_profile", new Boolean(setAsProfilePicture));
		if (externalLinks == true)
			ht.put("external_links", new Boolean(externalLinks));
		String responseString = this.makeRequest(ht, sourceStream, length);
		if (responseString == null)
			return null;
		try {
			JSONObject object = this.parseResponse(responseString);
			return ImageInfo.fromJson(object);
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	public byte[] getImageData(ImageRendition rendition) throws PhanfareException {
		ByteArrayOutputStream outputStream = new ByteArrayOutputStream((int) rendition.fileSize);
		if (this.getImageData(rendition, outputStream) < 0)
			return null;
		else
			return outputStream.toByteArray();
	}

	public long getImageData(ImageRendition rendition, OutputStream outputStream) throws PhanfareException {
		this.assertSessionValid();
		this.assertParameterNotNull("rendition", rendition);
		this.assertParameterNotNull("outputStream", outputStream);
		return _platform.download(this.sessionCookie, rendition.url, outputStream);
	}

	private void assertSessionValid() throws PhanfareException {
		if ((this.sessionCookie == null) || (this.sessionCookie.length() == 0))
			throw new PhanfareException("Session is not valid");
	}

	private void assertParameterNotNull(String name, Object value) throws PhanfareException {
		if (value == null)
			throw new PhanfareException("Parameter " + name + " must not be null");
	}

	private void assertParameterNotNullOrEmpty(String name, String value) throws PhanfareException {
		if ((value == null) || (value.length() == 0))
			throw new PhanfareException("Parameter " + name + " must not be null or empty");
	}

	private void assertParameterValidId(String name, long id) throws PhanfareException {
		if (id <= 0)
			throw new PhanfareException("Parameter " + name + " must be a valid ID");
	}

	private ParameterList methodCall(String methodName) {
		return new ParameterList(_apiKey, methodName);
	}

	private String makeRequest(ParameterList parameters) throws PhanfareException {
		return this.makeRequest(parameters, _useHttps, null, 0);
	}

	private String makeRequest(ParameterList parameters, boolean secure) throws PhanfareException {
		return this.makeRequest(parameters, secure, null, 0);
	}

	private String makeRequest(ParameterList parameters, InputStream sourceStream, long length)
			throws PhanfareException {
		return this.makeRequest(parameters, _useHttps, sourceStream, length);
	}

	private String makeRequest(ParameterList parameters, boolean secure, InputStream sourceStream, long length)
			throws PhanfareException {
		String url = makeUrl(parameters, secure);
		return _platform.makeRequest(this.sessionCookie, url, secure, sourceStream, length);
	}

	private JSONObject parseResponse(String rawResponse) throws JSONException, PhanfareException {
		JSONObject obj;
		try {
			obj = new JSONObject(rawResponse);
		} catch (JSONException e) {
			e.printStackTrace();
			throw e;
		}
		String stat = obj.getString("stat");
		if (stat.equals("fail") == true) {
			// Failed
			int errorCode = obj.getInt("error_code");
			String errorCodeValue = obj.getString("code_value");
			String message = obj.getString("msg");
			throw new PhanfareException(errorCode, errorCodeValue, message);
		} else {
			// OK
			return obj;
		}
	}

	private String makeUrl(ParameterList parameters, boolean secure) {
		StringBuffer sb = new StringBuffer((secure == true) ? "https://" : "http://");
		sb.append(PhanfareService.BASE_URL);

		String parameterString = parameters.toString();
		sb.append(parameterString);

		MD5 md5 = new MD5((parameterString + _apiSecret).getBytes());
		sb.append("&sig=");
		sb.append(MD5.toHex(md5.doFinal()));

		return sb.toString();
	}
}
