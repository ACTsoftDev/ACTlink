using System;

namespace actchargers
{
    public class LcdRequest
    {
        public UInt16 desulfateCapacity;
        public UInt32 desulfateLength;
        public byte chargeScreenStop;
        public byte chargeScreenStart;
        public byte chargeScreenReturn;
        public byte chargeScreenResume;
        public byte chargeScreenExit;
        public byte EQRequest;
        public byte desulfateRequest;
        public byte desulfateVoltage;

        public LcdRequest()
        {
            desulfateCapacity = 0;
            desulfateLength = 0;
            chargeScreenStart = 0;
            chargeScreenStop = 0;
            chargeScreenReturn = 0;
            chargeScreenResume = 0;
            chargeScreenExit = 0;
            EQRequest = 0;
            desulfateRequest = 0;
            desulfateVoltage = 0;
        }

        internal byte[] getArray()
        {
            byte[] a = new byte[32];
            int loc = 0;
            Array.Copy(BitConverter.GetBytes(desulfateLength), 0, a, loc, 2);
            loc += 4;
            Array.Copy(BitConverter.GetBytes(desulfateCapacity), 0, a, loc, 2);
            loc += 2;

            a[loc++] = chargeScreenStart;
            a[loc++] = chargeScreenStop;
            a[loc++] = chargeScreenReturn;
            a[loc++] = chargeScreenResume;
            a[loc++] = chargeScreenExit;
            a[loc++] = EQRequest;
            a[loc++] = desulfateRequest;
            a[loc++] = desulfateVoltage;
            for (int i = loc; i < 32; i++)
            {
                a[loc++] = 0;
            }

            return a;
        }
    }
}
