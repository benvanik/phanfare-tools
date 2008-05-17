package com.phanfare.api.platform;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLConnection;
import java.util.zip.GZIPInputStream;

import com.phanfare.api.PhanfareException;

public class DefaultPlatformUtilities extends PlatformUtilities {
	public long download(String sessionCookie, String sourceUrl, OutputStream outputStream) throws PhanfareException {
		URL url;
		try {
			url = new URL(sourceUrl);
		} catch (MalformedURLException ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
			return -1;
		}
		URLConnection c;
		try {
			c = url.openConnection();
		} catch (IOException ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
			return -1;
		}
		c.setConnectTimeout(60 * 1000);
		c.setReadTimeout(60 * 1000);
		c.setUseCaches(true);
		c.addRequestProperty("User-Agent", "PhanfareJavaAPI");
		c.addRequestProperty("cookie", "phanfare2=" + sessionCookie + ';');
		c.addRequestProperty("Accept-Encoding", "gzip");
		BufferedInputStream stream;
		try {
			stream = new BufferedInputStream(new GZIPInputStream(c.getInputStream()));
		} catch (IOException ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
			return -1;
		}
		long totalBytes = 0;
		byte[] buffer = new byte[64 * 1024];
		try {
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
		}
		try {
			stream.close();
		} catch (IOException ex) {
		}
		return totalBytes;
	}

	public String makeRequest(String sessionCookie, String sourceUrl, boolean secure, InputStream sourceStream,
			long length) {
		URL url;
		try {
			url = new URL(sourceUrl);
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
		c.addRequestProperty("cookie", "phanfare2=" + sessionCookie + ';');
		c.addRequestProperty("Accept-Encoding", "gzip");
		if (sourceStream != null) {
			c.setRequestProperty("Content-Type", "multipart/form-data");
			c.setRequestProperty("Content-Length", String.valueOf(length));
			// request.Method = "POST";
			try {
				BufferedInputStream input = new BufferedInputStream(sourceStream);
				BufferedOutputStream output = new BufferedOutputStream(c.getOutputStream());
				int read = 0;
				byte[] buffer = new byte[64 * 1024];
				while ((read = input.read(buffer)) > 0) {
					output.write(buffer, 0, read);
				}
			} catch (IOException ex) {
				// TODO Auto-generated catch block
				ex.printStackTrace();
				return null;
			}
		}
		BufferedReader stream = null;
		StringBuilder sb = new StringBuilder();
		try {
			try {
				stream = new BufferedReader(new InputStreamReader(new GZIPInputStream(c.getInputStream())));
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
