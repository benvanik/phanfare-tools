package com.phanfare.phanberry.ops;

import com.phanfare.api.Album;
import com.phanfare.api.PhanfareException;
import com.phanfare.api.PhanfareService;
import com.phanfare.api.Session;
import com.phanfare.phanberry.cache.ObjectStore;

public class NewAlbumOperation extends BaseOperation {
	private Album _album;

	public NewAlbumOperation(Album album) {
		this.isWrite = true;
		_album = album;
	}

	public boolean execute(ObjectStore store, PhanfareService service, Session session) throws PhanfareException {
		long[] groups = new long[_album.groups.length];
		for (int n = 0; n < groups.length; n++) {
			groups[n] = _album.groups[n].groupId;
		}
		Album result = service.newAlbum(session.userId, _album, groups);
		if (result == null)
			return false;
		_album.albumId = result.albumId;
		for (int n = 0; n < _album.sections.length; n++) {
			_album.sections[n].albumId = result.albumId;
		}
		_album.sections[0].sectionId = result.sections[0].sectionId;
		return true;
	}
}
