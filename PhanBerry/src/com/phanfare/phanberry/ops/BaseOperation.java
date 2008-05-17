package com.phanfare.phanberry.ops;

import com.phanfare.api.PhanfareException;
import com.phanfare.api.PhanfareService;
import com.phanfare.api.Session;

public abstract class BaseOperation {
	public boolean isWrite;

	public abstract boolean execute(ObjectStore store, PhanfareService service, Session session)
			throws PhanfareException;
}
