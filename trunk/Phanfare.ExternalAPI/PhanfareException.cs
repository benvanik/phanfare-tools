using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phanfare.ExternalAPI
{
	public class PhanfareException : ApplicationException
	{
		public readonly int ErrorCode;
		public readonly string ErrorName;
		
		public PhanfareException( string message )
			: base( message )
		{
		}

		public PhanfareException( int errorCode, string errorName, string message )
			: base( message )
		{
			this.ErrorCode = errorCode;
			this.ErrorName = errorName;
		}
	}
}
