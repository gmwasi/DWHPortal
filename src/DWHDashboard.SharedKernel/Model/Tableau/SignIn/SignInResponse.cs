using Newtonsoft.Json;

namespace DWHDashboard.SharedKernel.Model.Tableau.SignIn
{
    public class SignInResponse
    {
        [JsonProperty("credentials")]
        public Credentials Credentials { get; set; }

        public string GetAuthUserId()
        {
            return Credentials.User.Id;
        }

        public string GetAuthToken()
        {
            return Credentials.Token;
        }

        public string GetAuthSiteId()
        {
            return Credentials.Site.Id;
        }

        public bool IsSignedIn()
        {
            return !string.IsNullOrWhiteSpace(GetAuthToken());
        }

        public override string ToString()
        {
            return $"{Credentials.Site.ContentUrl} | {Credentials.User.Id}  [{Credentials.Token}]";
        }
    }
}