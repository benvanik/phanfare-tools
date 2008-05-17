package com.phanfare.phanberry.ops;

import com.phanfare.api.Album;
import com.phanfare.api.PhanfareException;
import com.phanfare.api.PhanfareService;
import com.phanfare.api.Session;
import com.phanfare.phanberry.cache.AlbumYear;
import com.phanfare.phanberry.cache.ObjectStore;

public class GetYearAlbumsOperation extends BaseOperation {
	private int _year;

	public GetYearAlbumsOperation(int year) {
		_year = year;
	}

	public boolean execute(ObjectStore store, PhanfareService service, Session session) throws PhanfareException {
		Album[] albums = service.getAlbumList(session.userId, false, _year);
		if (albums == null)
			return false;

		AlbumYear year = store.getAlbumYear(_year);
		year.endUpdate(albums);

		return true;
	}
}
