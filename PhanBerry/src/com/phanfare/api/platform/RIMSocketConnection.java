package com.phanfare.api.platform;

import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;

import javax.microedition.io.Connector;
import javax.microedition.io.SocketConnection;

import net.rim.device.api.io.SocketConnectionEnhanced;
import net.rim.device.api.util.StringUtilities;

public class RIMSocketConnection {
	public static String postData(String sessionCookie, String url, InputStream sourceStream, long length) {
		// URL comes in with http://, need to change to socket:// as well as
		// extract the server hostname
		String hostName;
		String socketUrl;
		{
			int firstSlash = url.indexOf('/') + 2;
			int secondSlash = url.indexOf('/', firstSlash);
			hostName = url.substring(firstSlash, secondSlash);
			socketUrl = "socket://" + hostName + ":80";
			url = url.substring(secondSlash);
		}

		// Construct request headers
		StringBuffer sb = new StringBuffer();
		sb.append("POST " + url + " HTTP/1.1\r\n");
		sb.append("Host: " + hostName + "\r\n");
		sb.append("User-Agent: PhanfareJavaAPI\r\n");
		sb.append("cookie: phanfare2=" + sessionCookie + ";\r\n");
		sb.append("Connection: close\r\n");
		// sb.append("Expect: 100-continue\r\n");
		// sb.append( "Accept-Encoding: gzip\r\n");
		sb.append("Accept: */*\r\n");
		sb.append("Cache-Control: no-cache\r\n");
		sb.append("Pragma: no-cache\r\n");
		sb.append("Content-Type: application/octet-stream\r\n");
		sb.append("Content-Length: " + length + "\r\n");
		sb.append("\r\n");
		byte[] requestBytes;
		{
			String request = sb.toString();
			requestBytes = request.getBytes();
		}
		sb = new StringBuffer();

		SocketConnection c;
		try {
			c = (SocketConnection) Connector.open(socketUrl, Connector.READ_WRITE, false);
			c.setSocketOption(SocketConnection.LINGER, 500);
			// c.setSocketOption(SocketConnection.KEEPALIVE, 1);
			SocketConnectionEnhanced ce = (SocketConnectionEnhanced) c;
			ce.setSocketOptionEx(SocketConnectionEnhanced.READ_TIMEOUT, 1000 * 60 * 10);
		} catch (IOException ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
			return null;
		}

		OutputStream output = null;
		InputStream input = null;
		try {
			output = c.openOutputStream();
		} catch (IOException ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
			try {
				if (output != null)
					output.close();
				c.close();
			} catch (IOException ex1) {
			}
			return null;
		}

		try {
			// Write request header
			output.write(requestBytes);

			// Write data contents
			int read = 0;
			int total = 0;
			byte[] buffer = new byte[16 * 1024];
			while ((read = sourceStream.read(buffer)) > 0) {
				output.write(buffer, 0, read);
				total += read;
			}
		} catch (IOException ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
			try {
				output.close();
				input.close();
				c.close();
			} catch (IOException ex1) {
			}
			return null;
		}

		try {
			output.flush();
			output.close();
			output = null;
			input = c.openInputStream();
		} catch (IOException ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
			return null;
		}

		try {
			// Read response header/contents
			/*
			 * int read = 0; int total = 0; byte[] buffer = new byte[16 * 1024];
			 * while (true) { int available = input.available(); if (available >
			 * 0) { while ((read = input.read(buffer, 0, available)) >= 0) {
			 * StringUtilities.append(sb, buffer, 0, read); total += read; } if
			 * (read < 0) break; } try { Thread.sleep(10); } catch
			 * (InterruptedException ex) { } }
			 */
			int ch;
			while ((ch = input.read()) != -1)
				sb.append((char) ch);
		} catch (IOException ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
			return null;
		}

		try {
			input.close();
			c.close();
		} catch (IOException ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
			return null;
		}

		String responseAll = sb.toString();
		// HTTP/1.0 200
		int statusCode = Integer.parseInt(responseAll.substring(9, 9 + 3));
		if ((statusCode != 100) && (statusCode != 200)) {
			// TODO invalid return
			return null;
		}
		int contentsStart = responseAll.indexOf("\r\n\r\n") + 4;
		return responseAll.substring(contentsStart);
	}
}
