using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class ListViewItem : MvxViewModel
    {
        public ListViewItem()
        {
            KeepIndex = -1;
            SelectedIndex = -1;
            IsVisible = true;
            Text = "";
            SubTitle = "";
            Enable = true;
        }

        public int Index { get; set; }

        string _imageName;
        public string ImageName
        {
            get { return _imageName; }
            set
            {
                _imageName = value;
                RaisePropertyChanged(() => ImageName);
            }
        }

        string _title;
        public string Title
        {
            get
            { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public int ParentIndex { get; set; }

        string _subTitle;
        public string SubTitle
        {
            get
            { return _subTitle; }
            set
            {
                _subTitle = value;
                RaisePropertyChanged(() => SubTitle);
            }
        }

        string _Title2;
        public string Title2
        {
            get
            { return _Title2; }
            set
            {
                _Title2 = value;
                RaisePropertyChanged(() => Title2);
            }
        }

        string _SubTitle2;
        public string SubTitle2
        {
            get
            { return _SubTitle2; }
            set
            {
                _SubTitle2 = value;
                RaisePropertyChanged(() => SubTitle2);
            }
        }

        string _Seconds;
        public string Seconds
        {
            get
            { return _Seconds; }
            set
            {
                _Seconds = value;
                RaisePropertyChanged(() => Seconds);
            }
        }

        string _Title3;
        public string Title3
        {
            get
            { return _Title3; }
            set
            {
                _Title3 = value;
                RaisePropertyChanged(() => Title3);
            }
        }

        string _SubTitle3;
        public string SubTitle3
        {
            get
            { return _SubTitle3; }
            set
            {
                _SubTitle3 = value;
                RaisePropertyChanged(() => SubTitle3);
            }
        }

        bool enable;
        public bool Enable
        {
            get
            {
                return enable;
            }
            set
            {
                SetProperty(ref enable, value);
            }
        }

        bool isEditable;
        public bool IsEditable
        {
            get
            {
                return isEditable;
            }
            set
            {
                SetProperty(ref isEditable, value);
            }
        }

        bool isEditEnabled;
        public bool IsEditEnabled
        {
            get
            {
                return isEditEnabled;
            }
            set
            {
                SetProperty(ref isEditEnabled, value);
            }
        }

        bool isFailed;
        public bool IsFailed
        {
            get
            {
                return isFailed;
            }
            set
            {
                SetProperty(ref isFailed, value);
            }
        }

        string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                if (TextMaxLength > 0)
                {
                    if (value.Length <= TextMaxLength)
                    {
                        _text = value;
                        if (TextValueChanged != null)
                        {
                            TextValueChanged.Execute();
                        }
                    }

                }
                else
                {
                    _text = value;
                    if (TextValueChanged != null)
                    {
                        TextValueChanged.Execute();
                    }
                }

                RaisePropertyChanged(() => Text);
            }
        }

        public ACUtility.InputType TextInputType { get; set; }

        public Type ViewModelType { get; set; }

        public Color ForeColor { get; internal set; }

        public string ItemHeader { get; set; }

        public ACUtility.ListSelectorType ListSelectorType { get; set; }

        public ACUtility.CellTypes DefaultCellType { get; set; }

        public ACUtility.CellTypes EditableCellType { get; set; }

        ACUtility.CellTypes _cellType;
        public ACUtility.CellTypes CellType
        {
            get { return _cellType = IsEditable ? EditableCellType : DefaultCellType; }
            set
            {
                _cellType = value;
                RaisePropertyChanged(() => CellType);
            }
        }

        bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                RaisePropertyChanged(() => IsVisible);
            }
        }

        public bool IsVisibleAndEditable()
        {
            return IsVisible && isEditable;
        }

        public List<int> EnableItemsList { get; set; }

        public int KeepIndex { get; set; }

        public ICommand ListSelectionCommand { get; set; }

        public ICommand SwitchValueChanged { get; set; }

        public IMvxCommand TextValueChanged { get; set; }

        bool _isSwitchEnabled;
        public bool IsSwitchEnabled
        {
            get { return _isSwitchEnabled; }
            set
            {
                _isSwitchEnabled = value;
                if (SwitchValueChanged != null)
                {
                    SwitchValueChanged.Execute(this);
                }
                if (CellType != ACUtility.CellTypes.DatePickerWithSwitch)
                {
                    Text = value ? AppResources.yes : AppResources.no;

                    if (string.IsNullOrEmpty(SubTitle))
                    {
                        SubTitle = Text;
                    }
                }
                RaisePropertyChanged(() => IsSwitchEnabled);
            }
        }

        DateTime _maxDate = DateTime.MaxValue;
        public DateTime MaxDate
        {
            get
            {
                return _maxDate;
            }
            set
            {
                _maxDate = value;
                RaisePropertyChanged(() => MaxDate);
            }
        }

        DateTime _minDate = DateTime.MinValue;
        public DateTime MinDate
        {
            get
            {
                return _minDate;
            }
            set
            {
                _minDate = value;
                RaisePropertyChanged(() => MinDate);
            }
        }

        DateTime _date;
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                RaisePropertyChanged(() => Date);

                Text = Date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
            }
        }

        string _selectedItem;
        public string SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            internal set
            {
                _selectedItem = value;
                Text = SubTitle = value;
            }
        }


        List<object> _Items;
        public List<object> Items
        {
            get
            {
                return _Items;
            }
            internal set
            {
                _Items = value;
                RaisePropertyChanged(() => Items);
            }
        }

        int _selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            internal set
            {
                _selectedIndex = value;
            }
        }

        public int TextMinLength { get; set; }

        public int TextMaxLength { get; set; }

        public void ChangeEditMode(bool editingMode)
        {
            IsEditable = editingMode && IsEditEnabled;
        }

        public void Apply()
        {
            SubTitle = Text;
        }

        public IMvxCommand ButtonSelectorCommand
        {
            get
            {
                return new MvxCommand(ButtonClick);
            }
        }

        public IMvxCommand ListSelectorCommand
        {
            get
            {
                return new MvxCommand(ButtonClick);
            }
        }

        void ButtonClick()
        {
            if (ListSelectionCommand != null)
                ListSelectionCommand.Execute(this);
        }

        public void Reset()
        {
            if (IsEditable)
                ResetIfEditable();
        }

        void ResetIfEditable()
        {
            SubTitle = string.Empty;
        }
    }
}
