package com.phanfare.phanberry.ops;

import com.phanfare.api.PhanfareException;
import com.phanfare.api.PhanfareService;
import com.phanfare.api.Session;
import com.phanfare.phanberry.cache.ObjectStore;

public abstract class BaseOperation {
	public boolean isWrite;
	public boolean isIncremental;
	public boolean isComplete;
	public OperationListener listener;

	public void setListener(OperationListener listener) {
		this.listener = listener;
	}

	public abstract boolean execute(ObjectStore store, PhanfareService service, Session session)
			throws PhanfareException;

	public int queryMaximum() {
		return 0;
	}

	public int queryCurrent() {
		return 0;
	}
}
