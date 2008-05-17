package com.phanfare.phanberry;

import java.util.Vector;

import com.phanfare.api.PhanfareException;
import com.phanfare.api.PhanfareService;
import com.phanfare.api.platform.RIMPlatformUtilities;
import com.phanfare.phanberry.cache.ObjectStore;
import com.phanfare.phanberry.ops.BaseOperation;

public class WorkQueue extends Thread {
	private ObjectStore _store;
	private PhanfareService _service;

	private Vector _queue;
	private boolean _isRunning;

	public WorkQueue(ObjectStore store) throws PhanfareException {
		_store = store;
		_service = new PhanfareService(AppConstants.ApiPublicKey, AppConstants.ApiSecretKey, new RIMPlatformUtilities());

		_queue = new Vector();
		_isRunning = true;
		this.start();
	}

	public void enqueue(BaseOperation operation) {
		synchronized (_queue) {
			_queue.addElement(operation);
			this.interrupt();
		}
	}

	public void abortAll() {
		synchronized (_queue) {
			_queue.removeAllElements();
		}
	}

	public int size() {
		synchronized (_queue) {
			return _queue.size();
		}
	}

	private static final int RESULT_SUCCESSFUL = 0;
	private static final int RESULT_FAILED = 1;
	private static final int RESULT_NETWORK_ERROR = 2;
	private static final int RESULT_ACCOUNT_ERROR = 3;
	private static final int RESULT_COHERENCE_ERROR = 4;

	public void run() {
		while (_isRunning == true) {
			int queueSize = 0;
			synchronized (_queue) {
				queueSize = _queue.size();
			}
			while (queueSize == 0) {
				try {
					// Wait a looonnngggg time
					Thread.sleep(5 * 60 * 1000);
				} catch (InterruptedException e) {
					break;
				}
			}
			if (_isRunning == false)
				return;
			BaseOperation operation = null;
			synchronized (_queue) {
				if (_queue.size() == 0)
					continue;
				operation = (BaseOperation) _queue.elementAt(0);
				_queue.removeElementAt(0);
			}
			int result = this.runOne(operation);
			synchronized (operation) {
				operation.notifyAll();
			}
			switch (result) {
			case RESULT_SUCCESSFUL:
				if (operation.listener != null) {
					operation.listener.operationSucceeded();
				}
				break;
			case RESULT_NETWORK_ERROR:
				// Try again?
				synchronized (_queue) {
					_queue.insertElementAt(operation, 0);
				}
				break;
			case RESULT_FAILED:
			case RESULT_ACCOUNT_ERROR:
			case RESULT_COHERENCE_ERROR:
				if (operation.listener != null) {
					operation.listener.operationFailed();
				}
				break;
			}
		}
	}

	private int runOne(BaseOperation operation) {
		try {
			if (operation.execute(_store, _service, _store.getSession()) == true) {
				operation.isComplete = true;
				return RESULT_SUCCESSFUL;
			} else {
				operation.isComplete = true;
				return RESULT_FAILED;
			}
		} catch (PhanfareException ex) {
			// TODO: other failure types
			return RESULT_FAILED;
		}
		// } catch (ConnectionNotFoundException cne) {
		// // Connection specified in URL can't be created.
		// // Handle exception, throw exception or return error.
		// return BaseOperation.ResultNetworkError;
		// } catch (IllegalArgumentException iae) {
		// // One of the arguments is in error. In this example, the
		// // only argument to open is URL, so the only expected
		// // exceptions are ConnectionNotFoundException or IOException.
		// // Handle exception, throw exception or return error.
		// return BaseOperation.ResultFailed;
		// } catch (IOException ioe) {
		// // Handle exception, throw exception or return error.
		// return BaseOperation.ResultNetworkError;
		// }
	}
}
