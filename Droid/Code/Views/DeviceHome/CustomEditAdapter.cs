using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace actchargers.Droid
{
    public class CustomEditAdapter : MvxAdapter
    {
        public CustomEditAdapter(BaseFragment fragment)
            : base(fragment.Context, (IMvxAndroidBindingContext)fragment.BindingContext)
        {
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            return base.GetView(position, convertView, parent);
        }

        protected override View GetBindableView(View convertView, object dataContext, int templateId)
        {
            ListViewItem listItem = (ListViewItem)dataContext;

            switch (listItem.CellType)
            {
                case ACUtility.CellTypes.LabelSwitch:
                    templateId = Resource.Layout.item_switch;
                    break;
                case ACUtility.CellTypes.LabelLabel:
                    templateId = Resource.Layout.item_label;
                    break;
                case ACUtility.CellTypes.LabelTextEdit:
                    templateId = Resource.Layout.item_edit;
                    break;
                case ACUtility.CellTypes.ListSelector:
                    templateId = Resource.Layout.item_list;
                    break;
                case ACUtility.CellTypes.DatePicker:
                    templateId = Resource.Layout.item_date_picker;
                    break;
                case ACUtility.CellTypes.DatePickerWithSwitch:
                    templateId = Resource.Layout.item_Switch_With_Date_Picker;
                    break;
                case ACUtility.CellTypes.Label:
                    templateId = Resource.Layout.item_title;
                    break;
                case ACUtility.CellTypes.Button:
                    templateId = Resource.Layout.item_button;
                    break;
                case ACUtility.CellTypes.ButtonTextEdit:
                    templateId = Resource.Layout.item_edit_button;
                    break;
                case ACUtility.CellTypes.LabelSwitchButton:
                    templateId = Resource.Layout.item_switch_with_button;
                    break;
                case ACUtility.CellTypes.LabelText:
                    templateId = Resource.Layout.item_label_text;
                    break;
                case ACUtility.CellTypes.Days:
                    templateId = Resource.Layout.item_days_grid;
                    break;
                case ACUtility.CellTypes.TimePicker:
                    templateId = Resource.Layout.item_time_picker;
                    break;
                case ACUtility.CellTypes.SectionHeader:
                    templateId = Resource.Layout.item_section_header;
                    break;
            }

            View view = base.GetBindableView(convertView, dataContext, templateId);

            if (listItem.CellType == ACUtility.CellTypes.DatePickerWithSwitch || listItem.CellType == ACUtility.CellTypes.DatePicker)
            {
                AcCustomDateTextView datePickerTextView = view.FindViewById<AcCustomDateTextView>(Resource.Id.dateTextView);
                datePickerTextView.DataContext = listItem;
            }
            else if (listItem.CellType == ACUtility.CellTypes.TimePicker)
            {
                AcCustomTimePickerTextView timePickerTextView = view.FindViewById<AcCustomTimePickerTextView>(Resource.Id.timePickerText);
                timePickerTextView.DataContext = listItem;
            }
            else if (listItem.CellType == ACUtility.CellTypes.LabelTextEdit
                     || listItem.CellType == ACUtility.CellTypes.ButtonTextEdit)
            {
                AcEditText editTextView = view.FindViewById<AcEditText>(Resource.Id.value);

                switch (listItem.TextInputType)
                {
                    case ACUtility.InputType.Decimal:
                        editTextView.InputType = Android.Text.InputTypes.NumberFlagDecimal;
                        break;
                    case ACUtility.InputType.Number:
                        editTextView.InputType = Android.Text.InputTypes.ClassNumber;
                        break;
                    case ACUtility.InputType.Default:
                        editTextView.InputType = Android.Text.InputTypes.ClassText;
                        break;
                }
            }

            view.Visibility = listItem.IsVisible ? ViewStates.Visible : ViewStates.Gone;

            return view;
        }

        public override int ViewTypeCount
        {
            get
            {
                return 14;
            }
        }

        public override int GetItemViewType(int position)
        {
            ListViewItem listItem = (ListViewItem)GetRawItem(position);
            switch (listItem.CellType)
            {
                case ACUtility.CellTypes.LabelSwitch:
                    return 0;
                case ACUtility.CellTypes.LabelLabel:
                    return 1;
                case ACUtility.CellTypes.LabelTextEdit:
                    return 2;
                case ACUtility.CellTypes.ListSelector:
                    return 3;
                case ACUtility.CellTypes.DatePicker:
                    return 4;
                case ACUtility.CellTypes.DatePickerWithSwitch:
                    return 5;
                case ACUtility.CellTypes.Label:
                    return 6;
                case ACUtility.CellTypes.Button:
                    return 7;
                case ACUtility.CellTypes.ButtonTextEdit:
                    return 8;
                case ACUtility.CellTypes.LabelSwitchButton:
                    return 9;
                case ACUtility.CellTypes.LabelText:
                    return 10;
                case ACUtility.CellTypes.Days:
                    return 11;
                case ACUtility.CellTypes.TimePicker:
                    return 12;
                case ACUtility.CellTypes.SectionHeader:
                    return 13;
            }

            return 1;
        }
    }
} 