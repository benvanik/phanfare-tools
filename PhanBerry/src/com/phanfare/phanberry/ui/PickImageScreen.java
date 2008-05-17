package com.phanfare.phanberry.ui;

import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.container.MainScreen;

public class PickImageScreen extends MainScreen {
	public PickImageScreen() {
		super(DEFAULT_MENU | DEFAULT_CLOSE);

		this.setTitle(new LabelField("PhanBerry", LabelField.ELLIPSIS | LabelField.USE_ALL_WIDTH));
	}
}
