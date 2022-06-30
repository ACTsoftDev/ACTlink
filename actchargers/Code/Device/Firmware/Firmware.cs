using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace actchargers
{
    public class Firmware
    {
        const float VERSION_FLOAT_MARGIN = 0.001f;

        public const float software_MCBFirmwareFileVersion = 3.11f;
        public const float software_BattFirmwareFileVersion = 2.74f;
        public const float software_BATT_ExtraFWFileVersion = 2.67f;
        public const float software_calibratorFirmwareVersion = 2.07f;
        public const float software_WiFiFirmwareVersion = 1.33f;
        public const float software_DcGctFirmwareVersion = 2.07f;
        public const float software_DcNnavitasFirmwareVersion = 2.06f;
        public const float software_DcElectrovayaFirmwareVersion = 2.10f;

        static float latestMCBDownloadedFirmwareVersion;
        static float latestBattDownloadedFirmwareVersion;

        static float MCB_UpdateVersion;
        static float Batt_UpdateVersion;

        public static float GetLatestBattViewFirmware()
        {
            return Batt_UpdateVersion;
        }

        public static float GetLatestMCBFirmware()
        {
            return MCB_UpdateVersion;
        }

        public static bool DoesBattViewRequireUpdate(DeviceObjectParent device)
        {
            device.RequireFirmwareUpdate = DoesBattViewRequireFirmwareUpdate(device.FirmwareRevision);
            device.RequireFirmwareWiFiUpdate = DeviceWiFiRequireUpdate(device.FirmwareWiFiVersion);

            return device.RequireFirmwareUpdate || device.RequireFirmwareWiFiUpdate;
        }

        public static bool DoesBattViewRequireUpdate(float firmwareRevision, float firmareWiFiVersion)
        {
            bool requireFirmwareUpdate = DoesBattViewRequireFirmwareUpdate(firmwareRevision);
            bool requireFirmwareWiFiUpdate = DeviceWiFiRequireUpdate(firmareWiFiVersion);

            return requireFirmwareUpdate || requireFirmwareWiFiUpdate;
        }

        public static bool DoesBattViewRequireFirmwareUpdate(float firmwareVersion)
        {
            if (firmwareVersion > 90 && Batt_UpdateVersion < 90)
                return true;

            if (Batt_UpdateVersion > 90 && firmwareVersion < 90)
                return true;

            return Batt_UpdateVersion - firmwareVersion > VERSION_FLOAT_MARGIN;
        }

        public static bool DoesMcbRequireUpdate(DeviceObjectParent device)
        {
            device.RequireFirmwareUpdate = DoesMcbRequireFirmwareUpdate(device.FirmwareRevision);
            device.RequireFirmwareWiFiUpdate = DeviceWiFiRequireUpdate(device.FirmwareWiFiVersion);
            device.RequireFirmwareDcUpdate = DoesDcRequireFirmwareUpdate(device.DcId, device.FirmwareDcVersion);

            return device.RequireFirmwareUpdate || device.RequireFirmwareWiFiUpdate || device.RequireFirmwareDcUpdate;
        }

        public static bool DoesMcbRequireUpdate(byte dcId, float firmwareRevision, float firmareWiFiVersion, float firmwareDcVersion)
        {
            bool requireFirmwareUpdate = DoesMcbRequireFirmwareUpdate(firmwareRevision);
            bool requireFirmwareWiFiUpdate = DeviceWiFiRequireUpdate(firmareWiFiVersion);
            bool requireFirmwareDcUpdate = DoesDcRequireFirmwareUpdate(dcId, firmwareDcVersion);

            return requireFirmwareUpdate || requireFirmwareWiFiUpdate || requireFirmwareDcUpdate;
        }

        public static bool DoesMcbRequireFirmwareUpdate(float firmwareRevision)
        {
            if (firmwareRevision > 90 && MCB_UpdateVersion < 90)
                return true;

            if (MCB_UpdateVersion > 90 && firmwareRevision < 90)
                return true;

            return MCB_UpdateVersion - firmwareRevision > VERSION_FLOAT_MARGIN;
        }

        public static bool DeviceWiFiRequireUpdate(float firmareWiFiVersion)
        {
            if (firmareWiFiVersion < 1.0f)
                return false;

            return software_WiFiFirmwareVersion - firmareWiFiVersion > VERSION_FLOAT_MARGIN;
        }

        public static bool DoesDcRequireFirmwareUpdate(byte dcId, float firmwareRevision)
        {
            if (firmwareRevision.Equals(0.0f))
                return false;

            float latestVersion = 0.0f;

            switch (dcId)
            {
                case 1:
                    latestVersion = software_DcGctFirmwareVersion;

                    break;

                case 2:
                    latestVersion = software_DcNnavitasFirmwareVersion;

                    break;

                case 3:
                    latestVersion = software_DcElectrovayaFirmwareVersion;

                    break;
            }

            return latestVersion - firmwareRevision > VERSION_FLOAT_MARGIN;
        }

        public static bool LoadLatestFirmWareIDFromDB()
        {
            MCB_UpdateVersion = software_MCBFirmwareFileVersion;
            Batt_UpdateVersion = software_BattFirmwareFileVersion;
            try
            {
                latestMCBDownloadedFirmwareVersion =
                    DbSingleton.DBManagerServiceInstance.GetFirmwareDownloadedLoader()
                               .GetMaxFirmwareVersion(true);

                if (software_MCBFirmwareFileVersion < latestMCBDownloadedFirmwareVersion)
                {
                    MCB_UpdateVersion = latestMCBDownloadedFirmwareVersion;
                }

                latestBattDownloadedFirmwareVersion =
                    DbSingleton.DBManagerServiceInstance.GetFirmwareDownloadedLoader()
                               .GetMaxFirmwareVersion(false);

                if (software_BattFirmwareFileVersion < latestBattDownloadedFirmwareVersion)
                {
                    Batt_UpdateVersion = latestBattDownloadedFirmwareVersion;
                }

            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X73" + "FIRMWARE INIT:" + ex.ToString());
                return false;
            }

            return true;
        }

        UInt32[] outputB;
        UInt32[,] config;

        internal bool CheckCode(string code)
        {
            foreach (char c in code)
            {
                switch (c)
                {
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '0':
                    case 'A':
                    case 'a':
                    case 'B':
                    case 'b':
                    case 'C':
                    case 'c':
                    case 'd':
                    case 'D':
                    case 'e':
                    case 'E':
                    case 'f':
                    case 'F':
                        continue;
                    default:
                        return false;

                }
            }
            return true;
        }

        FirmwareResult ReadDecodedFile(string fileContent, ref List<HexLineCode> dcodes)
        {
            char[] seprator = { ':' };
            char[] trimmer = { ' ', '\r', '\n', '\t' };
            string[] codes = fileContent.Split(seprator);

            string cleanCode = "";
            dcodes.Clear();
            foreach (string code in codes)
            {
                cleanCode = code.Trim(trimmer);
                if (cleanCode == "")
                    continue;
                if (!CheckCode(cleanCode))
                {

                    return FirmwareResult.badFileFormat;
                }
                HexLineCode dCode = new HexLineCode();
                if (!dCode.ParseCode(cleanCode))
                {
                    return FirmwareResult.badFileFormat;
                }

                dcodes.Add(dCode);
            }
            return FirmwareResult.fileOK;
        }

        FirmwareResult ParseHexFile(DeviceBaseType dType, string fileContent)
        {
            List<HexLineCode> dcodes = new List<HexLineCode>();

            outputB = null;
            config = null;

            FirmwareResult status = ReadDecodedFile(fileContent, ref dcodes);

            if (status != FirmwareResult.fileOK)
            {
                return status;
            }

            if (!Hexencoder.Parser(dType, dcodes, ref outputB, ref config))
            {
                return FirmwareResult.badFileEncode;
            }

            return FirmwareResult.fileOK;
        }

        public FirmwareResult GetPLCBinaries(ref byte[] serial)
        {
            //get latest MCB file from DB
            try
            {
                BinaryReader stream = null;
                //TODO Remove the String conversions while implementing Firmware
                if (MCB_UpdateVersion.ToString() == software_MCBFirmwareFileVersion.ToString())
                {
                    ////set the Firmware
                    var curAsm = typeof(StaticDataAndHelperFunctions).GetTypeInfo().Assembly;

                    //System.Reflection.Assembly curAsm = System.Reflection.Assembly.GetExecutingAssembly();
                    string embeddedResource = "actchargers.DefaultFirmwareFiles.img-passthru-xxr-evk2_opa564.bundle";
                    //BinaryReader rr = new BinaryReader(new FileStream(embeddedResource, FileMode.Open));
                    using (stream = new BinaryReader(curAsm.GetManifestResourceStream(embeddedResource)))
                    {
                        // Either the file is not existed or it is not mark as embedded resource
                        if (stream == null)
                            throw new Exception(embeddedResource + " is not found in Embedded Resources.");
                        //BinaryReader reader = new BinaryReader(stream);
                        const int bufferSize = 4096;
                        var ms = new MemoryStream();

                        byte[] buffer = new byte[bufferSize];

                        int count;

                        while ((count = stream.Read(buffer, 0, buffer.Length)) != 0)
                            ms.Write(buffer, 0, count);

                        serial = new byte[ms.Length];
                        Array.Copy(ms.ToArray(), serial, (int)ms.Length);
                        //stream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X74" + "PLC Load:" + ex.ToString());
                return FirmwareResult.badFileFormat; ;
            }
            return FirmwareResult.fileOK;
        }

        public static FirmwareResult GetDCBinaries(ref byte[] serial, int id)
        {
            try
            {
                BinaryReader stream = null;
                string embeddedResource;

                var curAsm = typeof(StaticDataAndHelperFunctions).GetTypeInfo().Assembly;

                switch (id)
                {
                    case 1:
                        embeddedResource = "actchargers.DefaultFirmwareFiles.DCFW_GCT.bin";
                        break;

                    case 2:
                        embeddedResource = "actchargers.DefaultFirmwareFiles.DCFW_Navitas.bin";
                        break;

                    case 3:
                        embeddedResource = "actchargers.DefaultFirmwareFiles.DCFW_Electrovaya.bin";
                        break;

                    default:
                        embeddedResource = "";
                        break;
                }

                using (stream = new BinaryReader(curAsm.GetManifestResourceStream(embeddedResource)))
                {
                    // Either the file is not existed or it is not mark as embedded resource
                    if (stream == null)
                        throw new Exception(embeddedResource + " is not found in Embedded Resources.");
                    //BinaryReader reader = new BinaryReader(stream);

                    const int bufferSize = 4096;
                    var ms = new MemoryStream();

                    byte[] buffer = new byte[bufferSize];
                    int count;
                    while ((count = stream.Read(buffer, 0, buffer.Length)) != 0)
                        ms.Write(buffer, 0, count);


                    serial = new byte[ms.Length];
                    Array.Copy(ms.ToArray(), serial, (int)ms.Length);
                    //stream.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X74" + "Daughter Card Load:" + ex);

                return FirmwareResult.badFileFormat;
            }

            return FirmwareResult.fileOK;
        }

        public FirmwareResult UpdateFileBinary(DeviceBaseType dType, ref byte[] serial)
        {
            if (dType == DeviceBaseType.MCB)
            {
                string MCB_hexFile = "";
                //get latest MCB file from DB
                try
                {
                    StreamReader stm = null;
                    if (MCB_UpdateVersion == software_MCBFirmwareFileVersion)
                    {
                        //set the Firmware
                        var curAsm = typeof(StaticDataAndHelperFunctions).GetTypeInfo().Assembly;
                        string embeddedResource = "actchargers.DefaultFirmwareFiles.MCB.X.production.hex";
                        using (stm = new StreamReader(curAsm.GetManifestResourceStream(embeddedResource)))
                        {
                            // Either the file is not existed or it is not mark as embedded resource
                            if (stm == null)
                                throw new Exception(embeddedResource + " is not found in Embedded Resources.");
                            // Get byte[] from the file from embedded resource
                            MCB_hexFile = stm.ReadToEnd();
                            stm.Dispose();
                        }
                    }
                    else
                    {
                        MCB_hexFile =
                            DbSingleton.DBManagerServiceInstance
                                       .GetFirmwareDownloadedLoader()
                                       .GetFirmwareByVersion
                                       (true, latestMCBDownloadedFirmwareVersion);
                    }
                }
                catch (Exception ex)
                {
                    Logger.AddLog(true, "X75" + "FIRMWARE Load:" + ex.ToString());
                    return FirmwareResult.badFileFormat; ;
                }
                return DoGetHexFileBinary(dType, MCB_hexFile, ref serial);
            }
            else if (dType == DeviceBaseType.CALIBRATOR)
            {
                string calibrator_hexFile = "";
                //get latest MCB file from DB
                try
                {
                    StreamReader stm = null;

                    //set the Firmware
                    var curAsm = typeof(StaticDataAndHelperFunctions).GetTypeInfo().Assembly;
                    string embeddedResource = "actchargers.DefaultFirmwareFiles.calibrator.X.production.hex";
                    using (stm = new StreamReader(curAsm.GetManifestResourceStream(embeddedResource)))
                    {
                        // Either the file is not existed or it is not mark as embedded resource
                        if (stm == null)
                            throw new Exception(embeddedResource + " is not found in Embedded Resources.");
                        // Get byte[] from the file from embedded resource
                        calibrator_hexFile = stm.ReadToEnd();
                        stm.Dispose();
                    }

                }
                catch (Exception ex)
                {
                    Logger.AddLog(true, "X75" + "FIRMWARE Load:" + ex.ToString());
                    return FirmwareResult.badFileFormat; ;
                }
                return DoGetHexFileBinary(dType, calibrator_hexFile, ref serial);
            }
            else
            {
                string Batt_hexFile = "";
                //get latest MCB file from DB
                try
                {
                    StreamReader stm = null;
                    if (Batt_UpdateVersion == software_BattFirmwareFileVersion)
                    {
                        //set the Firmware
                        var curAsm = typeof(StaticDataAndHelperFunctions).GetTypeInfo().Assembly;
                        string embeddedResource = "actchargers.DefaultFirmwareFiles.battview.X.production.hex";
                        using (stm = new StreamReader(curAsm.GetManifestResourceStream(embeddedResource)))
                        {
                            // Either the file is not existed or it is not mark as embedded resource
                            if (stm == null)
                                throw new Exception(embeddedResource + " is not found in Embedded Resources.");
                            // Get byte[] from the file from embedded resource
                            Batt_hexFile = stm.ReadToEnd();
                            stm.Dispose();
                        }
                    }
                    else
                    {
                        Batt_hexFile =
                            DbSingleton.DBManagerServiceInstance
                                       .GetFirmwareDownloadedLoader()
                                       .GetFirmwareByVersion
                                       (false, latestMCBDownloadedFirmwareVersion);
                    }
                }
                catch (Exception ex)
                {
                    Logger.AddLog(true, "X76" + "FIRMWARE Load:" + ex.ToString());
                    return FirmwareResult.badFileFormat; ;
                }
                return DoGetHexFileBinary(dType, Batt_hexFile, ref serial);
            }
        }

        public byte[] UpdateWiFiFileBinary()
        {
            byte[] serial = null;

            string embeddedResource = "actchargers.DefaultFirmwareFiles.wifi.bin";

            try
            {
                var curAsm = typeof(StaticDataAndHelperFunctions).GetTypeInfo().Assembly;

                using (var streamReader = new StreamReader(curAsm.GetManifestResourceStream(embeddedResource)))
                {
                    if (streamReader == null)
                        throw new Exception(embeddedResource + " is not found in Embedded Resources.");

                    using (var memoryStream = new MemoryStream())
                    {
                        streamReader.BaseStream.CopyTo(memoryStream);

                        serial = memoryStream.ToArray();

                        memoryStream.Dispose();
                    }
                    streamReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X75" + "FIRMWARE Load:" + ex);
            }

            return serial;
        }

        FirmwareResult DoGetHexFileBinary(DeviceBaseType dType, string fileContent, ref byte[] serial)
        {
            FirmwareResult status = FirmwareResult.badFileFormat;
            try
            {
                status = ParseHexFile(dType, fileContent);
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X77" + ex.ToString());
            }
            if (status != FirmwareResult.fileOK)
                return status;
            int serialSkipStart = 0;

            if (dType == DeviceBaseType.MCB)
                serialSkipStart = McbChip.serialSkipStart;
            else if (dType == DeviceBaseType.BATTVIEW)
                serialSkipStart = BATTViewChip.serialSkipStart;
            else if (dType == DeviceBaseType.CALIBRATOR)
                serialSkipStart = CalibratorChip.serialSkipStart;


            serial = new byte[(outputB.Length - serialSkipStart) * 3];
            for (int i = serialSkipStart; i < outputB.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(outputB[i]), 0, serial, (i - serialSkipStart) * 3, 3);
            }
            return FirmwareResult.fileOK;
        }

        public FirmwareResult GetHexFileBinary(DeviceBaseType dType, string hexFilePath, ref byte[] serial, byte[] contentBytes)
        {
            String fileContent = "";
            TextReader reader = null;
            if (hexFilePath == "")
            {
                return FirmwareResult.fileNotFound;
            }
            try
            {
                using (var stream = new MemoryStream(contentBytes))
                {
                    using (reader = new StreamReader(stream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
                reader.Dispose();
            }
            catch (Exception ex)
            {
                if (reader != null)
                    reader.Dispose();
                Logger.AddLog(true, "X78" + ex.ToString());
                return FirmwareResult.noAcessFile;
            }
            return DoGetHexFileBinary(dType, fileContent, ref serial);
        }

    }
}
