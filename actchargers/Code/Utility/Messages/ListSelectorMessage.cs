using MvvmCross.Plugins.Messenger;

namespace actchargers
{
    public class ListSelectorMessage : MvxMessage
    {
        public int ParentItemIndex { get; set; }

        public string SelectedItem { get; set; }

        public int SelectedIndex { get; set; }

        public int SelectedItemindex { get; set; }

        public int SelectedChildItemindex { get; set; }

        public ListSelectorMessage
        (object sender, int parentItemIndex, string selectedItem,
         int selectedItemindex, int selectedChildItemindex, int selectedIndex)
            : base(sender)
        {
            ParentItemIndex = parentItemIndex;
            SelectedItem = selectedItem;
            SelectedItemindex = selectedItemindex;
            SelectedChildItemindex = selectedChildItemindex;
            SelectedIndex = selectedIndex;
        }
    }
}