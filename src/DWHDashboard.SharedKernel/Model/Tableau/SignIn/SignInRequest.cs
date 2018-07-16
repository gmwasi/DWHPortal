using Newtonsoft.Json;

namespace DWHDashboard.SharedKernel.Model.Tableau.SignIn
{
    public class SignInRequest
    {
        [JsonProperty("credentials")]
        public Credentials Credentials { get; set; }

        public SignInRequest()
        {
        }

        private SignInRequest(Credentials credentials)
        {
            Credentials = credentials;
        }

        public static SignInRequest Create(string name, string password, string site)
        {
            var cred = SignIn.Credentials.Create(name, password, site);
            return new SignInRequest(cred);
        }
    }
}