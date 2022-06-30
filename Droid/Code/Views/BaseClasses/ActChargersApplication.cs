using System;
using Acr.UserDialogs;
using Android.App;
using Android.Runtime;

namespace actchargers.Droid
{
    [Application]
    public class ActChargersApplication : Application
    {
		
        public ActChargersApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            UserDialogs.Init(this);
        }

    }
}
