using Newtonsoft.Json;

namespace DWHDashboard.Web.Models
{
    public class View
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ContentUrl { get; set; }
        public string WorkbookId { get; set; }
        [JsonIgnore]
        public Workbook Workbook { get; set; }
        [JsonIgnore]
        public AuthSession AuthSession { get; set; }
        [JsonIgnore]
        public AuthTicket AuthTicket { get; set; }
        [JsonIgnore]
        public string InteractiveUrl { get; set; }
        [JsonIgnore]
        public string PreviewImageUrl { get; set; }

        [JsonIgnore]
        public bool IsPublic
        {
            get
            {

                /*
                 dashboard^implementation
dashboard^gapr
dashboard^general
dashboard^cop
dashboard^cohort
                 */
                return
                    (
                        Name.ToLower().Contains("implementation".ToLower()) ||
                        Name.ToLower().Contains("gapr".ToLower()) ||
                        Name.ToLower().Contains("general".ToLower()) ||
                        Name.ToLower().Contains("cop".ToLower()) ||
                        Name.ToLower().Contains("cohort".ToLower())
                    ) &&
                    Name.ToLower().Contains("dashboard".ToLower()) &&
                    !Name.ToLower().Contains("live".ToLower());
            }
        }

        [JsonIgnore]
        public bool IsChart
        {
            get
            {
                return
                    !Name.ToLower().Contains("dashboard") &&
                    !Name.ToLower().Contains("stripe".ToLower()) &&
                    !Name.ToLower().Contains("img".ToLower());
            }
        }

        public View()
        {
            Workbook=new Workbook();
            AuthSession=new AuthSession();
            AuthTicket=new AuthTicket();
        }

        public override string ToString()
        {
            return $@"{Name} ({Id})";
        }
    }
}