using System;

namespace actchargers
{
    class HexLineCode
    {
        public UInt32 byteCount;
        public UInt32 offset;
        public byte recordType;
        public byte[] data;

        public bool ParseCode(string code)
        {
            UInt32 actuallCheckSum = 0;
            UInt32 addressL;
            UInt32 addressH;
            int checkSum;

            byteCount = Convert.ToUInt32(code.Substring(0, 2), 16);
            addressL = Convert.ToUInt32(code.Substring(2, 2), 16);
            addressH = Convert.ToUInt32(code.Substring(4, 2), 16);
            offset = Convert.ToUInt32(code.Substring(2, 4), 16);
            recordType = Convert.ToByte(code.Substring(6, 2), 16);
            if (code.Length - 10 != 2 * byteCount)
                return false;
            int i = 0;
            data = new byte[byteCount];
            for (i = 0; i < byteCount; i++)
            {
                data[i] = Convert.ToByte(code.Substring(8 + 2 * i, 2), 16);
                actuallCheckSum += data[i];
            }
            checkSum = Convert.ToByte(code.Substring(8 + 2 * i, 2), 16);

            actuallCheckSum += addressL;
            actuallCheckSum += addressH;
            actuallCheckSum += recordType;
            actuallCheckSum += byteCount;
            if ((byte)(~((byte)(actuallCheckSum & 0x000000FF)) + 1) != checkSum)
                return false;
            offset /= 2;
            return true;
        }
    }
}
