namespace RegOnline.RegressionTest.PageObject.Manager.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.Utilities;

    public class PaymentGateways : Window
    {
        public GatewayCreditCardVerification Frame_GatewayCreditCardVerification = new GatewayCreditCardVerification("plain");
        private Clickable link_AddPaymentGateway = new Clickable("ctl00_ctl00_cphDialog_cpMgrMain_addPmtGateway", UIUtility.LocateBy.Id);
    }

    public class GatewayRow : Window
    {
        private const string Locator_GatewayTable = "//table[@id='ctl00_ctl00_cphDialog_cpMgrMain_grdMAs_ctl00']";
        private const string LocatorFormat_GatewayRow = Locator_GatewayTable + "/tbody/tr[td[a[text()='{0}']]]";

        public Clickable Description;
        public Label GatewayCategory;
        public Label Currency;
        public Clickable VerifyCCProcessing;

        public GatewayRow(DataCollection.EventData_Common.Gateway gateway, DataCollection.EventData_Common.GatewayCategory category)
        {
            this.Description = new Clickable(
                string.Format("{0}/td[2]/a", this.Compose_Locator_GatewayRow(gateway)), 
                UIUtility.LocateBy.XPath);

            this.GatewayCategory = new Label(
                string.Format("{0}/td[3]/span", this.Compose_Locator_GatewayRow(gateway)), 
                UIUtility.LocateBy.XPath);

            this.Currency = new Label(
                string.Format("{0}/td[5]", this.Compose_Locator_GatewayRow(gateway)),
                UIUtility.LocateBy.XPath);

            this.VerifyCCProcessing = new Clickable(
                string.Format("{0}/td[8]//a[img[@title='Verify credit card processing']]", this.Compose_Locator_GatewayRow(gateway)),
                UIUtility.LocateBy.XPath);
        }

        private string Compose_Locator_GatewayRow(DataCollection.EventData_Common.Gateway gateway)
        {
            return string.Format(LocatorFormat_GatewayRow, CustomStringAttribute.GetCustomString(gateway));
        }

        public void VerifyCCProcessing_Click()
        {
            this.VerifyCCProcessing.WaitForDisplayAndClick();
            WaitForAJAX();
            WaitForLoad();
            Utility.ThreadSleep(2);
        }
    }

    public class GatewayCreditCardVerification : Frame
    {
        public MultiChoiceDropdown CardType = new MultiChoiceDropdown("ddlCardType", UIUtility.LocateBy.Id);
        public Input CardNumber = new Input("txtCardNumber", UIUtility.LocateBy.Id);
        public Input SecurityCode = new Input("txtCVV", UIUtility.LocateBy.Id);
        public MultiChoiceDropdown ExpirationDate_Month = new MultiChoiceDropdown("ddlMonth", UIUtility.LocateBy.Id);
        public MultiChoiceDropdown ExpirationDate_Year = new MultiChoiceDropdown("ddlYear", UIUtility.LocateBy.Id);
        public Input CardHolderName = new Input("txtName", UIUtility.LocateBy.Id);
        public Input BillingAddressLine_1 = new Input("txtAddress1", UIUtility.LocateBy.Id);
        public Input BillingAddressLine_2 = new Input("txtAddress2", UIUtility.LocateBy.Id);
        public Input City = new Input("txtCity", UIUtility.LocateBy.Id);
        public Input StateOrProvince = new Input("txtStateProvince", UIUtility.LocateBy.Id);
        public Input ZipCode = new Input("txtZip", UIUtility.LocateBy.Id);
        public MultiChoiceDropdown Country = new MultiChoiceDropdown("ddlCountry", UIUtility.LocateBy.Id);
        public Clickable Button_VerifyCard = new Clickable("//span[@class='BiggerButtonBaseBlue']/a/span[text()='Verify Card']", UIUtility.LocateBy.XPath);
        public Clickable Button_Cancel = new Clickable("//span[@class='BiggerButtonBase']/a/span[text()='Cancel']", UIUtility.LocateBy.XPath);
        private Label SuccessfulMessage = new Label("//div[@id='divSuccess']/span", UIUtility.LocateBy.XPath);

        public GatewayCreditCardVerification(string name)
            : base(name)
        {
        }

        public void BillingInfo_Type(DataCollection.BillingInfo info)
        {
            this.CardType.WaitForDisplay();
            this.CardType.SelectWithText(Utilities.CustomStringAttribute.GetCustomString(info.CC_Type));
            this.CardNumber.Type(info.CC_Number);
            this.SecurityCode.Type(info.SecurityCode);
            this.ExpirationDate_Month.WaitForDisplay();
            this.ExpirationDate_Month.SelectWithValue(info.ExpirationDate.Month.ToString());
            this.ExpirationDate_Year.WaitForDisplay();
            this.ExpirationDate_Year.SelectWithValue(info.ExpirationDate.Year.ToString());
            this.CardHolderName.Type(info.CardholderName);
            this.Country.WaitForDisplay();
            this.Country.SelectWithText(Utilities.CustomStringAttribute.GetCustomString(info.CC_Country));
            this.BillingAddressLine_1.Type(info.BillingAddressLineOne);
            this.BillingAddressLine_2.Type(info.BillingAddressLineTwo);
            this.City.Type(info.BillingCity);
            this.StateOrProvince.Type(info.BillingStateOrProvince);
            this.ZipCode.Type(info.BillingZip);
        }

        public void VerifyCard_Click()
        {
            this.Button_VerifyCard.WaitForDisplayAndClick();
            WaitForAJAX();
            WaitForLoad();
            Utility.ThreadSleep(2);
        }

        public void Cancel_Click()
        {
            this.Button_Cancel.WaitForDisplayAndClick();
            WaitForAJAX();
            WaitForLoad();
            Utility.ThreadSleep(2);
        }
    }
}
