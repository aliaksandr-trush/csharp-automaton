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
                UIUtil.DefaultProvider.SelectPopUpFrameByName(FrameID);
            }
            catch
            {
                // Ignore
            }
        }

        public void SaveAndStay()
        {
            this.SelectThisFrame();
            UIUtil.DefaultProvider.ClickSaveAndStay();
            Utility.ThreadSleep(1);
        }

        [Step]
        public void SaveAndClose()
        {
            this.SelectThisFrame();
            UIUtil.DefaultProvider.ClickSaveAndClose();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void Cancel()
        {
            this.SelectThisFrame();
            UIUtil.DefaultProvider.ClickCancel();
            Utility.ThreadSleep(1);
            SelectBuilderWindow();
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        [Step]
        public void ClickHotelTemplateLink()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(HotelTemplateLinkLocator, LocateBy.Id);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectPopUpFrameByName(HotelTemplateManager.FrameID);
        }

        public void SelectHotelTemplate(string hotel)
        {
            UIUtil.DefaultProvider.SelectWithText("ctl00_cphDialog_ddlHotels", hotel, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void SetDisable(bool check)
        {
            UIUtil.DefaultProvider.SetCheckbox("ctl00_cphDialog_cbEnableRoomBlockRestrictions", check, LocateBy.Id);
        }

        public void AddRoomBlock(string date, string roomType, int capacity)
        {
            this.AddRoomBlock(date);

            string capacityLocator = string.Format("//*[text()='{0}']/following-sibling::*/input[contains(@id,'BlockSize')]", roomType);
            UIUtil.DefaultProvider.Type(capacityLocator, capacity, LocateBy.XPath);
        }

        public void AddRoomBlock(string date)
        {
            string newRoomBlockLocator = "ctl00_cphDialog_mdNewRoomBlocks";

            UIUtil.DefaultProvider.WaitForDisplayAndClick(newRoomBlockLocator, LocateBy.Id);
            string calendarLocator = "//span/input[contains(@id,'RoomBlockDate')][contains(@onfocus,'DisplayCalendar')]";
            UIUtil.DefaultProvider.Type(calendarLocator, date, LocateBy.XPath);
        }

        public void AddRoomBlockNoDate()
        {
            string newRoomBlockLocator = "ctl00_cphDialog_mdNewRoomBlocks";

            UIUtil.DefaultProvider.WaitForDisplayAndClick(newRoomBlockLocator, LocateBy.Id);
        }

        public void SetCapacityAndRates(string roomType, int capacity, double? rate, int number)
        {
            string capacityLocator = string.Format("//*[text()='{0}']/following-sibling::*/input[contains(@id,'BlockSize')]", roomType);

            int id = System.Convert.ToInt32((UIUtil.DefaultProvider.GetId(capacityLocator, LocateBy.XPath)).Substring(29, 4));
            string order = UIUtil.DefaultProvider.GetId(capacityLocator, LocateBy.XPath).Substring(34);

            for (int i = 0; i <= number; i++)
            {
                UIUtil.DefaultProvider.Type(string.Format("ctl00_cphDialog_rntBlockSize_{0}_{1}", id + i, order), capacity, LocateBy.Id);

                if (rate != null)
                {
                    UIUtil.DefaultProvider.Type(string.Format("ctl00_cphDialog_rntRoomRate_{0}_{1}", id + i, order), rate, LocateBy.Id);
                }
            }
        }
    }
}
