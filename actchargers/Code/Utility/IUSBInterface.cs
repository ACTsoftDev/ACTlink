namespace actchargers
{
	public interface IUSBInterface
	{
        void RequestSoftDisconnect(string serialNumber);

		string[] GetDevicesSerialNumbers();

		CommunicationResult SendReceive
        (byte cmd, byte[] data, string serialNumber, int expectedSize,
         bool verifyExpectedSize, ref byte[] resultArray, TimeoutLevel timeoutLevel);
	}
}
