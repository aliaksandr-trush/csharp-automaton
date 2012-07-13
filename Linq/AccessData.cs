namespace RegOnline.RegressionTest.DataAccess
{
    using RegOnline.RegressionTest.Configuration;

    public class AccessData
    {
        public static void ApprovedXAuthRoleForCustomer(bool isApproved)
        {
            var db = new ClientDataContext(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ClientDbConnection);
            db.ExecuteCommand("update XAuthConfiguration set approved='" + isApproved + "' where CustomerId=" + ConfigurationProvider.XmlConfig.AccountConfiguration.Id);
        }

        public static void RemoveLiveRegForEvent(int eventId)
        {
            var db = new ClientDataContext(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ClientDbConnection);
            db.ExecuteCommand("update Registrations set Test = 1 where Event_Id = " + eventId);
        }
    }
}
