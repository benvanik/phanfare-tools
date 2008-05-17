package com.phanfare.api.platform;

import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;

import javax.microedition.io.Connector;
import javax.microedition.io.HttpConnection;

import net.rim.device.api.compress.GZIPInputStream;

import com.phanfare.api.PhanfareException;

public class RIMPlatformUtilities extends PlatformUtilities {
	public long download(String sessionCookie, String sourceUrl, OutputStream outputStream) throws PhanfareException {
		HttpConnection c = null;
		try {
			c = (HttpConnection) Connector.open(sourceUrl);
			c.setRequestMethod(HttpConnection.GET);
			c.setRequestProperty("User-Agent", "PhanfareJavaAPI");
			c.setRequestProperty("cookie", "phanfare2=" + sessionCookie + ';');
			c.setRequestProperty("Accept-Encoding", "gzip");
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return -1;
		}
		long totalBytes = 0;
		GZIPInputStream stream = null;
		try {
			if (c.getResponseCode() != HttpConnection.HTTP_OK)
				throw new PhanfareException("HTTP Error " + c.getResponseCode() + ": " + c.getResponseMessage());
			stream = new GZIPInputStream(c.openInputStream());
			byte[] buffer = new byte[64 * 1024];
			int read = 0;
			do {
				read = stream.read(buffer);
				if (read == -1)
					break;
				totalBytes += read;
				outputStream.write(buffer, 0, read);
			} while (read > 0);
		} catch (IOException ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
			return -1;
		} finally {
			try {
				stream.close();
			} catch (IOException ex) {
			}
		}
		return totalBytes;
	}

	public String makeRequest(String sessionCookie, String url, boolean secure, InputStream sourceStream, long length)
			throws PhanfareException {
		HttpConnection c = null;
		try {
			c = (HttpConnection) Connector.open(url);
			c.setRequestProperty("User-Agent", "PhanfareJavaAPI");
			c.setRequestProperty("cookie", "phanfare2=" + sessionCookie + ';');
			c.setRequestProperty("Accept-Encoding", "gzip");
			if (sourceStream != null) {
				c.setRequestProperty("Content-Type", "multipart/form-data");
				c.setRequestProperty("Content-Length", String.valueOf(length));
				c.setRequestMethod(HttpConnection.POST);
				try {
					OutputStream output = c.openOutputStream();
					int read = 0;
					byte[] buffer = new byte[64 * 1024];
					while ((read = sourceStream.read(buffer)) > 0) {
						output.write(buffer, 0, read);
					}
				} catch (IOException ex) {
					// TODO Auto-generated catch block
					ex.printStackTrace();
					return null;
				}
			} else {
				c.setRequestMethod(HttpConnection.GET);
			}
			if (c.getResponseCode() != HttpConnection.HTTP_OK)
				throw new PhanfareException("HTTP Error " + c.getResponseCode() + ": " + c.getResponseMessage());
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
		InputStreamReader stream = null;
		StringBuffer sb = new StringBuffer();
		try {
			try {
				stream = new InputStreamReader(new GZIPInputStream(c.openInputStream()));
			} catch (IOException ex) {
				// TODO Auto-generated catch block
				ex.printStackTrace();
				return null;
			}
			try {
				int read = 0;
				char[] buffer = new char[16 * 1024];
				while ((read = stream.read(buffer)) > 0) {
					sb.append(buffer, 0, read);
				}
			} catch (IOException ex) {
				// TODO Auto-generated catch block
				ex.printStackTrace();
				return null;
			}
		} finally {
			try {
				stream.close();
			} catch (IOException ex) {
			}
		}
		return sb.toString();
	}
}
