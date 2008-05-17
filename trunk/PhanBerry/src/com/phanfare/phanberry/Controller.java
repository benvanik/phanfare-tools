package com.phanfare.phanberry;

import java.util.Vector;

import net.rim.blackberry.api.invoke.CameraArguments;
import net.rim.blackberry.api.invoke.Invoke;
import net.rim.device.api.system.Characters;
import net.rim.device.api.system.EventInjector;
import net.rim.device.api.ui.UiApplication;

import com.phanfare.phanberry.cache.ObjectStore;
import com.phanfare.phanberry.io.ImageFileListener;
import com.phanfare.phanberry.ui.NewImageScreen;
import com.phanfare.phanberry.ui.PickImageScreen;

public class Controller extends Thread {
	public ObjectStore objectStore;
	public WorkQueue workQueue;
	public Options options;

	private PickImageScreen _pickImageScreen;
	private ImageFileListener _fileListener;
	private boolean _isRunning;

	public Controller(ObjectStore objectStore, WorkQueue workQueue, Options options) {
		this.objectStore = objectStore;
		this.workQueue = workQueue;
		this.options = options;
		this.setup();
		this.start();
	}

	public void setup() {
		UiApplication app = UiApplication.getUiApplication();
		_fileListener = new ImageFileListener(this, options.rootPath);
		app.addFileSystemJournalListener(_fileListener);
	}

	public void destroy() {
		UiApplication app = UiApplication.getUiApplication();
		app.removeFileSystemJournalListener(_fileListener);
		_fileListener = null;
	}

	public void optionsChanged() {
		_fileListener.setRootPath(options.rootPath);
	}

	public void run() {
		_isRunning = true;
		while (_isRunning == true) {
			try {
				Thread.sleep(5 * 60 * 1000);
			} catch (InterruptedException e) {
			}
		}
		this.destroy();
	}

	public void showPicker() {
		if (_pickImageScreen == null) {
			_pickImageScreen = new PickImageScreen();
		}
		UiApplication app = UiApplication.getUiApplication();
		app.pushScreen(_pickImageScreen);
		this.launchCamera();
	}

	public void launchCamera() {
		Invoke.invokeApplication(Invoke.APP_TYPE_CAMERA, new CameraArguments());
	}

	public void closeCamera() {
		EventInjector.KeyEvent inject = new EventInjector.KeyEvent(EventInjector.KeyEvent.KEY_DOWN, Characters.ESCAPE,
				0, 50);
		inject.post();
		inject.post();
	}

	public void processFiles(Vector files) {
		this.closeCamera();
		UiApplication app = UiApplication.getUiApplication();
		app.activate();
		for (int n = 0; n < files.size(); n++) {
			String path = (String) files.elementAt(n);
			NewImageScreen newImageScreen = new NewImageScreen(this.workQueue, this, path);
			app.pushModalScreen(newImageScreen);
		}
	}
}
