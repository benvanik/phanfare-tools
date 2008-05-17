package com.phanfare.phanberry.cache;

import java.util.Hashtable;
import java.util.Vector;

import com.phanfare.api.Session;
import com.phanfare.api.Year;

public class ObjectStore {
	private Session _session;
	private Vector _years;
	private Hashtable _tempObjects;
	private int _tempCounter;

	private boolean hasLoadedYears;
	private boolean isLoadingYears;

	public ObjectStore() {
		_years = new Vector();
		_tempObjects = new Hashtable();
		_tempCounter = 100;
	}

	public Session getSession() {
		return _session;
	}

	public void setSession(Session session) {
		_session = session;
	}

	public int addTempObject(Object temp) {
		synchronized (this) {
			_tempObjects.put(new Integer(_tempCounter++), temp);
			return _tempCounter;
		}
	}

	public Object removeTempObject(int tempId) {
		synchronized (this) {
			return _tempObjects.remove(new Integer(tempId));
		}
	}

	public void clear() {
		synchronized (this) {
			_years.removeAllElements();
			this.hasLoadedYears = false;
		}
	}

	public void beginUpdateYearList() {
		this.isLoadingYears = true;
	}

	public void endUpdateYearList(Year[] years) {
		synchronized (this) {
			_years.removeAllElements();
			for (int n = 0; n < years.length; n++) {
				Year year = years[n];
				AlbumYear albumYear = new AlbumYear(year.yearOrdinal, year.isTimeless());
				_years.addElement(albumYear);
			}
			this.isLoadingYears = false;
			this.hasLoadedYears = true;
		}
	}

	public AlbumYear getAlbumYear(int year) {
		synchronized (this) {
			int yearCount = _years.size();
			for (int n = 0; n < yearCount; n++) {
				AlbumYear albumYear = (AlbumYear) _years.elementAt(n);
				if (albumYear.year == year)
					return albumYear;
			}
			return null;
		}
	}
}
