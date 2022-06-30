using System;
using System.Threading;

namespace actchargers.Droid
{
    class WIFI_MODULE
	{
		public string deviceID;
		public bool isCharger;
		//public string myID;
		public byte requestCommand;
		public CommunicationResult responsetoTheLastcommand;
		public TimeoutLevel timeout;
		public byte[] resultData;
		public byte[] toSendData;
		public Mutex mutex;
		public int cc;
		public int disconnectCount;
		public int expectedSize = 0;
		public bool verifyExpectedSize = false;
		public DateTime lastCommunicationTime;

		DefineObjectInfo defineObj;
		public DefineObjectInfo getDefineObject()
		{
			return defineObj;

		}
		public WIFI_MODULE(string random, bool isthisMCB, DefineObjectInfo defineObj)
		{
			isCharger = isthisMCB;
			cc = 0;
			deviceID = random;
			requestCommand = 0;
			disconnectCount = 0;
			responsetoTheLastcommand = CommunicationResult.holdMobileMode;
			lastCommunicationTime = DateTime.UtcNow;
			this.defineObj = defineObj;
			mutex = new Mutex(false);
			//Random r = new Random();
			//myID = r.Next().ToString();
		}
		public override string ToString()
		{
			return deviceID;
		}
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			WIFI_MODULE objAsPart = obj as WIFI_MODULE;
			if (objAsPart == null) return false;
			else return Equals(objAsPart);
		}
		public override int GetHashCode()
		{
			return deviceID.GetHashCode();
		}
		public bool Equals(WIFI_MODULE other)
		{
			if (other == null) return false;
			return (this.deviceID.Equals(other.deviceID));
		}
	}
}
