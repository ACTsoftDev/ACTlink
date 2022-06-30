using MvvmCross.Binding.Droid.Views;

namespace actchargers.Droid
{
    public abstract class BasicListFragment : BaseFragmentWithOutMenu
    {
        CustomEditAdapter adapter;

        internal void InitList(MvxListView listView)
        {
            adapter = new CustomEditAdapter(this);

            listView.Adapter = adapter;
        }

        internal void RefreshList()
        {
            adapter.NotifyDataSetChanged();
        }
    }
}
