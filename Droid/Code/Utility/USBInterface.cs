using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Com.Ftdi.J2xx;

namespace actchargers.Droid
{
    public class USBInterface : IUSBInterface
    {
        D2xxManager ftdid2xx = D2xxManager.GetInstance(Application.Context);
        FT_Device ftDevice;
        int baudRate = 200000; //the data received from the microcontroller side
        bool lastCommandStatus;
        byte[] readData = new byte[CommProtocol.MAX_EXPECTED_PACKET_SIZE];
        object commLock = new object();
        readonly Dictionary<string, DateTime> blockTemp = new Dictionary<string, DateTime>();

        public void RequestSoftDisconnect(string serialNumber)
        {
            if (blockTemp.ContainsKey(serialNumber))
                blockTemp[serialNumber] = DateTime.UtcNow;
            else
                blockTemp.Add(serialNumber, DateTime.UtcNow);
        }


        #region PrepareFtdiDevice: opens the device with specific serial number

        FT_Device PerpareFtdiDevice(string SN, ref D2xxManager myFtdiDevice)
        {
            myFtdiDevice = D2xxManager.GetInstance(Application.Context);
            //FTDI.FT_STATUS ftStatus;
            UInt32 numBytesAvailable = 0;  //counts for the number of bytes available for receiving
            string discarderString = "";
            //Byte[] discarderString= new Byte[1000];
            uint discarderCounter = 0;
            ftDevice = myFtdiDevice.OpenBySerialNumber(Application.Context, SN);

            if (ftDevice.IsOpen == false)
            {
                ftDevice.Close();
                return ftDevice;
            }

            //Setting the Baud rate and checking the status if set or not.
            if (!ftDevice.SetBaudRate(baudRate))
            {
                ftDevice.Close();
                return ftDevice;
            }


            // Set data characteristics - Data bits, Stop bits, Parity
            if (!ftDevice.SetDataCharacteristics
                (D2xxManager.FtDataBits8, D2xxManager.FtStopBits1,
                 D2xxManager.FtParityNone))
            {
                ftDevice.Close();
                return ftDevice;
            }

            // Set flow control - set RTS/CTS flow control
            if (!ftDevice.SetFlowControl(D2xxManager.FtFlowNone, 0x11, 0x13))
            {
                ftDevice.Close();
                return ftDevice;
            }

            numBytesAvailable = (uint)ftDevice.QueueStatus;
            while (numBytesAvailable != 0)//Empty the data buffer
            {
                discarderCounter =
                    (uint)ftDevice.Read(Encoding.ASCII.GetBytes(discarderString),
                                        (int)numBytesAvailable);
                Thread.Sleep(1);
                numBytesAvailable = (uint)ftDevice.QueueStatus;
            }
            return ftDevice;
        }

        #endregion

        public byte[] GetDataReceived()
        {
            //depacketize the packet
            UInt16 length;


            if (lastCommandStatus)                                                                          //else, extract the data series from the packet
            {
                length = BitConverter.ToUInt16(readData, 0);
                byte[] CMDDATA = new byte[length + 2
                                          - CommProtocol.MIN_EXPECTED_PACKET_SIZE];
                Array.Copy(readData, 4, CMDDATA, 0, CMDDATA.Length);
                return CMDDATA; //return the extracted data
            }
            else return null;

        }

        public CommunicationResult SendReceive
        (byte cmd, byte[] data, string serialNumber, int expectedSize,
         bool verifyExpectedSize, ref byte[] resultArray, TimeoutLevel timeoutLevel)
        {
            lock (commLock)
            {
                serialNumber = serialNumber.Remove(0, 5);
                CommunicationResult res =
                    SendReceiveInternal
                    (cmd, data, serialNumber, expectedSize, verifyExpectedSize, timeoutLevel);
                if (res == CommunicationResult.RECEIVING_ERROR)
                    return
                        SendReceiveInternal
                        (cmd, data, serialNumber, expectedSize, verifyExpectedSize, timeoutLevel);

                resultArray = new byte[readData.Length];
                Array.Copy(readData, resultArray, readData.Length);

                return res;
            }
        }

        public CommunicationResult SendReceive
                           (byte cmd, byte[] data, string SN,
                            int expectedSize = 0, bool verifyExpectedSize = false,
                            TimeoutLevel t = TimeoutLevel.normal)
        {
            lock (commLock)
            {
                SN = SN.Remove(0, 5);
                CommunicationResult res =
                                SendReceiveInternal
                                (cmd, data, SN, expectedSize, verifyExpectedSize, t);
                if (res == CommunicationResult.RECEIVING_ERROR)
                    return SendReceiveInternal(cmd, data, SN, expectedSize, verifyExpectedSize, t);//try again
                return res;
            }
        }

