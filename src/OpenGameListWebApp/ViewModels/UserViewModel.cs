using Newtonsoft.Json;

namespace OpenGameListWebApp.ViewModels
{
    [JsonObject(MemberSerialization.OptOut)]
    public class UserViewModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }
         
        public string PasswordNew { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }
    }
}