using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Newtonsoft.Json;

namespace actchargers.ViewModels
{
    public class UsageAgreementViewModel : BaseViewModel
    {
        IUserPreferences userPreferences { set; get; }

        /// <summary>
        /// Gets or sets the decline button title.
        /// </summary>
        /// <value>The decline button title.</value>
        public string DeclineBtnTitle { get; set; }

        /// <summary>
        /// Gets or sets the accept button title.
        /// </summary>
        /// <value>The accept button title.</value>
        public string AcceptBtnTitle { get; set; }

        /// <summary>
        /// Gets or sets the usage agreement text.
        /// </summary>
        /// <value>The usage agreement text.</value>
        public string UsageAgreementText { get; set; }

        /// <summary>
        /// The log in user object.
        /// </summary>
        private string logInUserObj;
        public string HTMLFileName { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.ViewModels.UsageAgreementViewModel"/> class.
        /// </summary>
        public UsageAgreementViewModel()
        {
            userPreferences = Mvx.Resolve<IUserPreferences>();

            ViewTitle = AppResources.usage_agreement;
            DeclineBtnTitle = AppResources.decline;
            AcceptBtnTitle = AppResources.accept;
            UsageAgreementText = AppResources.user_agreement_text;
            HTMLFileName = "usage_agreement";
        }

        /// <summary>
        /// Init the specified userObj.
        /// </summary>
        /// <param name="userObj">User object.</param>
        public void Init(string userObj)
        {
            logInUserObj = userObj;
        }

        /// <summary>
        /// Gets the accept button click command.
        /// </summary>
        /// <value>The accept button click command.</value>
        /// 
        public IMvxCommand AcceptButtonClickCommand
        {
            get { return new MvxCommand(OnButtonAcceptClick); }
        }

        /// <summary>
        /// Ons the accept click.
        /// </summary>
        void OnButtonAcceptClick()
        {
            SaveUserObj();
            ShowViewModel<MainContainerViewModel>();
        }

        /// <summary>
        /// Gets the accept button click command.
        /// </summary>
        /// <value>The accept button click command.</value>
        public IMvxCommand AcceptBtnClickCommand
        {
            get { return new MvxCommand(OnAcceptClick); }
        }

        /// <summary>
        /// Ons the accept click.
        /// </summary>
        void OnAcceptClick()
        {
            SaveUserObj();
            ShowViewModel<MainContainerViewModel>();
        }

        void SaveUserObj()
        {
            DBUser user = JsonConvert.DeserializeObject<DBUser>(logInUserObj);
            user.Password = userPreferences.GetString(ACConstants.PREF_PASSWORD);

            DbSingleton.DBManagerServiceInstance
                       .GetDBUserLoader()
                       .InsertOrReplaceWithChildren(user);

            userPreferences.SetInt(ACConstants.USER_PREFS_USER_ID, user.UserID);
            userPreferences.SetBool(ACConstants.USER_PREFS_USER_LOGGED_IN, true);
            userPreferences.SetBool(ACConstants.USER_PREFS_ALLOW_EDITING,
                                    user.IsAllowEditing());

            DbSingleton
                .DBManagerServiceInstance
                .GetGenericObjectsLoader()
                .SetAsAgreed();
        }

        /// <summary>
        /// Gets the decline button click command.
        /// </summary>
        /// <value>The decline button click command.</value>
        public IMvxCommand DeclineBtnClickCommand
        {
            get { return new MvxCommand(OnDeclineClick); }
        }

        /// <summary>
        /// Ons the decline click.
        /// </summary>
        void OnDeclineClick()
        {
            //declined agreement change loggedin to false
            userPreferences.SetBool(ACConstants.USER_PREFS_USER_LOGGED_IN, false);
            ShowViewModel<UsageAgreementViewModel>(new { pop = "pop" });
        }

    }
}
