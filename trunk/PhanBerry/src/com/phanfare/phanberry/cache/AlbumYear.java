package com.phanfare.phanberry.cache;

import java.util.Vector;

import com.phanfare.api.Album;

public class AlbumYear {
	public int year;
	public boolean isTimeless;
	public Vector albums;

	public boolean hasLoaded;
	public boolean isLoading;

	public AlbumYear(int year, boolean isTimeless) {
		this.year = year;
		this.isTimeless = isTimeless;
		this.albums = new Vector();
	}

	public void clear() {
		synchronized (this) {
			this.albums.removeAllElements();
			this.hasLoaded = false;
		}
	}

	public void beginUpdate() {
		this.isLoading = true;
	}

	public void endUpdate(Album[] albums) {
		synchronized (this) {
			this.albums.removeAllElements();
			for (int n = 0; n < albums.length; n++) {
				this.albums.addElement(albums[n]);
			}
			this.isLoading = false;
			this.hasLoaded = true;
		}
	}
}
