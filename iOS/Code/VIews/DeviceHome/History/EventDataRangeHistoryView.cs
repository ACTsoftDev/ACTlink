using System;
using System.Collections.Generic;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using UIKit;

namespace actchargers.iOS
{
    public partial class EventDataRangeHistoryView : MvxPageViewController
    {
        IMvxPageViewModel pageVM;
        IMvxPagedViewModel actvieVM;
        IMvxPagedViewModel nextVM;
        IMvxPagedViewModel prevVM;

        MvxSubscriptionToken _nextSelector;
        Dictionary<string, UIViewController> _pagedViewControllerCache = null;
        UIBarButtonItem declineBtn;


        /// <summary>
        /// Views the did load.
        /// </summary>
		public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.BackgroundColor = UIColor.White;

            UIButton backButton = new UIButton(new CoreGraphics.CGRect(0, 0, 17, 20));
            backButton.SetImage(UIImage.FromBundle("back_arrow.png"), UIControlState.Normal);
            backButton.ShowsTouchWhenHighlighted = true;
            backButton.TouchUpInside += BackButton_TouchUpInside;
            var menuItem = new UIBarButtonItem(backButton);
            NavigationItem.LeftBarButtonItem = menuItem;


            UILabel titleLabel = new UILabel(new CoreGraphics.CGRect(0, 0, 0, 44));
            titleLabel.BackgroundColor = UIColor.Clear;
            titleLabel.TextColor = UIColorUtility.FromHex(ACColors.BLACK_COLOR);
            titleLabel.Font = UIFont.BoldSystemFontOfSize(16);
            titleLabel.TextAlignment = UITextAlignment.Center;
            titleLabel.Lines = 0;

            titleLabel.Text = (ViewModel as EventDataRangeHistoryViewModel).ViewTitle;
            //var bindingSet = this.CreateBindingSet<BaseView, BaseViewModel>();
            //bindingSet.Bind(titleLabel).To((BaseViewModel vm) => vm.ViewTitle);
            //bindingSet.Apply();

            NavigationItem.TitleView = titleLabel;


            _pagedViewControllerCache = new Dictionary<string, UIViewController>();

            pageVM = ViewModel as IMvxPageViewModel;
            actvieVM = pageVM.GetDefaultViewModel();

            UIViewController defaultVC = this.CreateViewControllerFor(actvieVM) as UIViewController;
            SetViewControllers(new UIViewController[] { defaultVC }, UIPageViewControllerNavigationDirection.Forward, true, null);
            NavigationItem.Title = actvieVM.PagedViewId;

            _nextSelector = Mvx.Resolve<IMvxMessenger>().Subscribe<EventsHistoryMessage>(HandleEventHandler);

            GetNextViewController = delegate (UIPageViewController pc, UIViewController rc)
            {
                IMvxIosView rcTV = rc as IMvxIosView;
                if (rcTV == null)
                {
                    return (null);
                }
                IMvxPagedViewModel currentVM = rcTV.ViewModel as IMvxPagedViewModel;
                if (currentVM == null)
                {
                    return (null);
                }
                nextVM = pageVM.GetNextViewModel(currentVM);
                if (nextVM == null)
                {
                    return (null);
                }
                actvieVM = currentVM;
                UIViewController nextVC = null;
                if (_pagedViewControllerCache.ContainsKey(nextVM.PagedViewId))
                {
                    nextVC = _pagedViewControllerCache[nextVM.PagedViewId];
                }
                else
                {
                    nextVC = this.CreateViewControllerFor(nextVM) as UIViewController;
                    _pagedViewControllerCache[nextVM.PagedViewId] = nextVC;
                }
                return (nextVC);
            };

            GetPreviousViewController = delegate (UIPageViewController pc, UIViewController rc)
            {
                IMvxIosView rcTV = rc as IMvxIosView;
                if (rcTV == null)
                {
                    return (null);
                }
                IMvxPagedViewModel currentVM = rcTV.ViewModel as IMvxPagedViewModel;
                if (currentVM == null)
                {
                    return (null);
                }
                actvieVM = currentVM;
                prevVM = pageVM.GetPreviousViewModel(currentVM);
                if (prevVM == null)
                {
                    return (null);
                }
                UIViewController prevVC = null;
                if (_pagedViewControllerCache.ContainsKey(prevVM.PagedViewId))
                {
                    prevVC = _pagedViewControllerCache[prevVM.PagedViewId];
                }
                else
                {
                    prevVC = this.CreateViewControllerFor(prevVM) as UIViewController;
                    _pagedViewControllerCache[prevVM.PagedViewId] = prevVC;
                }

                return (prevVC);
            };

        }

        /// <summary>
        /// Back button touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            this.NavigationController.PopViewController(true);
        }

        void HandleEventHandler(EventsHistoryMessage obj)
        {
            //Next Button
            if (obj.isNextButton)
            {
                nextVM = pageVM.GetNextViewModel(actvieVM);

                if (nextVM != null)
                {
                    actvieVM = nextVM;
                    UIViewController nextVC = null;
                    if (_pagedViewControllerCache.ContainsKey(nextVM.PagedViewId))
                    {
                        nextVC = _pagedViewControllerCache[nextVM.PagedViewId];
                    }
                    else
                    {
                        nextVC = this.CreateViewControllerFor(nextVM) as UIViewController;
                        _pagedViewControllerCache[nextVM.PagedViewId] = nextVC;
                    }
                    SetViewControllers(new UIViewController[] { nextVC }, UIPageViewControllerNavigationDirection.Forward, true, null);
                }
            }
            else
            {
                prevVM = pageVM.GetPreviousViewModel(actvieVM);

                if (prevVM != null)
                {
                    actvieVM = prevVM;
                    UIViewController prevVC = null;
                    if (_pagedViewControllerCache.ContainsKey(prevVM.PagedViewId))
                    {
                        prevVC = _pagedViewControllerCache[prevVM.PagedViewId];
                    }
                    else
                    {
                        prevVC = this.CreateViewControllerFor(prevVM) as UIViewController;
                        _pagedViewControllerCache[prevVM.PagedViewId] = prevVC;
                    }
                    SetViewControllers(new UIViewController[] { prevVC }, UIPageViewControllerNavigationDirection.Reverse, true, null);
                }
            }
        }
    }
}