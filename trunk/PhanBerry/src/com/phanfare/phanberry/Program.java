package com.phanfare.phanberry;

import net.rim.device.api.ui.UiApplication;

import com.phanfare.api.PhanfareException;
import com.phanfare.phanberry.cache.ObjectStore;
import com.phanfare.phanberry.ui.OptionsScreen;

public final class Program extends UiApplication {
	public static void main(String[] args) {
		Program program = new Program();
		program.enterEventDispatcher();
	}

	public static ObjectStore objectStore;
	public static WorkQueue workQueue;
	public static Controller controller;

	public Program() {
		objectStore = new ObjectStore();
		try {
			workQueue = new WorkQueue(objectStore);
		} catch (PhanfareException ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
			return;
		}

		// Load options or create defaults
		Options options = Options.getOptions();

		controller = new Controller(objectStore, workQueue, options);

		if (objectStore.getSession() == null) {
			OptionsScreen optionsScreen = new OptionsScreen(workQueue, controller);
			this.pushScreen(optionsScreen);
		} else {
			controller.showPicker();
		}
	}
}
