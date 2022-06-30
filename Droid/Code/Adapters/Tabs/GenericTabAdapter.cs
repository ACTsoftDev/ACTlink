using System.Collections.Generic;
using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace actchargers.Droid
{
    public class GenericTabAdapter : MvxViewPagerFragmentAdapter
    {
        readonly Context context;

        public GenericTabAdapter(
            Context context, FragmentManager fragmentManager, IEnumerable<FragmentInfo> fragments)
                : base(context, fragmentManager, fragments)
        {
            this.context = context;
        }

        public View GetTabView(int position, bool isSelected, string title, int count)
        {
            View view =
                LayoutInflater.From(context).Inflate(Resource.Layout.CustomTabLayout, null);

            TextView tabTitle = (TextView)view.FindViewById(Resource.Id.tabTitle);
            tabTitle.Text = title;

            return view;
        }

    }
}
