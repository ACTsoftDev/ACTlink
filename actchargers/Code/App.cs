using Acr.UserDialogs;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;

namespace actchargers
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        /// <summary>
        /// Initialize this instance.
        /// </summary>
		public override void Initialize()
		{
			CreatableTypes().EndingWith("Service").AsInterfaces().RegisterAsLazySingleton();
			//Register all the Interface Instances Here
			/** Register user dialog instance here **/
			Mvx.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);
			RegisterAppStart(new CustomAppStart());
        }
    }
}
