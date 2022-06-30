using actchargers.ViewModels;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class LoginFragment : BaseFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.LoginLayout, null);
            var activity = (LoginView)Activity;
            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            activity.SupportActionBar.SetDisplayShowHomeEnabled(false);
             var _loginViewModel = ViewModel as LoginViewModel;
             activity.mTitle.Text = _loginViewModel.ViewTitle;

            //Handle RegisterAt click event
            TextView registerClick = view.FindViewById<TextView>(Resource.Id.registerAt);
            var currentViewModel = ViewModel as LoginViewModel;
            registerClick.Click += delegate
            {
                var uri = Android.Net.Uri.Parse(currentViewModel.RegisterAtUrl);
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            };
            //Handling registerAt ends here
            return view;
        }
      
    }
}
