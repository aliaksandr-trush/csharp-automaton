namespace RegOnline.RegressionTest.Managers.Register
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers.Builder;

    /// <summary>
    /// Should be replaced by DataHelper
    /// </summary>
    public partial class RegisterManager : ManagerBase
    {
        public List<Fee> Fetch_Merchandise(int eventId)
        {
            ClientDataContext db = new ClientDataContext();
            return (from f in db.Fees where f.EventId == eventId select f).ToList();
        }

        public List<int> Fetch_AgendaIdList()
        {
            List<Custom_Field> agendaItems = Fetch_AgendaItemsFor(
                CurrentEventId,
                CustomFieldManager.CustomFieldCategory.Agenda,
                CustomFieldManager.CustomFieldLocation.Agenda,
                false);

            List<int> ids = new List<int>();

            foreach (Custom_Field customField in agendaItems)
            {
                int cfID = Convert.ToInt32(customField.Id);
                ids.Add(cfID);
            }

            return ids;
        }

        public List<Custom_Field> Fetch_CustomFieldsFor(
            int eventId, 
            CustomFieldManager.CustomFieldCategory category, 
            CustomFieldManager.CustomFieldLocation location, 
            bool adminOnly)
        {         
            ClientDataContext db = new ClientDataContext();

            return (from c in db.Custom_Fields where c.EventId == eventId &&
                        c.CategoryID == (int)category && c.LocationId == (int)location &&
                        c.AdminOnly == adminOnly select c).ToList();
        }

        public List<Custom_Field> Fetch_AgendaItemsFor(
            int eventId, 
            CustomFieldManager.CustomFieldCategory category, 
            CustomFieldManager.CustomFieldLocation location, 
            bool adminOnly)
        {
            ClientDataContext db = new ClientDataContext();

            return (from c in db.Custom_Fields where c.EventId == eventId &&
                        c.CategoryID == (int)category && c.LocationId == (int)location &&
                        c.AdminOnly == adminOnly select c).ToList();
        }

        public List<Custom_Field_List_Item> Fetch_CustomFieldListItems(int eventId, int customFieldId)
        {
            ClientDataContext db = new ClientDataContext();

            return (from c in db.Custom_Field_List_Items where c.EventId == eventId && c.CFId == customFieldId select c).ToList();
        }
    }
}
