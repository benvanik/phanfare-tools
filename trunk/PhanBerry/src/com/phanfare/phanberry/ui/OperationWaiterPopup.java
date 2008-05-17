package com.phanfare.phanberry.ui;

import com.phanfare.phanberry.ops.BaseOperation;
import com.phanfare.phanberry.ops.OperationListener;

import net.rim.device.api.ui.UiApplication;
import net.rim.device.api.ui.component.GaugeField;
import net.rim.device.api.ui.component.LabelField;
import net.rim.device.api.ui.container.DialogFieldManager;
import net.rim.device.api.ui.container.PopupScreen;

public final class OperationWaiterPopup extends Thread implements OperationListener {
	private boolean _isRunning;
	private BaseOperation _operation;
	private OperationListener _callback;
	private boolean _isIncremental;

	private PopupScreen _popup;
	private GaugeField _gaugeField;

	private boolean _failed;
	private int _maxValue;
	private int _currentValue;

	public OperationWaiterPopup(BaseOperation operation, String message, OperationListener callback) {
		_isRunning = true;
		_operation = operation;
		_callback = callback;
		_isIncremental = operation.isIncremental;

		_operation.setListener(this);

		_maxValue = 100;

		DialogFieldManager manager = new DialogFieldManager();
		_popup = new PopupScreen(manager);
		_gaugeField = new GaugeField(null, 1, 100, 1, GaugeField.NO_TEXT);
		manager.addCustomField(new LabelField(message));
		manager.addCustomField(_gaugeField);
	}

	public void run() {
		UiApplication.getUiApplication().invokeAndWait(new Runnable() {
			public void run() {
				UiApplication.getUiApplication().pushScreen(_popup);
			}
		});

		_currentValue = 0;
		while (_isRunning == true) {
			try {
				if (_isIncremental == true) {
					synchronized (_operation) {
						_operation.wait();
					}
				} else
					Thread.sleep(250);
			} catch (Exception e) {
			}

			if (_operation.isComplete == true) {
				_isRunning = false;
				break;
			}

			if (_isIncremental == true) {
				_maxValue = _operation.queryMaximum() + 1;
				int currentValue = _operation.queryCurrent();
				int pct = (int) (((float) currentValue / (float) _maxValue) * 100.0f);
				if (pct != _currentValue) {
					_currentValue = Math.min(100, pct);
					_gaugeField.setValue(_currentValue);
				}
			} else {
				_currentValue++;
				if (_currentValue > _maxValue)
					_currentValue = 1;
				_gaugeField.setValue(_currentValue);
			}
		}

		if (_popup.isDisplayed() == true) {
			UiApplication.getUiApplication().invokeLater(new Runnable() {
				public void run() {
					UiApplication.getUiApplication().popScreen(_popup);
					if (_failed == true)
						_callback.operationFailed();
					else
						_callback.operationSucceeded();
				}
			});
		}
	}

	public void operationFailed() {
		_failed = true;
	}

	public void operationSucceeded() {
		_failed = false;
	}
}
