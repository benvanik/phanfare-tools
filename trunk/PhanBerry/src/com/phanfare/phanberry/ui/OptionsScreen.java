package com.phanfare.phanberry.ui;

import net.rim.device.api.ui.Field;
import net.rim.device.api.ui.FieldChangeListener;
import net.rim.device.api.ui.UiApplication;
import net.rim.device.api.ui.component.ButtonField;
import net.rim.device.api.ui.component.EmailAddressEditField;
import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.component.PasswordEditField;
import net.rim.device.api.ui.container.MainScreen;

import com.phanfare.phanberry.Controller;
import com.phanfare.phanberry.Options;
import com.phanfare.phanberry.WorkQueue;
import com.phanfare.phanberry.ops.AuthenticateOperation;
import com.phanfare.phanberry.ops.OperationListener;

public class OptionsScreen extends MainScreen {
	private WorkQueue _workQueue;
	private Controller _controller;
	private EmailAddressEditField _emailField;
	private PasswordEditField _passwordField;

	public OptionsScreen(WorkQueue workQueue, Controller controller) {
		super(DEFAULT_MENU | DEFAULT_CLOSE);

		_workQueue = workQueue;
		_controller = controller;
		final Options options = controller.options;

		this.setTitle(new LabelField("PhanBerry Options", LabelField.ELLIPSIS | LabelField.USE_ALL_WIDTH));

		String emailAddress = options.emailAddress;
		String password = options.password;

		_emailField = new EmailAddressEditField("E-mail: ", emailAddress);
		_passwordField = new PasswordEditField("Password: ", password);
		this.add(_emailField);
		this.add(_passwordField);

		ButtonField loginButton = new ButtonField("Save", FOCUSABLE | FIELD_HCENTER | ButtonField.CONSUME_CLICK);
		loginButton.setChangeListener(new FieldChangeListener() {
			public void fieldChanged(Field field, int context) {
				options.emailAddress = _emailField.getText();
				options.password = _passwordField.getText();
				options.save();
				_controller.optionsChanged();
				OptionsScreen.this.authenticate(options.emailAddress, options.password);
			}
		});
		this.add(loginButton);
	}

	private void authenticate(String emailAddress, String password) {
		AuthenticateOperation op = new AuthenticateOperation(emailAddress, password);
		OperationWaiterPopup popup = new OperationWaiterPopup(op, "Logging in...", new OperationListener() {
			public void operationFailed() {
				// Popup error screen
			}

			public void operationSucceeded() {
				UiApplication app = UiApplication.getUiApplication();
				_controller.showPicker();
				app.popScreen(OptionsScreen.this);
			}
		});
		popup.start();
		_workQueue.enqueue(op);
	}

	public boolean onClose() {
		return super.onClose();
	}
}
