using System;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class DaysNewTableViewCell : MvxTableViewCell,IUICollectionViewDelegateFlowLayout
    {
        public static readonly NSString Key = new NSString("DaysNewTableViewCell");
        public static readonly UINib Nib;

        private bool _editMode = false;
        public bool CellEditingMode
        {
            get
            {
                return _editMode;
            }
            set
            {
                _editMode = value;
                if(value)
                {
                    separatorView.BackgroundColor = UIColorUtility.FromHex(ACColors.LIST_SEPERATOR_BLUE_COLOR);
                }
                else
                {
                    separatorView.BackgroundColor = UIColorUtility.FromHex(ACColors.LIST_SEPERATOR_GRAY_COLOR);
                }
            }
        }

        static DaysNewTableViewCell()
        {
            Nib = UINib.FromName("DaysNewTableViewCell", NSBundle.MainBundle);
        }

        protected DaysNewTableViewCell(IntPtr handle) : base(handle)
        {
            BackgroundColor = UIColor.White;
            Initialize();
            // Note: this .ctor should not contain any initialization logic.
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        void Initialize()
        {
            this.SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                listCollectionView.RegisterNibForCell(UINib.FromName("ListCollectionViewCell", NSBundle.MainBundle), "ListCollectionViewCell");
                var source = new MvxCollectionViewSource(listCollectionView, new NSString("ListCollectionViewCell"));


                var set = this.CreateBindingSet<DaysNewTableViewCell, ListViewItem>();
                set.Bind(source).For(s => s.ItemsSource).To(item => item.Items).Apply();
                set.Bind(titleLabel).To(item => item.Title).Apply();
                listCollectionView.Source = source;

                //IUICollectionViewDelegateFlowLayout delegate
                listCollectionView.Delegate = this;

            });
        }

        /// <summary>
        /// Gets the size for item.
        /// </summary>
        /// <returns>The size for item.</returns>
        /// <param name="collectionView">Collection view.</param>
        /// <param name="layout">Layout.</param>
        /// <param name="indexPath">Index path.</param>
        [Export("collectionView:layout:sizeForItemAtIndexPath:")]
        public virtual CoreGraphics.CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            return new CGSize(this.Frame.Width / 3-15, 30);
        }
    }
}
