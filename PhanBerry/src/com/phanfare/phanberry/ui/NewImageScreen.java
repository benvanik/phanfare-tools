package com.phanfare.phanberry.ui;

import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.container.MainScreen;

import com.phanfare.phanberry.Controller;
import com.phanfare.phanberry.WorkQueue;

public class NewImageScreen extends MainScreen {

	public NewImageScreen(WorkQueue workQueue, Controller controller, String path) {
		super(DEFAULT_MENU | DEFAULT_CLOSE);

		String fileName = path.substring(path.lastIndexOf('/') + 1);
		this.setTitle(new LabelField("PhanBerry: " + fileName, LabelField.ELLIPSIS | LabelField.USE_ALL_WIDTH));
	}
}
