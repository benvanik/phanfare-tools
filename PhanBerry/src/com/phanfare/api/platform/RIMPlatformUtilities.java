package com.phanfare.api.platform;

import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.InterruptedIOException;
import java.io.OutputStream;

import javax.microedition.io.Connector;
import javax.microedition.io.HttpConnection;

import net.rim.device.api.ui.UiApplication;
import net.rim.device.api.ui.component.Status;

import com.phanfare.api.PhanfareException;

public class RIMPlatformUtilities extends PlatformUtilities {
	public long download(String sessionCookie, String sourceUrl, OutputStream outputStream) throws PhanfareException {
		HttpConnection c = null;
		try {
			c = (HttpConnection) Connector.open(sourceUrl);
			c.setRequestMethod(HttpConnection.GET);
			c.setRequestProperty("User-Agent", "PhanfareJavaAPI");
			c.setRequestProperty("cookie", "phanfare2=" + sessionCookie + ';');
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return -1;
		}
		long totalBytes = 0;
		InputStream stream = null;
		try {
			if (c.getResponseCode() != HttpConnection.HTTP_OK)
				throw new PhanfareException("HTTP Error " + c.getResponseCode() + ": " + c.getResponseMessage());
			stream = c.openInputStream();
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
		if (sourceStream != null) {
			return RIMSocketConnection.postData(sessionCookie, url, sourceStream, length);
		} else {
			HttpConnection c = null;
			StringBuffer sb = new StringBuffer();
			try {
				try {
					c = (HttpConnection) Connector.open(url, Connector.READ_WRITE, false);
					c.setRequestProperty("User-Agent", "PhanfareJavaAPI");
					c.setRequestProperty("cookie", "phanfare2=" + sessionCookie + ';');
					c.setRequestProperty("Connection", "keep-alive");
					c.setRequestProperty("Cache-Control", "no-cache");
					c.setRequestProperty("Pragma", "no-cache");
					// c.setRequestProperty("Accept-Encoding", "gzip");
					c.setRequestMethod(HttpConnection.GET);
				} catch (IOException ex) {
					// TODO Auto-generated catch block
//					final String exm = ex.toString();
//					final String exs = ex.getMessage();
//					UiApplication.getUiApplication().invokeAndWait(new Runnable() {
//						public void run() {
//							Status.show(exm);
//							Status.show(exs);
//						}
//					});
					ex.printStackTrace();
					return null;
				}
//				UiApplication.getUiApplication().invokeAndWait(new Runnable() {
//					public void run() {
//						Status.show("starting read...");
//					}
//				});
				InputStream stream = null;
				try {
					try {
						int responseCode = c.getResponseCode();
						if (responseCode != HttpConnection.HTTP_OK)
							throw new PhanfareException("HTTP Error " + responseCode + ": " + c.getResponseMessage());
						stream = c.openInputStream();
						// stream = new InputStreamReader(new
						// GZIPInputStream(c.openInputStream()));
					} catch (InterruptedIOException ex) {
//						final int t = ex.bytesTransferred;
//						final String exm = ex.toString();
//						final String exs = ex.getMessage();
//						UiApplication.getUiApplication().invokeAndWait(new Runnable() {
//							public void run() {
//								Status.show(exm);
//								Status.show(exs);
//								Status.show(String.valueOf(t));
//								Status.show(String.valueOf(t));
//								Status.show(String.valueOf(t));
//								Status.show(String.valueOf(t));
//							}
//						});
						ex.printStackTrace();
						return null;
					} catch (IOException ex) {
						// TODO Auto-generated catch block
						ex.printStackTrace();
						return null;
					}
					try {
						int read = 0;
						int total = 0;
						char[] buffer = new char[16 * 1024];
						InputStreamReader reader = new InputStreamReader(stream);
						while ((read = reader.read(buffer)) > 0) {
							sb.append(buffer, 0, read);
							total += read;
//							final String s = "read " + read + " bytes, total " + total;
//							UiApplication.getUiApplication().invokeAndWait(new Runnable() {
//								public void run() {
//									Status.show(s);
//								}
//							});
						}
//						UiApplication.getUiApplication().invokeAndWait(new Runnable() {
//							public void run() {
//								Status.show("finished read");
//							}
//						});
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
			} catch (Exception ex) {
				ex.printStackTrace();
			} finally {
				try {
					c.close();
				} catch (IOException ex) {
				}
			}
			return sb.toString();
		}
	}
}
