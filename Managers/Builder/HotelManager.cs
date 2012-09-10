namespace RegOnline.RegressionTest.Managers.Builder
{
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;
    using System.Collections.Generic;

    public class HotelManager : ManagerBase
    {
        public const string FrameID = "dialog";
        private const string HotelTemplateLinkLocator = "ctl00_cphDialog_mdNewHotelTemplate_mdNewHotelTemplate_linkBtn";

        public HotelTemplateManager HotelTemplateMgr { get; private set; }

        public HotelManager()
        {
            this.HotelTemplateMgr = new HotelTemplateManager();
        }

        private void SelectThisFrame()
        {
            try
            {
                WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(FrameID);
            }
            catch
            {
                // Ignore
            }
        }

        public void SaveAndStay()
        {
            this.SelectThisFrame();
            WebDriverUtility.DefaultProvider.ClickSaveAndStay();
            Utility.ThreadSleep(1);
        }

        [Step]
        public void SaveAndClose()
        {
            this.SelectThisFrame();
            WebDriverUtility.DefaultProvider.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void Cancel()
        {
            this.SelectThisFrame();
            WebDriverUtility.DefaultProvider.ClickCancel();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        [Step]
        public void ClickHotelTemplateLink()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(HotelTemplateLinkLocator, LocateBy.Id);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(HotelTemplateManager.FrameID);
        }

        public void SelectHotelTemplate(string hotel)
        {
            WebDriverUtility.DefaultProvider.SelectWithText("ctl00_cphDialog_ddlHotels", hotel, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void SetDisable(bool check)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cphDialog_cbEnableRoomBlockRestrictions", check, LocateBy.Id);
        }

        public void AddRoomBlock(string date, string roomType, int capacity)
        {
            this.AddRoomBlock(date);

            string capacityLocator = string.Format("//*[text()='{0}']/following-sibling::*/input[contains(@id,'BlockSize')]", roomType);
            WebDriverUtility.DefaultProvider.Type(capacityLocator, capacity, LocateBy.XPath);
        }

        public void AddRoomBlock(string date)
        {
            string newRoomBlockLocator = "ctl00_cphDialog_mdNewRoomBlocks";

            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(newRoomBlockLocator, LocateBy.Id);
            string calendarLocator = "//span/input[contains(@id,'RoomBlockDate')][contains(@onfocus,'DisplayCalendar')]";
            WebDriverUtility.DefaultProvider.Type(calendarLocator, date, LocateBy.XPath);
        }

        public void AddRoomBlockNoDate()
        {
            string newRoomBlockLocator = "ctl00_cphDialog_mdNewRoomBlocks";

            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(newRoomBlockLocator, LocateBy.Id);
        }

        public void SetCapacityAndRates(string roomType, int capacity, double? rate, int number)
        {
            string capacityLocator = string.Format("//*[text()='{0}']/following-sibling::*/input[contains(@id,'BlockSize')]", roomType);

            int id = System.Convert.ToInt32((WebDriverUtility.DefaultProvider.GetId(capacityLocator, LocateBy.XPath)).Substring(29, 4));
            string order = WebDriverUtility.DefaultProvider.GetId(capacityLocator, LocateBy.XPath).Substring(34);

            for (int i = 0; i <= number; i++)
            {
                WebDriverUtility.DefaultProvider.Type(string.Format("ctl00_cphDialog_rntBlockSize_{0}_{1}", id + i, order), capacity, LocateBy.Id);

                if (rate != null)
                {
                    WebDriverUtility.DefaultProvider.Type(string.Format("ctl00_cphDialog_rntRoomRate_{0}_{1}", id + i, order), rate, LocateBy.Id);
                }
            }
        }
    }
}
