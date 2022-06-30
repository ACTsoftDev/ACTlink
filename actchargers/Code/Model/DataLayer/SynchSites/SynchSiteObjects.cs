using System;

namespace actchargers
{
    public class SynchSiteObjects : DBModel
    {
        public string SiteName { get; set; }

        public string CustomerName { get; set; }

        public UInt32 SiteId { get; set; }

        public UInt32 CustomerId { get; set; }

        public override bool KeysEqual(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var i = (SynchSiteObjects)obj;
            return SiteId == i.SiteId
                              && CustomerId == i.CustomerId;
        }
    }
}
