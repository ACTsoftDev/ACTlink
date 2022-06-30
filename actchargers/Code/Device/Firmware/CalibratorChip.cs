using System;

namespace actchargers
{
    class CalibratorChip
    {
        public const int chipSize = 87552;
        public const int configLen = 11;
        public const int pageSize = 512;
        public const int bulkSize = 64;
        public const int serialSkipStart = 3072 / 2;//Remove bootloader bytes (4608), the EEprom size is 256k, and we need 256.5k ..i start from 0xC00 = 3072
        public static readonly UInt32[,] productID = { { 16252944, 0x57 }, { 16252946, 0x89 }, { 16252948, 0x24 }, { 16252950, 0xA5 } };
        public const int startCompareproductID = 7;
    }
}
