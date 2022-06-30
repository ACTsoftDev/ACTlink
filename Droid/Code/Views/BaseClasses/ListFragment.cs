using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace actchargers.Droid
{
    public class ListFragment : BaseFragmentWithOutMenu, View.IOnTouchListener
    {
        public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            View view = base.OnCreateView(inflater, container, savedInstanceState);

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();

            View.SetOnTouchListener(this);
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            return true;
        }

        public void InitListViewUI(View view, IList<ListViewItem> itemSource)
        {
            try
            {
                if (view == null)
                {
                    return;
                }
                var layout = (LinearLayout)view.FindViewById(Resource.Id.content_panel);
                var adapter = new CustomEditAdapter(this)
                {
                    ItemsSource = itemSource
                };
                CreateListViewUI(layout, adapter, itemSource);
            }
            catch (Exception e)
            {
                Debug.WriteLine("itemSource:InitFormUI >>> " + e);
            }
        }

        public void CreateListViewUI(LinearLayout rootLayout, MvxAdapter adapter, IList<ListViewItem> items)
        {
            rootLayout.RemoveAllViews();

            for (int j = 0; j < items.Count; j++)
            {
                View view = adapter.GetView(j, null, null);
                rootLayout.AddView(view);
            }

            TextView footerView = new TextView(rootLayout.Context);
            footerView.SetSingleLine(false);
            footerView.Text = "\n";
            footerView.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            rootLayout.AddView(footerView);
        }

        public void InitExpandableListViewUI(View view, IList itemsSource, BaseViewModel model)
        {
            try
            {
                if (view == null)
                {
                    return;
                }

                var vm = ViewModel as BaseViewModel;
                var layout = (LinearLayout)view.FindViewById(Resource.Id.content_panel);
                var adapter = new CustomSectionEditAdapter(view.Context, (IMvxAndroidBindingContext)BindingContext, itemsSource, model);
                CreateExpandableListViewUI(layout, adapter, itemsSource);
            }
            catch (Exception e)
            {
                Debug.WriteLine("ExpandableListView:InitFormUI >>> " + e);
            }
        }

        public void CreateExpandableListViewUI(LinearLayout rootLayout, IExpandableListAdapter adapter, IList items)
        {
            IList<TableHeaderItem> headerItems = (IList<TableHeaderItem>)items;

            rootLayout.RemoveAllViews();

            for (int i = 0; i < headerItems.Count; i++)
            {

                View groupView = adapter.GetGroupView(i, true, null, null);

                rootLayout.AddView(groupView);

                for (int j = 0; j < headerItems[i].Count; j++)
                {

                    bool isLastChild = (headerItems[i].Count - 1) == j;

                    View childView = adapter.GetChildView(i, j, isLastChild, null, null);

                    rootLayout.AddView(childView);
                }

            }

            TextView footerView = new TextView(rootLayout.Context);
            footerView.SetSingleLine(false);
            footerView.Text = " \n";
            footerView.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            rootLayout.AddView(footerView);
        }
    }
}
