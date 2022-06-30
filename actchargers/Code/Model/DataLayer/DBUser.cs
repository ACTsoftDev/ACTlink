using actchargers.Code.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;

namespace actchargers
{
    public class DBUser : DBModel
    {
        [JsonProperty("userID")]
        public int UserID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("roleid")]
        public int RoleID { get; set; }

        [JsonProperty("roleName")]
        public string RoleName { get; set; }

        public string EmailId { get; set; }

        [Ignore]
        [MaxLength(2048)]
        public JObject AccessObject { get; set; }

        public bool IsAllowEditing()
        {
            if (AccessObject == null)
                return false;

            AccessPermission accessPermission = ConvertToAccessPermission();

            return accessPermission.AllowEditing;
        }

        public AccessPermission ConvertToAccessPermission()
        {
            AccessPermission accessPermission =
                JsonParser.DeserializeObject<AccessPermission>(AccessObject.ToString());

            return accessPermission;
        }

        string accessObjectText;
        [MaxLength(2048)]
        public string AccessObjectText
        {
            get
            {
                if (accessObjectText == null)
                    accessObjectText = AccessObjectToText();

                return accessObjectText;
            }
            set
            {
                accessObjectText = value;
            }
        }

        public string AccessObjectToText()
        {
            if (AccessObject == null)
                return "";

            string text = AccessObject.ToString();

            return text;
        }

        public override bool KeysEqual(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var i = (DBUser)obj;
            return UserID == i.UserID;
        }
    }
}