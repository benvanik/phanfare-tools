package com.phanfare.phanberry.ops;

import com.phanfare.api.Album;
import com.phanfare.api.PhanfareException;
import com.phanfare.api.PhanfareService;
import com.phanfare.api.Session;
import com.phanfare.phanberry.cache.ObjectStore;

public class GetMobileAlbumOperation extends BaseOperation {
	public boolean execute(ObjectStore store, PhanfareService service, Session session) throws PhanfareException {
		Album album = service.getMobileAlbum(session.userId, false);
		if (album == null)
			return false;
		store.setMobileAlbum(album);
		return true;
	}
}
