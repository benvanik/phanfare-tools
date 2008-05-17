package com.phanfare.phanberry.io;

import java.util.Vector;

import com.phanfare.phanberry.Controller;

import net.rim.device.api.io.file.FileSystemJournal;
import net.rim.device.api.io.file.FileSystemJournalEntry;
import net.rim.device.api.io.file.FileSystemJournalListener;

public class ImageFileListener implements FileSystemJournalListener {
	private long _lastUSN;
	private Controller _controller;
	private String _rootPath;

	public ImageFileListener(Controller controller, String rootPath) {
		_controller = controller;
		_rootPath = rootPath;
	}

	public void setRootPath(String rootPath) {
		_rootPath = rootPath;
	}

	public void fileJournalChanged() {
		Vector foundPaths = new Vector();
		long nextUSN = FileSystemJournal.getNextUSN();
		for (long lookUSN = nextUSN - 1; lookUSN >= _lastUSN; --lookUSN) {
			FileSystemJournalEntry entry = FileSystemJournal.getEntry(lookUSN);
			if (entry == null) {
				break;
			}

			// Ignore if not setup right
			if (_rootPath == null) {
				continue;
			}

			String path = entry.getPath();
			if (path != null) {
				switch (entry.getEvent()) {
				case FileSystemJournalEntry.FILE_ADDED:
					String lowerPath = path.toLowerCase();
					if (lowerPath.startsWith(_rootPath) == false)
						continue;
					if (lowerPath.endsWith(".jpg") == true) {
						foundPaths.addElement(path);
					}
					break;
				}
			}
		}

		// _lastUSN must be updated before doing anything else to prevent dupes
		_lastUSN = nextUSN;

		if (foundPaths.size() > 0) {
			_controller.processFiles(foundPaths);
		}
	}
}
