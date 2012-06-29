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
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//span[text()='Save & Stay']", LocateBy.XPath);
            Utility.ThreadSleep(1);
        }

        [Step]
        public void SaveAndClose()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//span[text()='Save & Close']", LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
        }

        public void Cancel()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//span[text()='Cancel']", LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        #region Hotel Information
        [Step]
        public void TypeName(string name)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cphDialog_tbHotelName", name, LocateBy.Id);
        }

        public void TypeEmail(string email)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cphDialog_tbHotelEmail", email, LocateBy.Id);
        }

        public void TypePhone(string phone)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cphDialog_tbHotelPhone", phone, LocateBy.Id);
        }

        public void TypeDefaultTaxRate(double tax)
        {
            UIUtilityProvider.UIHelper.TypeRADNumericById("ctl00_cphDialog_rntDefaultTaxRate", tax);
        }

        public void TypeWebsite(string website)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cphDialog_tbHotelWebsite", website, LocateBy.Id);
        }

        public void SetShowMap(bool check)
        {
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cphDialog_chkShowMap", check, LocateBy.Id);
        }

        public void SelectCountry(string country)
        {
            UIUtilityProvider.UIHelper.SelectWithValue("ctl00_cphDialog_ddlHotelCountry", country, LocateBy.Id);
        }

        public void TypeAddress1(string address)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cphDialog_tbHotelAddressLine1", address, LocateBy.Id);
        }

        public void TypeAddress2(string address)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cphDialog_tbHotelAddressLine2", address, LocateBy.Id);
        }

        public void TypeCity(string city)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cphDialog_tbHotelCity", city, LocateBy.Id);
        }

        public void TypeFax(string fax)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cphDialog_tbHotelFax", fax, LocateBy.Id);
        }

        public void TypeStateProvince(string stateProvince)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cphDialog_tbHotelState", stateProvince, LocateBy.Id);
        }

        public void TypeZipCode(string zipCode)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cphDialog_tbHotelZipcode", zipCode, LocateBy.Id);
        }
        #endregion

        #region Room Type
        // Add new room type
        [Step]
        public void AddNewRoomType(string name, double rate)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphDialog_lbNewRoomType", LocateBy.Id);
            Utility.ThreadSleep(0.5);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.Type("//table[@class='expando roomTypeTable']/tbody/tr[last()]/td[1]/input", name, LocateBy.XPath);
            UIUtilityProvider.UIHelper.TypeRADNumericById(UIUtilityProvider.UIHelper.GetAttribute("//table[@class='expando roomTypeTable']/tbody/tr[last()]/td[2]/span/input", "id", LocateBy.XPath).Replace("_text", ""), rate);
        }
        #endregion
    }
}
