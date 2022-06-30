using System.Collections.ObjectModel;

namespace actchargers
{
    public class UIAccessControlUtility
    {
        int visibleCount;
        int savedCount;

        public UIAccessControlUtility()
        {
            visibleCount = 0;
            savedCount = 0;
        }

        public int GetVisibleCount()
        {
            return visibleCount;
        }

        public int GetSavedCount()
        {
            return savedCount;
        }

        public bool DoApplyAccessControlDayList
        (int accessGranted, ListViewItem con, TableHeaderItem tableLayout)
        {
            if (accessGranted == AccessLevelConsts.noAccess)
            {
                con.IsVisible = false;
                return false;
            }
            visibleCount++;
            if (tableLayout != null)
            {
                tableLayout.Add(con);
            }
            if (accessGranted == AccessLevelConsts.readOnly)
            {
                con.IsEditEnabled = false;
            }
            else
            {
                con.IsEditEnabled = true;
                savedCount++;
            }

            return true;
        }

        public bool DoApplyAccessControl
        (int accessGranted, ListViewItem con, ObservableCollection<ListViewItem> tableLayout)
        {
            if (accessGranted == AccessLevelConsts.noAccess)
            {
                con.IsVisible = false;

                return false;
            }

            visibleCount++;

            if (tableLayout != null)
                tableLayout.Add(con);

            if (accessGranted == AccessLevelConsts.readOnly)
            {
                con.IsEditEnabled = false;
            }
            else
            {
                con.IsEditEnabled = true;
                savedCount++;
            }

            return true;
        }
    }
}