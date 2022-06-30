using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class DayViewItem : MvxViewModel
    {
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        //public bool IsEditable { get; set; }
        public bool OriginalValue { get; set; }


        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }


        private bool _isEditable;
        public bool IsEditable
        {
            get { return _isEditable; }
            set
            {
                _isEditable = value;
                RaisePropertyChanged(() => IsEditable);
            }
        }

        private int _id;
        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged(() => id);
            }
        }

        public ICommand DayButtonClicked { get; set; }

        public IMvxCommand ButtonCommand
        {
            get
            {
                return new MvxCommand(ButtonClick);
            }
        }

        void ButtonClick()
        {
            if (DayButtonClicked != null)
            {
                DayButtonClicked.Execute(this);
            }
        }
    }
}
