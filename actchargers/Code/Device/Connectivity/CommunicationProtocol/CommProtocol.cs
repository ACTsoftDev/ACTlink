using System;

namespace actchargers
{
    public static class CommProtocol
    {
        public const byte defineCommand = 0x03;

        public const byte chargerDefineKey = 0xE3;
        public const byte battViewDefineKey = 0xF1;
        public const byte CalibratorDefineKey = 0x1D;
        public const byte BlackBoxConvterInterfaceDefineKey = 0x3B;

        public const byte senderKey = 0x25;                            //The key for the sender( the PC side )
        public const byte receiverKey = 0x50;                          //The key for the receiver( the microcontroller side )
        public const byte ack = 0x33;                                  //Ack byte
        public const byte ackDelayed = 0x44;                                  //Ack delayed byte
        public const byte nack = 0x55;                                 //Nack byte
        public const byte nackBusy = 0x66;                                 //Nack busy (charger running) byte
        public const uint MAX_PACKET_SIZE = 8704;                        //Max size for the packet to be sent
        public const uint MAX_EXPECTED_PACKET_SIZE = 8704;               //Max expected number of bytes for the packet
        public const uint MIN_EXPECTED_PACKET_SIZE = 7;                //Min expected number of bytes for the packet
        public enum Communication_Result : uint     //represents the type of communication result when doing the COMM. with the UART module
        {
            FTDI_OPENING_ERROR = 0,                   //Error: error in opening the ftdi device
            SENDING_ERROR = 1,                        //Error: error in sending the series of bytes representing the packet to the microcontroller
            EXPECTED_DATA_COUNT_ERROR = 2,            //Error: there must be an expected min and max count of data bytes to be received from the microcontroller, so if it was out the ranges it will return this error 
            RECEIVING_ERROR = 3,                      //Error: error in receiving the series of bytes representing the packet to be received from the microcontroller
            PACKET_SIZE_ERROR = 4,                    //Error: generated when the packet size does not equate to the first byte of the packet plus one
            CRC_ERROR = 5,                            //Error: when computing the crc for the received packet and the result is not zero, then this error is generated 
            RECEIVER_KEY_ERROR = 6,                   //Error: generated when there is an error in the ID for the reponding microcontroller
            ACK_NACK_ERROR = 7,                       //Error: generated when the ack nack byte does not meet the ack or nack property
            OK = 8,                                    // this result represents that all communication process was well
            ERROR_IN_EEPROM_READING = 9,
            ERROR_IN_EEPROM_WRITING = 10,
            ACCESS_ERROR = 11,
            SIZEERROR = 12,
            ReadSomethingElse = 13,
            CHARGER_BUSY = 14,
            COMMAND_DELAYED = 15,
            NOT_EXIST = 16,
            invalidArg = 17,
            internalFailure = 18,
            mutexKilled = 19,
            holdMobileMode = 20,
            STOPPED = 21,
        }


        #region CRC: calculates the CRC for packets sent and received
        public static ushort CRCCalculation(byte[] str, int dSize)
        {
            ushort CRC = 0x0000;
            int dataSize = 0;
            int forCounter;
            while (dSize > dataSize)
            {
                CRC ^= (ushort)(((ushort)(str[dataSize])) << 8);
                for (forCounter = 0; forCounter < 8; forCounter++)
                {
                    if ((CRC & 0x8000) != 0)
                        CRC = (ushort)((CRC << 1) ^ (0x1021));
                    else
                        CRC <<= 1;
                }
                dataSize++;
            }
            return CRC;
        }
        public static CommunicationResult ValidateRecievdedPacket(byte hexCommand, int bytesCount, byte[] readData, ref bool lastCommandStatus, int expectedIncomingSize = 0, bool verifySize = false)
        {
            try
            {
                lastCommandStatus = false;
                if (CRCCalculation(readData, (int)(bytesCount + 2)) != 0)
                    return CommunicationResult.CRC_ERROR;                                                                //5: crc error
                else if (readData[3] != hexCommand)
                    return CommunicationResult.RECEIVER_KEY_ERROR;
                else if (readData[2] != receiverKey)                                                                                   //receiver key 
                    return CommunicationResult.RECEIVER_KEY_ERROR;                                                       //6: key error
                else if (readData[bytesCount + 2 - 3] != ack && readData[bytesCount + 2 - 3] != nack && readData[bytesCount + 2 - 3] != ackDelayed && readData[bytesCount + 2 - 3] != nackBusy)                                 //one of both, ack or nack
                    return CommunicationResult.ACK_NACK_ERROR;
                else if (verifySize)
                {
                    int realBytesCount = bytesCount - 5;

                    if (hexCommand == defineCommand)
                    {
                        if (expectedIncomingSize != realBytesCount
                            && (expectedIncomingSize * 2) != realBytesCount)
                            return CommunicationResult.SIZEERROR;
                    }
                    else
                    {
                        if (expectedIncomingSize != realBytesCount)
                            return CommunicationResult.SIZEERROR;
                    }
                }

                lastCommandStatus = true;
                if (readData[bytesCount + 2 - 3] == nack)
                    return CommunicationResult.ACCESS_ERROR;
                else if (readData[bytesCount + 2 - 3] == ackDelayed)
                    return CommunicationResult.COMMAND_DELAYED;

                else if (readData[bytesCount + 2 - 3] == nackBusy)
                    return CommunicationResult.CHARGER_BUSY;
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X39" + ex);
                return CommunicationResult.NOT_EXIST;
            }

            return CommunicationResult.OK;
        }
        #endregion
        #region Packetize: formulates the packet
        public static int GetPacket(byte[] data, byte[] Packet, byte command)
        {
            if (data != null)
            {
                Packetize(data, data.Length, Packet, command);
                return (int)MIN_EXPECTED_PACKET_SIZE + data.Length;
            }
            else
            {
                Packetize(data, 0, Packet, command);
                return (int)MIN_EXPECTED_PACKET_SIZE;

            }
        }
        static void Packetize(byte[] data, int dataSize, byte[] Packet, byte command)
        {
            Int16 length = (Int16)(MIN_EXPECTED_PACKET_SIZE - 2 + dataSize);
            Array.Copy(BitConverter.GetBytes(length), Packet, 2);

            //Packet[0] = (byte)(MIN_EXPECTED_PACKET_SIZE - firstByteSubtractor + dataSize);
            Packet[2] = senderKey;
            Packet[3] = command;


            if (dataSize != 0)
                Array.Copy(data, 0, Packet, 4, dataSize);



            Packet[4 + dataSize] = ack;
            ushort CRC = CRCCalculation(Packet, 5 + dataSize);


            Packet[6 + dataSize] = (byte)CRC;
            Packet[5 + dataSize] = (byte)(CRC >> 8);
        }

        #endregion
    }
}
