using actchargers.ViewModels;
using Android.Views;
using Android.Webkit;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid.Code.Views.Login
{

	class UsageAgreementView : BaseFragment
    {
        

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.UsageAgreement, null);
			var activity = (LoginView)Activity;
			activity.SupportActionBar.SetDisplayHomeAsUpEnabled(false);
			activity.SupportActionBar.SetDisplayShowHomeEnabled(false);
			var _usageAgreementViewModel = ViewModel as UsageAgreementViewModel;
			activity.mTitle.Text=_usageAgreementViewModel.ViewTitle;
            //webview to render usage agreement file
             WebView webView = (WebView)view.FindViewById(Resource.Id.usageAgreementWebView);
            // webview.getSettings().setLayoutAlgorithm(LayoutAlgorithm.SINGLE_COLUMN);
            //Loading url into webview
            webView.LoadUrl("file:///android_asset/" + _usageAgreementViewModel.HTMLFileName + ".html");
            return view;
        }

        
    }
}