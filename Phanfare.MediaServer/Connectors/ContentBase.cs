using System;
using System.Collections.Generic;
using System.Text;

namespace Phanfare.MediaServer.Connectors
{
	abstract class ContentBase
	{
		private ContainerBase _parent;
		private string _id;

		public ContentBase( ContainerBase parent )
		{
			_parent = parent;
		}

		public ContainerBase Parent { get { return _parent; } }
		public string ID { get { return _id; } set { _id = value; } }

		public abstract string GetDescription();
	}
}
