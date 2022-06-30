namespace actchargers
{
    public class ListSelectorParameter
    {
        public ACUtility.ListSelectorType SelectorType { get; set; }

        public int ParentItemIndex { get; set; }

        public int SelectedItemIndex { get; set; }

        public string ItemSourceStr { get; set; }

        public string KeepSubValue { get; set; }

        public string Title { get; set; }
    }
}
