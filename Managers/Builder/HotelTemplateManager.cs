namespace RegOnline.RegressionTest.Managers.Builder
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;

    public class HotelTemplateManager : ManagerBase
    {
        public const string FrameID = "dialog2";

        public void SaveAndStay()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//span[text()='Save & Stay']", LocateBy.XPath);
            Utility.ThreadSleep(1);
        }

        [Step]
        public void SaveAndClose()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//span[text()='Save & Close']", LocateBy.XPath);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog");
        }

        public void Cancel()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//span[text()='Cancel']", LocateBy.XPath);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        #region Hotel Information
        [Step]
        public void TypeName(string name)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_tbHotelName", name, LocateBy.Id);
        }

        public void TypeEmail(string email)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_tbHotelEmail", email, LocateBy.Id);
        }

        public void TypePhone(string phone)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_tbHotelPhone", phone, LocateBy.Id);
        }

        public void TypeDefaultTaxRate(double tax)
        {
            WebDriverUtility.DefaultProvider.TypeRADNumericById("ctl00_cphDialog_rntDefaultTaxRate", tax);
        }

        public void TypeWebsite(string website)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_tbHotelWebsite", website, LocateBy.Id);
        }

        public void SetShowMap(bool check)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cphDialog_chkShowMap", check, LocateBy.Id);
        }

        public void SelectCountry(string country)
        {
            WebDriverUtility.DefaultProvider.SelectWithValue("ctl00_cphDialog_ddlHotelCountry", country, LocateBy.Id);
        }

        public void TypeAddress1(string address)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_tbHotelAddressLine1", address, LocateBy.Id);
        }

        public void TypeAddress2(string address)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_tbHotelAddressLine2", address, LocateBy.Id);
        }

        public void TypeCity(string city)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_tbHotelCity", city, LocateBy.Id);
        }

        public void TypeFax(string fax)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_tbHotelFax", fax, LocateBy.Id);
        }

        public void TypeStateProvince(string stateProvince)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_tbHotelState", stateProvince, LocateBy.Id);
        }

        public void TypeZipCode(string zipCode)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_tbHotelZipcode", zipCode, LocateBy.Id);
        }
        #endregion

        #region Room Type
        // Add new room type
        [Step]
        public void AddNewRoomType(string name, double rate)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_lbNewRoomType", LocateBy.Id);
            Utility.ThreadSleep(0.5);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.Type("//table[@class='expando roomTypeTable']/tbody/tr[last()]/td[1]/input", name, LocateBy.XPath);
            WebDriverUtility.DefaultProvider.TypeRADNumericById(WebDriverUtility.DefaultProvider.GetAttribute("//table[@class='expando roomTypeTable']/tbody/tr[last()]/td[2]/span/input", "id", LocateBy.XPath).Replace("_text", ""), rate);
        }
        #endregion
    }
}
