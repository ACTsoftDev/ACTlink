using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace actchargers
{
    public abstract class SiteviewDeviceDownloader
    {
        public event EventHandler OnRefresh;

        internal CancellationToken cancellationToken;

        int progressCompleted;
        public int ProgressCompleted
        {
            get { return progressCompleted; }
            set
            {
                progressCompleted = value;

                FireOnRefresh();
            }
        }

        public bool IsDownloading { get; set; }

        protected SiteviewDeviceDownloader()
        {
            cancellationToken = new CancellationToken();
        }

        public async Task<bool> DownloadDevice(SiteViewDevice siteViewDevice)
        {
            IsDownloading = true;

            ProgressCompleted = 10;

            bool result = false;

            try
            {
                result = await TryDownloadDevice(siteViewDevice);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Logger.AddLog(true, "x5394)" + ex);

                return false;
            }
            finally
            {
                IsDownloading = false;
            }

            SetProgressBasedOnResult(result);

            return result;
        }

        internal abstract Task<bool> TryDownloadDevice(SiteViewDevice siteViewDevice);

        public void SetStackedProgressCompleted(int stackedProgressCompleted)
        {
            if (stackedProgressCompleted <= ProgressCompleted)
                return;

            if (stackedProgressCompleted < 100)
                ProgressCompleted = stackedProgressCompleted;
            else
                ProgressCompleted = 95;
        }

        void SetProgressBasedOnResult(bool result)
        {
            if (result)
                ProgressCompleted = 100;
        }

        public void Abort()
        {
            cancellationToken.ThrowIfCancellationRequested();
        }

        void FireOnRefresh()
        {
            OnRefresh?.Invoke(this, EventArgs.Empty);
        }
    }
}
