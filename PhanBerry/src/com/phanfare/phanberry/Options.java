package com.phanfare.phanberry;

import net.rim.device.api.system.PersistentObject;
import net.rim.device.api.system.PersistentStore;
import net.rim.device.api.util.Persistable;

public class Options implements Persistable {
	public String emailAddress;
	public String password;

	public boolean autoCapture;

	public String rootPath;

	private static final long PERSIST = 0x9dbb6535e730ff7fL;
	private static PersistentObject _persist;

	public static Options getOptions() {
		_persist = PersistentStore.getPersistentObject(PERSIST);
		Options options = (Options) _persist.getContents();
		if (options == null) {
			options = new Options();
			options.autoCapture = true;
			options.rootPath = "/store/home/user/pictures/";
			_persist.setContents(options);
			_persist.commit();
		}
		return options;
	}

	public synchronized void save() {
		_persist.setContents(this);
		_persist.commit();
	}
}
