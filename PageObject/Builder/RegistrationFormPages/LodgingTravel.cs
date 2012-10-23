namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using System.Collections.Generic;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class LodgingTravel : Window
    {
        public Clickable LTPageHeader = new Clickable("//*[text()='Add Lodging & Travel Page Header']", LocateBy.XPath);
        public HtmlEditor LTPageHeaderEditor = new HtmlEditor("dialog");
        public Clickable LTPageFooter = new Clickable("//*[text()='Add Lodging & Travel Page Footer']", LocateBy.XPath);
        public HtmlEditor LTPageFooterEditor = new HtmlEditor("dialog");
        public Clickable AddNewHotel = new Clickable("//a[text()='Add New Hotel']", LocateBy.XPath);
        public AddHotelFrame AddHotelFrame = new AddHotelFrame("dialog");
        public RadioButton ChargeLodgingFee = new RadioButton("ctl00_cph_radCharge", LocateBy.Id);

        public void SetLodgingStandardFieldVisible(DataCollection.EventData_Common.LodgingStandardFields field, bool visible)
        {
            CheckBox LodgingStandardFieldVisible = new CheckBox(string.Format("ctl00_cph_chk{0}V", field.ToString()), LocateBy.Id);
            LodgingStandardFieldVisible.Set(visible);
        }

        public void SetLodgingStandardFieldRequired(DataCollection.EventData_Common.LodgingStandardFields field, bool requied)
        {
            CheckBox LodgingStandardFieldRequired = new CheckBox(string.Format("ctl00_cph_chk{0}R", field.ToString()), LocateBy.Id);
            LodgingStandardFieldRequired.Set(requied);
        }

        public void AddNewHotel_Click()
        {
            this.AddNewHotel.WaitForDisplay();
            this.AddNewHotel.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void ChargeLodgingFee_Click()
        {
            this.ChargeLodgingFee.WaitForDisplay();
            this.ChargeLodgingFee.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void LTPageHeader_Click()
        {
            this.LTPageHeader.WaitForDisplay();
            this.LTPageHeader.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void LTPageFooter_Click()
        {
            this.LTPageFooter.WaitForDisplay();
            this.LTPageFooter.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }

    public class AddHotelFrame : Frame
    {
        public AddHotelFrame(string name) : base(name) { }

        public Clickable AddHotelTemplate = new Clickable("ctl00_cphDialog_mdNewHotelTemplate_mdNewHotelTemplate_linkBtn", LocateBy.Id);
        public MultiChoiceDropdown ChooseHotel = new MultiChoiceDropdown("ctl00_cphDialog_ddlHotels", LocateBy.Id);
        public HotelDefine HotelDefine = new HotelDefine("dialog2");
        public Clickable AddRoomBlock = new Clickable("ctl00_cphDialog_mdNewRoomBlocks", LocateBy.Id);

        public Input RoomBlockCapacity(RoomType roomType, int roomBlockIndex)
        {
            Label roomTypeTrLabel = new Label(string.Format(
                "//div[@class='lowerSubSection frmDashPanelBase']//td[text()='{0}']/..", roomType.RoomTypeName), LocateBy.XPath);

            List<OpenQA.Selenium.IWebElement> roomBlocks = UIUtil.DefaultProvider.GetElements(roomTypeTrLabel.Locator, roomTypeTrLabel.TypeOfLocator);

            string id = roomBlocks[roomBlockIndex].GetAttribute("id").Substring(28, 4);
            string index = roomBlocks[roomBlockIndex].GetAttribute("id").Substring(33, 1);

            return new Input(string.Format("ctl00_cphDialog_rntBlockSize_{0}_{1}", id, index), LocateBy.Id);
        }

        public Input RoomBlockRate(RoomType roomType, int roomBlockIndex)
        {
            Label roomTypeTrLabel = new Label(string.Format(
                           "//div[@class='lowerSubSection frmDashPanelBase']//td[text()='{0}']/..", roomType.RoomTypeName), LocateBy.XPath);

            List<OpenQA.Selenium.IWebElement> roomBlocks = UIUtil.DefaultProvider.GetElements(roomTypeTrLabel.Locator, roomTypeTrLabel.TypeOfLocator);

            string id = roomBlocks[roomBlockIndex].GetAttribute("id").Substring(28, 4);
            string index = roomBlocks[roomBlockIndex].GetAttribute("id").Substring(33, 1);

            return new Input(string.Format("ctl00_cphDialog_rntRoomRate_{0}_{1}", id, index), LocateBy.Id);
        }

        public Input RoomBlockDate(RoomType roomType, int roomBlockIndex)
        {
            Label roomTypeTrLabel = new Label(string.Format(
                "//div[@class='lowerSubSection frmDashPanelBase']//td[text()='{0}']/..", roomType.RoomTypeName), LocateBy.XPath);

            List<OpenQA.Selenium.IWebElement> roomBlocks = UIUtil.DefaultProvider.GetElements(roomTypeTrLabel.Locator, roomTypeTrLabel.TypeOfLocator);

            string id = roomBlocks[roomBlockIndex].GetAttribute("id").Substring(28, 4);

            return new Input(string.Format("ctl00_cphDialog_dtpRoomBlockDate{0}_dateInput_text", id), LocateBy.Id);
        }

        public void AddRoomBlock_Click()
        {
            this.AddRoomBlock.WaitForDisplay();
            this.AddRoomBlock.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AddHotelTemplate_Click()
        {
            this.AddHotelTemplate.WaitForDisplay();
            this.AddHotelTemplate.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void SaveAndClose()
        {
            PageObjectHelper.PopupFrame_Helper.SaveAndClose_Click();
            SwitchToMain();
        }
    }

    public class HotelDefine : Frame
    {
        public HotelDefine(string name) : base(name) { }

        public Input HotelName = new Input("ctl00_cphDialog_tbHotelName", LocateBy.Id);
        public Clickable AddRoomType = new Clickable("ctl00_cphDialog_lbNewRoomType", LocateBy.Id);
        public Clickable SaveAndClose = new Clickable("//span[@class='BiggerButtonBase']/a/span[text()='Save & Close']", LocateBy.XPath);

        public Input RoomType(int index)
        {
            string locatorFormat = "ctl00_cphDialog_reRoomTypes_ctl{0}_tbRoomTypeName";
            string toFormat = (index <= 10) ? ("0" + index.ToString()) : index.ToString();
            string locator = string.Format(locatorFormat, toFormat);

            return new Input(locator, LocateBy.Id);
        }

        public Input RoomRate(int index)
        {
            string locatorFormat = "ctl00_cphDialog_reRoomTypes_ctl{0}_rntRoomTypeRate_text";
            string toFormat = (index <= 9) ? ("0" + index.ToString()) : index.ToString();
            string locator = string.Format(locatorFormat, toFormat);

            return new Input(locator, LocateBy.Id);
        }

        public void AddRoomType_Click()
        {
            this.AddRoomType.WaitForDisplay();
            this.AddRoomType.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void SaveAndClose_Click()
        {
            this.SaveAndClose.WaitForDisplay();
            this.SaveAndClose.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
            SwitchToMain();
        }
    }
}
