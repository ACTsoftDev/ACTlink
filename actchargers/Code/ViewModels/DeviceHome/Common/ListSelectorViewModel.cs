using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;

namespace actchargers
{
    public class ListSelectorViewModel : BaseViewModel
    {
        public int ParentItemIndex { get; set; }

        int SelectedItemIndex { get; set; }

        int SelectedChildPostion { get; set; }

        int SelectedIndex { get; set; }

        public List<string> ListItemSource { get; set; }

        public List<string> Batt_Tr_Amps { get; set; }
        public List<string> Batt_Cc_Amps { get; set; }
        public List<string> Batt_Fi_Amps { get; set; }
        public List<string> Batt_Eq_Amps { get; set; }

        public DefaultChargeProfileViewModel CPViewmodel = new DefaultChargeProfileViewModel();

        public ListSelectorViewModel()
        {
            ListItemSource = new List<string>();
        }

        void SetViewTitle(string title)
        {
            ViewTitle = AppResources.select + " " + title;
        }

        public void Init
        (ACUtility.ListSelectorType type, int selectedItemIndex,
         string ItemSourceStr, int selectedChildPosition, string keepSubvalue,
         string title)
        {
            InitValues
            (type, 0, selectedItemIndex, ItemSourceStr,
             selectedChildPosition, keepSubvalue, title);
        }

        public void Init
        (ACUtility.ListSelectorType type, int parentItemIndex, int selectedItemIndex,
         string ItemSourceStr, int selectedChildPosition, string keepSubvalue,
         string title)
        {
            InitValues
            (type, parentItemIndex, selectedItemIndex, ItemSourceStr,
             selectedChildPosition, keepSubvalue, title);
        }

