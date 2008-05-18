package com.phanfare.phanberry.ops;

import java.io.DataInputStream;
import java.io.IOException;

import javax.microedition.io.Connector;
import javax.microedition.io.file.FileConnection;

import com.phanfare.api.Album;
import com.phanfare.api.ImageInfo;
import com.phanfare.api.PhanfareException;
import com.phanfare.api.PhanfareService;
import com.phanfare.api.Section;
import com.phanfare.api.Session;
import com.phanfare.phanberry.cache.ObjectStore;

public class NewImageOperation extends BaseOperation {
	private boolean _toMobileAlbum;
	private Album _album;
	private Section _section;
	private ImageInfo _imageInfo;

	public NewImageOperation(ImageInfo imageInfo) {
		_toMobileAlbum = true;
		_imageInfo = imageInfo;
	}

	public NewImageOperation(Album album, Section section, ImageInfo imageInfo) {
		_album = album;
		_section = section;
		_imageInfo = imageInfo;
	}

	public boolean execute(ObjectStore store, PhanfareService service, Session session) throws PhanfareException {
		if (_toMobileAlbum == true) {
			_album = store.getMobileAlbum();
			_section = _album.sections[0];
		}

		FileConnection file;
		try {
			file = (FileConnection) Connector.open("file://" + _imageInfo.fileName);
		} catch (IOException ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
			return false;
		}
		DataInputStream fileStream = null;
		try {
			fileStream = file.openDataInputStream();
			ImageInfo result = service.newImage(session.userId, _album.albumId, _section.sectionId, _imageInfo,
					fileStream, file.fileSize());
			if (result == null) {
				return false;
			}
		} catch (IOException ex) {
			// TODO Auto-generated catch block
			ex.printStackTrace();
			return false;
		} finally {
			if (fileStream != null) {
				try {
					fileStream.close();
				} catch (IOException ex) {
				}
			}
		}

		return true;
	}
}
