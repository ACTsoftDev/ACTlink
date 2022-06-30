using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using OxyPlot;

namespace actchargers
{
    public class GroupChartViewItem : ObservableCollection<ChartViewItem>
    {

    }

    public class ChartViewItem : MvxViewModel
    {
        /// <summary>
        /// The plot object.
        /// </summary>
		private PlotModel _plotObj;
        public PlotModel PlotObject
        {
            get
            { return _plotObj; }
            set
            {
                _plotObj = value;
                //RaisePropertyChanged(() => PlotObject);
            }
        }


        /// <summary>
        /// The plot object.
        /// </summary>
        private List<PlotModel> _plotObjList;
        public List<PlotModel> PlotObjList
        {
            get
            { return _plotObjList; }
            set
            {
                _plotObjList = value;
                RaisePropertyChanged(() => PlotObject);
            }
        }

        /// <summary>
        /// The name of the chart image.
        /// </summary>
		private string _chartImageName;
        public string ChartImageName
        {
            get { return _chartImageName; }
            set
            {
                _chartImageName = value;
                RaisePropertyChanged(() => ChartImageName);
            }
        }

        /// <summary>
        /// The type of the chart.
        /// </summary>
        private string _chartType;
        public string ChartType
        {
            get { return _chartType; }
            set
            {
                _chartType = value;
                RaisePropertyChanged(() => ChartType);
            }
        }

        /// <summary>
        /// The type of the chart.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                RaisePropertyChanged(() => Text);
            }
        }

        /// <summary>
        /// Gets or sets the selection command.
        /// </summary>
        /// <value>The selection command.</value>
        public ICommand SelectionCommand { get; set; }



        public IMvxCommand ButtonSelectorCommand
        {
            get
            {
                return new MvxCommand(ButtonClick);
            }
        }

        void ButtonClick()
        {
            if (SelectionCommand != null)
            {
                SelectionCommand.Execute(this);
            }
        }


        /// <summary>
        /// The type of the cell.
        /// </summary>
        public ACUtility.CellTypes CellType { get; set; }

        /// <summary>
        /// Gets or sets the type of the list selector.
        /// </summary>
        /// <value>The type of the list selector.</value>
        public ACUtility.ListSelectorType ListSelectorType { get; set; }

        public IMvxCommand ListSelectorCommand
        {
            get
            {
                return new MvxCommand(ButtonClick);
            }
        }
    }
}
