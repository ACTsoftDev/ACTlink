using actchargers.Code.Helpers.Versions;
using System;

namespace actchargers
{
    public class AboutUsController : BaseController
    {
        AllVersions AllVersions;

        public AboutUsController()
        {
            AllVersions = new AllVersions();
        }

        public string AppVersionWithTitle
        {
            get
            {
                return string.Format("{0} {1}", AppResources.version, AllVersions.SoftwareVersion);
            }
        }

        public string McbVersionWithTitle
        {
            get
            {
                return string.Format("{0} {1:0.00}", AppResources.mcb_version, AllVersions.McbVersion);
            }
        }

        public string BattViewVersionWithTitle
        {
            get
            {
                return string.Format("{0} {1:0.00}", AppResources.batt_view_version, AllVersions.BattviewVersion);
            }
        }

        public string CalibratorVersionWithTitle
        {
            get
            {
                return string.Format("{0} {1:0.00}", AppResources.calibrator_version, AllVersions.CalibratorVersion);
            }
        }

        public string CopyrightVersionWithTitle
        {
            get
            {
                return string.Format("{0} {1}", AppResources.copyright, DateTime.Now.Year);
            }
        }

        public string AboutAct
        {
            get
            {
                return AppResources.about_act;
            }
        }

        public string PoweredTitle
        {
            get
            {
                return AppResources.powered_by;
            }
        }
    }
}
