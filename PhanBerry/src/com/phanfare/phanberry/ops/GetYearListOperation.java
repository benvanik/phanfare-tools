package com.phanfare.phanberry.ops;

import com.phanfare.api.PhanfareException;
import com.phanfare.api.PhanfareService;
import com.phanfare.api.Session;
import com.phanfare.api.Year;
import com.phanfare.phanberry.cache.ObjectStore;

public class GetYearListOperation extends BaseOperation {
	public GetYearListOperation() {
	}

	public boolean execute(ObjectStore store, PhanfareService service, Session session) throws PhanfareException {
		Year[] years = service.getYearList(session.userId);
		if (years == null)
			return false;
		store.endUpdateYearList(years);
		return true;
	}
}
