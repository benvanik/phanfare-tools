package com.phanfare.phanberry;

import net.rim.device.api.ui.UiApplication;

public final class Program extends UiApplication {
	public static void main(String[] args) {
		Program program = new Program();
		program.enterEventDispatcher();
	}

	// private LoginScreen _loginScreen;
	// public static LocalCache LocalCache;
	// public static WorkQueue WorkQueue;
	// public static AutoUploader AutoUploader;

	public Program() {
		// LocalCache = new LocalCache();
		// WorkQueue = new WorkQueue(LocalCache);
		// WorkQueue.start();
		// AutoUploader = new AutoUploader(WorkQueue);
		// AutoUploader.SetRootPath("/SDCard/");
		// AutoUploader.start();
		// _loginScreen = new LoginScreen(WorkQueue);
		// this.pushScreen(_loginScreen);
	}
}
