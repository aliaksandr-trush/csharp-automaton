namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public partial class FormDetailManager : ManagerBase
    {
        private const string CFFrameID = "dialog";

        public CustomFieldManager CFMgr { get; set; }

        public void OpenCustomField(string cfName)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(
                string.Format("//table[@id='ctl00_cph_grdCustomFieldPersonal_tblGrid']//a[text()='{0}']", cfName), 
                LocateBy.XPath);

            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(CFFrameID);
        }

        private void ClickAddCustomField(CustomFieldManager.CustomFieldLocation cfLocation)
        {
            string linkLocator;

            switch (cfLocation)
            {
                case CustomFieldManager.CustomFieldLocation.PI:
                    linkLocator = this.GetAddGridItemLocator(PIPageAddCFLinkLocatorPrefix);
                    break;

                case CustomFieldManager.CustomFieldLocation.LT_Lodging:
                    linkLocator = this.GetAddGridItemLocator(LTPageAddLodgingCFLinkLocatorPrefix);
                    break;

                case CustomFieldManager.CustomFieldLocation.LT_Travel:
                    linkLocator = this.GetAddGridItemLocator(LTPageAddTravelCFLinkLocatorPrefix);
                    break;

                case CustomFieldManager.CustomFieldLocation.LT_Preferences:
                    linkLocator = this.GetAddGridItemLocator(LTPageAddPreferenceCFLinkLocatorPrefix);
                    break;

                default:
                    throw new Exception(string.Format("Invalid custom field location: '{0}'", cfLocation.ToString()));
            }

            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(linkLocator, LocateBy.Id);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(CFFrameID);

            switch (cfLocation)
            {
                case CustomFieldManager.CustomFieldLocation.PI:
                    this.ResizeCustomFieldRADWindowAndAdjustPosition(1000, 800, 20, 800);
                    break;

                case CustomFieldManager.CustomFieldLocation.LT_Lodging:
                    this.ResizeCustomFieldRADWindowAndAdjustPosition(1000, 800, 20, 600);
                    break;

                case CustomFieldManager.CustomFieldLocation.LT_Travel:
                    this.ResizeCustomFieldRADWindowAndAdjustPosition(1000, 800, 20, 1700);
                    break;

                case CustomFieldManager.CustomFieldLocation.LT_Preferences:
                    this.ResizeCustomFieldRADWindowAndAdjustPosition(1000, 800, 20, 20);
                    break;

                default:
                    break;
            }
        }

        private void AddCustomField(
            CustomFieldManager.CustomFieldLocation cfLocation,
            CustomFieldManager.CustomFieldType type, 
            string name)
        {
            this.ClickAddCustomField(cfLocation);
            CFMgr.SetName(name);
            CFMgr.SetTypeWithDefaults(type);
            CFMgr.SaveAndClose();
        }

        private void VerifyCustomFieldInDatabase(CustomFieldManager.CustomFieldType type, string name, int location)
        {
            List<Custom_Field> cf = null;

            ClientDataContext db = new ClientDataContext();

            cf = (from c in db.Custom_Fields 
                  where c.Description == name && c.LocationId == location && c.TypeId == (int)type 
                  orderby c.Id ascending 
                  select c).ToList();

            Assert.That(cf.Count != 0);

            switch (type)
            {
                case CustomFieldManager.CustomFieldType.RadioButton:
                case CustomFieldManager.CustomFieldType.Dropdown:
                    Assert.That(cf.Last().Custom_Field_List_Items != null);
                    break;
                case CustomFieldManager.CustomFieldType.Number:
                case CustomFieldManager.CustomFieldType.OneLineText:
                    Assert.That(cf.Last().Length == ((int)type == 2 ? 50 : 5));
                    break;
            }
        }

        public string AddPrefixToCFID(string prefix, string CFnum)
        {
            return prefix + CFnum;
        }

        private void ResizeCustomFieldRADWindowAndAdjustPosition(int widthPx, int heightPx, int position_LeftPx, int position_TopPx)
        {
            WebDriverUtility.DefaultProvider.SwitchToMainContent();

            this.AdjustCustomFieldRADWindowPosition(position_LeftPx, position_TopPx);

            string js = string.Format(
                "document.getElementsByName('{0}')[0].style.width='{1}px';document.getElementsByName('{0}')[0].style.height='{2}px';",
                CFFrameID,
                widthPx,
                heightPx);

            WebDriverUtility.DefaultProvider.ExecuteJavaScript(js);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(CFFrameID);
        }

        // Custom field rad window's element id on personal info page and LT page are the same
        private void AdjustCustomFieldRADWindowPosition(int leftPx, int topPx)
        {
            string locator_Id_RADWindowDiv = "RadWindowWrapper_ctl00_dialog";

            string js = string.Format(
                "document.getElementById('{0}').style.left='{1}px';document.getElementById('{0}').style.top='{2}px';",
                locator_Id_RADWindowDiv,
                leftPx,
                topPx);

            WebDriverUtility.DefaultProvider.ExecuteJavaScript(js);
        }
    }
}
