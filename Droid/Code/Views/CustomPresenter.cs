using actchargers.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;

namespace actchargers.Droid
{
    /// <summary>
    /// Fragment host.
    /// </summary>
	public interface IFragmentHost
    {
        bool Show(MvxViewModelRequest request);
        void Close(IMvxViewModel viewModel);
        void ChangePresentation(MvxPresentationHint hint);
    }

    /// <summary>
    /// Custom presenter.
    /// </summary>
	public interface ICustomPresenter
    {
        void Register(IFragmentHost host);
    }

    public class CustomPresenter : MvxAndroidViewPresenter, ICustomPresenter
    {
        IFragmentHost Host;

        public override void Show(MvxViewModelRequest request)
        {
            if (request.ViewModelType == typeof(ListSelectorViewModel))
            {
                if (request.ParameterValues != null &&
                request.ParameterValues.ContainsKey("pop"))
                {
                    if (Activity != null)
                    {
                        Activity.Finish();
                    }
                }
                else {
                    base.Show(request); 
                }
                return;
            }

            //Check for LoginViewModel type
            if ((request.ViewModelType == typeof(MainContainerViewModel)) 
                || (request.ViewModelType == typeof(LoginViewModel)))
            {
                if (Activity != null)
                {
                    Activity.Finish();
                }
                Host = null;
            }
           
            if (Host != null)
            {
                if (Host.Show(request))
                {
                    return;
                }
            }
            else {
                base.Show(request);
            }
        }

        /// <summary>
        /// Register the specified host.
        /// </summary>
        /// <param name="host">Host.</param>
		public void Register(IFragmentHost host)
        {
            Host = host;
        }

        public override void Close(IMvxViewModel viewModel)
        {
            if (Host != null)
            {
                Host.Close(viewModel);
            }
            base.Close(viewModel);
        }

        /// <summary>
        /// Changes the presentation.
        /// </summary>
        /// <param name="hint">Hint.</param>
		public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (Host != null)
            {
                Host.ChangePresentation(hint);
            }
            base.ChangePresentation(hint);
        }


    }
}
