using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using static actchargers.ACUtility;

namespace actchargers
{
    public class CableSettingsSubController : SiteViewSettingsBaseSubController
    {
        #region Cable Settings Items

        ListViewItem ChargerCableLength;
        ListViewItem CableGauge;
        ListViewItem NumberOfCables;
        ListViewItem BatteryCableLength;
        ListViewItem Calculate;
        ListViewItem Cancel;

        #endregion

        public CableSettingsSubController() : base(false, false)
        {
        }

        public async override Task Start()
        {
            await base.Start();

            ShowEdit = false;
        }

        #region Init Items

        internal override void InitSharedBattViewItems()
        {
        }

        internal override void InitSharedMcbItems()
        {
            ChargerCableLength = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.charger_cable_length,
                DefaultCellType = CellTypes.LabelTextEdit,
                EditableCellType = CellTypes.LabelTextEdit,
                IsEditable = true,
                TextInputType = InputType.Number
            };

            CableGauge = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.cable_gauge,
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            CableGauge.Items = new List<object>();
            CableGauge.Items.AddRange(
                new object[]
            {
                "1/0",
                "2/0",
                "3/0",
                "4/0"
            });
            CableGauge.SelectedIndex = -1;

            NumberOfCables = new ListViewItem
            {
                Index = 2,
                Title = AppResources.number_of_cables,
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            NumberOfCables.Items = new List<object>
            {
                AppResources.single,
                AppResources.dual
            };
            NumberOfCables.SelectedIndex = -1;

            BatteryCableLength = new ListViewItem
            {
                Index = 3,
                Title = AppResources.battery_cable_length,
                DefaultCellType = CellTypes.LabelTextEdit,
                EditableCellType = CellTypes.LabelTextEdit,
                IsEditable = true,
                TextInputType = InputType.Number
            };

            Calculate = new ListViewItem
            {
                Index = 4,
                Title = AppResources.calculate,
                DefaultCellType = CellTypes.Button,
                ListSelectionCommand = CalculateSelectionCommand
            };

            Cancel = new ListViewItem
            {
                Index = 5,
                Title = AppResources.cancel,
                DefaultCellType = CellTypes.Button,
                ListSelectionCommand = CancelSelectionCommand
            };
        }

        #endregion

        public IMvxCommand CalculateSelectionCommand
        {
            get
            {
                return new MvxCommand(ExecuteCalculateSelectionCommand);
            }
        }

        #region Calculate

        void ExecuteCalculateSelectionCommand()
        {
            try
            {
                TryExecuteCalculateSelectionCommand();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                ACUserDialogs.ShowAlert(AppResources.input_error);
            }
        }

        void TryExecuteCalculateSelectionCommand()
        {
            float ir = CalculateIR();

            SendIrMessage(ir);

            FireOnClosed();
        }

        float CalculateIR()
        {
            float ir = 0.0f;

            float chargerCableLength = float.Parse(ChargerCableLength.Text);

            int cableGaugeIndex = CableGauge.SelectedIndex;

            int nmberOfCablesIndex = NumberOfCables.SelectedIndex;
            bool isDualCable = (nmberOfCablesIndex == 1);

            float batteryCableLength = float.Parse(BatteryCableLength.Text);

            ir = chargerCableLength + batteryCableLength;

            if (isDualCable)
                ir *= 4;
            else
                ir *= 2;

            switch (nmberOfCablesIndex)
            {
                case 0:
                    ir *= 0.105f;

                    break;

                case 1:
                    ir *= 0.084f;

                    break;

                case 2:
                    ir *= 0.067f;

                    break;

                case 3:
                    ir *= 0.053f;

                    break;
            }

            return ir;
        }

        #endregion

        void SendIrMessage(float ir)
        {
            Mvx.Resolve<IMvxMessenger>().Publish(new CableSettingsMessage(this, ir));
        }

        public IMvxCommand CancelSelectionCommand
        {
            get
            {
                return new MvxCommand(ExecuteCancelSelectionCommand);
            }
        }

        void ExecuteCancelSelectionCommand()
        {
            FireOnClosed();
        }

        internal override void InitExclusiveBattViewItems()
        {
        }

        internal override void InitExclusiveMcbItems()
        {
        }

        internal override void LoadBattViewValues()
        {
        }

        internal override void LoadMcbValues()
        {
        }

        internal override void LoadExclusiveValues()
        {
        }

        internal override void AddExclusiveItems()
        {
        }

        internal override int BattViewAccessApply()
        {
            return 0;
        }

        internal override int McbAccessApply()
        {
            ItemSource.Add(Cancel);
            ItemSource.Add(Calculate);
            ItemSource.Add(BatteryCableLength);
            ItemSource.Add(NumberOfCables);
            ItemSource.Add(CableGauge);
            ItemSource.Add(ChargerCableLength);

            return ItemSource.Count;
        }

        internal override VerifyControl VerfiyBattViewSettings()
        {
            return new VerifyControl();
        }

        internal override VerifyControl VerfiyMcbSettings()
        {
            return new VerifyControl();
        }

        internal override void SaveBattViewToConfigObject(BattViewObject device)
        {
        }

        internal override void SaveMcbToConfigObject(MCBobject device)
        {
        }

        public override void LoadDefaults()
        {
        }
    }
}
