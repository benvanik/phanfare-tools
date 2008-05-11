using System;
using System.Collections.Generic;
using System.Text;

namespace Phanfare.MediaServer.Connectors
{
	abstract class ContainerBase
	{
		private IContentSystem _system;
		private ContainerBase _parent;
		private string _id;
		private Dictionary<string, ContainerBase> _childContainers;
		private Dictionary<string, ContentBase> _childContent;

		public ContainerBase( ContainerBase parent )
		{
			if( parent == null )
				_system = ( IContentSystem )this;
			else
			{
				_parent = parent;
				_system = parent.System;
			}
			_childContainers = new Dictionary<string, ContainerBase>();
			_childContent = new Dictionary<string, ContentBase>();
		}

		public IContentSystem System { get { return _system; } }
		public ContainerBase Parent { get { return _parent; } }
		public string ID { get { return _id; } set { _id = value; } }
		public string BaseURL { get { return _system.BaseURL; } }

		public abstract string Name { get; }
		public virtual bool FlattenSingleChild { get { return false; } }

		protected string AddTemporaryChild( ContainerBase child )
		{
			string id = this.ID + "/" + Environment.TickCount.ToString();
			child.ID = id;
			_system.RegisterContainer( id, child );
			return id;
		}

		protected string AddChild( string uniqueId, ContainerBase child )
		{
			string id = this.ID + "/" + uniqueId;
			_childContainers[ id ] = child;
			child.ID = id;
			_system.RegisterContainer( id, child );
			return id;
		}

		protected string AddChild( string uniqueId, ContentBase child )
		{
			string id = this.ID + "_" + uniqueId;
			_childContent[ id ] = child;
			child.ID = id;
			_system.RegisterContent( id, child );
			return id;
		}

		protected void ClearChildren()
		{
			_childContainers.Clear();
			_childContent.Clear();
		}

		public ContainerBase this[ string id ]
		{
			get
			{
				return _childContainers[ id ];
			}
		}

		public EnumerationResult Enumerate( uint startingIndex, uint requestedCount )
		{
			if( _system.EnsurePopulated( this ) == false )
				return null;

			// Hack for albums with 1 section
			if( ( this.FlattenSingleChild == true ) &&
				( _childContainers.Count == 1 ) &&
				( _childContent.Count == 0 ) )
			{
				foreach( ContainerBase child in _childContainers.Values )
					return child.Enumerate( startingIndex, requestedCount );
			}

			StringBuilder sb = new StringBuilder();
			sb.Append( Didl.Begin() );

			uint returnCount = 0;
			uint totalCount = 0;
			uint index = 0;

			foreach( ContainerBase container in _childContainers.Values )
			{
				if( index >= startingIndex )
				{
					if( returnCount < requestedCount )
					{
						sb.Append( Didl.GetContainer( container.ID, this.ID, true, true, container.Name, "object.container.storageFolder", container.GetChildCount() ) );
						returnCount++;
					}
				}
				index++;
				totalCount++;
			}

			foreach( ContentBase content in _childContent.Values )
			{
				if( index >= startingIndex )
				{
					if( returnCount < requestedCount )
					{
						sb.Append( content.GetDescription() );
						returnCount++;
					}
				}
				index++;
				totalCount++;
			}

			sb.Append( Didl.End() );

			return new EnumerationResult( sb.ToString(), startingIndex, returnCount, totalCount );
		}

		public EnumerationResult RandomContent( uint startingIndex, uint requestedCount )
		{
			StringBuilder sb = new StringBuilder();
			sb.Append( Didl.Begin() );

			RandomContentResult result = this.OnRandomContentRequested( startingIndex, requestedCount );

			foreach( ContentBase content in result.Content )
				sb.Append( content.GetDescription() );

			sb.Append( Didl.End() );

			return new EnumerationResult( sb.ToString(), result.StartingIndex, result.ReturnCount, result.TotalCount );
		}

		protected virtual RandomContentResult OnRandomContentRequested( uint startingIndex, uint requestCount )
		{
			return new RandomContentResult( new List<ContentBase>(), startingIndex, 0, 0 );
		}

		public virtual int GetChildCount()
		{
			return _childContainers.Count + _childContent.Count;
		}
	}

	class EnumerationResult
	{
		public readonly string Result;
		public readonly uint StartingIndex;
		public readonly uint ReturnCount;
		public readonly uint TotalCount;
		public EnumerationResult( string result, uint startingIndex, uint returnCount, uint totalCount )
		{
			this.Result = result;
			this.StartingIndex = startingIndex;
			this.ReturnCount = returnCount;
			this.TotalCount = totalCount;
		}
	}

	class RandomContentResult
	{
		public readonly List<ContentBase> Content;
		public readonly uint StartingIndex;
		public readonly uint ReturnCount;
		public readonly uint TotalCount;
		public RandomContentResult( List<ContentBase> content, uint startingIndex, uint returnCount, uint totalCount )
		{
			this.Content = content;
			this.StartingIndex = startingIndex;
			this.ReturnCount = returnCount;
			this.TotalCount = totalCount;
		}
	}
}
