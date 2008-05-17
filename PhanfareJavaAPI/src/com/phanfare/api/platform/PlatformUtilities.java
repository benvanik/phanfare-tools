package com.phanfare.api.platform;

import java.io.InputStream;
import java.io.OutputStream;

import com.phanfare.api.PhanfareException;

public abstract class PlatformUtilities {
	public abstract long download(String sessionCookie, String sourceUrl, OutputStream outputStream)
			throws PhanfareException;

	public abstract String makeRequest(String sessionCookie, String url, boolean secure, InputStream sourceStream,
			long length) throws PhanfareException;
}
