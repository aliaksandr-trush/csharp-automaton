namespace RegOnline.RegressionTest.DataAccess
{
    using System.Linq;
    using RegOnline.RegressionTest.Configuration;
    using System.Collections.Generic;
    using System.Text;

    public class AccessData
    {
        public static void ApprovedXAuthRoleForCurrentCustomer(bool isApproved)
        {
            ApprovedXAuthRoleForCustomer(ConfigReader.DefaultProvider.AccountConfiguration.Id, isApproved);
        }

        public static void ApprovedXAuthRoleForCustomer(int customerId, bool isApproved)
        {
            var db = new ClientDataContext(ConfigReader.DefaultProvider.EnvironmentConfiguration.ClientDbConnection);
            
            string command = string.Format(
                "update XAuthConfiguration set approved='{0}' where CustomerId={1}",
                isApproved,
                customerId);

            db.ExecuteCommand(command);
        }

        public static void SetLiveRegToTest(int eventId)
        {
            var db = new ClientDataContext(ConfigReader.DefaultProvider.EnvironmentConfiguration.ClientDbConnection);
            db.ExecuteCommand("update Registrations set Test = 1 where Event_Id = " + eventId);
        }

        public static void SetLiveRegToTest(List<int> eventIds)
        {
            var db = new ClientDataContext(ConfigReader.DefaultProvider.EnvironmentConfiguration.ClientDbConnection);

            StringBuilder command = new StringBuilder("update Registrations set Test = 1 where Event_Id in (");

            foreach (int id in eventIds)
            {
                command.Append(string.Format("{0},", id.ToString()));
            }

            command.Replace(',', ')', command.Length - 1, 1);

            db.ExecuteCommand(command.ToString());
        }

        public static string GetEncryptString(string strToEncrypt)
        {
            var db = new ROMasterDataContext(ConfigReader.DefaultProvider.EnvironmentConfiguration.ROMasterConnection);
            return db.fn_Encrypt(strToEncrypt);
        }

        public static string FetchConfirmationEmailId(int eventId)
        {
            int regMailTriggerId = 1;
            string emailId = string.Empty;
            var db = new ClientDataContext(ConfigReader.DefaultProvider.EnvironmentConfiguration.ClientDbConnection);
            var id = (from i in db.RegMailResponders where i.EventId == eventId && i.RegMailTypeId == 2 && i.RegMailTriggerId == regMailTriggerId orderby i.Id descending select i.Id).ToList();
            emailId = id[0].ToString();
            return emailId;
        }

        public static int FetchAttendeeId(int registrantId)
        {
            var db = new ClientDataContext(ConfigReader.DefaultProvider.EnvironmentConfiguration.ClientDbConnection);
            return (from r in db.Registrations where r.Register_Id == registrantId select r.Attendee_Id).Single();
        }

        public static int FetchRegTypeId(int eventId, string regTypeName)
        {
            var db = new ClientDataContext(ConfigReader.DefaultProvider.EnvironmentConfiguration.ClientDbConnection);
            return (from r in db.EventRegTypes where r.EventId == eventId && r.Description == regTypeName select r.Id).Single();
        }

        public static void SetSSOEndpointUrl(int customerId, string endpointUrl)
        {
            var db = new ClientDataContext();
            var config = db.XAuthConfigurations.Single(c => c.CustomerId == customerId);
            config.SSOServiceEndpointUrl = endpointUrl;
            db.SubmitChanges();
        }
    }
}
