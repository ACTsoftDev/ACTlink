using Newtonsoft.Json;

namespace actchargers
{
    public class AccessPermission : DBModel
    {
        public int LocalID { get; set; }

        public int UserID { get; set; }

        [JsonProperty("Batt_onlyForEnginneringTeam")]
        public string BattOnlyForEngineeringTeam { get; set; }

        [JsonProperty("MCB_onlyForEnginneringTeam")]
        public string MCBOnlyForEngineeringTeam { get; set; }


        //To check login user type Admin/User
        public bool AllowEditing
        {
            get
            {
                //checking BattOnlyForEngineeringTeam and MCBOnlyForEngineeringTeam permissions for differentiating user permission
                if (BattOnlyForEngineeringTeam == "write" && MCBOnlyForEngineeringTeam == "write")
                {
                    return true;
                }
                return false;
            }
        }

        public override bool KeysEqual(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var i = (AccessPermission)obj;
            return LocalID == i.LocalID;
        }
    }
}
