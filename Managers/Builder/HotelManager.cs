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
                UIUtilityProvider.UIHelper.SelectPopUpFrameByName(FrameID);
            }
            catch
            {
                // Ignore
            }
        }

        public void SaveAndStay()
        {
            this.SelectThisFrame();
            UIUtilityProvider.UIHelper.ClickSaveAndStay();
            Utility.ThreadSleep(1);
        }

        [Step]
        public void SaveAndClose()
        {
            this.SelectThisFrame();
            UIUtilityProvider.UIHelper.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void Cancel()
        {
            this.SelectThisFrame();
            UIUtilityProvider.UIHelper.ClickCancel();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        [Step]
        public void ClickHotelTemplateLink()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(HotelTemplateLinkLocator, LocateBy.Id);
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(HotelTemplateManager.FrameID);
        }

        public void SelectHotelTemplate(string hotel)
        {
            UIUtilityProvider.UIHelper.SelectWithText("ctl00_cphDialog_ddlHotels", hotel, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void SetDisable(bool check)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cphDialog_cbEnableRoomBlockRestrictions", check, LocateBy.Id);
        }

        public void AddRoomBlock(string date, string roomType, int capacity)
        {
            this.AddRoomBlock(date);

            string capacityLocator = string.Format("//*[text()='{0}']/following-sibling::*/input[contains(@id,'BlockSize')]", roomType);
            UIUtilityProvider.UIHelper.Type(capacityLocator, capacity, LocateBy.XPath);
        }

        public void AddRoomBlock(string date)
        {
            string newRoomBlockLocator = "ctl00_cphDialog_mdNewRoomBlocks";

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(newRoomBlockLocator, LocateBy.Id);
            string calendarLocator = "//span/input[contains(@id,'RoomBlockDate')][contains(@onfocus,'DisplayCalendar')]";
            UIUtilityProvider.UIHelper.Type(calendarLocator, date, LocateBy.XPath);
        }

        public void AddRoomBlockNoDate()
        {
            string newRoomBlockLocator = "ctl00_cphDialog_mdNewRoomBlocks";

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(newRoomBlockLocator, LocateBy.Id);
        }

        public void SetCapacityAndRate(string roomType, int capacity, double? rate)
        {
            string capacityLocator = string.Format("//*[text()='{0}']/following-sibling::*/input[contains(@id,'BlockSize')]", roomType);
            UIUtilityProvider.UIHelper.Type(capacityLocator, capacity, LocateBy.XPath);

            if (rate != null)
            {
                string rateLocator = string.Format("//*[text()='{0}']/following-sibling::*/input[contains(@id,'RoomRate')]", roomType);
                UIUtilityProvider.UIHelper.Type(rateLocator, rate, LocateBy.XPath);
            }
        }


        public void SetCapacityAndRates(string roomType, int capacity, double? rate, int number)
        {
            string capacityLocator = string.Format("//*[text()='{0}']/following-sibling::*/input[contains(@id,'BlockSize')]", roomType);

            int id = System.Convert.ToInt32((UIUtilityProvider.UIHelper.GetId(capacityLocator, LocateBy.XPath)).Substring(29, 4));
            string order = UIUtilityProvider.UIHelper.GetId(capacityLocator, LocateBy.XPath).Substring(34);


            for (int i = 0; i <= number; i++)
            {
                UIUtilityProvider.UIHelper.Type(string.Format("ctl00_cphDialog_rntBlockSize_{0}_{1}", id + i, order), capacity, LocateBy.Id);

                if (rate != null)
                {
                    UIUtilityProvider.UIHelper.Type(string.Format("ctl00_cphDialog_rntRoomRate_{0}_{1}", id + i, order), rate, LocateBy.Id);

                }
            }
        }
    }
}
