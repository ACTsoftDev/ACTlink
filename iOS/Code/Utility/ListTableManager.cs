using UIKit;

namespace actchargers.iOS
{
    public static class ListTableManager
    {
        public static void SetHeight
        (UITableView listTableView, int estimatedRowHeight)
        {
            listTableView.EstimatedRowHeight = estimatedRowHeight;
            listTableView.RowHeight = UITableView.AutomaticDimension;
        }

        public static void SetHeight
        (UITableView listTableView, int estimatedRowHeight, int height)
        {
            listTableView.EstimatedRowHeight = estimatedRowHeight;
            listTableView.RowHeight = height;
        }
    }
}
