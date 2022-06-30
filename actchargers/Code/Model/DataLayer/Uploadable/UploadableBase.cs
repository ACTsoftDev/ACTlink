using actchargers.Code.Utility;

namespace actchargers
{
    public abstract class UploadableBase : DBModel
    {
        public string Serialize()
        {
            return JsonParser.SerializeObject(this);
        }
    }
}
