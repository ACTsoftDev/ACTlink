using actchargers.Droid.Code.Views.Login;
using actchargers.ViewModels;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platform;
using Android.Hardware.Usb;
using static Android.Support.V4.App.FragmentManager;

namespace actchargers.Droid
{
    [Activity(Theme = "@style/AppTheme", Label = "@string/app_name", WindowSoftInputMode = SoftInput.AdjustResize, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.KeyboardHidden | ConfigChanges.Orientation, ScreenOrientation = OrientationManager.DEFAULT_ORIENTATION)]
    public class LoginView : BaseActivity, IFragmentHost, IOnBackStackChangedListener
    {
        LoginViewModel _loginViewModel;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            if (Intent.Action != null)
            {
                if (Intent.Action == UsbManager.ActionUsbAccessoryAttached)
                {

                }
            }

            _loginViewModel = ViewModel as LoginViewModel;

            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            SupportActionBar.SetDisplayShowHomeEnabled(false);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            SupportFragmentManager.AddOnBackStackChangedListener(this);
            topActivity = this;

            var transaction = SupportFragmentManager.BeginTransaction();

            MvxFragment frag = new LoginFragment
            {
                ViewModel = _loginViewModel
            };

            transaction.Replace(Resource.Id.logincontent_frame, frag, frag.GetType().Name);
            transaction.AddToBackStack(frag.GetType().Name);
            transaction.CommitAllowingStateLoss();
        }

        void RegisterForDetailsRequests()
        {
            var customPresenter = Mvx.Resolve<ICustomPresenter>();
            customPresenter.Register(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            RegisterForDetailsRequests();
        }

        #region IFragmentHost implementation

        bool IFragmentHost.Show(MvxViewModelRequest request)
        {

            if (request.ViewModelType == typeof(UsageAgreementViewModel))
            {
                if (request.ParameterValues != null && request.ParameterValues.ContainsKey("pop"))
                {
                    if (null != SupportFragmentManager)
                    {
                        MvxFragment topFragment = (MvxFragment)SupportFragmentManager.FindFragmentById(Resource.Id.logincontent_frame);
                        if (topFragment != null)
                        {
                            Close((topFragment.ViewModel as UsageAgreementViewModel));
                            SupportFragmentManager.PopBackStackImmediate();
                        }
                    }
                }
                else if (request.ParameterValues != null && request.ParameterValues.ContainsKey("clearRoot"))
                {
                    if (null != SupportFragmentManager)
                    {
                        Finish();
                    }
                }
                else if (request.ViewModelType == typeof(UsageAgreementViewModel))
                {
                    MvxFragment frag;
                    frag = new UsageAgreementView();
                    var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
                    var viewModelLocal = loaderService.LoadViewModel(request, null /* saved state */);

                    //Normally we would do this, but we already have it
                    var transaction = SupportFragmentManager.BeginTransaction();
                    frag.ViewModel = viewModelLocal;
                    transaction.Replace(Resource.Id.logincontent_frame, frag, null);
                    transaction.AddToBackStack(frag.GetType().Name);
                    transaction.CommitAllowingStateLoss();
                }
            }
            else if (request.ViewModelType == typeof(ContactUsViewModel))
            {
                MvxFragment frag;
                frag = new ContactUsView();
                var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
                var viewModelLocal = loaderService.LoadViewModel(request, null /* saved state */);
                var transaction = SupportFragmentManager.BeginTransaction();
                frag.ViewModel = viewModelLocal;
                transaction.Replace(Resource.Id.logincontent_frame, frag, null);
                transaction.AddToBackStack(frag.GetType().Name);
                transaction.CommitAllowingStateLoss();
            }

            return true;

        }
        public override void OnBackPressed()
        {
            var topFragment = (MvxFragment)SupportFragmentManager.FindFragmentById(Resource.Id.logincontent_frame);
            if (topFragment != null && topFragment.ViewModel is LoginViewModel)
            {
                Finish();
            }
            else
            {
                base.OnBackPressed();
            }

        }
        void IFragmentHost.Close(IMvxViewModel viewModel)
        {

        }

        void IFragmentHost.ChangePresentation(MvxPresentationHint hint)
        {

        }

        #endregion

        void IOnBackStackChangedListener.OnBackStackChanged()
        {
        }

        void ShowKeyboard()
        {
            View view = CurrentFocus;
            if (view != null)
            {
                InputMethodManager imm = (InputMethodManager)GetSystemService(InputMethodService);
                imm.ShowSoftInput(view, ShowFlags.Implicit);
            }
        }

        protected override int GetLayoutResource()
        {
            return Resource.Layout.LoginContainerLayout;
        }
    }
}