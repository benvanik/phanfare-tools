package com.phanfare.api;

public class PhanfareException extends Exception {
	private static final long serialVersionUID = 1L;
	public int errorCode;
	public String errorValue;

	public PhanfareException(String message) {
		super(message);
		this.errorCode = -1;
		this.errorValue = "LibraryError";
	}

	public PhanfareException(int errorCode, String errorValue, String message) {
		super(message);
		this.errorCode = errorCode;
		this.errorValue = errorValue;
	}
}
