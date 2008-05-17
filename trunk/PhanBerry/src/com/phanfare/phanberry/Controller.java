package com.phanfare.phanberry;

import java.util.Vector;

import net.rim.blackberry.api.invoke.CameraArguments;
import net.rim.blackberry.api.invoke.Invoke;
import net.rim.device.api.system.Characters;
import net.rim.device.api.system.EventInjector;
import net.rim.device.api.ui.UiApplication;

import com.phanfare.phanberry.io.ImageFileListener;

public class Controller extends Thread {
	public Options options;
	private ImageFileListener _fileListener;
	private boolean _isRunning;

	public Controller(Options options) {
		this.options = options;
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
				Thread.sleep(600000);
			} catch (InterruptedException e) {
			}
		}
		this.destroy();
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
		for (int n = 0; n < files.size(); n++) {
			String path = (String) files.elementAt(n);
			// Handle path
		}
	}
}
