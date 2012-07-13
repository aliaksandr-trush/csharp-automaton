namespace RegOnline.RegressionTest.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataAccess;

    /// <summary>
    /// Database related
    /// </summary>
    public class DataHelper
    {
        public int HideEventsInDeletedFolder(int customerId)
        {
            var db = new ClientDataContext();

            string command = string.Format(
                "DELETE NodesCustom WHERE (CustomerID={0} AND ParentNodeCustomID=-3) OR (EventID IN (SELECT Id FROM Events WHERE Customer_Id={0} AND Deleted=1))", 
                customerId);

            return db.ExecuteCommand(command);
        }

        public int HideEventsInDeletedFolder()
        {
            return this.HideEventsInDeletedFolder(ConfigurationProvider.XmlConfig.AccountConfiguration.Id);
        }

        public void ChangeAllRegsToTestForEvent(int eventId)
        {
            var db = new ClientDataContext();
            int rowsAffected;
            string command = string.Format("UPDATE Registrations SET Test = 1 WHERE Event_Id = {0}", eventId);
            rowsAffected = db.ExecuteCommand(command);
        }

        public void RemoveXAuthLiveRegistration(int accountId)
        {
            var db = new ClientDataContext();
            int rowsAffected;
            string command = string.Format("update Registrations set Test=1 "
            +"from Registrations r "
            +"inner join Events e on r.Event_Id=e.Id "
            +"inner join event_statuses es on e.StatusId=es.Id "
            +"inner join EventRegTypes ert on ert.Id=r.RegTypeId and r.Event_Id=ert.EventId "
            +"where e.Customer_Id={0} and r.Test=0", accountId);
            rowsAffected = db.ExecuteCommand(command);
        }

        public void ChangeAllRegsToTestAndDelete(int eventId)
        {
            var db = new ClientDataContext();
            int rowsAffected;
            string command = string.Format("UPDATE Registrations SET Test = 1 WHERE Event_Id = {0}", eventId);
            rowsAffected = db.ExecuteCommand(command);
            command = string.Format("DELETE FROM Registrations WHERE Event_Id={0}", eventId);
            db.ExecuteCommand(command);
        }

        public void DeleteRegistrationSession(List<string> registrationSessionIds)
        {
            var db = new ClientDataContext();
            
            // Step #1: delete those registrations' sessions from clientDB
            foreach (string sessionId in registrationSessionIds)
            {
                string command = string.Format("delete Sessions where Id ='{0}'", sessionId);
                db.ExecuteCommand(command);
            }

            // Step #2: execute stored procedure 'IncompleteRegistrationsCleanup'
            string cleanupCommand = "Exec IncompleteRegistrationsCleanup";
            db.ExecuteCommand(cleanupCommand);
        }

        public void VerifyRegType(int eventId, string name)
        {
            ClientDataContext db = new ClientDataContext();
            Event evt = (from e in db.Events where e.Id == eventId select e).Single();
            List<EventRegType> regTypes = (from rt in db.EventRegTypes where rt.Event == evt && rt.Description == name select rt).ToList();
            Assert.That(regTypes.Count != 0);
        }

        public List<Registration> GetEventRegistrations(int eventId)
        {
            List<Registration> registrations = new List<Registration>();

            ClientDataContext db = new ClientDataContext(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ClientDbConnection);
            var regs = (from r in db.Registrations where r.Event_Id == eventId select r).ToList();
            registrations = regs;

            return registrations;
        }

        public string GetRegistrationEmail(Registration reg)
        {
            string email = null;

            ClientDataContext db = new ClientDataContext(ConfigurationProvider.XmlConfig.EnvironmentConfiguration.ClientDbConnection);
            var attendee = (from a in db.Attendees where a.Id == reg.Attendee_Id select a).Single();
            email = attendee.Email_Address;

            return email;
        }

        public List<Custom_Field> GetPersonalInfoCustomFields(int eventId)
        {
            List<Custom_Field> customFields = null;

            try
            {
                ClientDataContext db = new ClientDataContext();
                customFields = (from c in db.Custom_Fields where c.EventId == eventId select c).ToList();
            }
            catch
            {
                Assert.Fail("Failed to fetch personal info custom fields from database!");
            }

            return customFields;
        }

        public Custom_Field GetPersonalInfoCustomField(int eventId, string cfName)
        {
            Custom_Field customField = null;

            try
            {
                customField = this.GetPersonalInfoCustomFields(eventId).Find(
                    delegate(Custom_Field cf)
                    {
                        return cf.Description == cfName;
                    });
            }
            catch
            {
                Assert.Fail("Failed to fetch personal info custom field '{0}' from database!", cfName);
            }

            return customField;
        }

        public List<Custom_Field_List_Item> GetCustomFieldListItems(int eventId, int customFieldId)
        {
            List<Custom_Field_List_Item> customFieldListItems = null;

            try
            {
                ClientDataContext db = new ClientDataContext();
                customFieldListItems = (from c in db.Custom_Field_List_Items where c.EventId == eventId && c.Custom_Field.Id == customFieldId select c).ToList();
            }
            catch
            {
                Assert.Fail("Failed to fetch custom field list items for custom field {0} from database!", customFieldId);
            }

            return customFieldListItems;
        }

        public List<Custom_Field> GetAgendaItems(int eventId)
        {
            List<Custom_Field> agendaItems = null;

            try
            {
                ClientDataContext db = new ClientDataContext();
                agendaItems = (from a in db.Custom_Fields where a.EventId == eventId &&
                                   a.LocationId == 0 && a.CategoryID == 2 select a).ToList();
            }
            catch
            {
                Assert.Fail("Failed to fetch agenda items from database!");
            }

            return agendaItems;
        }

        public Custom_Field GetAgendaItem(int eventId, string agendaItemName)
        {
            Custom_Field agendaItem = null;

            try
            {
                ClientDataContext db = new ClientDataContext();
                agendaItem = (from a in db.Custom_Fields where a.EventId == eventId && a.Description == agendaItemName select a).Single();
            }
            catch
            {
                Assert.Fail("Failed to fetch agenda item '{0}' from database!", agendaItemName);
            }

            return agendaItem;
        }

        public List<Location> GetHotel(string hotelName)
        {
            List<Location> targetHotel = null;

            try
            {
                ClientDataContext db = new ClientDataContext();
                targetHotel = (from t in db.Locations where t.Loc_Name == hotelName select t).ToList();
            }
            catch
            {
                Assert.Fail("Failed to fetch hotel '{0}' from database!", hotelName);
            }

            return targetHotel;
        }

        public List<Fee> GetMerchandiseItems(int eventId)
        {
            List<Fee> merchandiseItems = null;

            try
            {
                ClientDataContext db = new ClientDataContext();
                merchandiseItems = (from m in db.Fees where m.EventId == eventId select m).ToList();
            }
            catch
            {
                Assert.Fail("Failed to fetch merchandise items from database!");
            }

            return merchandiseItems;
        }

        public Fee GetMerchandiseItem(int eventId, string merchandiseItemName)
        {
            Fee merchandiseItem = null;

            try
            {
                merchandiseItem = this.GetMerchandiseItems(eventId).Find(
                    delegate(Fee merch)
                    {
                        return merch.Description == merchandiseItemName;
                    });
            }
            catch
            {
                Assert.Fail("Failed to fetch merchandise item '{0}' from database!", merchandiseItemName);
            }

            return merchandiseItem;
        }
    }
}
