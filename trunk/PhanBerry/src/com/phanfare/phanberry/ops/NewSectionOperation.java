package com.phanfare.phanberry.ops;

import com.phanfare.api.PhanfareException;
import com.phanfare.api.PhanfareService;
import com.phanfare.api.Section;
import com.phanfare.api.Session;
import com.phanfare.phanberry.cache.ObjectStore;

public class NewSectionOperation extends BaseOperation {
	private long _albumId;
	private int _tempId;
	private String _name;

	public NewSectionOperation(long albumId, int tempId, String sectionName) {
		this.isWrite = true;
		_albumId = albumId;
		_tempId = tempId;
		_name = sectionName;
	}

	public boolean execute(ObjectStore store, PhanfareService service, Session session) throws PhanfareException {
		Section section = new Section();
		section.name = _name;
		Section result = service.newSection(session.userId, _albumId, section);
		if (result == null)
			return false;
		Section temp = (Section) store.removeTempObject(_tempId);
		temp.sectionId = result.sectionId;
		return true;
	}
}