        CommunicationResult SendReceiveInternal
                    (byte cmd, byte[] data, string SN, int expectedSize = 0,
                     bool verifyExpectedSize = false, TimeoutLevel t = TimeoutLevel.normal)
        {
            int numBytesAvailable = 0;  //counts for the number of bytes available for receiving
            int numBytesWritten = 0;
            int numBytesRead = 0;
            DateTime thisTime;
            int bytesCount;
            TimeSpan ts = new TimeSpan(1000000);
            switch (t)
            {
                case TimeoutLevel.extended:
                    ts = new TimeSpan(12000000);
                    break;
                case TimeoutLevel.normal:
                    ts = new TimeSpan(40000000);
                    break;
                case TimeoutLevel.shortTimeout:
                    ts = new TimeSpan(600000);
                    break;
            }


            byte[] Packet = new byte[CommProtocol.MAX_PACKET_SIZE];       //the whole packet to be sent
                                                                          /************************************************/
            lastCommandStatus = false;
            ftDevice = PerpareFtdiDevice(SN, ref ftdid2xx);
            if (!ftDevice.IsOpen)
            {
                ftDevice.Close();
                return CommunicationResult.FTDI_OPENING_ERROR;
            }
            int numberOgByteTosend = 0;
            numberOgByteTosend = CommProtocol.GetPacket(data, Packet, cmd);
            numBytesWritten = ftDevice.Write(Packet, numberOgByteTosend);
            if (numBytesWritten != numberOgByteTosend)
            {
                ftDevice.Close();
                return CommunicationResult.SENDING_ERROR;
            }
            thisTime = DateTime.UtcNow;
            int counter = 0;

            while (numBytesAvailable == 0 && DateTime.UtcNow.Subtract(thisTime) <= ts)// && 
            {
                Thread.Sleep(100);
                numBytesAvailable = ftDevice.QueueStatus;
                counter++;
            }
            if (numBytesAvailable == 0)
            {
                ftDevice.Close();
                return CommunicationResult.RECEIVING_ERROR;

            }
            numBytesRead = Task.Run(() =>
            {
                return ftDevice.Read(readData, 2);
            }
                                   ).Result;

            bytesCount = BitConverter.ToInt16(readData, 0);
            if (bytesCount < CommProtocol.MIN_EXPECTED_PACKET_SIZE - 2)
            {
                ftDevice.Close();
                return CommunicationResult.RECEIVING_ERROR;
            }

            int counter_bytes = 0;

            while (numBytesAvailable != bytesCount && DateTime.UtcNow.Subtract(thisTime) <= ts)
            {
                Thread.Sleep(100);//100 earlier
                                  //ftDevice.getQueueStatus(ref numBytesAvailable);
                numBytesAvailable = ftDevice.QueueStatus;
                counter_bytes++;
            }

            if (numBytesAvailable != bytesCount)
            {
                ftDevice.Close();
                return CommunicationResult.EXPECTED_DATA_COUNT_ERROR;
            }


            numBytesRead = Task.Run(() =>
            {
                return ftDevice.Read(readData, numBytesAvailable);
            }
                                   ).Result;

            Array.Copy(readData, 0, readData, 2, numBytesRead);

            Array.Copy(BitConverter.GetBytes((UInt16)bytesCount), readData, 2);
            ftDevice.Close();
            return CommProtocol.ValidateRecievdedPacket
                               (cmd, bytesCount, readData, ref lastCommandStatus,
                                expectedSize, verifyExpectedSize);
        }

        //#endregion

        public string[] GetDevicesSerialNumbers()
        {
            string[] Serials = new string[0];
            D2xxManager.FtDeviceInfoListNode[] ftdiDeviceListSN;
            int ftdiDeviceCount = ftdid2xx.CreateDeviceInfoList(Application.Context);
            if (ftdiDeviceCount == 0)
            {
                return Serials;
            }
            string[] tempSerials = new string[ftdiDeviceCount];
            ftdiDeviceListSN = new D2xxManager.FtDeviceInfoListNode[ftdiDeviceCount];
            for (int i = 0; i < ftdiDeviceCount; i++)
            {
                D2xxManager.FtDeviceInfoListNode deviceInfo = ftdid2xx.GetDeviceInfoListDetail(i);
                ftdiDeviceListSN[i] = deviceInfo;
                tempSerials[i] = deviceInfo.SerialNumber;
            }

            DateTime temp = DateTime.UtcNow;
            int passedSerials = 0;
            for (int i = 0; i < ftdiDeviceCount; i++)
            {
                if (tempSerials[i] == "")
                    continue;
                if (blockTemp.ContainsKey(ftdiDeviceListSN[i].SerialNumber) && (temp - blockTemp[ftdiDeviceListSN[i].SerialNumber]).TotalSeconds < 12)
                {
                    continue;
                }
                passedSerials++;
            }
            Serials = new string[passedSerials];
            int j = 0;
            for (int i = 0; i < ftdiDeviceCount; i++)
            {
                if (tempSerials[i] == "")
                    continue;
                if (blockTemp.ContainsKey(ftdiDeviceListSN[i].SerialNumber) && (temp - blockTemp[ftdiDeviceListSN[i].SerialNumber]).TotalSeconds < 12)
                {
                    continue;
                }
                Serials[j++] = tempSerials[i];

            }
            return Serials;//return the serials list    

        }
    }
}
