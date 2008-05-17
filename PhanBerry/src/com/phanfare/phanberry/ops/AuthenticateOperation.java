package com.phanfare.phanberry.ops;

import com.phanfare.api.PhanfareException;
import com.phanfare.api.PhanfareService;
import com.phanfare.api.Session;
import com.phanfare.phanberry.cache.ObjectStore;

public class AuthenticateOperation extends BaseOperation {
	private String _emailAddress;
	private String _password;

	public AuthenticateOperation(String emailAddress, String password) {
		_emailAddress = emailAddress.trim();
		_password = password.trim();
	}

	public boolean execute(ObjectStore store, PhanfareService service, Session session) throws PhanfareException {
		Session newSession = service.authenticate(_emailAddress, _password);
		if (newSession == null)
			return false;

		store.setSession(session);

		return true;
	}
}
