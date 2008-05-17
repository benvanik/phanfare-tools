package com.phanfare.phanberry.ops;

import com.phanfare.api.Album;
import com.phanfare.api.PhanfareException;
import com.phanfare.api.PhanfareService;
import com.phanfare.api.Session;
import com.phanfare.phanberry.cache.ObjectStore;

public class NewAlbumOperation extends BaseOperation {
	private int _tempId;
	private String _name;
	private long[] _groups;

	public NewAlbumOperation(int tempId, String albumName, long[] groups) {
		this.isWrite = true;
		_tempId = tempId;
		_name = albumName;
		_groups = groups;
	}

	public boolean execute(ObjectStore store, PhanfareService service, Session session) throws PhanfareException {
		Album album = new Album();
		album.name = _name;
		Album result = service.newAlbum(session.userId, album, _groups);
		if (result == null)
			return false;
		Album temp = (Album) store.removeTempObject(_tempId);
		temp.albumId = result.albumId;
		return true;
	}
}
