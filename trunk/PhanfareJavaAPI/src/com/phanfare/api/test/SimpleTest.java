package com.phanfare.api.test;

import com.phanfare.api.Album;
import com.phanfare.api.ImageInfo;
import com.phanfare.api.PhanfareException;
import com.phanfare.api.PhanfareService;
import com.phanfare.api.Section;
import com.phanfare.api.Session;
import com.phanfare.api.Year;

public class SimpleTest {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		PhanfareService service = new PhanfareService("XXXX", "YYYY");
		try {
			Session session = service.authenticate("foo@foo.com", "debug");
			Year[] years = service.getYearList(session.userId);
			Album[] albums = service.getAlbumList(session.userId);
			for (int n = 0; n < albums.length; n++) {
				Album album = albums[n];
				Album full = service.getAlbum(session.userId, album.albumId);
				int y = 7;
				for (int m = 0; m < album.sections.length; m++) {
					Section section = album.sections[m];
					ImageInfo[] images = service.getSectionImages(session.userId, album.albumId, section.sectionId,
							true);
					int z = 8;
				}
			}
			int x = 6;
		} catch (PhanfareException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
}
