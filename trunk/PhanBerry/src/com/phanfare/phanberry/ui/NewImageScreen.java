package com.phanfare.phanberry.ui;

import net.rim.device.api.system.Characters;
import net.rim.device.api.ui.Field;
import net.rim.device.api.ui.UiApplication;
import net.rim.device.api.ui.component.CheckboxField;
import net.rim.device.api.ui.component.EditField;
import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.component.ObjectChoiceField;
import net.rim.device.api.ui.component.SeparatorField;
import net.rim.device.api.ui.component.Status;
import net.rim.device.api.ui.container.MainScreen;

import com.phanfare.api.Album;
import com.phanfare.api.ImageInfo;
import com.phanfare.api.Section;
import com.phanfare.phanberry.Controller;
import com.phanfare.phanberry.WorkQueue;
import com.phanfare.phanberry.ops.GetMobileAlbumOperation;
import com.phanfare.phanberry.ops.NewImageOperation;
import com.phanfare.phanberry.ops.OperationListener;

public class NewImageScreen extends MainScreen {
	private WorkQueue _workQueue;
	private Controller _controller;

	private EditField _captionField;
	private ObjectChoiceField _albumModeField;
	private CheckboxField _hideField;
	private String _path;

	public NewImageScreen(WorkQueue workQueue, Controller controller, String path) {
		super(DEFAULT_MENU | DEFAULT_CLOSE);

		_workQueue = workQueue;
		_controller = controller;
		_path = path;

		String fileName = path.substring(path.lastIndexOf('/') + 1);
		String fileNameWithoutExtension = fileName.substring(0, fileName.length() - 4);
		this.setTitle(new LabelField("PhanBerry: " + fileNameWithoutExtension, LabelField.ELLIPSIS
				| LabelField.USE_ALL_WIDTH));

		String[] choices;
		int defaultAlbumMode;
		Album lastAlbum = controller.objectStore.getLastAlbum();
		if (lastAlbum == null) {
			choices = new String[] { "Today's Album", "New Album", "Existing Album" };
			defaultAlbumMode = 0;
		} else {
			choices = new String[] { "Today's Album", "New Album", "Existing Album", lastAlbum.name };
			defaultAlbumMode = 3;
		}

		_albumModeField = new ObjectChoiceField("Target: ", choices, defaultAlbumMode);
		this.add(_albumModeField);

		this.add(new SeparatorField());

		this.add(new LabelField("Caption:"));
		_captionField = new EditField("", "", 2000, Field.EDITABLE | Field.USE_ALL_WIDTH | Field.USE_ALL_HEIGHT);
		this.add(_captionField);

		this.add(new SeparatorField());

		_hideField = new CheckboxField("Hide", false);
		this.add(_hideField);
	}

	private void postImage() {
		// _path
		int albumMode = _albumModeField.getIndex();
		String caption = _captionField.getText().trim();
		boolean hide = _hideField.getChecked();

		ImageInfo imageInfo = new ImageInfo();
		imageInfo.fileName = _path;
		imageInfo.imageDate = null; // Should be grabbed by the service
		imageInfo.isHidden = hide;
		imageInfo.caption = caption;

		boolean toMobileAlbum = false;
		Album album = null;
		Section section = null;

		switch (albumMode) {
		case 0: // today's album
			toMobileAlbum = true;
			break;
		case 1: // new album
			break;
		case 2: // existing album
			break;
		case 3: // last album
			break;
		}

		NewImageOperation op;
		if (toMobileAlbum == true) {
			// Request mobile album
			GetMobileAlbumOperation albumOp = new GetMobileAlbumOperation();
			_workQueue.enqueue(albumOp);

			op = new NewImageOperation(imageInfo);
		} else {
			op = new NewImageOperation(album, section, imageInfo);
		}

		Status.show("beginning post");
		// OperationWaiterPopup popup = new OperationWaiterPopup(op, "Posting
		// Image...", new OperationListener() {
		// public void operationFailed() {
		// Status.show("Failed to post image");
		// }
		//
		// public void operationSucceeded() {
		// Status.show("Posted image");
		// UiApplication app = UiApplication.getUiApplication();
		// // app.pushScreen(new TocScreen(_workQueue,
		// // _emailField.getText()));
		// app.popScreen(NewImageScreen.this);
		// }
		// });
		// popup.start();
		op.setListener(new OperationListener() {
			public void operationFailed() {
				Status.show("Failed to post image");
			}

			public void operationSucceeded() {
				UiApplication.getUiApplication().invokeAndWait(new Runnable() {
					public void run() {
						UiApplication app = UiApplication.getUiApplication();
						Status.show("Posted image");
						// app.pushScreen(new TocScreen(_workQueue,
						// _emailField.getText()));
						app.popScreen(NewImageScreen.this);
					}
				});
			}
		});
		_workQueue.enqueue(op);
	}

	protected boolean keyChar(char key, int status, int time) {
		if (key == Characters.ENTER) {
			this.postImage();
			return true;
		} else {
			return super.keyChar(key, status, time);
		}
	}
}
