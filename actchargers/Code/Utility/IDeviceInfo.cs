using System;
namespace actchargers
{
	public interface IDeviceInfo
	{
		string ModelNumber { get; }

		string SerialNumber { get; }

		string DeviceId { get; }
	}
}