        void InitValues
       (ACUtility.ListSelectorType type, int parentItemIndex, int selectedItemIndex,
        string ItemSourceStr, int selectedChildPosition, string keepSubvalue,
        string title)
        {
            ParentItemIndex = parentItemIndex;
            SelectedItemIndex = selectedItemIndex;
            SelectedChildPostion = selectedChildPosition;

            switch (type)
            {
                case ACUtility.ListSelectorType.TemperatureFormate:
                    ListItemSource = ACUtility.Instance.TemperatureUnits;
                    SetViewTitle(AppResources.temperature_format);
                    break;
                case ACUtility.ListSelectorType.TemperatureFallbackControl:
                    SetViewTitle(AppResources.temperature_fallback_control);
                    ListItemSource = ACUtility.Instance.TemperatureFallbackControllValues;
                    break;
                case ACUtility.ListSelectorType.ApplicationType:
                    ListItemSource = JsonConvert.DeserializeObject<List<string>>(ItemSourceStr);
                    SetViewTitle(AppResources.application_type);
                    break;
                case ACUtility.ListSelectorType.BatteryType:
                    ListItemSource = JsonConvert.DeserializeObject<List<string>>(ItemSourceStr);
                    SetViewTitle(AppResources.battery_type);
                    break;
                case ACUtility.ListSelectorType.BatteryVoltageUnits:
                    ListItemSource = JsonConvert.DeserializeObject<List<string>>(ItemSourceStr);
                    SetViewTitle(AppResources.battery_voltage);
                    break;
                case ACUtility.ListSelectorType.Timezone:
                    ListItemSource = JsonConvert.DeserializeObject<List<JsonZone>>(ItemSourceStr).Select(o => o.display_name).ToList();
                    SetViewTitle(AppResources.time_zone);
                    break;
                case ACUtility.ListSelectorType.HistoryTimeUnits:
                    ListItemSource = ACUtility.Instance.HistoryTimeUnits;
                    SetViewTitle(AppResources.rt_records);
                    break;
                case ACUtility.ListSelectorType.FinishCycleSettingsType:
                    ListItemSource = ACUtility.Instance.FinishCycleSettingsTypes;
                    SetViewTitle(AppResources.finish_cycle_settings);
                    break;
                case ACUtility.ListSelectorType.DurationIntervalTypes:
                    ListItemSource = ACUtility.Instance.DurationIntervalTypes;
                    SetViewTitle(AppResources.duration);
                    break;
                case ACUtility.ListSelectorType.RTRecordsList:
                    ListItemSource = ACUtility.Instance.RTRecordsList;
                    ViewTitle = AppResources.rt_records;
                    break;
                case ACUtility.ListSelectorType.VoltageDetectThreshold:
                    SetViewTitle(AppResources.voltage_detect_threshold);
                    ListItemSource = ACUtility.Instance.VoltageDetectThresholdList;
                    break;
                case ACUtility.ListSelectorType.CurrentDetectThreshold:
                    SetViewTitle(AppResources.current_detect_threshold);
                    ListItemSource = ACUtility.Instance.CurrentDetectThresholdList;
                    break;
                case ACUtility.ListSelectorType.TimerDetectThreshold:
                    SetViewTitle(AppResources.timer_detect_threshold);
                    ListItemSource = ACUtility.Instance.TimerDetectThresholdList;
                    break;
                case ACUtility.ListSelectorType.DefaultCharge_Tr_Amps:
                    SetViewTitle(AppResources.tr_current_rate);
                    ListItemSource.Clear();

                    for (float i = ControlObject.FormLimits.trCurrentRate_min; i <= ControlObject.FormLimits.trCurrentRate_max; i++)
                        ListItemSource.Add(Batt_FormatRate(i, 1));

                    break;
                case ACUtility.ListSelectorType.DefaultCharge_Cc_Amps:
                    SetViewTitle(AppResources.cc_current_rate);
                    ListItemSource.Clear();
                    for (float i = ControlObject.FormLimits.ccCurrentRate_min; i <= ControlObject.FormLimits.ccCurrentRate_max; i++)
                    {
                        ListItemSource.Add(Batt_FormatRate(i, 1));
                    }

                    break;
                case ACUtility.ListSelectorType.DefaultCharge_Fi_Amps:
                    SetViewTitle(AppResources.fi_current_rate);
                    ListItemSource.Clear();

                    for (float i = ControlObject.FormLimits.fiCurrentRate_min; i < ControlObject.FormLimits.fiCurrentRate_max; i++)
                        ListItemSource.Add(Batt_FormatRate(i, 1));

                    break;
                case ACUtility.ListSelectorType.DefaultCharge_Eq_Amps:
                    SetViewTitle(AppResources.eq_current_rate);
                    ListItemSource.Clear();
                    for (float i = ControlObject.FormLimits.eqCurrentRate_min; i < ControlObject.FormLimits.eqCurrentRate_max; i++)
                    {
                        ListItemSource.Add(Batt_FormatRate(i, 1));
                    }

                    break;
                case ACUtility.ListSelectorType.DefaultCharge_Cv_Current:
                    float ccRate = Batt_getValueFromRates(keepSubvalue);
                    ccRate /= 100;
                    SetViewTitle(AppResources.cv_current_step);
                    ListItemSource.Clear();
                    ListItemSource.Add("Default");
                    for (float i = ControlObject.FormLimits.cvCurrentStep_min; i < ControlObject.FormLimits.cvCurrentStep_max; i += 0.5f)
                    {
                        ListItemSource.Add(Batt_FormatRate(i, ccRate));
                    }

                    break;
                case ACUtility.ListSelectorType.DefaultCharge_Cv_Timer:
                    SetViewTitle(AppResources.cv_timeout);
                    ListItemSource.Clear();
                    for (int i = ControlObject.FormLimits.cvTimerStart; i <= ControlObject.FormLimits.cvTimerEnd; i += ControlObject.FormLimits.cvTimerStep)
                    {
                        if (i != 0)
                        {
                            ListItemSource.Add(String.Format("{0:00}:{1:00}", i / 4, (i % 4) * 15));

                        }
                        else
                        {
                            ListItemSource.Add(String.Format("00:01"));

                        }
                    }

                    break;
                case ACUtility.ListSelectorType.DefaultCharge_Eq_Timer:
                    SetViewTitle(AppResources.equilize_timeout);
                    ListItemSource.Clear();
                    for (int i = ControlObject.FormLimits.eqTimerStart; i <= ControlObject.FormLimits.eqTimerEnd; i += ControlObject.FormLimits.eqTimerStep)
                    {
                        if (i != 0)
                        {
                            ListItemSource.Add(String.Format("{0:00}:{1:00}", i / 4, (i % 4) * 15));
                        }
                        else
                        {
                            ListItemSource.Add(String.Format("00:01"));

                        }
                    }

                    break;
                case ACUtility.ListSelectorType.DefaultCharge_Finish_Dv_Voltage:
                    SetViewTitle(AppResources.finish_dv_voltage);
                    ListItemSource.Clear();
                    for (int i = ControlObject.FormLimits.fidVStart; i <= ControlObject.FormLimits.fidVEnd; i++)
                    {
                        ListItemSource.Add((i * 5).ToString());
                    }

                    break;
                case ACUtility.ListSelectorType.DefaultCharge_Finish_Timer:
                    SetViewTitle(AppResources.finish_timeout);
                    ListItemSource.Clear();
                    for (int i = ControlObject.FormLimits.fiTimerStart; i <= ControlObject.FormLimits.fiTimerEnd; i += ControlObject.FormLimits.fiTimerStep)
                    {
                        if (i != 0)
                        {
                            ListItemSource.Add(String.Format("{0:00}:{1:00}", i / 4, (i % 4) * 15));
                        }
                        else
                        {
                            ListItemSource.Add(String.Format("00:01"));

                        }
                    }

                    break;
                case ACUtility.ListSelectorType.DefaultCharge_Desulfate_Timer:
                    SetViewTitle(AppResources.desulfation_duration);
                    ListItemSource.Clear();
                    for (int i = ControlObject.FormLimits.desTimerStart; i <= ControlObject.FormLimits.desTimerEnd; i += ControlObject.FormLimits.desTimerStep)
                    {
                        if (i != 0)
                        {
                            ListItemSource.Add(String.Format("{0:00}:{1:00}", i / 4, (i % 4) * 15));

                        }
                        else
                        {
                            ListItemSource.Add(String.Format("00:01"));

                        }

                    }

                    break;
                case ACUtility.ListSelectorType.DefaultCharge_Dt_Time:
                    SetViewTitle(AppResources.finish_dt_time);
                    ListItemSource.Clear();
                    for (int i = ControlObject.FormLimits.fidtStart; i <= ControlObject.FormLimits.fidtEnd; i++)
                    {
                        ListItemSource.Add(i.ToString());

                    }

                    break;
                case ACUtility.ListSelectorType.DefaultCharge_PartialChargeStop:
                    SetViewTitle(AppResources.partial_charge_stop);
                    ListItemSource.Clear();
                    for (float i = ControlObject.FormLimits.cvFinishCurrentRate_min; i <= ControlObject.FormLimits.cvFinishCurrentRate_max; i += 0.5f)
                    {
                        ListItemSource.Add(Batt_FormatRate(i, 1));
                    }

                    break;
                case ACUtility.ListSelectorType.OEM:
                    ListItemSource = JsonConvert.DeserializeObject<List<OEM>>(ItemSourceStr).Select(o => o.name).ToList();
                    SetViewTitle(title);
                    break;
                case ACUtility.ListSelectorType.Dealer:
                    ListItemSource = JsonConvert.DeserializeObject<List<Dealer>>(ItemSourceStr).Select(o => o.name).ToList();
                    SetViewTitle(title);
                    break;
                case ACUtility.ListSelectorType.ServiceDealer:
                    ListItemSource = JsonConvert.DeserializeObject<List<Dealer>>(ItemSourceStr).Select(o => o.name).ToList();
                    SetViewTitle(title);
                    break;
                case ACUtility.ListSelectorType.Customers:
                    ListItemSource = JsonConvert.DeserializeObject<List<Customer>>(ItemSourceStr).Select(o => o.name).ToList();
                    SetViewTitle(title);
                    break;
                case ACUtility.ListSelectorType.Sites:
                    ListItemSource = JsonConvert.DeserializeObject<List<Site>>(ItemSourceStr).Select(o => o.name).ToList();
                    SetViewTitle(title);
                    break;
                default:
                    if (!string.IsNullOrEmpty(ItemSourceStr))
                    {
                        SetViewTitle(title);
                        ListItemSource = JsonConvert.DeserializeObject<List<string>>(ItemSourceStr);

                    }

                    break;
            }
        }

        string Batt_FormatRate(float percent, float select)
        {
            return percent.ToString() + "% - " + Math.Round(0.01f * percent * BattViewQuantum.Instance.GetBATTView().Config.ahrcapacity * select).ToString() + "A";
        }

        float Batt_getValueFromRates(string value)
        {
            return float.Parse(value.Split(new char[] { '%' })[0]);
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<ListSelectorViewModel>(new { pop = "pop" });
        }

        public IMvxCommand ListSelectorCommand
        {
            get { return new MvxCommand<string>(ExecuteListSelectorCommand); }
        }

        void ExecuteListSelectorCommand(string item)
        {
            SelectedIndex = ListItemSource.FindIndex(o => o.Equals(item));

            var listSelectorMessage =
                new ListSelectorMessage
                (this, ParentItemIndex, item, SelectedItemIndex,
                 SelectedChildPostion, SelectedIndex);

            Mvx.Resolve<IMvxMessenger>().Publish(listSelectorMessage);

            ShowViewModel<ListSelectorViewModel>(new { pop = "pop" });
        }
    }
}