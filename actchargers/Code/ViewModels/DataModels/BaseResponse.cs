using System;
namespace actchargers
{
	public class BaseResponse
	{
		public bool failed { get; set; }
		public bool validSession { get; set; }
		public bool operationAuthenticated { get; set; }
		public object returnObject { get; set; }
		public bool expired { get; set; }
	}
}
