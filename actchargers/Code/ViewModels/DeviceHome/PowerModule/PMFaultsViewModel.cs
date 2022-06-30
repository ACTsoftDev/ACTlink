using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace actchargers
{
    public class PMFaultsViewModel : BaseViewModel
    {
        public string PMFaults_Sequence { get; set; }
        public string PMFaults_ID { get; set; }
        public string PMFaults_Date { get; set; }
        public string PMFaults_PowerModule_ID { get; set; }
        public string PMFaults_Valid { get; set; }

        ObservableCollection<PMFaultsModel> _ListItemSource;
        public ObservableCollection<PMFaultsModel> ListItemSource
        {
            get { return _ListItemSource; }
            set
            {
                _ListItemSource = value;
                RaisePropertyChanged(() => ListItemSource);
            }
        }

        public PMFaultsViewModel()
        {
            if (ControlObject.UserAccess.MCB_PM_canReadFaults == AccessLevelConsts.noAccess)
            {
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<PMFaultsViewModel>(new { pop = "pop" }); });
                return;
            }

            PMFaults_Sequence = AppResources.pmfaults_sequence;
            PMFaults_ID = AppResources.pmfaults_id;
            PMFaults_Date = AppResources.pmfaults_date;
            PMFaults_PowerModule_ID = AppResources.pmfaults_powermodule_id;
            PMFaults_Valid = AppResources.pmfaults_valid;

            ViewTitle = AppResources.pm_faults;
            ListItemSource = new ObservableCollection<PMFaultsModel>();

            UInt32 startRecord = 0;


            startRecord = MCBQuantum.Instance.GetMCB().globalRecord.PMfaults > 200 ? MCBQuantum.Instance.GetMCB().globalRecord.PMfaults - 200 : 0;

            if (startRecord < MCBQuantum.Instance.GetMCB().minPMFaultRecordID)
            {
                startRecord = MCBQuantum.Instance.GetMCB().minPMFaultRecordID;
            }
            List<object> genericlist = new List<object>();
            genericlist.Add(startRecord);

            try
            {


                Task.Run(async () =>
                {
                    ACUserDialogs.ShowProgress();
                    List<object> results = await MCB_PowerModulesFaultsHistoryReadButton_Click_doWork(genericlist);

                    if (results != null && results.Count > 0)
                    {


                        bool internalFailure = (bool)results[0];
                        string internalFailureString = (string)results[1];
                        CommunicationResult status = (CommunicationResult)results[2];

                        if (internalFailure)
                        {
                            Logger.AddLog(true, "X88" + internalFailureString);
                            //if (!setSepcialStatus(StatusSpecialMessage.ERROR))
                            //{
                            //    setFormBusy(false);
                            //}
                            return;
                        }

                        if (status != CommunicationResult.OK)
                        {
                            //if (!setStatus(status))

                            //    setFormBusy(false);
                            return;
                        }

                        List<PMfault> tt = MCBQuantum.Instance.GetMCB().getPowerModeulesFaultsArrayList();

                        foreach (PMfault r in tt)
                        {
                            PMFaultsModel pmfaultsModel = new PMFaultsModel();
                            bool isWifiDebug = false;
                            if ((r.PMmacAddress & ~(0x40000000 | 0x10000000)) == 0 && ControlObject.isDebugMaster)
                            {
                                isWifiDebug = true;
                            }
                            if (!isWifiDebug)
                            {
                                pmfaultsModel.faultID = r.faultID;
                                pmfaultsModel.isValidCRC7 = r.isValidCRC7;
                                pmfaultsModel.debugHeader = MCBobject.AddressToSerial(r.PMmacAddress);
                                pmfaultsModel.faultTime = r.faultTime;
                                pmfaultsModel.DebugString = PMfault.getFaultString(r.fault) + " " + (r.isError ? "ERROR" : "WARNING");
                            }
                            else
                            {
                                string debugHeader = "WiFi Debug";
                                string _debugString = "";
                                switch (r.fault)
                                {
                                    case 10: _debugString = "Module Restart: " + ((byte)r.isErrorRaw).ToString("X"); break;
                                    case 11: _debugString = "RESTART Issye @step  " + r.isErrorRaw.ToString(); break;
                                    case 12: _debugString = "SSID CONNECT TO  " + r.isErrorRaw.ToString(); break;
                                    case 13: _debugString = "Request Disconnect to : " + r.isErrorRaw.ToString(); break;
                                    case 14: _debugString = "CIP CLOSE WITH  " + r.isErrorRaw.ToString(); break;
                                    case 15: _debugString = "CIP START WITH : " + r.isErrorRaw.ToString(); break;
                                    case 16: _debugString = "SSID DISOCNNECTED  : " + r.isErrorRaw.ToString(); break;
                                    case 17: _debugString = "Abnormal disconnect : " + r.isErrorRaw.ToString(); break;
                                    case 18: _debugString = "CONNECTED TO SSID : " + r.isErrorRaw.ToString(); break;
                                    case 20: _debugString = "LCD Stop button (SIM MODE) " + r.isErrorRaw.ToString(); debugHeader = "Stop charger debug"; break;
                                    case 21: _debugString = "Stop from Quick View (actview or software) " + r.isErrorRaw.ToString(); debugHeader = "Stop charger debug"; break;
                                    case 22: _debugString = "LCD Stop button  : " + r.isErrorRaw.ToString(); debugHeader = "Stop charger debug"; break;
                                    case 23: _debugString = "push button stop request: " + r.isErrorRaw.ToString(); debugHeader = "Stop charger debug"; break;
                                    case 24: _debugString = "stop from software (debug mode) " + r.isErrorRaw.ToString(); debugHeader = "Stop charger debug"; break;
                                }
                                pmfaultsModel.faultID = r.faultID;
                                pmfaultsModel.isValidCRC7 = r.isValidCRC7;
                                pmfaultsModel.debugHeader = debugHeader;
                                pmfaultsModel.faultTime = r.faultTime;
                                pmfaultsModel.DebugString = _debugString == "" ? r.fault.ToString() : _debugString;

                            }
                            InvokeOnMainThread(() =>
                                     {

                                         ListItemSource.Add(pmfaultsModel);
                                     });

                        }

                    }

                    InvokeOnMainThread(()=> {
                        RaisePropertyChanged(() => ListItemSource);
                    });
                    ACUserDialogs.HideProgress();
                });
            }
            catch (Exception ex)
            {
                ACUserDialogs.HideProgress();

            }


        }

        async Task<List<object>> MCB_PowerModulesFaultsHistoryReadButton_Click_doWork(List<object> parameters)
        {
            List<object> results = new List<object>();
            CommunicationResult status = CommunicationResult.NOT_EXIST;
            bool internalFailure = false;
            string internalFailureString = "";
            List<object> genericlist = parameters;
            UInt32 startRecord = (UInt32)genericlist[0];

            try
            {
                status = await MCBQuantum.Instance.GetMCB().getPMErrorLog(startRecord);
            }
            catch (Exception ex)
            {
                internalFailure = true;
                internalFailureString = ex.ToString();
            }

            results.Add(internalFailure);
            results.Add(internalFailureString);
            results.Add(status);
            return results;
        }

        /// <summary>
        /// Ons the back button click.
        /// </summary>
        public void OnBackButtonClick()
        {
            ShowViewModel<PMFaultsViewModel>(new { pop = "pop" });
        }
    }
}
