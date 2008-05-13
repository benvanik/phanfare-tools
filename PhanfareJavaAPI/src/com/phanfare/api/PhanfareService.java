package com.phanfare.api;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLConnection;
import java.util.Date;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;
import java.util.Set;
import java.util.zip.GZIPInputStream;

import org.json.me.JSONArray;
import org.json.me.JSONException;
import org.json.me.JSONObject;

import com.phanfare.util.MD5;

public class PhanfareService {
	private final static String BASE_URL = "www.phanfare.com/api/?";

	private String _apiKey;
	private String _apiSecret;
	private boolean _useHttps;

	public String sessionCookie;

	public PhanfareService(String apiKey, String apiSecret) {
		this(apiKey, apiSecret, false);
	}

	public PhanfareService(String apiKey, String apiSecret, boolean useHttps) {
		this.assertParameterNotNullOrEmpty("apiKey", apiKey);
		this.assertParameterNotNullOrEmpty("apiSecret", apiSecret);
		_apiKey = apiKey;
		_apiSecret = apiSecret;
		_useHttps = useHttps;
		this.sessionCookie = "";
	}

	public Session authenticate(String emailAddress, String password) throws PhanfareException {
		this.assertParameterNotNullOrEmpty("emailAddress", emailAddress);
		this.assertParameterNotNullOrEmpty("password", password);
		Map ht = methodCall("authenticate");
		ht.put("email_address", emailAddress);
		ht.put("password", password);
		String responseString = this.makeRequest(ht);
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
		Map ht = methodCall("authenticate");
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
		Map ht = methodCall("getyearlist");
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

		Map ht = methodCall("getalbumlist");
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

		Map ht = methodCall("getalbum");
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

		Map ht = methodCall("getsectionimages");
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

	private void assertSessionValid() {
		if ((this.sessionCookie == null) || (this.sessionCookie.isEmpty() == true)) {
			// TODO
		}
	}

	private void assertParameterNotNull(String name, Object value) {
		if (value == null) {
			// TODO
		}
	}

	private void assertParameterNotNullOrEmpty(String name, String value) {
		if ((value == null) || (value.isEmpty() == true)) {
			// TODO
		}
	}

	private void assertParameterValidId(String name, long id) {
		if (id <= 0) {
			// TODO
		}
	}

	private static Map methodCall(String methodName) {
		HashMap ht = new HashMap(10);
		ht.put("method", methodName);
		return ht;
	}

	private String makeRequest(Map parameters) {
		return this.makeRequest(parameters, _useHttps);
	}

	private String makeRequest(Map parameters, boolean secure) {
		URL url;
		try {
			url = new URL(makeUrl(parameters, secure));
		} catch (MalformedURLException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
			return null;
		}
		URLConnection c;
		try {
			c = url.openConnection();
		} catch (IOException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
			return null;
		}
		// Code to use fiddler:
		// java.net.ProxySelector.setDefault(new ProxySelector() {
		// public void connectFailed(URI uri, SocketAddress sa, IOException ioe)
		// {
		// }
		// public List select(URI uri) {
		// ArrayList l = new ArrayList();
		// Proxy proxy = new Proxy(Proxy.Type.HTTP, new
		// InetSocketAddress("localhost", 8888));
		// l.add(proxy);
		// return l;
		// }
		// });
		c.setConnectTimeout(60 * 1000);
		c.setReadTimeout(60 * 1000);
		c.setUseCaches(false);
		c.addRequestProperty("User-Agent", "PhanfareJavaAPI");
		c.addRequestProperty("cookie", "phanfare2=" + this.sessionCookie + "; ");
		c.addRequestProperty("Accept-Encoding", "gzip");
		BufferedReader stream;
		try {
			stream = new BufferedReader(new InputStreamReader(new GZIPInputStream(c.getInputStream())));
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
		StringBuilder sb = new StringBuilder();
		try {
			while (true) {
				String line = stream.readLine();
				if (line == null)
					break;
				sb.append(line);
			}
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
		try {
			stream.close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return sb.toString();
	}

	private String makeRequest(Map parameters, InputStream sourceStream, long length) {
		return null;
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

	private String makeUrl(Map parameters, boolean secure) {
		StringBuilder url = new StringBuilder(150 + (parameters.size() * 50));
		if (secure == true) {
			url.append("https://");
		} else {
			url.append("http://");
		}
		url.append(PhanfareService.BASE_URL);

		StringBuilder paramList = new StringBuilder(50 + (parameters.size() * 50));
		paramList.append("api_key=" + _apiKey + "&as_json=1");
		Set entries = parameters.entrySet();
		Iterator entryIterator = entries.iterator();
		while (entryIterator.hasNext() == true) {
			Map.Entry entry = (Map.Entry) entryIterator.next();
			paramList.append("&" + entry.getKey() + "=");
			Object value = entry.getValue();
			if (value != null) {
				if (value.getClass() == Boolean.class) {
					paramList.append(((Boolean) value).booleanValue() ? "1" : "0");
				} else if (value.getClass() == Date.class) {
					paramList.append(((Date) value).toString()); // TODO
				} else {
					paramList.append(value.toString());
				}
			}
		}
		String parameterString = paramList.toString();
		MD5 md5 = new MD5((parameterString + _apiSecret).getBytes());
		String sig = MD5.toHex(md5.doFinal());

		url.append(parameterString);
		url.append("&sig=" + sig);
		return url.toString();
	}
}
