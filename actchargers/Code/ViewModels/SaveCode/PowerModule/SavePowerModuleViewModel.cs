using System.Collections.ObjectModel;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class SavePowerModuleViewModel : BaseViewModel
    {
        /// <summary>
        /// The history view item source.
        /// </summary>
        private ObservableCollection<ListViewItem> _powerModuleViewItemSource;
        public ObservableCollection<ListViewItem> PowerModuleViewItemSource
        {
            get { return _powerModuleViewItemSource; }
            set
            {
                _powerModuleViewItemSource = value;
                RaisePropertyChanged(() => PowerModuleViewItemSource);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.PowerModuleViewModel"/> class.
        /// </summary>
        public SavePowerModuleViewModel()
        {
            ViewTitle = AppResources.power_module;

            PowerModuleViewItemSource = new ObservableCollection<ListViewItem>
            {
                new ListViewItem
                {
                    Title = AppResources.pm_info,
                    ViewModelType = typeof(PmInfoViewModel)
                },
                new ListViewItem
                {
                    Title = AppResources.pm_faults,
                    ViewModelType = typeof(PMFaultsViewModel)
                },
                new ListViewItem
                {
                    Title = AppResources.pm_live_title,
                    ViewModelType = typeof(PmLiveViewModel)
                }
            };
        }


        /// <summary>
        /// The m select item command.
        /// </summary>
        private MvxCommand<ListViewItem> m_SelectItemCommand;
        public ICommand SelectItemCommand
        {
            get
            {
                return this.m_SelectItemCommand ?? (this.m_SelectItemCommand = new MvxCommand<ListViewItem>(this.ExecuteSelectItemCommand));
            }
        }

        void ExecuteSelectItemCommand(ListViewItem item)
        {
            if (item.ViewModelType == typeof(PmInfoViewModel))
            {
                ShowViewModel<PmInfoViewModel>();
            }
            else if (item.ViewModelType == typeof(PMFaultsViewModel))
            {
                ShowViewModel<PMFaultsViewModel>();
            }
            else if (item.ViewModelType == typeof(PmLiveViewModel))
            {
                ShowViewModel<PmLiveViewModel>();
            }
        }
    }
}
