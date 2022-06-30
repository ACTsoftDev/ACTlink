using System;
using System.Collections.Generic;

namespace actchargers
{
    class Hexencoder
    {
        public static bool Parser(DeviceBaseType dType, List<HexLineCode> dcodes, ref UInt32[] codeList, ref UInt32[,] configurationBits)
        {
            int chipSize = 0;
            int configLen = 0;
            UInt32[,] productID;
            int startCompareproductID = 0;
            if (dType == DeviceBaseType.MCB)
            {
                chipSize = McbChip.chipSize;
                configLen = McbChip.configLen;
                productID = McbChip.productID;
                startCompareproductID = McbChip.startCompareproductID;
            }
            else if (dType == DeviceBaseType.BATTVIEW)
            {
                chipSize = BATTViewChip.chipSize;
                configLen = BATTViewChip.configLen;
                productID = BATTViewChip.productID;
                startCompareproductID = BATTViewChip.startCompareproductID;

            }
            else if (dType == DeviceBaseType.CALIBRATOR)
            {
                chipSize = CalibratorChip.chipSize;
                configLen = CalibratorChip.configLen;
                productID = CalibratorChip.productID;
                startCompareproductID = CalibratorChip.startCompareproductID;
            }
            else
            {
                throw new Exception("NOT SUPPORTED");
            }


            codeList = new UInt32[chipSize];
            configurationBits = new UInt32[configLen, 2];
            for (int i = 0; i < chipSize; i++)
            {

                codeList[i] = 0x00FFFFFF;
            }
            UInt32 addrss = 0;
            bool ex = false;
            int configBitPtr = 0;
            foreach (HexLineCode code in dcodes)
            {

                switch (code.recordType)
                {
                    case 0:
                        if (ex)
                        {
                            //Data Record
                            switch (code.byteCount)
                            {

                                case 4:
                                    if ((addrss + code.offset) / 2 >= codeList.Length)
                                        return false;
                                    codeList[(addrss + code.offset) / 2] = BitConverter.ToUInt32(code.data, 0);
                                    break;
                                case 8:
                                    if ((addrss + code.offset) / 2 + 1 >= codeList.Length)
                                        return false;
                                    codeList[(addrss + code.offset) / 2 + 1] = BitConverter.ToUInt32(code.data, 4);
                                    codeList[(addrss + code.offset) / 2] = BitConverter.ToUInt32(code.data, 0);
                                    break;
                                case 12:
                                    if ((addrss + code.offset) / 2 + 2 >= codeList.Length)
                                        return false;
                                    codeList[(addrss + code.offset) / 2 + 2] = BitConverter.ToUInt32(code.data, 8);
                                    codeList[(addrss + code.offset) / 2 + 1] = BitConverter.ToUInt32(code.data, 4);
                                    codeList[(addrss + code.offset) / 2] = BitConverter.ToUInt32(code.data, 0);
                                    break;
                                case 16:
                                    if ((addrss + code.offset) / 2 + 3 >= codeList.Length)
                                        return false;
                                    codeList[(addrss + code.offset) / 2 + 3] = BitConverter.ToUInt32(code.data, 12);
                                    codeList[(addrss + code.offset) / 2 + 2] = BitConverter.ToUInt32(code.data, 8);
                                    codeList[(addrss + code.offset) / 2 + 1] = BitConverter.ToUInt32(code.data, 4);
                                    codeList[(addrss + code.offset) / 2] = BitConverter.ToUInt32(code.data, 0);
                                    break;
                                default:
                                    return false;


                            }
                        }
                        else
                        {
                            switch (code.byteCount)
                            {

                                case 4:
                                    if (configBitPtr >= configurationBits.Length)
                                        return false;
                                    configurationBits[configBitPtr, 0] = (addrss / 2) + code.offset;
                                    configurationBits[configBitPtr++, 1] = BitConverter.ToUInt32(code.data, 0);
                                    break;
                                case 8:
                                    if (configBitPtr + 1 >= configurationBits.Length)
                                        return false;
                                    configurationBits[configBitPtr, 0] = (addrss + code.offset) / 2 + 1;
                                    configurationBits[configBitPtr++, 1] = BitConverter.ToUInt32(code.data, 4);
                                    configurationBits[configBitPtr, 0] = (addrss + code.offset) / 2;
                                    configurationBits[configBitPtr++, 1] = BitConverter.ToUInt32(code.data, 0);
                                    break;
                                case 12:
                                    if (configBitPtr + 2 >= configurationBits.Length)
                                        return false;
                                    configurationBits[configBitPtr, 0] = (addrss + code.offset) / 2 + 2;
                                    configurationBits[configBitPtr++, 1] = BitConverter.ToUInt32(code.data, 8);
                                    configurationBits[configBitPtr, 0] = (addrss + code.offset) / 2 + 1;
                                    configurationBits[configBitPtr++, 1] = BitConverter.ToUInt32(code.data, 4);
                                    configurationBits[configBitPtr, 0] = (addrss + code.offset) / 2;
                                    configurationBits[configBitPtr++, 1] = BitConverter.ToUInt32(code.data, 0);
                                    break;
                                case 16:
                                    if (configBitPtr + 3 >= configurationBits.Length)
                                        return false;
                                    configurationBits[configBitPtr, 0] = (addrss + code.offset) / 2 + 3;
                                    configurationBits[configBitPtr++, 1] = BitConverter.ToUInt32(code.data, 12);
                                    configurationBits[configBitPtr, 0] = (addrss + code.offset) / 2 + 2;
                                    configurationBits[configBitPtr, 1] = BitConverter.ToUInt32(code.data, 8);
                                    configurationBits[configBitPtr, 0] = (addrss + code.offset) / 2 + 1;
                                    configurationBits[configBitPtr++, 1] = BitConverter.ToUInt32(code.data, 4);
                                    configurationBits[configBitPtr, 0] = (addrss + code.offset) / 2;
                                    configurationBits[configBitPtr++, 1] = BitConverter.ToUInt32(code.data, 0);
                                    break;
                                default:
                                    return false;


                            }
                        }

                        break;
                    case 1:

                        //End Of File Record
                        for (int i = 0; i < productID.GetLength(0); i++)
                        {
                            if (productID[i, 0] != configurationBits[i + startCompareproductID, 0] ||
                                productID[i, 1] != (0x00FF & configurationBits[i + startCompareproductID, 1]))
                            {
                                return false;
                            }
                        }
                        return true;
                    case 2:
                        return false;
                    //Extended Segment Address Record

                    case 3:
                        //Start Segment Address Record
                        return false;
                    case 4:
                        //Extended Linear Address Record,
                        if (code.byteCount != 2 || code.offset != 0)
                            return false;

                        byte[] address = new byte[2];
                        address[0] = code.data[1];
                        address[1] = code.data[0];
                        addrss = (UInt32)(BitConverter.ToInt16(address, 0));

                        addrss <<= 15;//Confirm this

                        if (addrss > chipSize * 2)
                        {
                            addrss <<= 1;
                            ex = false;
                        }
                        else
                        {

                            ex = true;
                        }
                        break;
                    case 5:
                        //Start Linear Address Record
                        return false;
                    default:
                        return false;

                }
            }
            return false;
        }
    }
}
