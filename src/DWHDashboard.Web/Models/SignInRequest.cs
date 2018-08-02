using System.Net.Http;

namespace DWHDashboard.Web.Models
{
/*

<tsRequest>
  <credentials name="kenyahmis" password="S3tpassw0rd" >
    <site contentUrl="PublicDWH" />
  </credentials>
</tsRequest>

*/
    public class SignInRequest
    {
        private Credentials Credentials { get; }

        public SignInRequest(Credentials credentials)
        {
            Credentials = credentials;
        }

        public StringContent GetTsRequest()
        {
            var request = $"<tsRequest>" +
                          $"  <credentials name={Credentials.Name} password={Credentials.Password}>" +
                          $"     <site contentUrl={Credentials.Site.ContentUrl}/>" +
                          $"  </credentials>" +
                          $"</tsRequest>";

            return new StringContent(request);
        }
    }
}