using System;
using MvvmCross.Platform;

namespace actchargers
{
    public sealed class DbSingleton
    {
        static volatile IDBManagerService instance;
        static object syncRoot = new Object();

        DbSingleton() { }

        public static IDBManagerService DBManagerServiceInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = Mvx.Resolve<IDBManagerService>();
                    }
                }

                return instance;
            }
        }
    }
}
