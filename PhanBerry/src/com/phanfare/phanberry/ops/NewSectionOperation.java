package com.phanfare.phanberry.ops;

import com.phanfare.api.Album;
import com.phanfare.api.PhanfareException;
import com.phanfare.api.PhanfareService;
import com.phanfare.api.Section;
import com.phanfare.api.Session;
import com.phanfare.phanberry.cache.ObjectStore;

public class NewSectionOperation extends BaseOperation {
	private Album _album;
	private Section _section;

	public NewSectionOperation(Album album, Section section) {
		this.isWrite = true;
		_album = album;
		_section = section;
	}

	public boolean execute(ObjectStore store, PhanfareService service, Session session) throws PhanfareException {
		Section result = service.newSection(session.userId, _album.albumId, _section);
		if (result == null)
			return false;
		_section.sectionId = result.sectionId;
		return true;
	}
}
