using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Core;
using UIKit;

namespace actchargers.iOS
{
    public partial class ButtonTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("ButtonTableViewCell");
        public static readonly UINib Nib;

        static ButtonTableViewCell()
        {
            Nib = UINib.FromName("ButtonTableViewCell", NSBundle.MainBundle);
        }

        protected ButtonTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                var bindable1 = this as IMvxDataConsumer;
                MvxViewModel contextItem = bindable1.DataContext as MvxViewModel;

                if (contextItem.GetType() == typeof(ChartViewItem))
                {
                    var set = this.CreateBindingSet<ButtonTableViewCell, ChartViewItem>();
                    set.Bind(actionButton).For("Title").To(item => item.Title);
                    set.Bind(actionButton).To(item => item.SelectionCommand);
                    set.Apply();
                }
                else
                {
                    var set = this.CreateBindingSet<ButtonTableViewCell, ListViewItem>();
                    set.Bind(actionButton).For("Title").To(item => item.Title);
                    set.Bind(actionButton).To(item => item.ButtonSelectorCommand);
                    set.Bind(actionButton).For(arg => arg.Enabled).To(item => item.Enable);
                    set.Bind(actionButton).For(arg => arg.Alpha).To(item => item.Enable).WithConversion("BoolToAlpha").Apply();
                    set.Apply();
                }
            });
        }
    }
}
