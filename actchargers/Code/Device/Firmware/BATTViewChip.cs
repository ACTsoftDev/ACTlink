using System;

namespace actchargers
{
    class BATTViewChip
    {
        public const int chipSize = 44032;
        public const int configLen = 32;
        public const int pageSize = 512;
        public const int bulkSize = 64;
        public const int serialSkipStart = 3072 / 2;//Remove bootloader bytes (4608),

        public static readonly UInt32[,] productID =
        {
            { 16252944, 0x10 },
            { 16252946, 0x3f },
            { 16252948, 0x56 },
            { 16252950, 0x45 }
        };

        public const int startCompareproductID = 7;
    }
}
