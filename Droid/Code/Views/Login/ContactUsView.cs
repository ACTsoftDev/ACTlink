using Android.Content;
using Android.Net;
using Android.OS;
using Android.Views;
using Android.Webkit;
using MvvmCross.Binding.Droid.BindingContext;
namespace actchargers.Droid
{
    public class ContactUsView : BaseFragment
    {
        private WebView webView;
        private ContactUsViewModel _contactUsViewModel;
        private BaseActivity activity;
       
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.ContactUsLayout, null);
            activity = (BaseActivity)Activity;
            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            activity.SupportActionBar.SetDisplayShowHomeEnabled(true);
             _contactUsViewModel = ViewModel as ContactUsViewModel;
            activity.mTitle.Text = _contactUsViewModel.ViewTitle;
            activity.DisableDrawer();
            webView = (WebView)view.FindViewById(Resource.Id.contactUsWebView);
            webView.LoadUrl("file:///android_asset/" + _contactUsViewModel.HTMLFileName + ".html");
            webView.SetWebViewClient(new ContactUsWebViewClient(_contactUsViewModel, activity));
            return view;
        }
        public override void OnDestroyView()
        {
            base.OnDestroyView();
            activity.EnableDrawer();
        }
		
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                //handling backpress of actionbar back here
                case Android.Resource.Id.Home:
                    // this takes the user 'back', as if they pressed the left-facing triangle icon on the main android toolbar.
                    activity.OnBackPressed();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }


        private class ContactUsWebViewClient : WebViewClient
        {

			private ContactUsViewModel _contactUsViewModel;

			private Context _mContext;

		
			public ContactUsWebViewClient(ContactUsViewModel contactUsViewModel, Context context)
            {
                this._contactUsViewModel = contactUsViewModel;
				this._mContext = context;
            }
            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                if (url.ToString().Contains("tel:"))
                {
					//Clicked on telephone number
					Intent intent = new Intent(Intent.ActionDial,
					                           Uri.Parse(url));
					_mContext.StartActivity(intent);
					
				}
                else
                {
                    //Clicked on Email
                    _contactUsViewModel.EmailLinkClickCommand.Execute();
                }

                return true;
            }
        }
    }
}
